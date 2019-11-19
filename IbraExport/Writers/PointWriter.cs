using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace IbraExport.Writers
{
    class PointWriter : Writer
    {
        public override bool TryDump(RhinoObject obj, Model model, RhinoDoc document)
        {
            if (obj.ObjectType != ObjectType.Point)
                return false;

            var point = (Point)obj.Geometry;

            var key = obj.GetKey();

            var item = new Item(key, "Point3D");

            item.Set("Location", point.Location);

            model.Items.Add(item);

            return true;
        }
    }
}
