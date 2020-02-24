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
    class NurbsCurveGeometryReader : Reader
    {
        public override string Name => "NurbsCurveGeometryReader";

        protected override bool IsActiveByDefault => false;

        public override bool TryLoad(JObject data, Model model, RhinoDoc document)
        {
            if (!data.HasType(out int index, "nurbs_curve_geometry_2d", "nurbs_curve_geometry_3d"))
                return false;

            var attributes = GetAttributes(document, data);

            if (index == 0) // 2D
            {
                var geometry = GetNurbsCurve2D(data);

                document.Objects.AddCurve(geometry, attributes);
            }
            else            // 3D
            {
                var geometry = GetNurbsCurve3D(data);

                document.Objects.AddCurve(geometry, attributes);
            }

            return true;
        }
    }
}
