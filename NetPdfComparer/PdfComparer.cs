using Spire.Pdf;
using System.Drawing;

namespace NetPdfComparer
{
    public class PdfComparer
    {
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
            Bitmap BitmapOriginal = ReadBitmapFromPdf(FileName1);
            Bitmap BitmapToCompare = ReadBitmapFromPdf(FileName2);
            CompareMask = new Bitmap(BitmapOriginal.Width, BitmapOriginal.Height);

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
                            CompareMask.SetPixel(i, j, Color.Red);
                            ErrorPixels++;
                            IsSamePdf = false;
                        }
                        else
                        {
                            // Write into comparrison mask
                            CompareMask.SetPixel(i, j, Color.White);
                            CorrectPixels++;
                        }
                        Counter++;
                    }
                }
            }
            return IsSamePdf;
        }

        private static Bitmap ReadBitmapFromPdf(string PdfFile)
        {
            PdfDocument pdfDocument = new PdfDocument();
            pdfDocument.LoadFromFile(PdfFile);
            return (Bitmap)pdfDocument.SaveAsImage(0);
        }
    }
}
