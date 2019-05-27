using SolarPMS.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace SolarPMS.Controllers
{
    [Authorize]
    [RoutePrefix("api/fileprocess")]
    public class FileController : BaseApiController
    {
        [HttpPost]
        [Route("Upload")]
        public async Task<HttpResponseMessage> UploadIssue(string fileName)
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
                         String filePath = HostingEnvironment.MapPath("~/Upload/Attachment");
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

        [HttpPost]
        [Route("UploadTimesheetAttachement")]
        public async Task<HttpResponseMessage> UploadTimesheetAttachement(string fileName)
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
                        String filePath = HostingEnvironment.MapPath("~/Upload/Attachment/Timesheet");
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

        //[HttpPost]
        //[Route("Upload")]
        //public async Task<HttpResponseMessage> UploadIssue()
        //{
        //    if (Request.Content.IsMimeMultipartContent())
        //    {
        //        string root = HttpContext.Current.Server.MapPath("~/Upload/Attachment");
        //        var streamProvider = new MultipartFormDataStreamProvider(root);
        //        try
        //        {
        //            await Request.Content.ReadAsMultipartAsync(streamProvider);
        //            //return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");

        //            foreach (MultipartFileData fileData in streamProvider.FileData)
        //            {
        //                if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
        //                }
        //                string fileName = fileData.Headers.ContentDisposition.FileName;
        //                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
        //                {
        //                    fileName = fileName.Trim('"');
        //                }
        //                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
        //                {
        //                    fileName = Path.GetFileName(fileName);
        //                }
        //                File.Move(fileData.LocalFileName, root + @"\" + fileName);
        //                //File.Move(fileData.LocalFileName, Path.Combine(StoragePath, fileName));
        //            }
        //            return Request.CreateResponse(HttpStatusCode.OK);
        //        }
        //        catch (System.Exception e)
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
        //    }
        //}

        //[HttpPost]
        //[Route("UploadTimesheetAttachement")]
        //public async Task<HttpResponseMessage> UploadTimesheetAttachement()
        //{
        //    if (Request.Content.IsMimeMultipartContent())
        //    {
        //        string root = HttpContext.Current.Server.MapPath("~/Upload/Attachment/Timesheet");
        //        var streamProvider = new MultipartFormDataStreamProvider(root);
        //        try
        //        {
        //            await Request.Content.ReadAsMultipartAsync(streamProvider);
        //            //return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");

        //            foreach (MultipartFileData fileData in streamProvider.FileData)
        //            {
        //                if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
        //                {
        //                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
        //                }
        //                string fileName = fileData.Headers.ContentDisposition.FileName;
        //                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
        //                {
        //                    fileName = fileName.Trim('"');
        //                }
        //                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
        //                {
        //                    fileName = Path.GetFileName(fileName);
        //                }
        //                File.Move(fileData.LocalFileName, root + @"\" + fileName);
        //                //File.Move(fileData.LocalFileName, Path.Combine(StoragePath, fileName));
        //            }
        //            return Request.CreateResponse(HttpStatusCode.OK);
        //        }
        //        catch (System.Exception e)
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
        //    }
        //}

        //[Route("fileupload")]
        //public HttpResponseMessage Post(HttpContext context)
        //{
        //    HttpResponseMessage result = null;
        //    //var httpRequest = HttpContext.Current.Request;
        //    if (context.Request.Files.Count > 0)
        //    {
        //        var docfiles = new List<string>();
        //        foreach (string file in context.Request.Files)
        //        {
        //            var postedFile = context.Request.Files[file];
        //            var filePath = HttpContext.Current.Server.MapPath("~/Profile/" + postedFile.FileName);
        //            postedFile.SaveAs(filePath);

        //            docfiles.Add(filePath);
        //        }
        //        result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
        //    }
        //    else
        //    {
        //        result = Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //    return Ok();
        //}

    }
}
