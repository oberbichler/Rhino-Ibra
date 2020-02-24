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

                var curveItem = new Item($"{brepKey}.nurbs_curve_geometry_2d<{i}>", "nurbs_curve_geometry_2d");
                DumpNurbsCurve2D(curveItem, curve);

                model.Items.Add(curveItem);
            }

            for (int i = 0; i < brep.Surfaces.Count; i++)
            {
                var surface = brep.Surfaces[i].ToNurbsSurface(document.ModelAbsoluteTolerance, out var _);
                
                var surfaceItem = new Item($"{brepKey}.nurbs_surface_geometry_3d<{i}>", "nurbs_surface_geometry_3d");
                DumpNurbsSurface3D(surfaceItem, surface);

                model.Items.Add(surfaceItem);
            }

            foreach (var face in brep.Faces)
            {
                var faceItem = new Item($"{brepKey}.brep_face<{face.FaceIndex}>", "brep_face");
                faceItem.Set("brep", brepKey);
                faceItem.Set("loops", face.Loops.Select(o => $"{brepKey}.brep_loop<{o.LoopIndex}>"));
                faceItem.Set("geometry", $"{brepKey}.nurbs_surface_geometry_3d<{face.SurfaceIndex}>");

                model.Items.Add(faceItem);
            }

            foreach (var loop in brep.Loops)
            {
                var loopItem = new Item($"{brepKey}.brep_loop<{loop.LoopIndex}>", "brep_loop");
                loopItem.Set("brep", brepKey);
                loopItem.Set("face", $"{brepKey}.brep_face<{loop.Face.FaceIndex}>");
                loopItem.Set("trims", loop.Trims.Select(o => $"{brepKey}.brep_trim<{o.TrimIndex}>"));

                model.Items.Add(loopItem);
            }

            foreach (var trim in brep.Trims)
            {
                var trimItem = new Item($"{brepKey}.brep_trim<{trim.TrimIndex}>", "brep_trim");
                trimItem.Set("brep", brepKey);
                trimItem.Set("loop", $"{brepKey}.brep_loop<{trim.Loop.LoopIndex}>");
                if (trim.Edge != null)
                    trimItem.Set("Edge", $"{brepKey}.brep_edge<{trim.Edge.EdgeIndex}>");
                trimItem.Set("geometry", $"{brepKey}.nurbs_curve_geometry_2d<{trim.TrimCurveIndex}>");
                trimItem.Set("domain", trim.Domain);

                model.Items.Add(trimItem);
            }

            foreach (var edge in brep.Edges)
            {
                var edgeItem = new Item($"{brepKey}.brep_edge<{edge.EdgeIndex}>", "brep_edge");
                edgeItem.Set("brep", brepKey);
                edgeItem.Set("trims", edge.TrimIndices().Select(o => $"{brepKey}.brep_trim<{o}>"));

                model.Items.Add(edgeItem);
            }

            var brepItem = new Item(brepKey, "brep");
            brepItem.Set("curve_geometries_2d", brep.Curves2D.Select((o, i) => $"{brepKey}.nurbs_curve_geometry_2d<{i}>"));
            brepItem.Set("surface_geometries_3d", brep.Surfaces.Select((o, i) => $"{brepKey}.nurbs_surface_geometry_3d<{i}>"));
            brepItem.Set("faces", brep.Faces.Select(o => $"{brepKey}.brep_face<{o.FaceIndex}>"));
            brepItem.Set("loops", brep.Loops.Select(o => $"{brepKey}.brep_loop<{o.LoopIndex}>"));
            brepItem.Set("trims", brep.Trims.Select(o => $"{brepKey}.brep_trim<{o.TrimIndex}>"));
            brepItem.Set("edges", brep.Edges.Select(o => $"{brepKey}.brep_edge<{o.EdgeIndex}>"));

            model.Items.Add(brepItem);

            return true;
        }
    }
}
