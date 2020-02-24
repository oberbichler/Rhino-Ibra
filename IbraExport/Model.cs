using Newtonsoft.Json;
using Rhino.DocObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbraExport
{
    class Model
    {
        static List<Writers.Writer> s_writers;

        static Model()
        {
            s_writers = typeof(Model).Assembly.GetTypes()
                                              .Where(t => t.IsSubclassOf(typeof(Writers.Writer)))
                                              .Select(o => (Writers.Writer)Activator.CreateInstance(o))
                                              .ToList();
        }

        public List<Item> Items { get; private set; } = new List<Item>();

        public void Add(IEnumerable<RhinoObject> objs)
        {
            foreach (var obj in objs)
            {
                foreach (var writer in s_writers)
                {
                    if (writer.TryDump(obj, this, obj.Document))
                        break;
                }
            }
        }

        void Write(JsonTextWriter writer, string name, object value)
        {
            writer.WritePropertyName(name);
            writer.WriteRawValue(JsonConvert.SerializeObject(value));
        }

        public void Save(TextWriter textWriter)
        {
            using (var writer = new JsonTextWriter(textWriter))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartArray();

                var serializer = new JsonSerializer();

                foreach (var item in Items)
                {
                    writer.WriteStartObject();

                    Write(writer, "key", item.Key);
                    Write(writer, "type", item.Type);

                    foreach (var property in item)
                        Write(writer, property.Key, property.Value);

                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
            }
        }
    }
}
