using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OCRAPI
{
    public partial class Form1 : Form
    {

        public string ImagePath { get; set; }
        public string PdfPath { get; set; }

        public Form1()
        {
            InitializeComponent();
            cmbLanguage.SelectedIndex = 5;//English
        }

        private string getSelectedLanguage()
        {

            //https://ocr.space/OCRAPI#PostParameters

            //Czech = cze; Danish = dan; Dutch = dut; English = eng; Finnish = fin; French = fre; 
            //German = ger; Hungarian = hun; Italian = ita; Norwegian = nor; Polish = pol; Portuguese = por;
            //Spanish = spa; Swedish = swe; ChineseSimplified = chs; Greek = gre; Japanese = jpn; Russian = rus;
            //Turkish = tur; ChineseTraditional = cht; Korean = kor

            string strLang = "";
            switch (cmbLanguage.SelectedIndex)
            {
                case 0:
                    strLang = "chs";
                    break;

                case 1:
                    strLang = "cht";
                    break;
                case 2:
                    strLang = "cze";
                    break;
                case 3:
                    strLang = "dan";
                    break;
                case 4:
                    strLang = "dut";
                    break;
                case 5:
                    strLang = "eng";
                    break;
                case 6:
                    strLang = "fin";
                    break;
                case 7:
                    strLang = "fre";
                    break;
                case 8:
                    strLang = "ger";
                    break;
                case 9:
                    strLang = "gre";
                    break;
                case 10:
                    strLang = "hun";
                    break;
                case 11:
                    strLang = "jap";
                    break;
                case 12:
                    strLang = "kor";
                    break;
                case 13:
                    strLang = "nor";
                    break;
                case 14:
                    strLang = "pol";
                    break;
                case 15:
                    strLang = "por";
                    break;
                case 16:
                    strLang = "spa";
                    break;
                case 17:
                    strLang = "swe";
                    break;
                case 18:
                    strLang = "tur";
                    break;

            }
            return strLang;

        }

       private void button1_Click(object sender, EventArgs e)
        {
            PdfPath = ImagePath = ""; pictureBox.BackgroundImage = null;
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "jpeg files|*.jpg;*.JPG";
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(fileDlg.FileName);
                if (fileInfo.Length > 5* 1024 * 1024)
                {
                    //Size limit depends: Free API 1 MB, PRO API 5 MB and more
                    MessageBox.Show("Image file size limit reached (1MB free API)");
                    return;
                }
                pictureBox.BackgroundImage = Image.FromFile(fileDlg.FileName);
                ImagePath = fileDlg.FileName;
                lblInfo.Text = "Image loaded: "+ fileInfo.Name;
                lblInfo.BackColor = Color.LightGreen;
            }
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            PdfPath = ImagePath = "";
            pictureBox.BackgroundImage = null;
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "pdf files|*.pdf;";
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(fileDlg.FileName);
                if (fileInfo.Length > 5* 1024 * 1024 )
                {
                    //Size limit depends: Free API 1 MB, PRO API 5 MB and more
                    MessageBox.Show("PDF file size should not be larger than 5Mb");
                    return;
                }
                PdfPath = fileDlg.FileName;
                //PDF files are loaded, but can not be displayed in the image control. That does not affect the OCR.
                lblInfo.Text = "PDF loaded [but not displayed]: " + fileInfo.Name;
                lblInfo.BackColor = Color.LightSalmon;
            }
        }

        private byte[] ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                return imageBytes;
            }
        }

        public async void button2_Click(object sender, EventArgs e)
        {
            GetBankDetails(ImagePath);
        }


        public async Task<string> GetBankDetails(string ImagePath)
        {
            string sBankDetails = string.Empty;

            if (string.IsNullOrEmpty(ImagePath))
                return "File path is empty";

            //txtResult.Text = "";

            //button1.Enabled = false;
            //button2.Enabled = false;
            //btnPDF.Enabled = false;

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);

                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent("helloworld"), "apikey"); //Added api key in form data
                form.Add(new StringContent(getSelectedLanguage()), "language");


                if (string.IsNullOrEmpty(ImagePath) == false)
                {
                    byte[] imageData = File.ReadAllBytes(ImagePath);
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.jpg");
                }
                else if (string.IsNullOrEmpty(PdfPath) == false)
                {
                    byte[] imageData = File.ReadAllBytes(PdfPath);
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "PDF", "pdf.pdf");
                }

                HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);

                string strContent = await response.Content.ReadAsStringAsync();

                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(strContent);

                if (ocrResult.OCRExitCode == 1)
                {
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        txtResult.Text = txtResult.Text + ocrResult.ParsedResults[i].ParsedText;
                    }

                    sBankDetails = GetDetails(txtResult.Text);
                    return sBankDetails;
                }
                else
                {
                    return ("Error: " + strContent);
                }

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message.ToString();
            }
        }

        public string GetDetails(string strTextInput)
        {
            try
            {
                string sIFSCCode = string.Empty;
                string sAcctNo = string.Empty;
                string sBankDetails = string.Empty;

                string[] sLines = Regex.Split(strTextInput, "\r\n");

                foreach (string sLine in sLines)
                {
                    if (sLine.Contains("IFSC"))
                    {
                        sIFSCCode = sLine.Trim().Substring(sLine.Trim().Length - 11);
                    }

                    if ((sIFSCCode != "") && sIFSCCode.Length > 4)
                    {
                        sBankDetails = GetBankDetailsFromIFSCCode(sIFSCCode.Trim());
                    }

                    if (IsDigitsOnly(sLine.Trim()) && sLine.Trim().Length >= 9)
                    {
                        sAcctNo = Convert.ToString(sLine.Trim());
                    }   
                }

                //textBox1.Text = sIFSCCode;
                //textBox2.Text = sBankName;
                //textBox3.Text = sBankAddress;
                //textBox4.Text = sAcctNo;

                return sIFSCCode+ ";" + sBankDetails + ";" + sAcctNo;

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message.ToString();
            }
        }

        private string GetBankDetailsFromIFSCCode(string sIFSCCode)
        {
            try
            {
                return "BankName;Bank Address Here";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsDigitsOnly(string strInput)
        {
            foreach (char c in strInput)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

      
    }
}



