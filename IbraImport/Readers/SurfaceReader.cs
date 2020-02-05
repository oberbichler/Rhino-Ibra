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
    class SurfaceReader : Reader
    {
        public override string Name => "SurfaceReader";

        public override bool TryLoad(JObject data, Model model, RhinoDoc document)
        {
            if (!data.HasType(out int index, "Surface2D", "Surface3D"))
                return false;

            var attributes = GetAttributes(document, data);

            if (index == 0) // 2D
            {
                var geometryKey = data["Geometry"].As<string>();

                var geometry = GetNurbsSurface2D(model[geometryKey]);

                document.Objects.AddSurface(geometry, attributes);
            }
            else            // 3D
            {
                var geometryKey = data["Geometry"].As<string>();

                var geometry = GetNurbsSurface3D(model[geometryKey]);

                document.Objects.AddSurface(geometry, attributes);
            }

            return true;
        }
    }
}
