using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagItDatabaseModels;
using TagItDatabaseModels.Tables;
using TagItViewModels;
using TagIt.Common;

namespace TagItRepository
{
    public class UsersRepository
    {
        public RepositoryResponseMessage AddUser(UserViewModel user)
        {
            try
            {
                using (var dbContext = new TagItDbContext())
                {
                    var doesUserExists = dbContext.Users.FirstOrDefault(u => user.PhoneNumber == u.PhoneNumber);

                    if (doesUserExists != null) return new RepositoryResponseMessage { Code = TagItResponseCode.Success, Message = TagItResponseMessage.UserAlreadyExists };

                    var newUser = new User
                    {
                        UserId = Guid.NewGuid(),
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                        //CreatedDate = DateTime.Today
                    };

                    dbContext.Users.Add(newUser);
                    dbContext.SaveChanges();

                    return new RepositoryResponseMessage { Code = TagItResponseCode.Success, Message = TagItResponseMessage.UserAddedSuccessfully };
                }
            }
            catch (Exception ex)
            {
                return new RepositoryResponseMessage { Code = TagItResponseCode.FailUserAddException, Message = ex.Message };
            }
        }

        public UserViewModel GetUser(long phoneNumber)
        {
            try
            {
                UserViewModel user;
                using (var dbContext = new TagItDbContext())
                {
                    var userDto = dbContext.Users.FirstOrDefault(u => phoneNumber == u.PhoneNumber);

                    if (userDto == null) return null;

                    user = new UserViewModel(userDto);
                }

                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<UserViewModel> GetAllUsers()
        {
            try
            {
                List<User> allUsersDto;
                List<UserViewModel> allUsers = new List<UserViewModel>();

                using (var dbContext = new TagItDbContext())
                {
                    allUsersDto = dbContext.Users.ToList();
                }

                foreach (var userDto in allUsersDto)
                {
                    allUsers.Add(new UserViewModel(userDto));
                }

                return allUsers;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
