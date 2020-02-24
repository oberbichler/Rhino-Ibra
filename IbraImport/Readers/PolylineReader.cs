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
    class PolylineReader : Reader
    {
        public override string Name => "PolylineReader";

        public override bool TryLoad(JObject data, Model model, RhinoDoc document)
        {
            if (!data.HasType(out int index, "polyline_2d", "polyline_3d"))
                return false;

            var attributes = GetAttributes(document, data);

            List<Point3d> points;

            if (index == 0) // 2D
            {
                points = data["points"].AsListOfPoint2d()
                                       .Select(o => o.ToPoint3d())
                                       .ToList();
            }
            else            // 3D
            {
                points = data["points"].AsListOfPoint3d();
            }

            var id = document.Objects.AddPolyline(points, attributes);

            return true;
        }
    }
}
