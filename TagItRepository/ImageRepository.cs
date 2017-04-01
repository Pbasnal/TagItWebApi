using System;
using System.Linq;
using TagItDatabaseModels;
using TagIt.Common;
using TagItDatabaseModels.Tables;

namespace TagItRepository
{
    public class ImageRepository
    {
        private TagItDbContext _dbContext;
        public ImageRepository()
        {
            _dbContext = new TagItDbContext();
        }

        public ImageRepository(TagItDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ImageResponseMessage InsertImages(string imageInput)
        {
            var response = new ImageResponseMessage();
            try
            {
                if (string.IsNullOrWhiteSpace(imageInput))
                    return new ImageResponseMessage
                    {
                        Code = TagItResponseCode.Success,
                        Message = TagItResponseMessage.InputImageIsEmpty,
                        Image = null
                    };

                var existingImage = _dbContext.Images.FirstOrDefault(i => imageInput.Equals(i.ImagePath, StringComparison.OrdinalIgnoreCase));

                if(existingImage != null)
                    return new ImageResponseMessage
                    {
                        Code = TagItResponseCode.Success,
                        Message = TagItResponseMessage.ImagesAlreadyExists,
                        Image = existingImage
                    };

                var imageDto = new Image
                {
                    ImagePath = imageInput,
                    IsActive = true,
                    //CreatedDate = DateTime.Today
                };

                _dbContext.Images.Add(imageDto);
                _dbContext.SaveChanges();

                response.Code = TagItResponseCode.Success;
                response.Message = TagItResponseMessage.ImagesAddedSuccessfully;
                response.Image = _dbContext.Images.FirstOrDefault(i => imageInput.Equals(i.ImagePath, StringComparison.OrdinalIgnoreCase));

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public class ImageResponseMessage : RepositoryResponseMessage
    {
        public Image Image { get; set; }
    }
}
