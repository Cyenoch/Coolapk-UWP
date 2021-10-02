using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk.Models
{
    public class User : Entity
    {
        public uint Uid { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public uint Follow { get; set; }
        public uint Fans { get; set; }
        [JsonProperty(PropertyName = "userAvatar")]
        public string Avatar { get; set; }
        [JsonProperty(PropertyName = "userSmallAvatar")]
        public string SmallAvatar { get; set; }
        [JsonProperty(PropertyName = "userBigAvatar")]
        public string BigAvatar { get; set; }
        public string Cover { get; set; }
        [JsonProperty(PropertyName = "be_like_num")]
        public uint GotLike { get; set; }
        public string Astro { get; set; } // 星座
        public string City { get; set; }
        public string Province { get; set; }
        public ICollection<Entity> HomeTabCardRows { get; set; }
    }
    public class UserProfile : User
    {

    }
}
