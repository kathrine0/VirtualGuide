﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


using VirtualGuide.Mobile.Model;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace VirtualGuide.Mobile.Helper
{
    public class HttpHelper
    {
        private static SettingsDataHelper settingsDataHelper = new SettingsDataHelper();

        public static async Task<T> GetData<T>(string webPath)
        {
            HttpClient client = new HttpClient();
            
            try
            {
                //authenticate request
                if (settingsDataHelper.KeyExists(SettingsDataHelper.TOKEN))
                {
                    var token = settingsDataHelper.GetValue<string>(SettingsDataHelper.TOKEN);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.GetAsync(App.WebService + webPath);


                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var result = ParseData<T>(responseBody);
                    return result;
                }
                else
                {
                    throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                client.Dispose();
            }

        }

        private static T ParseData<T>(string responseBody)
        {
            if (!String.IsNullOrEmpty(responseBody))
            {
                var result = JsonConvert.DeserializeObject<T>(responseBody);

                return result;
            }

            throw new ArgumentNullException("Response Body is null or empty");
        }

        public async static Task ImageDownloader<T>(List<T> items) where T : BaseImageModel
        {
            foreach (var item in items)
            {
                if (!String.IsNullOrEmpty(item.ImageSrc))
                {
                    var type = item.GetType().ToString();
                    var name = type.Substring(type.LastIndexOf('.')+1, type.Length - type.LastIndexOf('.')-1);

                    var filename = String.Format("{0}_{1}", name, GetFileName(item.ImageSrc));
                    await HttpHelper.Download(App.WebService + item.ImageSrc, filename, "images");
                    item.ImageSrc = filename;
                }
            }

            await App.Connection.UpdateAllAsync(items);
        }

        public async static Task MapDownloader(List<Travel> travels)
        {
            foreach (var travel in travels)
            {
                //var UriTemplate = "http://dev.virtualearth.net/REST/v1/Imagery/Map/Road/{0},{1}/{2}?mapSize={3},{4}&format=png&key={5}";
                //var Uri = String.Format(UriTemplate, travel.Latitude, travel.Longitude, travel.ZoomLevel+1, 500, 300, App.GmapsToken);

                var UriTemplate = "http://maps.googleapis.com/maps/api/staticmap?center={0},{1}&zoom={2}&size={3}x{4}&key={5}";
                var Uri = String.Format(UriTemplate, travel.Latitude, travel.Longitude, travel.ZoomLevel+1, 500, 300, App.GmapsToken);

                await Download(Uri, travel.Name+"map.png", "maps");
            }
        }

        public async static Task Download(string path, string filename, string folder)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var imageFolder = await localFolder.CreateFolderAsync(folder, CreationCollisionOption.OpenIfExists);

            if (String.IsNullOrEmpty(path)) return;

            Uri source = new Uri(path);

            try
            {
                StorageFile destinationFile = await imageFolder.CreateFileAsync(filename, CreationCollisionOption.FailIfExists);

                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(source, destinationFile);

                await download.StartAsync();

                ResponseInformation response = download.GetResponseInformation();
                Uri imageUri;
                BitmapImage image = null;

                if (Uri.TryCreate(destinationFile.Path, UriKind.RelativeOrAbsolute, out imageUri))
                {
                    image = new BitmapImage(imageUri);
                }
            }
            catch (Exception e)
            {
                //file already exists.
                return;
            }
        }

        private static string GetFileName(string path)
        {
            int slash = path.LastIndexOf("/");
            if (slash == -1) return path;

            string name = path.Substring(slash + 1, path.Length - slash - 1);

            return name;
        }
    }
}
