using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VirtualGuide.Common.Helpers;

namespace VirtualGuide.WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class UploadController : ApiController
    {
       
        [HttpPost] // This is from System.Web.Http, and not from System.Web.Mvc
        [Route("Upload")]
        public HttpResponseMessage Upload()
        {
            try
            {
                HttpFileCollection files = HttpContext.Current.Request.Files;
                NameValueCollection form = HttpContext.Current.Request.Form;
                FileHelper.UploadFiles(files, form);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(HttpResponseException)
            {
                throw;
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }


        }
    }
}
