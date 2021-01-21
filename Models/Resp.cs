using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.Models {
    public class RespBase<T> {
        public int Status { get; set; }
        public string Error { get; set; }
        public string Message { get; set; } // 一般错误信息
        public string ForwardUrl { get; set; } // 一般没有 未登录时=/account/login
    }

    public class Resp<T> : RespBase<T> {
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }

    public class CollectionResp<T>: RespBase<T> {
        [JsonProperty(PropertyName = "data")]
        public ICollection<T> Data { get; set; }
    }
}
