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
    class NurbsSurfaceGeometryReader : Reader
    {
        public override string Name => "NurbsSurfaceGeometryReader";

        protected override bool IsActiveByDefault => false;

        public override bool TryLoad(JObject data, Model model, RhinoDoc document)
        {
            if (!data.HasType(out int index, "NurbsSurfaceGeometry2D", "NurbsSurfaceGeometry3D"))
                return false;

            var attributes = GetAttributes(document, data);

            if (index == 0) // 2D
            {
                var geometry = GetNurbsSurface2D(data);

                document.Objects.AddSurface(geometry, attributes);
            }
            else            // 3D
            {
                var geometry = GetNurbsSurface3D(data);

                document.Objects.AddSurface(geometry, attributes);
            }

            return true;
        }
    }
}
