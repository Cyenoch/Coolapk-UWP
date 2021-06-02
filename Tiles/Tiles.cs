using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace Coolapk_UWP.Tiles
{
    public partial class TilesUtil
    {
        public static void SetupWideTile(
            string backgroundImageUrl,
            string avatarUrl,
            string userName,
            uint fans,
            uint unreadNum,
            string newestMsg,
            string newestFrom
            )
        {
            var tileContent = new TileContent()
            {
                Visual = new TileVisual()
                {
                    Branding = TileBranding.NameAndLogo,
                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage
                            {
                                Source = backgroundImageUrl,
                            },
                            Children = {
                                //new AdaptiveGroup() // 怎么自动切换啊....
                                //{
                                //    Children = {
                                //        new AdaptiveSubgroup()
                                //        {
                                //            Children = {
                                //                new AdaptiveText()
                                //                {
                                //                    Text = "最新未读消息",
                                //                    HintStyle = AdaptiveTextStyle.Subtitle
                                //                },
                                //                new AdaptiveText()
                                //                {
                                //                    Text = newestFrom == null ? "" : $"来自{newestFrom}:",
                                //                    HintStyle = AdaptiveTextStyle.Base
                                //                },
                                //                new AdaptiveText()
                                //                {
                                //                    Text = newestFrom == null ? "" : $"{newestMsg}",
                                //                    HintWrap = true,
                                //                    HintStyle = AdaptiveTextStyle.Caption
                                //                },
                                //            }
                                //        }
                                //    }
                                //},
                                new AdaptiveGroup()
                                {
                                    Children = {
                                        new AdaptiveSubgroup()
                                        {
                                            HintWeight = 35,
                                            Children = {
                                                new AdaptiveImage()
                                                {
                                                    HintCrop = AdaptiveImageCrop.Circle,
                                                    Source = avatarUrl
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup()
                                        {
                                            Children = {
                                                new AdaptiveText()
                                                {
                                                    Text = userName,
                                                    HintStyle = AdaptiveTextStyle.Subtitle
                                                },
                                                new AdaptiveText()
                                                {
                                                    Text = $"{fans}粉丝",
                                                    HintStyle = AdaptiveTextStyle.Caption
                                                },
                                                new AdaptiveText()
                                                {
                                                    Text = $"{unreadNum}未读消息",
                                                    HintStyle = AdaptiveTextStyle.Caption
                                                }
                                            },
                                            HintTextStacking = AdaptiveSubgroupTextStacking.Center
                                        }
                                    }
                                },
                            }
                        }
                    }
                }
            };

            //// Create the tile notification
            var tileNotif = new TileNotification(tileContent.GetXml());

            //// And send the notification to the primary tile
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotif);
        }
    }
}