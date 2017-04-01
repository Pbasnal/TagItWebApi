using System;
using TagItDatabaseModels.Tables;

namespace TagItViewModels
{
    public class AppViewModel
    {
        public string AppName { get; set; }
        public Guid AppKey { get; set; }

        public AppViewModel(App appDto)
        {
            if (appDto != null)
            {
                this.AppName = appDto.AppName;
                this.AppKey = appDto.AppId;
            }
        }
    }
}
