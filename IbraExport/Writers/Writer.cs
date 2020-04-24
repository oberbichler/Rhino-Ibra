using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using System.Linq;

namespace IbraExport.Writers
{
    abstract class Writer
    {
        public abstract bool TryDump(RhinoObject obj, Model model, RhinoDoc document);

        protected void DumpNurbsCurve2D(Item item, NurbsCurve curve)
        {
            item.Set("degree", curve.Degree);
            item.Set("knots", curve.Knots);
            item.Set("nb_poles", curve.Points.Count);
            item.Set("poles", curve.Points.Select(o => o.Location.ToPoint2d()));

            if (curve.IsRational)
                item.Set("weights", curve.Points.Select(o => o.Weight));
        }

        protected void DumpNurbsCurve3D(Item item, NurbsCurve nurbsCurve)
        {
            item.Set("degree", nurbsCurve.Degree);
            item.Set("knots", nurbsCurve.Knots);
            item.Set("nb_poles", nurbsCurve.Points.Count);
            item.Set("poles", nurbsCurve.Points.Select(o => o.Location));

            if (nurbsCurve.IsRational)
                item.Set("weights", nurbsCurve.Points.Select(o => o.Weight));
        }

        protected void DumpNurbsSurface3D(Item item, NurbsSurface nurbsSurface)
        {
            if (Info.Instance.Settings.GetBool("EnableMinPolynomialDegree", false))
            {
                nurbsSurface = (NurbsSurface)nurbsSurface.Duplicate();

                var minPolynomialDegree = Info.Instance.Settings.GetInteger("MinPolynomialDegree", 1);

                nurbsSurface.IncreaseDegreeU(minPolynomialDegree);
                nurbsSurface.IncreaseDegreeV(minPolynomialDegree);
            }

            if (Info.Instance.Settings.GetBool("EnableMaxElementSize", false))
            {
                var maxElementSize = Info.Instance.Settings.GetDouble("MaxElementSize", 10.0);

                nurbsSurface = Utility.RefineSurface(nurbsSurface, maxElementSize);
            }

            item.Set("degree_u", nurbsSurface.Degree(0));
            item.Set("degree_v", nurbsSurface.Degree(1));
            item.Set("knots_u", nurbsSurface.KnotsU);
            item.Set("knots_v", nurbsSurface.KnotsV);
            item.Set("nb_poles_u", nurbsSurface.Points.CountU);
            item.Set("nb_poles_v", nurbsSurface.Points.CountV);
            item.Set("poles", nurbsSurface.Points.Select(o => o.Location));

            if (nurbsSurface.IsRational)
                item.Set("weights", nurbsSurface.Points.Select(o => o.Weight));
        }
    }
}
