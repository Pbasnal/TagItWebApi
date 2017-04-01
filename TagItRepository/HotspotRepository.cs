using System;
using System.Collections.Generic;
using System.Windows;
using TagItViewModels;
using TagItDatabaseModels;
using TagIt.Common;
using TagItDatabaseModels.Tables;
using System.Configuration;
using System.Linq;
using System.Device;
using System.Device.Location;

namespace TagItRepository
{
    public class HotspotRepository
    {
        private TagItDbContext _dbContext;
        public HotspotRepository()
        {
            _dbContext = new TagItDbContext();
        }

        public HotspotRepository(TagItDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public HotspotResponseMessage InsertHotspot(AddHotspotModel hotspotModel)
        {
            var response = new HotspotResponseMessage();
            try
            {
                if (hotspotModel == null)
                    return new HotspotResponseMessage
                    {
                        Code = TagItResponseCode.FailInputNull,
                        Message = TagItResponseMessage.InputArgumentIsNull,
                        Hotspot = null
                    };

                if(CreateNewHotspot(hotspotModel))
                {
                    var hotspotDto = new Hotspot
                    {
                        Name = hotspotModel.Name,
                        Latitude = hotspotModel.Location.Lat,
                        Longitude = hotspotModel.Location.Lng,
                        Portal = hotspotModel.Portal,
                        IsActive = true
                    };

                    _dbContext.Hotspots.Add(hotspotDto);
                    _dbContext.SaveChanges();
                }
                else
                    return new HotspotResponseMessage
                    {
                        Code = TagItResponseCode.FailHotspotAlreadyExists,
                        Message = TagItResponseMessage.HotspotAlreadyExists,
                        Hotspot = null
                    };

                response.Code = TagItResponseCode.Success;
                response.Message = TagItResponseMessage.HotspotAddedSuccessfully;
                response.Hotspot = _dbContext.Hotspots.FirstOrDefault(h => 
                                        hotspotModel.Name.Equals(h.Name, StringComparison.OrdinalIgnoreCase) &&
                                        hotspotModel.Location.Lat == h.Latitude && hotspotModel.Location.Lng == h.Longitude);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public HotspotResponseMessage InsertHotspot(AddHotspotModel hotspotModel, List<Tag> tags, Comment comment, Image image)
        {
            var response = new HotspotResponseMessage();
            try
            {
                if (hotspotModel == null || tags.Count == 0)
                    return new HotspotResponseMessage { Code = TagItResponseCode.FailInputNull, Message = TagItResponseMessage.InputArgumentIsNull, Hotspot = null };

                if (CreateNewHotspot(hotspotModel))
                {
                    var hotspotDto = new Hotspot
                    {
                        Name = hotspotModel.Name,
                        Latitude = hotspotModel.Location.Lat,
                        Longitude = hotspotModel.Location.Lng,
                        Portal = hotspotModel.Portal,
                        IsActive = true
                    };

                    var hotspotTags = new List<HotspotTag>();

                    foreach (var tag in tags)
                    {
                        var hotspotTag = new HotspotTag
                        {
                            Hotspot = hotspotDto,
                            Tag = tag,
                            IsActive = true,
                            //CreatedDate = DateTime.UtcNow
                        };
                        hotspotTags.Add(hotspotTag);
                        if (tag.HotspotTags == null)
                            tag.HotspotTags = new List<HotspotTag>();
                        tag.HotspotTags.Add(hotspotTag);
                    }
                    hotspotDto.HotspotTags = hotspotTags;

                    hotspotDto.HotspotComments = new List<Comment> { comment };
                    comment.Hotspot = hotspotDto;

                    hotspotDto.HotspotImages = new List<Image> { image };
                    image.Hotspot = hotspotDto;

                    _dbContext.Hotspots.Add(hotspotDto);
                    _dbContext.SaveChanges();

                    response.Code = TagItResponseCode.Success;
                    response.Message = TagItResponseMessage.HotspotAddedSuccessfully;
                    response.Hotspot = _dbContext.Hotspots.FirstOrDefault(h =>
                                            hotspotModel.Name.Equals(h.Name, StringComparison.OrdinalIgnoreCase) &&
                                            hotspotModel.Location.Lat == h.Latitude && hotspotModel.Location.Lng == h.Longitude);
                    return response;
                }
                else
                    return new HotspotResponseMessage { Code = TagItResponseCode.FailHotspotAlreadyExists, Message = TagItResponseMessage.HotspotAlreadyExists, Hotspot = null };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CreateNewHotspot(AddHotspotModel hotspotModel)
        {
            var hotspots = _dbContext.Hotspots.Where(h =>
                            hotspotModel.Name.Equals(h.Name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (hotspots.Count == 0) return true;

            foreach (var hotspot in hotspots)
            {
                var p1 = new GeoCoordinate(hotspot.Latitude, hotspot.Longitude);
                var p2 = new GeoCoordinate(hotspotModel.Location.Lat, hotspotModel.Location.Lng);

                double distance = p1.GetDistanceTo(p2);
                var minimumIdenticalHotspotDistance = Int32.Parse(ConfigurationManager.AppSettings["MinimumIdenticalHotspotDistance"]);

                if (distance < minimumIdenticalHotspotDistance)
                    return false;
            }

            return true;
        }
    }

    public class HotspotResponseMessage : RepositoryResponseMessage
    {
        public Hotspot Hotspot { get; set; }
    }
}
