using SuzlonBPP.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace SuzlonBPP.Controllers
{
    [Authorize]
    [RoutePrefix("api/fileprocess")]
    public class FileController : BaseApiController
    {

        [HttpPost]
        [Route("Upload")]
        public async Task<HttpResponseMessage> Upload(string fileName)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            if (Request.Content.IsMimeMultipartContent())
            {
                Request.Content.LoadIntoBufferAsync().Wait();
                await Request.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider()).ContinueWith((task) =>
                {
                    MultipartMemoryStreamProvider provider = task.Result;
                    foreach (HttpContent content in provider.Contents)
                    {
                        Stream stream = content.ReadAsStreamAsync().Result;
                        Image image = Image.FromStream(stream);
                        //var testName = content.Headers.ContentDisposition.Name;
                        String filePath = HostingEnvironment.MapPath("~/" + Constants.VENDOR_BANK_ATTACHMENT_PATH);
                        String fullPath = Path.Combine(filePath, fileName);
                        image.Save(fullPath);                        
                    }
                });
                return result;
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }

        }

        //    [HttpPost]
        //    [Route("Upload")]
        //    public async Task<HttpResponseMessage> Upload()
        //    {
        //        if (Request.Content.IsMimeMultipartContent())
        //        {
        //            string root = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Bank Detail Attachment");

        //            var streamProvider = new MultipartFormDataStreamProvider(root);
        //            try
        //            {
        //                await Request.Content.ReadAsMultipartAsync(streamProvider);
        //                //return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");

        //                foreach (MultipartFileData fileData in streamProvider.FileData)
        //                {
        //                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
        //                    {
        //                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
        //                    }
        //                    string fileName = fileData.Headers.ContentDisposition.FileName;
        //                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
        //                    {
        //                        fileName = fileName.Trim('"');
        //                    }
        //                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
        //                    {
        //                        fileName = Path.GetFileName(fileName);
        //                    }
        //                    File.Move(fileData.LocalFileName, root + @"\" + fileName);
        //                    //File.Move(fileData.LocalFileName, Path.Combine(StoragePath, fileName));
        //                }
        //                return Request.CreateResponse(HttpStatusCode.OK);
        //            }
        //            catch (System.Exception e)
        //            {
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        //            }
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
        //        }
        //    }

    }
}
