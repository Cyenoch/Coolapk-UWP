using Coolapk.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coolapk.Apis
{
    public class ListSortType
    {
        public const string LastupdateDesc = "lastupdate_desc"; // 默认排序
        public const string DatelineDesc = "dateline_desc"; // 按时间倒序
        public const string Popular = "popular"; // 按
    }

    public class FeedType
    {
        public const string FeedArticle = "feedArticle";
    }

    public class UploadFileFragment
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("resolution")]
        public string Resolution { get; set; }

        [JsonPropertyName("md5")]
        public string Md5 { get; set; }

        //public static async Task<UploadFileFragment> FromPictureFile(string filePath)
        //{
        //    var img = await BitmapDecoder.CreateAsync(File.OpenRead(filePath).AsRandomAccessStream());

        //    return new UploadFileFragment
        //    {
        //        Name = Path.GetFileName(filePath),
        //        Resolution = $"{img.PixelWidth}x{img.PixelHeight}",
        //        Md5 = GetMD5Hash(filePath),
        //    };
        //}
        public static string GetMD5Hash(string file)
        {
            byte[] computedHash = new MD5CryptoServiceProvider().ComputeHash(File.ReadAllBytes(file));
            var sBuilder = new StringBuilder();
            foreach (byte b in computedHash)
            {
                sBuilder.Append(b.ToString("x2").ToLower());
            }
            string result = sBuilder.ToString();
            return result;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            return obj is UploadFileFragment && Md5.Equals((obj as UploadFileFragment).Md5);
        }
    }

    public class OssUploadPicturePrepareBody
    {
        public IList<UploadFileFragment> UploadFileFragmentsSource { private get; set; }

        [AliasAs("uploadFileList")]
        public string UploadFileList
        {
            get => JsonConvert.SerializeObject(UploadFileFragmentsSource, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        [AliasAs("uploadBucket")]
        public string UploadBucket { get; set; } = "image";

        [AliasAs("uploadDir")]
        public string UploadDir { get; set; } = "feed"; // feed feed_cover

        [AliasAs("is_anonymous")]
        public uint IsAnonymous { get; set; } = 0;
    }

    public partial interface ICoolapkApis
    {

        [Post("/v6/upload/ossUploadPrepare")]
        Task<Resp<OssUploadPicturePrepareResult>> OssUploadPicturePrepare(
            [Body(BodySerializationMethod.UrlEncoded)] OssUploadPicturePrepareBody body
        );

        [Get("/v6/feed/detail")]
        Task<Resp<FeedDetail>> GetFeedDetail(uint id);


        [Get("/v6/feed/replyList")]
        Task<CollectionResp<FeedReply>> GetFeedReplyList(
            uint id,
            uint page = 1,
            string listType = ListSortType.LastupdateDesc,
            uint discussMode = 1,
            string feedType = FeedType.FeedArticle,
            uint blockStatus = 0,
            uint fromFeedAuthor = 0,
            uint? lastItem = null,
            uint? firstItem = null);

        [Post("/v6/feed/like")]
        Task<Resp<LikeActionResult>> DoLike(uint id);

        [Post("/v6/feed/unlike")]
        Task<Resp<LikeActionResult>> DoUnLike(uint id);


        //[Post("/v6/feed/createFeed")]
        //[Multipart]
        //Task<Resp<Unknow>> CreateHtmlArticleFeed(CreateHtmlArticleFeedBody body);

        [Post("/v6/feed/createFeed")]
        [Multipart]
        Task<Resp<Unknow>> CreateHtmlArticleFeed(string message,
            string messageTitle, string messageCover = "", string type = "feed", int isHtmlArticle = 1, string pic = "", int status = 1,
            string location = "", string longLocation = "", double latitude = 0.0, double longitude = 0.0, string mediaPic = "",
            string messageBrief = "", string extraTitle = "", string extraUrl = "", string extraKey = "", string extraPic = "",
            string extraInfo = "", int disallowRepost = 0, int isAnonymous = 0, int isEditindyh = 0, string forwardid = "", string fid = "",
            string dyhId = "", string targetType = "", string productId = "", string province = "", string cityCode = "", string targetId = "",
            string locationCity = "", string locationCountry = "", int voteScore = 0, int replyWithForward = 0, string mediaInfo = "",
            int insertProductMedia = 0);
    }
}
