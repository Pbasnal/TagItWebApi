using System.Collections.Generic;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using TagItRepository;
using TagItViewModels;
using TagIt.Common;

namespace TagItWebApi.Controllers
{
    public class UsersController : ApiController
    {
        UsersRepository _repository = new UsersRepository();

        public List<UserViewModel> GetAll()
        {
            return _repository.GetAllUsers();
        }

        public UserViewModel Get(long phoneNumber)
        {
            return _repository.GetUser(phoneNumber);
        }

        [HttpPost]
        public HttpResponseMessage Add([FromBody]UserViewModel user)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            if (!VerifyData(user))
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent(TagItResponseMessage.FailInvalidData);
                return response;
            }

            var result = _repository.AddUser(user);
            
            if (result.Code != 0)
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            response.Content = new StringContent(result.Message);
            return response;
        }

        private bool VerifyData(UserViewModel user)
        {
            if (user == null || user.UserName == null) return false;

            if (string.IsNullOrWhiteSpace(user.UserName) || user.PhoneNumber == 0) return false;

            return true;
        }
    }
}
