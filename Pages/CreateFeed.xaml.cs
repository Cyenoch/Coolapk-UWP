using Coolapk_UWP.Models;
using Coolapk_UWP.ViewModels;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Coolapk_UWP.Pages
{
    public delegate void ReLayoutFunc();
    public sealed partial class CreateFeed : Page
    {

        public CreateFeed()
        {
            this.InitializeComponent();
            var cropperFlyout = FlyoutBase.GetAttachedFlyout(CoverPanel);
            cropperFlyout.Closed += CropperFlyout_Closed;
        }

        private void CropperFlyout_Closed(object sender, object e)
        {
            CoverCropper.Source = null;
            SaveCoverCropperBtnPanel.Children.Clear();
        }

        private async void TestButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var rawStruct = await MPicTextEditor.UploadAndGenerateStructModel();
            await this.ViewModel.CreateHtmlArticleFeed(rawStruct, this.TitleBox.Text);
        }

        private async void CoverButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            try
            {
                var picker = new Windows.Storage.Pickers.FileOpenPicker
                {
                    ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
                };
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");

                Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
                if (file == null) return;
                using (var fs = await file.OpenAsync(FileAccessMode.Read))
                {
                    var img = new BitmapImage();
                    await img.SetSourceAsync(fs);
                    fs.Seek(0);
                    var wb = new WriteableBitmap(img.PixelWidth, img.PixelHeight);
                    await wb.SetSourceAsync(fs);

                    CoverCropper.Source = wb;
                    FlyoutBase.ShowAttachedFlyout(CoverPanel);

                    var saveBtn = new Button
                    {
                        Content = "保存"
                    };
                    SaveCoverCropperBtnPanel.Children.Add(saveBtn);

                    saveBtn.Click += async (object _sender, RoutedEventArgs args) =>
                    {
                        var fname = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpeg";
                        var tempFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fname);
                        using (var tfs = await tempFile.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            await CoverCropper.SaveAsync(tfs, BitmapFileFormat.Jpeg);
                            ViewModel.CoverSourceFile = tempFile;
                            var nimg = new BitmapImage();
                            nimg.UriSource = new Uri("ms-appdata:///temp/" + fname);
                            ViewModel.Cover = nimg;
                        }
                        FlyoutBase.GetAttachedFlyout(CoverPanel).Hide();
                    };

                }

            }
            catch (Exception err) { }
        }

        private async void InsertImageButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            // 让textnode记录位置
            if (MPicTextEditor.NodeList.FocusIndex != -1) MPicTextEditor.NodeList.FlagLosingFocus = true;
            try
            {
                var picker = new Windows.Storage.Pickers.FileOpenPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");

                Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        byte[] computedHash = new MD5CryptoServiceProvider().ComputeHash(fileStream.AsStream());
                        var sBuilder = new StringBuilder();
                        foreach (byte b in computedHash)
                        {
                            sBuilder.Append(b.ToString("x2").ToLower());
                        }
                        string md5 = sBuilder.ToString();
                        MPicTextEditor.NodeList.AutoInsert(new Controls.PicTextEditor.ImageNode(MPicTextEditor.NodeList, file, md5) { });
                    }
                }
            }
            catch (Exception _)
            {
                MPicTextEditor.NodeList.FlagLosingFocus = false;
            }
        }
    }
}
