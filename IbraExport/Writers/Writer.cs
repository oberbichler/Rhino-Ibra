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
            item.Set("Degree", curve.Degree);
            item.Set("Knots", curve.Knots);
            item.Set("NbPoles", curve.Points.Count);
            item.Set("Poles", curve.Points.Select(o => o.Location));

            if (curve.IsRational)
                item.Set("Weights", curve.Points.Select(o => o.Weight));
        }

        protected void DumpNurbsCurve3D(Item item, NurbsCurve nurbsCurve)
        {
            item.Set("Degree", nurbsCurve.Degree);
            item.Set("Knots", nurbsCurve.Knots);
            item.Set("NbPoles", nurbsCurve.Points.Count);
            item.Set("Poles", nurbsCurve.Points.Select(o => o.Location));

            if (nurbsCurve.IsRational)
                item.Set("Weights", nurbsCurve.Points.Select(o => o.Weight));
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

            item.Set("DegreeU", nurbsSurface.Degree(0));
            item.Set("DegreeV", nurbsSurface.Degree(1));
            item.Set("KnotsU", nurbsSurface.KnotsU);
            item.Set("KnotsV", nurbsSurface.KnotsV);
            item.Set("NbPolesU", nurbsSurface.Points.CountU);
            item.Set("NbPolesV", nurbsSurface.Points.CountV);
            item.Set("Poles", nurbsSurface.Points.Select(o => o.Location));

            if (nurbsSurface.IsRational)
                item.Set("Weights", nurbsSurface.Points.Select(o => o.Weight));
        }
    }
}
