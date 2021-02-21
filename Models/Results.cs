using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.Models {
    public class LikeActionResult {
        public uint Count { get; set; }
    }

    public class OssUploadPictureResponse {
        public string Url { get; set; }
    }

    public class OssUploadPicturePrepareResult {
        public IList<OssUploadPicturePrepareResultFileInfo> FileInfo { get; set; }
        [JsonProperty(Required = Required.Default)]
        public OssUploadPicturePrepareResultUploadPrepareInfo UploadPrepareInfo { get; set; }

    }
    public class OssUploadPicturePrepareResultFileInfo {
        public string Name { get; set; }
        public string Resolution { get; set; }
        public string Md5 { get; set; }
        public string Url { get; set; }
        public string UploadFileName { get; set; }
    }
    public class OssUploadPicturePrepareResultUploadPrepareInfo {
        public string AccessKeySecret { get; set; }
        public string AccessKeyId { get; set; }
        public string SecurityToken { get; set; }
        public string Expiration { get; set; }
        //public IList Process { get; set; } unknow
        public string UploadImagePrefix { get; set; }
        public string EndPoint { get; set; }
        public string Bucket { get; set; }
        public string CallbackUrl { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> OtherField { get; set; }
    }
}
