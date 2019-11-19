using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace IbraExport.Writers
{
    class BrepWriter : Writer
    {
        public override bool TryDump(RhinoObject obj, Model model, RhinoDoc document)
        {
            if (obj.ObjectType != ObjectType.Brep)
                return false;

            var brep = (Brep)obj.Geometry;

            var brepKey = obj.GetKey();

            for (int i = 0; i < brep.Curves2D.Count; i++)
            {
                var curve = brep.Curves2D[i].ToNurbsCurve();

                var curveItem = new Item($"{brepKey}.NurbsCurveGeometry2D<{i}>", "NurbsCurveGeometry2D");
                DumpNurbsCurve2D(curveItem, curve);

                model.Items.Add(curveItem);
            }

            for (int i = 0; i < brep.Surfaces.Count; i++)
            {
                var surface = brep.Surfaces[i].ToNurbsSurface(document.ModelAbsoluteTolerance, out var _);
                
                var surfaceItem = new Item($"{brepKey}.NurbsSurfaceGeometry3D<{i}>", "NurbsSurfaceGeometry3D");
                DumpNurbsSurface3D(surfaceItem, surface);

                model.Items.Add(surfaceItem);
            }

            foreach (var face in brep.Faces)
            {
                var faceItem = new Item($"{brepKey}.BrepFace<{face.FaceIndex}>", "BrepFace");
                faceItem.Set("Brep", brepKey);
                faceItem.Set("Loops", face.Loops.Select(o => $"{brepKey}.BrepLoop<{o.LoopIndex}>"));
                faceItem.Set("Geometry", $"{brepKey}.NurbsSurfaceGeometry3D<{face.SurfaceIndex}>");

                model.Items.Add(faceItem);
            }

            foreach (var loop in brep.Loops)
            {
                var loopItem = new Item($"{brepKey}.BrepLoop<{loop.LoopIndex}>", "BrepLoop");
                loopItem.Set("Brep", brepKey);
                loopItem.Set("Face", $"{brepKey}.BrepFace<{loop.Face.FaceIndex}>");
                loopItem.Set("Trims", loop.Trims.Select(o => $"{brepKey}.BrepTrim<{o.TrimIndex}>"));

                model.Items.Add(loopItem);
            }

            foreach (var trim in brep.Trims)
            {
                var trimItem = new Item($"{brepKey}.BrepTrim<{trim.TrimIndex}>", "BrepTrim");
                trimItem.Set("Brep", brepKey);
                trimItem.Set("Loop", $"{brepKey}.BrepLoop<{trim.Loop.LoopIndex}>");
                if (trim.Edge != null)
                    trimItem.Set("Edge", $"{brepKey}.BrepEdge<{trim.Edge.EdgeIndex}>");
                trimItem.Set("Geometry", $"{brepKey}.NurbsCurveGeometry2D<{trim.TrimCurveIndex}>");
                trimItem.Set("Domain", trim.Domain);

                model.Items.Add(trimItem);
            }

            foreach (var edge in brep.Edges)
            {
                var edgeItem = new Item($"{brepKey}.BrepEdge<{edge.EdgeIndex}>", "BrepEdge");
                edgeItem.Set("Brep", brepKey);
                edgeItem.Set("Trims", edge.TrimIndices().Select(o => $"{brepKey}.BrepTrim<{o}>"));

                model.Items.Add(edgeItem);
            }

            var brepItem = new Item(brepKey, "Brep");
            brepItem.Set("CurveGeometries2D", brep.Curves2D.Select((o, i) => $"{brepKey}.NurbsCurveGeometry2D<{i}>"));
            brepItem.Set("SurfaceGeometries3D", brep.Surfaces.Select((o, i) => $"{brepKey}.NurbsSurfaceGeometry3D<{i}>"));
            brepItem.Set("Faces", brep.Faces.Select(o => $"{brepKey}.BrepFace<{o.FaceIndex}>"));
            brepItem.Set("Loops", brep.Loops.Select(o => $"{brepKey}.BrepLoop<{o.LoopIndex}>"));
            brepItem.Set("Trims", brep.Trims.Select(o => $"{brepKey}.BrepTrim<{o.TrimIndex}>"));
            brepItem.Set("Edges", brep.Edges.Select(o => $"{brepKey}.BrepEdge<{o.EdgeIndex}>"));

            model.Items.Add(brepItem);

            return true;
        }
    }
}
