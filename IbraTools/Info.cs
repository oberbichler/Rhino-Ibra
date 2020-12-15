using Rhino.PlugIns;

namespace IbraExport
{
    public class Info : PlugIn
    {
        public Info()
        {
            Instance = this;
        }

        public static Info Instance { get; private set; }
    }
}
