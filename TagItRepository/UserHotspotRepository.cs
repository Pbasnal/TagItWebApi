using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TagItViewModels;
using TagItDatabaseModels;
using TagIt.Common;
using TagItDatabaseModels.Tables;
using System.Configuration;

namespace TagItRepository
{
    public class UserHotspotRepository
    {
        TagRepository _tagRepository;
        CommentRepository _commentRepository;
        ImageRepository _imageRepository;
        HotspotRepository _hotspotRepository;

        TagItDbContext _dbContext;

        public UserHotspotRepository()
        {
            _dbContext = new TagItDbContext();
        }

        public List<UserHotspotModel> GetUserHotspots(long phoneNumber)
        {
            var userHotspots = new List<UserHotspotModel>();

            try
            {
                var users = _dbContext.Users.FirstOrDefault(u => phoneNumber == u.PhoneNumber);

                if (users.UserHotspots == null) return new List<UserHotspotModel>();

                foreach (var hotspot in users.UserHotspots.ToList())
                {
                    var userHotspot = new UserHotspotModel
                    {
                        PhoneNumber = phoneNumber,
                        Hotspot = new HotspotModel
                        {
                            Comments = hotspot?.HotspotComments.ToList().FirstOrDefault(hc => hc.User.PhoneNumber == phoneNumber && hc.CommentType.Type == CommentTypes.Comment.ToString()).Text,
                            Location = new PositionModel
                            {
                                Lat = hotspot.Latitude,
                                Lng = hotspot.Longitude
                            },
                            Name = hotspot.Name,
                            Portal = hotspot.Portal,
                            Tags = hotspot.HotspotTags.Select(ht => ht.Tag.HashTag).ToList()
                        }
                    };

                    userHotspots.Add(userHotspot);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return userHotspots;
        }

        public RepositoryResponseMessage AddUserHotspot(AddUserHotspotModel userHotspotModel)
        {
            using (var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    var user = _dbContext.Users.FirstOrDefault(u => u.PhoneNumber == userHotspotModel.PhoneNumber);
                    if (user == null)
                        return new RepositoryResponseMessage { Code = TagItResponseCode.FailUserDoesNotExists, Message = TagItResponseMessage.UserDoesNotExists };

                    var hotspots = _dbContext.Hotspots.Where(h =>
                            userHotspotModel.request.Name.Equals(h.Name, StringComparison.OrdinalIgnoreCase)).ToList();

                    Hotspot hotspot = null;
                    List<HotspotTag> hotspotTagDtos = new List<HotspotTag>();
                    List<Tag> tagsDto = new List<Tag>();
                    Comment commentDto = new Comment();
                    List<Image> imagesDto = new List<Image>();

                    //InsertTags
                    _tagRepository = new TagRepository(_dbContext);
                    var tagInsertResponse = _tagRepository.InsertTags(userHotspotModel.request.Tags);
                    if (tagInsertResponse.Code != TagItResponseCode.Success)
                        new RepositoryResponseMessage { Code = TagItResponseCode.FailToInsertTags, Message = TagItResponseMessage.InsertTagsFailed };

                    //InsertComments
                    _commentRepository = new CommentRepository(_dbContext);
                    var commentInsertResponse = _commentRepository.InsertComments(userHotspotModel.request.Comments);
                    if (commentInsertResponse.Code != TagItResponseCode.Success)
                        new RepositoryResponseMessage { Code = TagItResponseCode.FailToInsertComment, Message = TagItResponseMessage.InsertCommentsFailed };

                    //InsertImages
                    _imageRepository = new ImageRepository(_dbContext);
                    var imageInsertResponse = _imageRepository.InsertImages(userHotspotModel.request.Comments);
                    if (imageInsertResponse.Code != TagItResponseCode.Success)
                        new RepositoryResponseMessage { Code = TagItResponseCode.FailToInsertImage, Message = TagItResponseMessage.InsertImagesFailed };

                    //InsertHotspot
                    _hotspotRepository = new HotspotRepository(_dbContext);
                    var hotspotInsertReponse = _hotspotRepository.InsertHotspot(userHotspotModel.request,
                        tagInsertResponse.Tags,
                        commentInsertResponse.Comment,
                        imageInsertResponse.Image);

                    if (hotspotInsertReponse.Code != TagItResponseCode.Success)
                        new RepositoryResponseMessage { Code = TagItResponseCode.FailToInsertHotspot, Message = TagItResponseMessage.InsertHotspotFailed };

                    //InsertUserAndReleventData
                    var response = InsertUserAndReleventData(user, hotspotInsertReponse.Hotspot, tagInsertResponse.Tags, commentInsertResponse.Comment, imageInsertResponse.Image);
                    if (response.Code != TagItResponseCode.Success)
                    {
                        dbTransaction.Rollback();
                        return response;
                    }

                    _dbContext.SaveChanges();
                    dbTransaction.Commit();
                    return new RepositoryResponseMessage { Code = TagItResponseCode.Success, Message = TagItResponseMessage.HotspotAddedSuccessfully };
                }

                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }

        private RepositoryResponseMessage InsertUserAndReleventData(User user, Hotspot hotspot, List<Tag> tags, Comment comment, Image image)
        {
            var response = new RepositoryResponseMessage();
            try
            {
                if (hotspot == null || tags.Count == 0)
                    return new RepositoryResponseMessage { Code = TagItResponseCode.FailInputNull, Message = TagItResponseMessage.InputArgumentIsNull };

                
                if (hotspot.User != null)
                    return new RepositoryResponseMessage { Code = TagItResponseCode.FailUserAlreadyMapedToHotspot, Message = TagItResponseMessage.UserAlreadyMappedToHotspot };

                //userHotspot
                InsertUserHotspots(hotspot, user);

                //usertags
                InsertUserTags(tags, user);

                //UserComments
                InsertUserComment(comment, user);

                //UserImage
                InsertUserImage(image, user);

                _dbContext.SaveChanges();

                response.Code = TagItResponseCode.Success;
                response.Message = TagItResponseMessage.HotspotAddedSuccessfully;

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void InsertUserHotspots(Hotspot hotspot, User user)
        {
            if (hotspot == null) return;

            if (user.UserHotspots == null)
                user.UserHotspots = new List<Hotspot>();
            user.UserHotspots.Add(hotspot);
            hotspot.User = user;
        }

        private void InsertUserTags(IList<Tag> tags, User user)
        {
            if (tags == null) return;

            foreach (var tag in tags)
            {
                if (user.UserTags == null)
                    user.UserTags = new List<Tag>();
                user.UserTags.Add(tag);
                tag.User = user;
            }
        }

        private void InsertUserComment(Comment comment, User user)
        {
            if (comment == null) return;

            if (user.UserComments == null)
                user.UserComments = new List<Comment>();
            user.UserComments.Add(comment);
            comment.User = user;

        }

        private void InsertUserImage(Image image, User user)
        {
            if (image == null) return;

            if (user.UserImages == null)
                user.UserImages = new List<Image>();
            user.UserImages.Add(image);
            image.User = user;
        }
    }
}
