using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace VirtualGuide.Common.Helpers
{
    public class FileHelper
    {
        public static MultipartFormDataStreamProvider GetMultipartProvider()
        {
            var root = System.IO.Path.GetTempPath();
            return new MultipartFormDataStreamProvider(root);
        }

        public static void UploadFiles(HttpFileCollection files, NameValueCollection form)
        {
            //single upload
            if (files.Count == 1 && files.AllKeys.First() != null)
            {
                HttpPostedFile file = files[files.AllKeys.First()];

                if (file != null)
                {
                    string filename = file.FileName;
                    if (form["filename"] != null)
                    {
                        filename = Path.GetFileName(form["filename"]);
                    }

                    var fileSavePath = Path.Combine(ImagePath(), filename);
                    file.SaveAs(fileSavePath);

                }
                
            }
            //multi upload
            else 
            {
                foreach (String key in files.AllKeys)
                {
                    if (files[key].ContentLength > 0)
                    {
                        var fileSavePath = Path.Combine(ImagePath(), files[key].FileName);
                        files[key].SaveAs(fileSavePath);
                    }

                }

            }
        }

        public static string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }

        public static string ImagePath()
        {
            var uploadFolder = "~/Uploads"; // you could put this to web.config
            return HttpContext.Current.Server.MapPath(uploadFolder);
        }
    }
}
