using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Data;

namespace OCRAPI
{
    public sealed class  OCRReader
    {
        public async Task<string> ReadImageData(string ImagePath)
        {
            string sBankDetails = string.Empty;

            if (string.IsNullOrEmpty(ImagePath))
                return "File path is empty";

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);

                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent("00c568cc6688957"), "apikey"); //Added api key in form data
                form.Add(new StringContent("eng"), "language");

                if (string.IsNullOrEmpty(ImagePath) == false)
                {
                    byte[] imageData = File.ReadAllBytes(ImagePath);
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.jpg");
                }
             
                HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);

                string strContent = await response.Content.ReadAsStringAsync();
                
                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(strContent);

                if (ocrResult.ErrorMessage != null)
                {
                    return "Error: " + ocrResult.ErrorMessage.ToString();
                }

                string result = "";

                if (ocrResult.OCRExitCode == 1)
                {
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        result = result + ocrResult.ParsedResults[i].ParsedText + Environment.NewLine;
                    }

                    sBankDetails = GetBankDetails(result);
                    return sBankDetails;
                }
                else
                {
                    return (strContent);
                }

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message.ToString();
            }
        }

        public string GetBankDetails(string strTextInput)
        {
            try
            {
                string sIFSCCode = string.Empty;
                string sAcctNo = string.Empty;
                string sBranchDetails = string.Empty;

                string[] sLines = Regex.Split(strTextInput, "\r\n");

                foreach (string sLine in sLines)
                {
                    if (sLine.Contains("IFSC") || sLine.Contains("IFS"))
                    {
                        sIFSCCode = sLine.Trim().Substring(sLine.Trim().Length - 11);
                    }

                    if (IsDigitsOnly(sLine.Trim()) && sLine.Trim().Length >= 9)
                    {
                        sAcctNo = Convert.ToString(sLine.Trim());
                    }
                }

                //if ((sIFSCCode != "") && sIFSCCode.Length > 4)
                //{
                //    sBranchDetails = GetBranchDetailsFromIFSCCode(sIFSCCode.Trim());
                //}

                return sIFSCCode + ";" + sAcctNo;

               // return sIFSCCode + ";" + sBranchDetails + ";" + sAcctNo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetBranchDetailsFromIFSCCode(string sIFSCCode)
        {
            try
            {
                DataTable dtBankDetails = new DataTable();
                string sQuery = "SELECT TOP 1 ISNULL(BankName,'') as 'BankName', ISNULL(BranchName,'') as 'BranchName' " +
                                " FROM dbo.IFSCCodeMaster " +
                                " WHERE IFSCCode = '" + sIFSCCode.Trim() + "'";
                dtBankDetails = Common.GetDataTable(sQuery);

                if (dtBankDetails.Rows.Count > 0)
                { return Convert.ToString(dtBankDetails.Rows[0]["BankName"]) + ";" + Convert.ToString(dtBankDetails.Rows[0]["BranchName"]); }
                else
                { return ";"; }
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
