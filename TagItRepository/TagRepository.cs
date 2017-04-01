using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagItDatabaseModels;
using TagIt.Common;
using TagItDatabaseModels.Tables;
using TagItViewModels;

namespace TagItRepository
{
    public class TagRepository
    {
        private TagItDbContext _dbContext;
        public TagRepository()
        {
            _dbContext = new TagItDbContext();
        }

        public TagRepository(TagItDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TagResponseMessage InsertTags(string tagsInputString)
        {
            var response = new TagResponseMessage();
            try
            {
                var tags = tagsInputString.Split(' ').ToList();

                for (int i = 0; i < tags.Count; i++)
                {
                    tags[i] = tags[i].Replace("#", string.Empty);
                }

                var existingTags = _dbContext.Tags.Where(t => tags.Any(tag => string.Equals(t.HashTag.ToLower(), tag.ToLower()))).ToList();
                var existingHashTags = existingTags.Select(t => t.HashTag).ToList();
                var nonExistingTags = tags.Where(t => !existingHashTags.Any(eh => string.Equals(t.ToLower(), eh.ToLower()))).ToList();

                if (nonExistingTags.Count == 0)
                {
                    response.Code = TagItResponseCode.Success;
                    response.Message = TagItResponseMessage.AlreadyTagsExists;
                    response.Tags = existingTags;
                    return response;
                }

                foreach (var tag in nonExistingTags)
                {
                    if (string.IsNullOrWhiteSpace(tag)) continue;
                    var tagDto = new Tag
                    {
                        HashTag = tag,
                        //CreatedDate = DateTime.Today
                    };

                    _dbContext.Tags.Add(tagDto);
                }
                _dbContext.SaveChanges();

                response.Code = TagItResponseCode.Success;
                response.Message = TagItResponseMessage.TagsAddedSuccessfully;
                response.Tags = _dbContext.Tags.Where(t => tags.Any(tag => string.Equals(tag.ToLower(), t.HashTag.ToLower()))).ToList();

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TagSearchResultModel> SearchTags(TagSearchModel tagSearchModel)
        {
            if (tagSearchModel == null) return null;
            var searchResults = new List<TagSearchResultModel>();

            try
            {
                // will be really slow with increasing size of the bounds
                var hotspotsInsideBounds = _dbContext.Hotspots.Where(h => h.Latitude < tagSearchModel.Bounds.NEPoint.Lat && h.Longitude < tagSearchModel.Bounds.NEPoint.Lng
                                                                    && h.Latitude > tagSearchModel.Bounds.SWPoint.Lat && h.Longitude > tagSearchModel.Bounds.SWPoint.Lng
                                                                    && h.HotspotTags.Any(ht => ht.Tag.HashTag.ToLower() == tagSearchModel.Query.ToLower())).ToList();

                foreach (var hotspot in hotspotsInsideBounds)
                {
                    var searchResult = new TagSearchResultModel
                    {
                        Name = hotspot.Name,
                        Portal = hotspot.Portal,
                        Location = new PositionModel
                        {
                            Lat = hotspot.Latitude,
                            Lng = hotspot.Longitude
                        },
                        Info = new HotspotInformation
                        {
                            Commends = hotspot.HotspotComments.Where(hc => hc.CommentType.Type == CommentTypes.Commend.ToString()).Select(c => c.Text).ToList(),
                            Comments = hotspot.HotspotComments.Where(hc => hc.CommentType.Type == CommentTypes.Comment.ToString()).Select(c => c.Text).ToList(),
                            Reports = hotspot.HotspotComments.Where(hc => hc.CommentType.Type == CommentTypes.Report.ToString()).Select(c => c.Text).ToList(),
                            Name = hotspot.Name,
                            Tags = hotspot.HotspotTags.Select(ht => ht.Tag.HashTag).ToList()
                        }
                    };

                    searchResults.Add(searchResult);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return searchResults;
        }
    }

    public class TagResponseMessage : RepositoryResponseMessage
    {
        public List<Tag> Tags { get; set; }
    }
}
