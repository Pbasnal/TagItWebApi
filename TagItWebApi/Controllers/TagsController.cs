using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TagItViewModels;
using TagItRepository;

namespace TagItWebApi.Controllers
{
    public class TagsController : ApiController
    {
        TagRepository _repository;
        [HttpPost]
        public List<TagSearchResultModel> Search([FromBody] TagSearchModel tagSearchModel)
        {
            _repository = new TagRepository();
            var response = _repository.SearchTags(tagSearchModel);

            return response;
        }
    }
}
