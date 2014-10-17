using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtualGuide.Services;
using VirtualGuide.Services.Repository;

namespace VirtualGuide.WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class PlaceController : ApiController
    {
        private PlaceRepository pr = new PlaceRepository();

        [Route("Place/Categories/{language}")]
        public HttpResponseMessage GetPlaceCategories(string language)
        {
            IList<PlaceCategoryViewModel> categories = pr.GetPlaceCategories(language);

            return Request.CreateResponse<IList<PlaceCategoryViewModel>>(HttpStatusCode.OK, categories);

        }
    }
}
