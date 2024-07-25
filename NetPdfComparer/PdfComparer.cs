using Spire.Pdf;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;

namespace NetPdfComparer;

public static class PdfComparer
{
    [SupportedOSPlatform("windows")]
    public static bool ComparePdf(string FileName1, string FileName2, out int ErrorPixels, out int CorrectPixels, out Bitmap CompareMask)
    {
        // temp variables
        string PixelOriginal;
        string PixelToCompare;
        ErrorPixels = 0;
        CorrectPixels = 0;
        bool IsSamePdf = true;
        int Counter = 0;

        // Create Bitmaps
        using Bitmap BitmapOriginal = ReadBitmapFromPdf(FileName1);
        using Bitmap BitmapToCompare = ReadBitmapFromPdf(FileName2);
        // Compare Size
        if (BitmapOriginal.Width == BitmapToCompare.Width && BitmapOriginal.Height == BitmapToCompare.Height)
        {
            // Loop pixels - x
            for (int i = 0; i < BitmapOriginal.Width; i++)
            {
                // Loop pixels - y
                for (int j = 0; j < BitmapOriginal.Height; j++)
                {
                    PixelOriginal = BitmapOriginal.GetPixel(i, j).ToString();
                    PixelToCompare = BitmapToCompare.GetPixel(i, j).ToString();
                    // Compare Pixel by Pixel
                    if (PixelOriginal != PixelToCompare)
                    {
                        // Write into comparrison mask
                        BitmapOriginal.SetPixel(i, j, Color.Red);
                        ErrorPixels++;
                        IsSamePdf = false;
                    }
                    else
                    {
                        // Write into comparrison mask
                        CorrectPixels++;
                    }
                    Counter++;
                }
            }
        }
        CompareMask = (Bitmap)BitmapOriginal.Clone();
        return IsSamePdf;
    }

    [SupportedOSPlatform("windows")]
    private static Bitmap ReadBitmapFromPdf(string PdfFile)
    {
        using PdfDocument pdfDocument = new();
        pdfDocument.LoadFromFile(PdfFile);
        return (Bitmap)Image.FromStream(pdfDocument.SaveAsImage(0));
    }
}
