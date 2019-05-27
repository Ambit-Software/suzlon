using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ReadPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string path = ReadImageFromPDF.WriteImageFile(@"E:\Projects\Suzlon\Read Pdf code\Pdfs\209516  PARAMTRONICS.pdf"); // write image file  
                string strTest = path;
                string str = path;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

       
    }
}
