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
using System.Web.Http.Description;
using VirtualGuide.Common.Helpers;

namespace VirtualGuide.WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class UploadController : ApiController
    {
       
        [HttpPost] // This is from System.Web.Http, and not from System.Web.Mvc
        [Route("Upload")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Upload()
        {
            try
            {
                HttpFileCollection files = HttpContext.Current.Request.Files;
                NameValueCollection form = HttpContext.Current.Request.Form;
                FileHelper.UploadFiles(files, form);
                return Ok(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return BadRequest();
            }


        }
    }
}
