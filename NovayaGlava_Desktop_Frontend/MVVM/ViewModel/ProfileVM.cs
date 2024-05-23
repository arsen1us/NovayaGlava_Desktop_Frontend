using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;
using Newtonsoft.Json;
using System.IO;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.Utilities;
using ClassLibForNovayaGlava_Desktop;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    class ProfileVM
    {
        UserIdHandler _userIdHandler;
        private UserModel _currentUser { get; set; }

        public RelayCommand GetContentForAll {  get; set; }
        public RelayCommand GetContentForFriends { get; set; }
        public RelayCommand GetContentForSubscribers { get; set; }
        public ObservableCollection<PostModel> Posts { get; set; }

        HttpClient _client;

        public ProfileVM()
        {
            _client = HttpClientSingleton.Client;
            _userIdHandler = new UserIdHandler();

            Posts = new ObservableCollection<PostModel>
            {
                new PostModel()
            };

            GetContentForAll = new RelayCommand(async o => await GetContentForAllFromDb());
            GetContentForFriends = new RelayCommand(async o => await GetContentForFriendsFromDb());
            GetContentForSubscribers = new RelayCommand(async o => await GetContentForSubscribersFromDb());

            Task.Run(async () => _currentUser = await GetUserByIdLocalDb());
        }

        public UserModel CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        }

        // Получить юзера по id из локальной бд
        private async Task<UserModel> GetUserByIdLocalDb()
        {
            HttpClient client = new HttpClient();
            string userId = _userIdHandler.GetFromCache();

            HttpResponseMessage response = await client.GetAsync($"https://localhost:7142/api/users/userById/localdb?userId={userId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Не удалось получить юзера по id. Status code - {response.StatusCode}");
            }
            else
            {
                string jsonUser = await response.Content.ReadAsStringAsync();
                UserModel user = JsonConvert.DeserializeObject<UserModel>(jsonUser);
                return user;
            }

        }

        private async Task GetContentForAllFromDb()
        {
            throw new Exception();
        }
        private async Task GetContentForFriendsFromDb()
        {
            throw new Exception();
        }
        private async Task GetContentForSubscribersFromDb()
        {
            throw new Exception();
        }

    }

    //Шаблон поста
    public class PostModel
    {
        public string AvatarImageSrc { get; set; } = "C:\\Users\\gamer\\source\\repos\\NovayaGlava-Desktop\\WPFForTest\\Images\\default_avatar.png";
        public string UserName { get; set; } = "Arseniy";
        public string TimePublicationOfThePost { get ; set; } = DateTime.Now.ToString();
        public string Discription { get; set; } = @"С другой стороны реализация намеченных плановых заданий играет важную роль
                                                    в формировании модели развития.Таким образом новая модель организационной
                                                    деятельности позволяет оценить значение дальнейших направлений развития.
                                                    Идейные соображения высшего порядка, а также укрепление и развитие структуры
                                                    обеспечивает широкому кругу (специалистов) участие в формировании дальнейших
                                                    направлений развития.";
        public string PostPhoto { get; set; } = "C:\\Users\\gamer\\source\\repos\\NovayaGlava-Desktop\\WPFForTest\\Images\\default_post_image.jpg";
    }
}

//Асинхронный метод получения пользователя по id. ХЗ как его заюзать в синхронном методе

//HttpClient client = new HttpClient();
//HttpResponseMessage response = await client.GetAsync($"https://localhost:7142/api/users/userById/localdb/{_userId}");
//if (response.StatusCode != HttpStatusCode.OK)
//    MessageBox.Show("Ошибка запроса. Не удалось получить пользователя по id");

//string jsonUser = await response.Content.ReadAsStringAsync();
//_currentUser =  JsonConvert.DeserializeObject<UserModel>(jsonUser);
