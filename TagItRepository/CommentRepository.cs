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
    public class CommentRepository
    {
        private TagItDbContext _dbContext;
        public CommentRepository()
        {
            _dbContext = new TagItDbContext();
        }

        public CommentRepository(TagItDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CommentResponseMessage InsertComments(string commentInput)
        {
            var response = new CommentResponseMessage();
            try
            {
                if (string.IsNullOrWhiteSpace(commentInput))
                    return new CommentResponseMessage
                    {
                        Code = TagItResponseCode.Success,
                        Message = TagItResponseMessage.InputCommentIsEmpty,
                        Comment = null
                    };

                var commentTypes = _dbContext.CommentTypes.ToList();

                var existingComment = _dbContext.Comments.FirstOrDefault(c => commentInput.Equals(c.Text, StringComparison.OrdinalIgnoreCase));

                if (existingComment != null)
                    return new CommentResponseMessage
                    {
                        Code = TagItResponseCode.Success,
                        Message = TagItResponseMessage.CommentAlreadyExists,
                        Comment = existingComment
                    };

                var commentDto = new Comment
                {
                    Text = commentInput,
                    CommentType = commentTypes.FirstOrDefault(c => CommentTypes.Comment.ToString().Equals(c.Type)),
                    IsActive = true,
                    //CreatedDate = DateTime.Today
                };

                _dbContext.Comments.Add(commentDto);
                _dbContext.SaveChanges();

                return new CommentResponseMessage
                {
                    Code = TagItResponseCode.Success,
                    Message = TagItResponseMessage.CommentAddedSuccessfully,
                    Comment = _dbContext.Comments.FirstOrDefault(c => commentInput.Equals(c.Text, StringComparison.OrdinalIgnoreCase))
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public class CommentResponseMessage : RepositoryResponseMessage
    {
        public Comment Comment { get; set; }
    }
}

