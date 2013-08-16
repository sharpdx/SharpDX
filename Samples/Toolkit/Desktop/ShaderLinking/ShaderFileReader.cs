namespace ShaderLinking
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal sealed class ShaderFileReader
    {
        private sealed class SectionInfo
        {
            private readonly string _begin;
            private readonly string _end;
            private readonly Action<string> _setter;

            public SectionInfo(string begin, string end, Action<string> setter)
            {
                _begin = begin;
                _end = end;
                _setter = setter;
            }

            public bool IsMatch(string line)
            {
                return line.StartsWith(_begin);
            }

            public void Append(TextReader reader)
            {
                var sb = new StringBuilder();
                string line;
                while((line = reader.ReadLine()) != null && !line.StartsWith(_end))
                    sb.AppendLine(line);

                _setter(sb.ToString());
            }
        }

        private readonly List<SectionInfo> _sections = new List<SectionInfo>();

        public ShaderFileReader(string filePath)
        {
            _sections.Add(new SectionInfo(@"// @@@ Begin Header", @"// @@@ End Header", x => HeaderText = x));
            _sections.Add(new SectionInfo(@"// @@@ Begin Source", @"// @@@ End Source", x => SourceText = x));
            _sections.Add(new SectionInfo(@"// @@@ Begin Hidden", @"// @@@ End Hidden", x => HiddenText = x));

            ReadAndParseFile(filePath);
        }

        public string HeaderText { get; private set; }
        public string SourceText { get; private set; }
        public string HiddenText { get; private set; }

        private void ReadAndParseFile(string filePath)
        {
            var sections = _sections.ToList();

            using (var stream = File.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (sections.Count == 0) break;

                    var section = sections.FirstOrDefault(x => x.IsMatch(line));
                    if (section == null) continue;

                    sections.Remove(section);

                    section.Append(reader);
                }
            }
        }
    }
}