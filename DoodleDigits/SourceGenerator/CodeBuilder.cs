using System.Text;

namespace SourceGenerator {
    class CodeBuilder {
        private readonly StringBuilder builder;
        private int indent;

        public CodeBuilder() {
            builder = new StringBuilder();
            indent = 0;
        }

        public void AddLine(string line) {
            if (line.Contains("\n")) {
                foreach (string str in line.Split('\n')) {
                    this.AddLine(str);
                }
                return;
            }
            builder.AppendLine(new string('\t', indent) + line);
        }

        public void AddLines(params string[] lines) {
            foreach (string line in lines) {
                builder.AppendLine(new string('\t', indent) + line);
            }
        }

        public void Indent() {
            indent++;
        }

        public void Unindent() {
            indent--;
        }

        public override string ToString() {
            return builder.ToString();
        }
    }
}
