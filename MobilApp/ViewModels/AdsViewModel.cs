﻿using MobilApp_Szakdolgozat.Models;
using MobilApp_Szakdolgozat.Services;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilApp_Szakdolgozat.ViewModels
{
    public partial class AdsViewModel: BindableObject, IQueryAttributable
    {
        public ObservableCollection<AdsModel> filteredAds { get; set; }        
        public ObservableCollection<string> uploadFileNames { get; set; }
        public AdsModel advertisement { get; set; }
       
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            uploadFileNames = new ObservableCollection<string>();
            filteredAds = query["filteredAds"] as ObservableCollection<AdsModel>;
            await getAllUploads();
            for (int i = 0; i < filteredAds.Count(); i++)
            {
                for (int y = 0; y < uploadFileNames.Count(); y++)
                {
                    string[] nameWithoutFileType = uploadFileNames[y].Split('.');
                    string[] nameParts = nameWithoutFileType[0].Split('_');
                    //nameParts[0] = UserId
                    //nameParts[1] = AdId
                    //nameParts[2] = ImgId
                    if (filteredAds[i].id == Int32.Parse(nameParts[1]))
                    {
                        filteredAds[i].adImages.Add($"{DataService.url}/pictures/upload/{nameParts[0]}_{nameParts[0]}_{nameParts[0]}.{nameWithoutFileType[1]}");
                    }
                }               
            }            
            OnPropertyChanged(nameof(filteredAds));
        }
        private async Task getAllUploads()
        {
            uploadFileNames.Clear();
            IEnumerable<string> list = await DataService.getUploads();
            list.ToList().ForEach(fn => uploadFileNames.Add(fn));
        }
    }
}
