using Coolapk_UWP.Models;
using Coolapk_UWP.Other;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolapk_UWP.ViewModels {

    public class UserProfileViewModel : AsyncLoadViewModel<User> {

        public ObservableCollection<string> Pivots = new ObservableCollection<string>() {
            "主页",
            "动态",
            "点评",
            "图文",
            "问答",
            "酷图",
            "好物",
            "好物单",
            "收藏单",
        };

        public IList<string> _userTag;
        public IList<string> UserTag {
            get {
                return new List<string>() {
                User?.City + User?.Astro,
                User?.Province,
            }.Where(item => item?.Length > 1).ToList();
            }
        }

        public User User {
            get { return Data; }
        }

        public override async Task<User> OnLoadAsync() {
            await Task.Delay(1000);
            return (await CoolapkApis.GetUser(1412645)).Data;
        }

        override public string[] NotifyChangedProperties() {
            return new string[] { "User", "UserTag" };
        }
    }
}
