using System;
using System.Collections.Generic;
using System.Web.Http;

using TagItDatabaseModels;
using TagItRepository;
using TagItViewModels;

namespace TagItWebApi.Controllers
{
    public class AppsController : ApiController
    {
        AppsRepository _repository = new AppsRepository();

        public List<AppViewModel> GetAll()
        {
            return _repository.GetAllApps();
        }

        public AppViewModel Get(string appName)
        {
            return _repository.GetApp(appName);
        }

        [HttpPost]
        public string Add([FromBody]string appName)
        {
            return _repository.AddApp(appName).Message;
        }
    }
}
