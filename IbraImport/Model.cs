using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbraImport
{
    public class Model
    {
        private static List<Readers.Reader> s_readers;

        static Model()
        {
            s_readers = typeof(Model).Assembly.GetTypes()
                                              .Where(t => t.IsSubclassOf(typeof(Readers.Reader)))
                                              .Select(o => (Readers.Reader)Activator.CreateInstance(o))
                                              .ToList();
        }

        public static IEnumerable<Readers.Reader> Readers => s_readers;

        public int Version { get; private set; }

        private Dictionary<string, int> _keys;

        public List<JObject> Chuncks { get; private set; }

        public Model()
        {
            _keys = new Dictionary<string, int>();
            Chuncks = new List<JObject>();
        }

        public void Add(JObject chunk)
        {
            if (chunk.ContainsKey("key"))
            {
                var key = chunk["key"].Value<string>();
                var index = Chuncks.Count();
                _keys.Add(key, index);
            }

            Chuncks.Add(chunk);
        }

        public JObject this[string key]
        {
            get
            {
                var index = _keys[key];
                return Chuncks[index];
            }
        }

        public bool Contains(string key) => _keys.ContainsKey(key);

        public bool TryGetItem(string key, out JObject value)
        {
            if (_keys.TryGetValue(key, out int index))
            {
                value = Chuncks[index];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public static Model Load(TextReader textReader)
        {
            var model = new Model();

            using (var reader = new JsonTextReader(textReader))
            {
                var serializer = new JsonSerializer();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        var chunk = serializer.Deserialize<JObject>(reader);

                        model.Add(chunk);
                    }
                }
            }

            if (model.TryGetItem("Info", out var info))
            {
                if (info.TryGetValue<int>("Version", out var version))
                    model.Version = version;
            }

            return model;
        }

        public void LoadItems(RhinoDoc document)
        {
            foreach (var chunck in Chuncks)
            {
                foreach (var reader in s_readers)
                {
                    if (reader.IsActive && reader.TryLoad(chunck, this, document))
                        break;
                }
            }
        }
    }
}
