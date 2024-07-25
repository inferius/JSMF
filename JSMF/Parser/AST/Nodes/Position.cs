using System.IO;

namespace JSMF.Parser.AST.Nodes
{
    public struct Position
    {
        private static long ContentCountLoaded = 0;
        public string Id { get; private set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public FileInfo FileName { get; set; }

        public bool IsDynamicScript { get; set; }

        public string DynamicScriptContent { get; set; }

        public Position(string filePath, string dynamicContent = null, int line = 0, int column = 0)
        {
            Line = line;
            Column = column;
            if (string.IsNullOrWhiteSpace(filePath) || !string.IsNullOrWhiteSpace(dynamicContent))
            {
                FileName = null;
                Id = $"VTDS{ContentCountLoaded++}.js";
                DynamicScriptContent = dynamicContent;
                IsDynamicScript = true;
            }
            else
            {
                FileName = new FileInfo(filePath);
                IsDynamicScript = false;
                DynamicScriptContent = string.Empty;
                Id = FileName.FullName;
            }
        }

        public override string ToString()
        {
            return $"{FileName?.Name ?? Id}:{Line}:{Column}";
        }
    }
}
