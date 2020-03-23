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
    class BoxReader : Reader
    {
        public override string Name => "BoxReader";

        public override bool TryLoad(JObject data, Model model, RhinoDoc document)
        {
            if (!data.HasType(out int index, "Box2D", "Box3D"))
                return false;

            var attributes = GetAttributes(document, data);

            if (index == 0) // 2D
            {
                var a = data["min"].AsPoint2d();
                var b = data["max"].AsPoint2d();

                var rectangle = new Rectangle3d(Plane.WorldXY, a.ToPoint3d(), b.ToPoint3d());

                document.Objects.AddRectangle(rectangle, attributes);
            }
            else            // 3D
            {
                var a = data["min"].AsPoint3d();
                var b = data["max"].AsPoint3d();

                var bbox = new BoundingBox(a, b);
                var box = new Box(bbox);

                document.Objects.AddBox(box, attributes);
            }

            return true;
        }
    }
}
