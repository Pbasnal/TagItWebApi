using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagIt.Common
{
    public enum TagItResponseCode
    {
        Success = 0,

        //Common
        FailInputNull = 4001,

        //AppRepo
        FailAppExists = 1001,
        FailAppAddException = 1002,
        
        //UserRepo
        FailUserExists = 2001,
        FailUserAddException = 2002,

        //UserHotspotRepo
        FailUserDoesNotExists = 3001,
        FailToInsertTags = 3002,
        FailToInsertComment = 3003,
        FailToInsertImage = 3004,
        FailToInsertHotspot = 3005,
        FailUserAlreadyMapedToHotspot = 3006,

        //HotspotRepo
        FailHotspotAlreadyExists = 5001,
    }

    public static class TagItResponseMessage
    {
        //Common
        public readonly static string InputArgumentIsNull = "Given argument is NUll";

        //AppRepo
        public readonly static string AppAddedSuccessfully = "App added successfully";
        public readonly static string AppAlreadyExists = "App is already registered";

        //UserRepo
        public readonly static string UserAddedSuccessfully = "User added successfully";
        public readonly static string UserAlreadyExists = "User is already registered";

        //UserHotspotRepo
        public readonly static string UserDoesNotExists= "User does not exists";
        public readonly static string InsertTagsFailed = "Error occurred during insertion of tags";
        public readonly static string InsertCommentsFailed = "Error occurred during insertion of Comments";
        public readonly static string InsertImagesFailed = "Error occurred during insertion of images";
        public readonly static string InsertHotspotFailed = "Error occurred during insertion of Hotspot";
        public readonly static string UserAlreadyMappedToHotspot = "User is already mapped to the given hotspot";

        //TagRepo
        public readonly static string TagsAddedSuccessfully = "Tags added successfully";
        public readonly static string AlreadyTagsExists = "No tags added as they already exist";

        //CommentRepo
        public readonly static string CommentAddedSuccessfully = "Comment added successfully";
        public readonly static string CommentAlreadyExists = "Comment already exists";
        public readonly static string InputCommentIsEmpty = "No comment provided as input";

        //ImageRepo
        public readonly static string ImagesAddedSuccessfully = "Images added successfully";
        public readonly static string ImagesAlreadyExists = "Images already exists";
        public readonly static string InputImageIsEmpty = "No image provided as input";

        //HotspotRepo
        public readonly static string HotspotAlreadyExists = "Hotspot already exists";
        public readonly static string HotspotAddedSuccessfully = "Hotspot added successfully";

        //Controller
        public readonly static string FailInvalidData = "Error in information provided.";

        
    }
}


