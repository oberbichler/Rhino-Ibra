using Rhino.DocObjects;
using Rhino.Geometry;
using System;

namespace IbraExport
{
    static class Utility
    {
        public static string GetKey(this RhinoObject obj)
        {
            var key = obj.Attributes.Name;

            if (string.IsNullOrWhiteSpace(key))
                key = $"rhino<{obj.Id}>";

            return key;
        }

        public static NurbsSurface RefineSurface(NurbsSurface nurbsSurface, double maxElementSize)
        {
            var refindeNurbsSurface = (NurbsSurface)nurbsSurface.Duplicate();

            var isoU = refindeNurbsSurface.IsoCurve(1, refindeNurbsSurface.Domain(0).ParameterAt(0.5));
            var spansU = refindeNurbsSurface.GetSpanVector(1);

            for (int i = 1; i < spansU.Length; i++)
            {
                var t0 = spansU[i - 1];
                var t1 = spansU[i];

                var isoSpan = isoU.Trim(t0, t1);

                var length = isoSpan.GetLength();

                var segment = length / Math.Ceiling(length / maxElementSize);

                var ts = isoSpan.DivideByLength(segment, false);

                foreach (var t in ts)
                {
                    if (refindeNurbsSurface.ClosestPoint(isoSpan.PointAt(t), out var u, out var v))
                        refindeNurbsSurface.KnotsV.InsertKnot(v);
                }
            }

            var isoV = refindeNurbsSurface.IsoCurve(0, refindeNurbsSurface.Domain(1).ParameterAt(0.5));
            var spansV = refindeNurbsSurface.GetSpanVector(0);

            for (int i = 1; i < spansV.Length; i++)
            {
                var t0 = spansV[i - 1];
                var t1 = spansV[i];

                var isoSpan = isoV.Trim(t0, t1);

                var length = isoSpan.GetLength();

                var segment = length / Math.Ceiling(length / maxElementSize);

                var ts = isoSpan.DivideByLength(segment, false);

                foreach (var t in ts)
                {
                    if (refindeNurbsSurface.ClosestPoint(isoSpan.PointAt(t), out var u, out var v))
                        refindeNurbsSurface.KnotsU.InsertKnot(u);
                }
            }

            return refindeNurbsSurface;
        }

        public static Point2d ToPoint2d(this Point3d point)
        {
            return new Point2d(point.X, point.Y);
        }
    }
}
