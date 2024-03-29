﻿using MobilApp_Szakdolgozat.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MobilApp_Szakdolgozat.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using MobilApp_Szakdolgozat.ViewModels;
using System.Net.Http.Headers;
using FFImageLoading.Args;
using MySqlX.XDevAPI;
//using ThreadNetwork;


namespace MobilApp_Szakdolgozat.Services
{
    public class DataService
    {
        public struct T
        {
            string token;
        }
        //     /\             (_)    | |/ /   | | |
        //    /  \    _ __ ___ _     | ' / ___| | |
        //   / /\ \ | '_ ` _ \| |    |  < / _ \ | |
        //  / ____ \| | | | | | |    | . \  __/ | |
        // /_/    \_\_| |_| |_|_|    |_|\_\___|_|_|
        // képváltáshoz:
        // - token eltárolás
        // - post token, post kép id (<--egyben)

        static string url202 = "http://10.0.22.14:9000";
        static string url303 = "http://10.0.33.12:9000";
        static string url103 = "http://10.0.13.5:9000";
        //Itthon 9090-es porton megy a szerver
        static string urlHome = "http://192.168.0.165:9000";
        static string url103local = "http://10.0.13.6:9090";
        static string url102local = "http://10.0.12.16:9000";
        static string url202local = "http://10.0.22.5:9090";
        static string url302local = "http://10.0.33.20:9090";
        public static string url = urlHome;

