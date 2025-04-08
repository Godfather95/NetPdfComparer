using Spire.Additions.Xps.Schema;
using Spire.Pdf;
using Spire.Pdf.Fields;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;

namespace NetPdfComparer;

public static class PdfComparer
{
    [SupportedOSPlatform("windows")]
    public static bool ComparePdf(string FileName1, string FileName2, out int ErrorPixels, out int CorrectPixels, out IList<Tuple<Bitmap, bool>> CompareMasks)
    {
        // temp variables
        CompareMasks = [];
        string PixelOriginal;
        string PixelToCompare;
        ErrorPixels = 0;
        CorrectPixels = 0;
        bool IsSamePdf = true;
        int Counter = 0;

        // PDFs öffnen
        using PdfDocument pdfDocument1 = new();
        pdfDocument1.LoadFromFile(FileName1);

        using PdfDocument pdfDocument2 = new();
        pdfDocument2.LoadFromFile(FileName2);

        int PageCount = GetPageCount(pdfDocument1);

        // Create Bitmaps
        if (GetPageCount(pdfDocument1) != GetPageCount(pdfDocument2))
        {
            throw new Exception("PDFs have different number of pages");
        }

        for (int PageIndex = 0; PageIndex < PageCount; PageIndex++)
        {
            using Bitmap BitmapOriginal = ReadBitmapFromPdf(pdfDocument1, PageIndex);
            using Bitmap BitmapToCompare = ReadBitmapFromPdf(pdfDocument2, PageIndex);
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
            CompareMasks.Add(new((Bitmap)BitmapOriginal.Clone(), IsSamePdf));
        }

        return IsSamePdf;
    }

    [SupportedOSPlatform("windows")]
    private static Bitmap ReadBitmapFromPdf(PdfDocument pdfDocument, int PageIndex)
    {
        Stream imageStream = pdfDocument.SaveAsImage(PageIndex);
        return (Bitmap)Image.FromStream(imageStream);
    }

    [SupportedOSPlatform("windows")]
    private static int GetPageCount(PdfDocument pdfDocument)
    {
        return pdfDocument.Pages.Count;
    }
}
