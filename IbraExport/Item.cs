using Rhino.Geometry;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace IbraExport
{
    public struct Item : IEnumerable<KeyValuePair<string, object>>
    {
        public Item(string key, string type)
        {
            Key = key;
            Type = type;

            m_property = new Dictionary<string, object>();
        }

        public string Key { get; private set; }

        public string Type { get; private set; }

        private Dictionary<string, object> m_property;

        public object this[string key]
        {
            get { return m_property[key]; }
            set { m_property[key] = value; }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return m_property.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_property.GetEnumerator();
        }

        public void Set(string key, int value)
        {
            m_property[key] = value;
        }

        public void Set(string key, double value)
        {
            m_property[key] = value;
        }

        public void Set(string key, string value)
        {
            m_property[key] = value;
        }

        public void Set(string key, IEnumerable<int> values)
        {
            m_property[key] = values.ToArray();
        }

        public void Set(string key, IEnumerable<double> values)
        {
            m_property[key] = values.ToArray();
        }

        public void Set(string key, IEnumerable<string> values)
        {
            m_property[key] = values.ToArray();
        }

        public void Set(string key, IEnumerable<Point2d> values)
        {
            m_property[key] = values.Select(o => new[] { o.X, o.Y }).ToArray();
        }

        public void Set(string key, IEnumerable<Point3d> values)
        {
            m_property[key] = values.Select(o => new[] { o.X, o.Y, o.Z }).ToArray();
        }

        public void Set(string key, Point3d value)
        {
            m_property[key] = new double[] { value.X, value.Y, value.Z };
        }

        public void Set(string key, Interval interval)
        {
            m_property[key] = new[] { interval.T0, interval.T1 };
        }
    }
}
