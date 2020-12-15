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
        static int? MinPolynomialDegree(NurbsSurface surface)
        {
            var userString = surface.GetUserString("Ibra.MinPolynomialDegree");

            if (userString != null)
                return int.Parse(userString);

            if (Info.Instance.Settings.GetBool("EnableMinPolynomialDegree", false))
                return Info.Instance.Settings.GetInteger("MinPolynomialDegree", 1);

            return null;
        }

        static double? MaxElementSize(NurbsSurface surface)
        {
            var userString = surface.GetUserString("Ibra.MaxElementSize");

            if (userString != null)
                return double.Parse(userString);

            if (Info.Instance.Settings.GetBool("EnableMaxElementSize", false))
                return Info.Instance.Settings.GetDouble("MaxElementSize", 10.0);

            return null;
        }

        public override bool TryDump(RhinoObject obj, Model model, RhinoDoc document)
        {
            if (obj.ObjectType != ObjectType.Brep)
                return false;

            var brep = (Brep)obj.Geometry;

            if (!brep.MakeValidForV2())
                throw new Exception("Converting Brep to NURBS failed");

            var brepKey = obj.GetKey();

            for (int i = 0; i < brep.Curves2D.Count; i++)
            {
                var curve = (NurbsCurve)brep.Curves2D[i];

                var curveItem = new Item($"{brepKey}.NurbsCurveGeometry2D<{i}>", "NurbsCurveGeometry2D");
                DumpNurbsCurve2D(curveItem, curve);

                model.Items.Add(curveItem);
            }

            for (int i = 0; i < brep.Surfaces.Count; i++)
            {
                var surface = (NurbsSurface)brep.Surfaces[i];
                
                var surfaceItem = new Item($"{brepKey}.NurbsSurfaceGeometry3D<{i}>", "NurbsSurfaceGeometry3D");
                DumpNurbsSurface3D(surfaceItem, surface, MinPolynomialDegree(surface), MaxElementSize(surface));

                model.Items.Add(surfaceItem);
            }

            foreach (var face in brep.Faces)
            {
                var faceItem = new Item($"{brepKey}.BrepFace<{face.FaceIndex}>", "BrepFace");
                faceItem.Set("brep", brepKey);
                faceItem.Set("loops", face.Loops.Select(o => $"{brepKey}.BrepLoop<{o.LoopIndex}>"));
                faceItem.Set("geometry", $"{brepKey}.NurbsSurfaceGeometry3D<{face.SurfaceIndex}>");

                model.Items.Add(faceItem);
            }

            foreach (var loop in brep.Loops)
            {
                var loopItem = new Item($"{brepKey}.BrepLoop<{loop.LoopIndex}>", "BrepLoop");
                loopItem.Set("brep", brepKey);
                loopItem.Set("face", $"{brepKey}.BrepFace<{loop.Face.FaceIndex}>");
                loopItem.Set("trims", loop.Trims.Select(o => $"{brepKey}.BrepTrim<{o.TrimIndex}>"));

                model.Items.Add(loopItem);
            }

            foreach (var trim in brep.Trims)
            {
                var trimItem = new Item($"{brepKey}.BrepTrim<{trim.TrimIndex}>", "BrepTrim");
                trimItem.Set("brep", brepKey);
                trimItem.Set("loop", $"{brepKey}.BrepLoop<{trim.Loop.LoopIndex}>");
                if (trim.Edge != null)
                    trimItem.Set("Edge", $"{brepKey}.BrepEdge<{trim.Edge.EdgeIndex}>");
                trimItem.Set("geometry", $"{brepKey}.NurbsCurveGeometry2D<{trim.TrimCurveIndex}>");
                trimItem.Set("domain", trim.Domain);

                model.Items.Add(trimItem);
            }

            foreach (var edge in brep.Edges)
            {
                var edgeItem = new Item($"{brepKey}.BrepEdge<{edge.EdgeIndex}>", "BrepEdge");
                edgeItem.Set("brep", brepKey);
                edgeItem.Set("trims", edge.TrimIndices().Select(o => $"{brepKey}.BrepTrim<{o}>"));

                model.Items.Add(edgeItem);
            }

            var brepItem = new Item(brepKey, "Brep");
            brepItem.Set("curve_geometries_2d", brep.Curves2D.Select((o, i) => $"{brepKey}.NurbsCurveGeometry2D<{i}>"));
            brepItem.Set("surface_geometries_3d", brep.Surfaces.Select((o, i) => $"{brepKey}.NurbsSurfaceGeometry3D<{i}>"));
            brepItem.Set("faces", brep.Faces.Select(o => $"{brepKey}.BrepFace<{o.FaceIndex}>"));
            brepItem.Set("loops", brep.Loops.Select(o => $"{brepKey}.BrepLoop<{o.LoopIndex}>"));
            brepItem.Set("trims", brep.Trims.Select(o => $"{brepKey}.BrepTrim<{o.TrimIndex}>"));
            brepItem.Set("edges", brep.Edges.Select(o => $"{brepKey}.BrepEdge<{o.EdgeIndex}>"));

            model.Items.Add(brepItem);

            return true;
        }
    }
}
