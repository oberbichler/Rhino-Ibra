using Newtonsoft.Json.Linq;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbraImport
{
    internal static class Utility
    {
        public static T As<T>(this JToken obj)
        {
            var value = ((JValue)obj)?.Value;

            if (value is T)
                return (T)value;
            else
                return default;
        }

        public static List<T> AsList<T>(this JToken obj)
        {
            var values = ((JArray)obj)?.Values();

            if (values != null)
                return values.Select(o => o.Value<T>()).ToList();
            else
                return default;
        }

        public static Point2d AsPoint2d(this JToken obj)
        {
            var xy = obj.AsList<double>();

            return new Point2d(xy[0], xy[1]);
        }

        public static Point3d AsPoint3d(this JToken obj)
        {
            var xyz = obj.AsList<double>();

            return new Point3d(xyz[0], xyz[1], xyz[2]);
        }

        public static List<Point2d> AsListOfPoint2d(this JToken obj)
        {
            var values = ((JArray)obj)?.Values<JArray>();

            if (values != null)
                return values.Select(o => o.AsPoint2d()).ToList();
            else
                return default;
        }

        public static List<Point3d> AsListOfPoint3d(this JToken obj)
        {
            var values = ((JArray)obj)?.Values<JArray>();

            if (values != null)
                return values.Select(o => o.AsPoint3d()).ToList();
            else
                return default;
        }

        public static bool TryGetValue<T>(this JObject obj, string key, out T value)
        {
            if (obj.TryGetValue(key, out var token))
            {
                value = token.As<T>();
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
        
        public static bool HasType(this JObject obj, string type)
        {
            if (obj.TryGetValue<string>("Type", out var value))
                return value == type;
            else
                throw new Exception("Type attribute is missing");
        }

        public static bool HasType(this JObject obj, out int index, params string[] types)
        {
            if (obj.TryGetValue<string>("Type", out var value))
            {
                index = Array.IndexOf(types, value);
                return index >= 0;
            }
            else
                throw new Exception("Type attribute is missing");
        }
        
        public static Point3d ToPoint3d(this Point2d xy)
        {
            return new Point3d(xy[0], xy[1], 0.0);
        }
    }
}
