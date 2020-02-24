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

            var item = new Item(key, "point_3d");

            item.Set("location", point.Location);

            model.Items.Add(item);

            return true;
        }
    }
}
