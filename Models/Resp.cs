using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.Models
{
    public class RespBase<T>
    {
        public int Status { get; set; }
        public string Error { get; set; }
        public string Message { get; set; } // 一般错误信息
        public string ForwardUrl { get; set; } // 一般没有 未登录时=/account/login
        [JsonProperty(PropertyName = "data")]
        public virtual T Data { get; set; }
    }

    public class Resp<T> : RespBase<T> { }

    public class CollectionResp<T> : RespBase<IList<T>> { }

    public class NotificationList : CollectionResp<Entity>
    {
        public NotificationList()
        {
            if (this.Data != null)
            {
                var tmp = new List<object>();
                foreach (var entity in this.Data)
                {
                    entity.AutoCast();
                    this.Data.Remove(entity);
                    tmp.Add(entity.AutoCast());
                }
                this.Data = (IList<Entity>)tmp;
            }
        }
    }
}
