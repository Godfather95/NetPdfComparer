using CommandLine;
using NetPdfComparer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;

namespace NetPdfComparerExample;

static class Program
{
    public class Options
    {
        [Option('o', "original", Required = false, HelpText = "Original PDF File")]
        public string Original { get; set; }

        [Option('c', "compare", Required = false, HelpText = "PDF to compare")]
        public string Compare { get; set; }

        [Option('m', "mask", Required = false, HelpText = "Path to output comparison mask")]
        public string Mask { get; set; }

        [Option('r', "result", Required = false, HelpText = "parsable result file")]
        public string Result { get; set; }
    }

    [SupportedOSPlatform("windows")]
    static void Main(string[] args) => _ = Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
                                       {
                                           bool Result = PdfComparer.ComparePdf(o.Original, o.Compare, out int nErrors, out int nCorrects, out IList<Tuple<Bitmap, bool>> CompareMask);
                                           string ResultFile = string.Empty;
                                           if (Result)
                                           {
                                               Console.WriteLine("PDF-Files are equal!");
                                               ResultFile += "{\"result\": \"equal\",";
                                           }
                                           else
                                           {
                                               Console.WriteLine($"PDF-Files are not equal! Correct pixels: {nCorrects} | Incorrect pixels: {nErrors}");
                                               ResultFile += "{\"result\": \"not equal\",";
                                           }
                                           Directory.CreateDirectory(o.Mask);
                                           for (int i = 0; i < CompareMask.Count; i++)
                                           {
                                               Bitmap maskBitmap = CompareMask[i].Item1;
                                               maskBitmap.Save(Path.Combine(o.Mask, $"mask{i}{(CompareMask[i].Item2 ? "" : "_diff")}.bmp"));
                                           }
                                           ResultFile += "\"masks\": \"" + o.Mask.Replace("\\", "\\\\") + "\"}";
                                           Console.WriteLine($"Comparison-Mask saved at {o.Mask}");
                                           File.WriteAllText(o.Result, ResultFile);
                                       });
}
