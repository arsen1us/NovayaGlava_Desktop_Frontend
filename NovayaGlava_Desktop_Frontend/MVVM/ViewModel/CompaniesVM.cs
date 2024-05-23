using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.Utilities;
using ClassLibForNovayaGlava_Desktop;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    class CompaniesVM
    {
        HttpClient _client;
        public CompaniesVM()
        {
            _client = HttpClientSingleton.Client;
        }
        //public ObservableCollection<Phone> Phones { get; set; } 
        //public CompaniesVM()
        //{

        //}

        //private void phonesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Phone currentPhone = (Phone)phonesList.
    }
    

    
}
