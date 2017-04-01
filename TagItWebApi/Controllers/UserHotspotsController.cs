using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TagIt.Common;
using TagItRepository;
using TagItViewModels;


namespace TagItWebApi.Controllers
{
    public class UserHotspotsController : ApiController
    {
        UserHotspotRepository _repository = new UserHotspotRepository();

        [HttpPost]
        public HttpResponseMessage Add([FromBody] AddUserHotspotModel userHotspotModel)
        {
            var response = _repository.AddUserHotspot(userHotspotModel);

            var retResposne = new HttpResponseMessage(HttpStatusCode.OK);
            if (response.Code != TagItResponseCode.Success)
            {
                retResposne.StatusCode = HttpStatusCode.BadRequest;
            }
            retResposne.Content = new StringContent(response.Message);

            return retResposne;
        }

        [HttpGet]
        public List<UserHotspotModel> GetUserHotspots(long phoneNumber)
        {
            return _repository.GetUserHotspots(phoneNumber);
        }
    }
}
