using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagItDatabaseModels;
using TagItDatabaseModels.Tables;
using TagIt.Common;
using TagItViewModels;

namespace TagItRepository
{
    public class AppsRepository
    {
        public RepositoryResponseMessage AddApp(string appName)
        {
            try
            {
                using (var dbContext = new TagItDbContext())
                {
                    var doesAppExists = dbContext.Apps.FirstOrDefault(a => appName.Equals(a.AppName, StringComparison.OrdinalIgnoreCase));

                    if (doesAppExists != null) return new RepositoryResponseMessage { Code = TagItResponseCode.FailAppExists, Message = TagItResponseMessage.AppAlreadyExists};

                    var app = new App
                    {
                        AppId = Guid.NewGuid(),
                        AppName = appName
                    };

                    dbContext.Apps.Add(app);
                    dbContext.SaveChanges();

                    return new RepositoryResponseMessage { Code = TagItResponseCode.Success, Message = TagItResponseMessage.AppAddedSuccessfully};
                }
            }
            catch (Exception ex)
            {
                return new RepositoryResponseMessage { Code = TagItResponseCode.FailAppAddException, Message = ex.Message };
            }
        }

        public AppViewModel GetApp(string appName)
        {
            try
            {
                AppViewModel app;
                using (var dbContext = new TagItDbContext())
                {
                    var appDto = dbContext.Apps.FirstOrDefault(a => appName.Equals(a.AppName, StringComparison.OrdinalIgnoreCase));
                    if (appDto == null) return null;

                    app = new AppViewModel(appDto);
                }

                return app;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<AppViewModel> GetAllApps()
        {
            try
            {
                List<App> allAppsDto;
                List<AppViewModel> allApps = new List<AppViewModel>();

                using (var dbContext = new TagItDbContext())
                {
                    allAppsDto = dbContext.Apps.ToList();
                }

                foreach (var appDto in allAppsDto)
                {
                    allApps.Add(new AppViewModel(appDto));
                }

                return allApps;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

