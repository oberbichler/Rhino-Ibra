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
    class PointReader : Reader
    {
        public override bool TryLoad(JObject data, Model model, RhinoDoc document)
        {
            if (!data.HasType(out int index, "Point2D", "Point3D"))
                return false;

            var attributes = GetAttributes(document, data);

            Point3d location;

            if (index == 0) // 2D
                location = data["Location"].AsPoint2d().ToPoint3d();
            else            // 3D
                location = data["Location"].AsPoint3d();

            if (data.TryGetValue<string>("Text", out var text))
                document.Objects.AddTextDot(text, location, attributes);
            else
                document.Objects.AddPoint(location, attributes);

            return true;
        }
    }
}
