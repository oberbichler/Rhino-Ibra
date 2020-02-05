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
    class CurveReader : Reader
    {
        public override string Name => "CurveReader";

        public override bool TryLoad(JObject data, Model model, RhinoDoc document)
        {
            if (!data.HasType(out int index, "Curve2D", "Curve3D"))
                return false;

            var attributes = GetAttributes(document, data);

            if (index == 0) // 2D
            {
                var geometryKey = data["Geometry"].As<string>();

                var geometry = GetNurbsCurve2D(model[geometryKey]);

                document.Objects.AddCurve(geometry, attributes);
            }
            else            // 3D
            {
                var geometryKey = data["Geometry"].As<string>();

                var geometry = GetNurbsCurve3D(model[geometryKey]);

                document.Objects.AddCurve(geometry, attributes);
            }

            return true;
        }
    }
}
