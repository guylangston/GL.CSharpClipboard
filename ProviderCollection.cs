using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            Add(new Provider()
            {
                Verb = "ToStringList",
                Example = @"BPSAvgMinus15m
BPSAvgMinus3m
BPSMinus3m
BPSMinus3mToPrice
EPSSumMinus15m
EPSSumMinus3m
EPSSumPlus21m
EPS12mFwdDayGradient
EPS24mFwdDayGradient
EPS24mFwd90DayGradient
EPSFy3Fwd90DayGradient
EPS3yrAnnYield
EPSFy2Fwd30DayGradient
ROE5yrVol
ROEGrowthMinus15mToPlus21m
ROEtoMinus3m
ChangeInShares
LeverageDtoE
PayoutRatio
R1
V1
G3",
                Help = @"Convert a list of strings to a C# array",
                Transform = s =>
                {
                    var sb = new StringBuilder();
                    using (var tw = new StringWriter(sb))
                    {
                        tw.WriteLine("var temp = new [] {");
                        using (var tr = new StringReader(s.Source))
                        {
                            string line = null;
                            while ((line = tr.ReadLine()) != null)
                            {
                                tw.WriteLine($"\t\"{line}\",");
                            }
                        }
                        tw.WriteLine("};");

                    }

                    return sb.ToString();
                }
            });


            Add(new Provider()
            {
                Verb = "ColumnHeaderClass",
                Example = @"Alias	Name	Group	Comments	Target.A	TargetMethod.A	P1.A	P2.A	OperatorN	Target.B	TargetMethod.B	P1.B	P2.B	OVER	Target.C	TargetMethod.C	P1.C	P2.C	OperatorD	Target.D	TargetMethod.D	P1.D	P2.D",
                Help = @"Paste an excel row to create column prop class",
                Transform = ColumnHeaderClass
            });


            Add(new Provider()
            {
                Verb = "ClassFromStrings",
                Example = @" Sector	
Status	
CompanyName	
Industry
Sedol
VATicker
BloombergTicker	",
                Help = @"Scaffold a class from a list props",
                Transform = s =>
                {
                    var sb = new StringBuilder();
                    using (var tw = new StringWriter(sb))
                    {
                        
                        using (var tr = new StringReader(s.Source))
                        {
                            string line = null;
                            while ((line = tr.ReadLine()) != null)
                            {
                                line = line.Trim();
                                tw.WriteLine($"\tpublic string {line} {{ get; set; }}");
                            }
                        }
                        

                    }

                    return sb.ToString();
                }
            });


        }

        private string ColumnHeaderClass(SourceTargetViewModel arg)
        {
            var cols = arg.Source.Split('\t');

            return cols
                .Select(x => $"\t[DisplayName(\"{x}\")]".PadRight(40) + $"\tstring {x.Replace('.', '_')} {{ get; set; }}")
                .ToStringConcat(x => x, "\n");
        }
    }


}