using TagItDatabaseModels.Tables;

namespace TagItViewModels
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public long PhoneNumber { get; set; }

        public UserViewModel(User userDto)
        {
            if (userDto == null)
                return;
            UserName = userDto.UserName;
            PhoneNumber = userDto.PhoneNumber;
        }
    }
}
