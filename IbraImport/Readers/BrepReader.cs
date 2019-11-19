using Newtonsoft.Json.Linq;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbraImport.Readers
{
    class BrepReader : Reader
    {
        public override bool TryLoad(JObject data, Model model, RhinoDoc document)
        {
            if (!data.HasType("Brep"))
                return false;

            var attributes = GetAttributes(document, data);

            var curves2d = new Dictionary<string, NurbsCurve>();
            var surfaces = new Dictionary<string, NurbsSurface>();

            foreach (var faceKey in data["Faces"].AsList<string>())
            {
                var faceData = model[faceKey];

                var loops = new List<List<Curve>>();

                foreach (var loopKey in faceData["Loops"].AsList<string>())
                {
                    var loop = new List<Curve>();

                    foreach (var trimKey in model[loopKey]["Trims"].AsList<string>())
                    {
                        var curveKey = model[trimKey]["Geometry"].As<string>();

                        if (!curves2d.TryGetValue(curveKey, out NurbsCurve curve))
                        {
                            curve = GetNurbsCurve2D(model[curveKey]);
                            curves2d.Add(curveKey, curve);
                        }

                        loop.Add(curve);
                    }

                    loops.Add(loop);
                }

                var surfaceKey = faceData["Geometry"].As<string>();

                if (!surfaces.TryGetValue(surfaceKey, out NurbsSurface surface))
                {
                    surface = GetNurbsSurface3D(model[surfaceKey]);
                    surfaces.Add(surfaceKey, surface);
                }

                var planarSurface = new PlaneSurface(Plane.WorldXY, surface.Domain(0), surface.Domain(1));

                var planarBrep = new Brep();

                var planarSurfaceIndex = planarBrep.AddSurface(planarSurface);

                var face = planarBrep.Faces.Add(planarSurfaceIndex);
                
                planarBrep.Loops.AddPlanarFaceLoop(face.FaceIndex, BrepLoopType.Outer, loops[0]);

                for (int i = 1; i < loops.Count; i++)
                    planarBrep.Loops.AddPlanarFaceLoop(0, BrepLoopType.Inner, loops[i]);

                var brep = Brep.CopyTrimCurves(face, surface, document.ModelAbsoluteTolerance);

                brep.SetTrimIsoFlags();

                brep.Faces[0].RebuildEdges(document.ModelAbsoluteTolerance, true, true);

                if (faceData.ContainsKey("Key"))
                    attributes.Name = faceData["Key"].As<string>();

                document.Objects.Add(brep, attributes);
            }

            return true;
        }
    }
}