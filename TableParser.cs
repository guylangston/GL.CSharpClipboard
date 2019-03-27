using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharpClipboard
{
    public class Column
    {
        public int Index { get; set; }
        public string Type { get; set; } = "string";
        public string Name { get; set; }
        public int MaxWidth { get; set; }
    }

    public class TableParser
    {
        private List<List<string>> table;
        private List<Column> cols;

        public static IEnumerable<string> SplitLines(string roughData)
        {
            if (!string.IsNullOrWhiteSpace(roughData))
            {
                var text = new StringReader(roughData);
                string line = null;
                while ((line = text.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public void ReadFromString(string text, bool inclHeader)
        {
            table = new List<List<string>>();
            foreach (var ln in SplitLines(text))
            {
                if (string.IsNullOrWhiteSpace(ln)) continue;

                if (inclHeader)
                {
                    if (cols == null)
                    {
                        cols = ln.Split('\t').WithIndex().Select(x => new Column()
                        {
                            Name = x.value.Trim(),
                            Index = x.index

                        }).ToList();
                    }
                    else
                    {
                        table.Add(ln.Split('\t').Select(x => x.Trim()).ToList());
                    }
                }
                else
                {
                    table.Add(ln.Split('\t').Select(x => x.Trim()).ToList());
                }

                
            }

            GetWidths();
            GuessTypes();
        }

        public IEnumerable<string> GetColWhereNotNullOrEmpty(int idx)
        {
            foreach (var ln in table)
            {
                if (idx < ln.Count && !string.IsNullOrWhiteSpace(ln[idx]))
                {
                    yield return ln[idx];
                }
            }
        }

        private void GuessTypes()
        {
            foreach (var column in cols)
            {
                if (GetColWhereNotNullOrEmpty(column.Index).All(c => double.TryParse(c, out _)))
                {
                    column.Type = "number";
                }
                else if (GetColWhereNotNullOrEmpty(column.Index).All(c => DateTime.TryParse(c, out _)))
                {
                    column.Type = "datetime";
                }
            }
        }

        private void GetWidths()
        {
            if (cols == null) this.cols = new List<Column>();
            foreach (var ln in table)
            {
                for (int cc = 0; cc < ln.Count; cc++)
                {
                    if (ln[cc] != null)
                    {
                        if (cc >= cols.Count) cols.Add(new Column()
                        {
                            Index = cc
                        });

                        if (ln[cc].Length > cols[cc].MaxWidth)
                        {
                            cols[cc].MaxWidth = ln[cc].Length;
                        }
                    }
                }

            }
        }


        public void FormatDict(TextWriter outp)
        {
            // Format
            outp.WriteLine($"var temp = new Dictionary<{cols[0].Type}, {cols[1].Type}>()");
            outp.WriteLine("{");

            var cellWidth = 20;
            foreach (var line in table)
            {
                outp.Write("\t{ ");
                int cc = 0;
                foreach (var cell in line)
                {
                    var cs = RenderValue(cols[cc], cell);
                    if (cc < line.Count-1)
                    {
                        cs += ",";
                    }
                    outp.Write(cs);
                    cc++;
                }

                outp.WriteLine(" },");
            }
            outp.WriteLine("};");

        }

        private string RenderValue(Column column, string cell)
        {
            if (column.Type == "number")
            {
                return cell.PadLeft(column.MaxWidth);
            }
            return $"\"{cell}\"".PadRight(column.MaxWidth+2);
        }
    }
}