        public static async Task<IEnumerable<ProfileModel>> getAllProfiles()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var uri = "/users";
                var result = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<List<ProfileModel>>(result);
            }
        }

        public static async Task<IEnumerable<PictureCatalogModel>> getAllPictures()
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = "/pictures";
                var result = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<List<PictureCatalogModel>>(result);
            }
        }

        public static async Task<IEnumerable<AdsModel>> getAds()
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = "/ads";
                var result = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<List<AdsModel>>(result);
            }
        }

        public static async Task<IEnumerable<CountyModel>> getCounties()
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = "/counties";
                var result = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<List<CountyModel>>(result);
            }
        }

        public static async Task<IEnumerable<SettlementModel>> getSettlements()
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = "/settlements";
                var result = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<List<SettlementModel>>(result);
            }
        }

        public static async Task<IEnumerable<string>> getUploads()
        {
            using (var client = new HttpClient())
            {
                string token = await SecureStorage.GetAsync("userToken");
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(url);
                var uri = "/pictures/upload";
                var result = await client.GetStringAsync(uri);

                return JsonConvert.DeserializeObject<List<string>>(result);
            }
        }

        public static async Task<RegisterModel> register(RegisterModel user)
        {
            RegisterModel errorRegister = new RegisterModel();

            string jsonData = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url + "/register", content);

            // Debug.WriteLine(response.StatusCode);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 400)
            {
                //hiba
                dynamic errorObj = JsonConvert.DeserializeObject<dynamic>(result);
                foreach (JProperty variable in errorObj)
                {
                    if (variable.Name == "name")
                    {
                        errorRegister.name = variable.Value[0].ToString();
                    }
                    if (variable.Name == "email")
                    {
                        errorRegister.email = variable.Value[0].ToString();
                    }
                    if (variable.Name == "password")
                    {
                        errorRegister.password = variable.Value[0].ToString();
                    }
                }
            }
            return errorRegister;
        }

        public static async Task<string> login(string email, string password)
        {
            string jsonData = JsonConvert.SerializeObject(new { email = email, password = password });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url + "/login", content);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 401)
            {
                return "Helytelen bejelentkezés!";
            }
            else
            {
                //var a = JsonConvert.DeserializeObject<T>(result);
                string[] fullJWT = result.Split(':');
                var fullResult = fullJWT[1].Trim('"');
                string[] trimmedJWT = fullResult.Split('"');
                var trimmedResult = trimmedJWT[0]; //<-- ez kell tokenhez                
                string[] payload = trimmedResult.Split('.');
                var finalPayload = payload[1];

                var Result = JWTTokenService.DecodeJwt(JWTTokenService.ConvertJwtStringToJwtSecurityToken(trimmedResult));
                string[] ResultwithoutMustacheOne = Result[0].ToString().Split("{");
                string[] ResultwithoutMustachetwo = ResultwithoutMustacheOne[1].Split("}");
                string[] finalResult = ResultwithoutMustachetwo[0].Split(",");

                string userImage = null;

                foreach (string item in finalResult)
                {
                    string[] keyValue = item.Split(':');
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0].Trim('"');
                        string value = string.Join(":", keyValue.Skip(1)).Trim('"');

                        if (key == "pPic")
                        {
                            userImage = value;
                            break;
                        }
                    }
                }
                if (userImage != null)
                {
                    await SecureStorage.SetAsync("userImage", userImage);
                }
                await SecureStorage.SetAsync("userId", finalResult[0].Split(':')[1].Trim('"'));
                await SecureStorage.SetAsync("userName", finalResult[1].Split(':')[1].Trim('"'));
                await SecureStorage.SetAsync("userEmail", finalResult[2].Split(':')[1].Trim('"'));
                //await SecureStorage.SetAsync("userLocation", finalResult[].Split(':')[1].Trim('"'));
                await SecureStorage.SetAsync("userRole", finalResult[4].Split(':')[1].Trim('"'));
                await SecureStorage.SetAsync("userToken", trimmedResult);

                return null;

            }
        }

        public static async Task<string> profilePictureUpdate(int id, string picUrl)
        {
            string jsonData = JsonConvert.SerializeObject(new {
                id = id,
                pPic = picUrl });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PostAsync(url + "/pictures", content);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 401)
            {
                return "error";
            }
            else
            {
                string success = "success";
                await SecureStorage.SetAsync("success", success);
                return null;
            }
        }

        public static async Task<string> profileUpdate(int id, string userName, string userEmail, string userLocation, string userMobile)
        {
            string jsonData = JsonConvert.SerializeObject(new
            {
                id = id,
                nev = userName,
                email = userEmail,
                hely = userLocation,
                telefonszam = userMobile
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PostAsync(url + "/users/patch", content);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 401)
            {
                return "error";
            }
            else
            {
                string success = "success";
                await SecureStorage.SetAsync("success", success);
                return null;
            }
        }

        public static async Task<string> newAdvertisementUpload(string name, string description, string category, int price, int countyId, string settlement, int ownerId)
        {
            string jsonData = JsonConvert.SerializeObject(new {
                name = name,
                description = description,
                category = category,
                price = price,
                countyId = countyId,
                settlement = settlement,
                ownerId = ownerId
            });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.PostAsync(url + "/ads", content);
            string result = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 401)
            {
                return "error";
            }
            else
            {
                await SecureStorage.SetAsync("uploaded", true.ToString());
                return null;
            }
        }

        public static async Task<string> imageUpload(int userId, int adId, int imgId) 
        {
            var uploadFile = await MediaPicker.PickPhotoAsync();

            if (uploadFile == null) return "error";

            var httpContent = new MultipartFormDataContent();         
            string[] fileType = uploadFile.FileName.Split('.');
            uploadFile.FileName = $"{userId}_{adId}_{imgId}.{fileType[1]}";
            httpContent.Add(new StreamContent(await uploadFile.OpenReadAsync()), "file", uploadFile.FileName);
            
            var httpClient = new HttpClient();
            string token = await SecureStorage.GetAsync("userToken");
            httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
            var result = await httpClient.PostAsync($"{url}/pictures/upload", httpContent);
            var response = await result.Content.ReadAsStringAsync();
            //await Shell.Current.DisplayAlert("Response from server", response, "K");
            await SecureStorage.SetAsync("imgId", imgId.ToString());
            return "";
        }
        public class TokenResponse
        {
            public string Token { get; set; }
        }

    }
}
