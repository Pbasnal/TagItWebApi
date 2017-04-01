using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TagItRepository;
using TagIt.Common;
using TagItViewModels;

namespace TagitUnitTest
{
    [TestClass]
    public class UserHotspotUnitTests
    {
        [TestMethod]
        public void InsertUserHotspotSuccess()
        {
            var repo = new UserHotspotRepository();
            var model = new AddUserHotspotModel
            {
                PhoneNumber = 7032925438,
                request = new AddHotspotModel
                {
                    Comments = "New Place",
                    Images = "no image",
                    Name = "office1",
                    Portal = "no portal",
                    Tags = "#office1 #work1",
                    Location = new PositionModel
                    {
                        Lat = 1,
                        Lng = 2
                    }

                }
            };

            var response = repo.AddUserHotspot(model);

            Assert.AreEqual(TagItResponseCode.Success, response.Code);
        }

        [TestMethod]
        public void SearchTagUnitTest()
        {
            var repo = new TagRepository();
            var model = new TagSearchModel
            {
                Query = "office",
                Bounds = new BoundsModel
                {
                    NEPoint = new PositionModel { Lat = 2, Lng = 3 },
                    SWPoint = new PositionModel { Lat = 0, Lng = 0 },
                }
            };

            var response = repo.SearchTags(model);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(response.Count, 0);
        }


        [TestMethod]
        public void GetUserRequestsUnitTest()
        {
            var repo = new UserHotspotRepository();
            

            var response = repo.GetUserHotspots(7032925438);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(response.Count, 0);
        }
    }
}
