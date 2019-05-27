using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Spire.Pdf;
using System.Drawing.Imaging;
using iTextSharp.text.pdf;
using System.Drawing.Drawing2D;

namespace ReadPdf
{
    public class ReadImageFromPDF
    {
        public static List<System.Drawing.Image> ExtractImages(String PDFSourcePath)
        {
            List<System.Drawing.Image> ImgList = new List<System.Drawing.Image>();

            iTextSharp.text.pdf.RandomAccessFileOrArray RAFObj = null;
            iTextSharp.text.pdf.PdfReader PDFReaderObj = null;
            iTextSharp.text.pdf.PdfObject PDFObj = null;
            iTextSharp.text.pdf.PdfStream PDFStremObj = null;

            try
            {
                RAFObj = new iTextSharp.text.pdf.RandomAccessFileOrArray(PDFSourcePath);
                PDFReaderObj = new iTextSharp.text.pdf.PdfReader(RAFObj, null);

                for (int i = 0; i <= PDFReaderObj.XrefSize - 1; i++)
                {
                    PDFObj = PDFReaderObj.GetPdfObject(i);

                    if ((PDFObj != null) && PDFObj.IsStream())
                    {
                        PDFStremObj = (iTextSharp.text.pdf.PdfStream)PDFObj;
                        iTextSharp.text.pdf.PdfObject subtype = PDFStremObj.Get(iTextSharp.text.pdf.PdfName.SUBTYPE);

                        if ((subtype != null) && subtype.ToString() == iTextSharp.text.pdf.PdfName.IMAGE.ToString())
                        {
                            try
                            {
                                iTextSharp.text.pdf.parser.PdfImageObject PdfImageObj =
                         new iTextSharp.text.pdf.parser.PdfImageObject((iTextSharp.text.pdf.PRStream)PDFStremObj);

                                System.Drawing.Image ImgPDF = PdfImageObj.GetDrawingImage();

                                ImgList.Add(ImgPDF);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
                PDFReaderObj.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ImgList;
        }

        public static string WriteImageFile(string strPath)
        {
            try
            {
                // Get a List of Image
                List<System.Drawing.Image> ListImage = ExtractImages(strPath);
                string strImagePath = string.Empty;

                if (ListImage.Count == 1)
                {
                    ListImage[0].Save(System.IO.Path.GetDirectoryName(strPath) + "\\Image" + 0 + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    return GetImage(strPath);
                }

                for (int i = 0; i < ListImage.Count; i++)
                {
                    try
                    {
                        // Write Image File
                        ListImage[i].Save(System.IO.Path.GetDirectoryName(strPath) + "\\Image" + i + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);

                        if (i % 2 != 0)
                        {
                            strImagePath = MergeImages(strPath, i);
                        }


                        if (i == 1)
                            break;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                return strImagePath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string MergeImages(string strPath, int i)
        {
            try
            {
                int j = i - 1;

                String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");

                string jpg1 = System.IO.Path.GetDirectoryName(strPath) + "\\Image" + j + ".jpeg";
                string jpg2 = System.IO.Path.GetDirectoryName(strPath) + "\\Image" + i + ".jpeg";
                string jpg3 = System.IO.Path.GetDirectoryName(strPath) + "\\Cheque-" + timeStamp + ".jpeg";

                Image img1 = Image.FromFile(jpg1);
                Image img2 = Image.FromFile(jpg2);

                int width = img1.Width + img2.Width;
                int height = Math.Max(img1.Height, img2.Height);

                Bitmap img3 = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(img3);

                g.Clear(Color.Black);
                g.DrawImage(img1, new Point(0, 0));
                g.DrawImage(img2, new Point(img1.Width, 0));

                g.Dispose();
                img1.Dispose();
                img2.Dispose();

                img3.Save(jpg3, System.Drawing.Imaging.ImageFormat.Jpeg);
                img3.Dispose();

                File.Delete(jpg1);
                File.Delete(jpg2);

                return jpg3;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetImage(string strPath)
        {
            try
            {
                string timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");

                string strInitialImage = System.IO.Path.GetDirectoryName(strPath) + "\\Image" + 0 + ".jpeg";
                string strTempImg = System.IO.Path.GetDirectoryName(strPath) + "\\Cheque.jpeg";
                string strFinalImg = System.IO.Path.GetDirectoryName(strPath) + "\\Cheque-" + timeStamp + ".jpeg";

                using (Bitmap bitmap = (Bitmap)Image.FromFile(strInitialImage))
                {
                    using (Bitmap newBitmap = new Bitmap(bitmap))
                    {
                        //Divide the PDF in two parts.Upper part will have cheque image.
                        Bitmap originalImage = new Bitmap(Image.FromFile(strInitialImage));
                        Rectangle rect = new Rectangle(0, 0, originalImage.Width, originalImage.Height / 4);
                        Bitmap imgFirstHalf = originalImage.Clone(rect, originalImage.PixelFormat);
                        imgFirstHalf.Save(strTempImg);

                        //Resize the image so that API will be able to read it.
                        Bitmap imgFinal = null;
                        Graphics objGraphic = null;

                        int reqW = Convert.ToInt32((imgFirstHalf.Width)*0.4);
                        int reqH = Convert.ToInt32((imgFirstHalf.Height)*0.4);

                        imgFinal = new Bitmap(reqW, reqH);
                        objGraphic = Graphics.FromImage(imgFinal);
                        objGraphic.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(0, 0, reqW, reqH));
                        objGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic; /* new way */

                        objGraphic.DrawImage(imgFirstHalf, 0, 0, reqW, reqH);
                        if (objGraphic != null) objGraphic.Dispose();

                        imgFinal.Save(strFinalImg);

                        if (originalImage != null) originalImage.Dispose();
                        if (imgFirstHalf != null) imgFirstHalf.Dispose();
                        if (imgFinal != null) imgFinal.Dispose();
                    }
                }
                return strFinalImg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
