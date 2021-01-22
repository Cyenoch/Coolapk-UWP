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

        public void OnPiovtSelect(string piovt = "主页") {
            // default "主页"
            Func<LoadMoreItemsAsyncFuncConfig, Task<ICollection<Entity>>> func = async (config) => {
                if (config.Page != 1) { return new Collection<Entity>(); }// 如果不是第一页 让它以为这是空的
                return this.Data?.HomeTabCardRows ?? new Collection<Entity>();
            };
            switch (piovt) {
                case "动态":
                    func = async (config) => {
                        return (await this.CoolapkApis.GetUserFeedList(this.Data.Uid, config.Page, config.LastItem)).Data;
                    };
                    break;
                case "点评":
                    func = async (config) => {
                        return (await this.CoolapkApis.GetDataList($"/feed/nodeRatingList?targetType=all&parseRatingToFeed=1&uid={this.Data.Uid}", "", config.Page, config.LastItem)).Data;
                    };
                    break;
                case "图文":
                    func = async (config) => {
                        return (await this.CoolapkApis.GetUserHtmlFeedList(this.Data.Uid, config.Page, config.LastItem)).Data;
                    };
                    break;
                case "问答":
                    func = async (config) => {
                        return (await this.CoolapkApis.GetUserHtmlFeedList(this.Data.Uid, config.Page, config.LastItem)).Data;
                    };
                    break;
                case "酷图":
                    func = async (config) => {
                        return (await this.CoolapkApis.GetDataList($"/feed/userCoolPictureFeedList?fragmentTemplate=flex&uid={this.Data.Uid}", "", config.Page, config.LastItem)).Data;
                    };
                    break;
                case "好物":
                    func = async (config) => {
                        return (await this.CoolapkApis.GetDataList($"/goods/goodsFeedList?type=default&fragmentTemplate=flex&uid={this.Data.Uid}", "", config.Page, config.LastItem)).Data;
                    };
                    break;
                case "好物单":
                    func = async (config) => {
                        return (await this.CoolapkApis.GetUserGoodsList(this.Data.Uid, config.Page)).Data;
                    };
                    break;
                case "收藏单":
                    func = async (config) => {
                        return (await this.CoolapkApis.GetUserCollections(this.Data.Uid, config.Page)).Data;
                    };
                    break;
            }
            Entities = new IncrementalLoadingEntityCollection<Entity>(func);
            NotifyChanged("Entities");
        }

        public IList<string> _userTag;
        public IList<string> UserTag {
            get {
                return new List<string>() {
                User?.Astro,
                User?.City + User?.Province,
            }.Where(item => item?.Length > 1).ToList();
            }
        }

        public User User {
            get { return Data; }
        }

        public IncrementalLoadingEntityCollection<Entity> Entities;

        public override async Task<User> OnLoadAsync() {
            await Task.Delay(1000);
            var data = (await CoolapkApis.GetUser(1412645)).Data;
            OnPiovtSelect();
            return data;
        }

        override public string[] NotifyChangedProperties() {
            return new string[] { "User", "UserTag" };
        }
    }
}
