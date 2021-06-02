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
        public static void SetupWideTile(string avatarUrl, string userName, uint fans)
        {
            var content = new TileContent();
            var tileContent = new TileContent()
            {
                Visual = new TileVisual()
                {
                    Branding = TileBranding.NameAndLogo,
                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new AdaptiveGroup()
                                {
                                    Children =
                                    {
                                        new AdaptiveSubgroup()
                                        {
                                            HintWeight = 34,
                                            Children =
                                            {
                                                new AdaptiveImage()
                                                {
                                                    HintCrop = AdaptiveImageCrop.Circle,
                                                    Source = avatarUrl
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup()
                                        {
                                            Children =
                                            {
                                                new AdaptiveText()
                                                {
                                                    Text = "Hi,",
                                                    HintStyle = AdaptiveTextStyle.Subtitle
                                                },
                                                new AdaptiveText()
                                                {
                                                    Text = userName,
                                                    HintStyle = AdaptiveTextStyle.SubtitleSubtle
                                                },
                                                new AdaptiveText()
                                                {
                                                    Text = $"{fans}粉丝",
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

//var tileContent = new TileContent()
//{
//    Visual = new TileVisual()
//    {
//        TileWide = new TileBinding()
//        {
//            Content = new TileBindingContentAdaptive()
//            {
//                Children =
//                {
//                    new AdaptiveGroup()
//                    {
//                        Children =
//                        {
//                            new AdaptiveSubgroup()
//                            {
//                                HintWeight = 33,
//                                Children =
//                                {
//                                    new AdaptiveImage()
//                                    {
//                                        HintCrop = AdaptiveImageCrop.Circle,
//                                        Source = "Assets/Apps/Hipstame/hipster.jpg"
//                                    }
//                                }
//                            },
//                            new AdaptiveSubgroup()
//                            {
//                                Children =
//                                {
//                                    new AdaptiveText()
//                                    {
//                                        Text = "Hi,",
//                                        HintStyle = AdaptiveTextStyle.Title
//                                    },
//                                    new AdaptiveText()
//                                    {
//                                        Text = "MasterHip",
//                                        HintStyle = AdaptiveTextStyle.SubtitleSubtle
//                                    }
//                                },
//                                HintTextStacking = AdaptiveSubgroupTextStacking.Center
//                            }
//                        }
//                    }
//                }
//            }
//        },
//        TileLarge = new TileBinding()
//        {
//            Content = new TileBindingContentAdaptive()
//            {
//                TextStacking = TileTextStacking.Center,
//                Children =
//                {
//                    new AdaptiveGroup()
//                    {
//                        Children =
//                        {
//                            new AdaptiveSubgroup()
//                            {
//                                HintWeight = 1
//                            },
//                            new AdaptiveSubgroup()
//                            {
//                                HintWeight = 2,
//                                Children =
//                                {
//                                    new AdaptiveImage()
//                                    {
//                                        HintCrop = AdaptiveImageCrop.Circle,
//                                        Source = "Assets/Apps/Hipstame/hipster.jpg"
//                                    }
//                                }
//                            },
//                            new AdaptiveSubgroup()
//                            {
//                                HintWeight = 1
//                            }
//                        }
//                    },
//                    new AdaptiveText()
//                    {
//                        Text = "Hi,",
//                        HintStyle = AdaptiveTextStyle.Title,
//                        HintAlign = AdaptiveTextAlign.Center
//                    },
//                    new AdaptiveText()
//                    {
//                        Text = "MasterHip",
//                        HintStyle = AdaptiveTextStyle.SubtitleSubtle,
//                        HintAlign = AdaptiveTextAlign.Center
//                    }
//                }
//            }
//        }
//    }
//};

//// Create the tile notification
//var tileNotif = new TileNotification(tileContent.GetXml());

//// And send the notification to the primary tile
//TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotif);