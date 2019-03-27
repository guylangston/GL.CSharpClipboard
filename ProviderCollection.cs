using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSharpClipboard
{
    public class Provider
    {
        public string Verb { get; set;  }
        public string Example { get; set; }
        public string Help { get; set; }
        public Func<SourceTargetViewModel,string> Transform { get; set; }
    }


    public class ProviderCollection : List<Provider>
    {
        public ProviderCollection()
        {
            Add(null);
            Add(new Provider()
            {
                Verb = "FormatDict",
                Example = @"DataItem	Expected
SHLDRS_EQ	37436
SALES	64759.6
COS	45125
EBITDA	9771.41
EBIT	6331
INC_TAX	1223
FCF	4064.5
FCFPS	4.425
DPS	3.3
ASSETS	86370
ASSETS_CURR	41546
LIABS_CURR	23095
DEBT	17832
WKCAP	17058.5
SHLDRS_EQ	37436
NET_DEBT	18488.25
BPS	39.92",
                Help = @"Transform XLS (tab-delimited) two-column data into a C# dictionary",
                Transform = s =>
                {
                    var tbl = new TableParser();
                    tbl.ReadFromString(s.Source, s.IncludeHeaders);

                    var sb = new StringBuilder();
                    using (var tw = new StringWriter(sb))
                    {
                        tbl.FormatDict(tw);
                    }
                
                    return sb.ToString();
                }
            });

            Add(new Provider()
            {
                Verb = "ToUpper",
                Example = @"A few weeks ago, a friend convinved me to start looking into WPF, XAML and the MVVM pattern. As I've been primarily working with ASP.NET WebForms ...",
                Help = @"Simple ToUpper func",
                Transform = s => s.Source.ToUpper()
            });
        }
    }
}