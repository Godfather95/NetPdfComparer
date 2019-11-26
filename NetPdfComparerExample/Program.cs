using NetPdfComparer;
using System;
using System.Drawing;

namespace NetPdfComparerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            bool Result = PdfComparer.ComparePdf(args[0], args[1], out int nErrors, out int nCorrects, out Bitmap CompareMask);
            if (Result)
            {
                Console.WriteLine("PDF-Files are equal!");
            }
            else
            {
                Console.WriteLine($"PDF-Files are not equal! Correct pixels: {nCorrects} | Incorrect pixels: {nErrors}");
            }
            CompareMask.Save(args[2]);
            Console.WriteLine($"Comparison-Mask saved at {args[2]}");
            Console.ReadLine();
        }
    }
}
