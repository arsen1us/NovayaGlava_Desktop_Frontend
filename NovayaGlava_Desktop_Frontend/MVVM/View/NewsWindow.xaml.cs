using ClassLibForNovayaGlava_Desktop.Comments;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NovayaGlava_Desktop_Frontend.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для NewsWindow.xaml
    /// </summary>
    public partial class NewsWindow : UserControl
    {
        public ObservableCollection<PostViewModel> Posts { get; set; }

        public NewsWindow()
        {
            InitializeComponent();
            Posts = new ObservableCollection<PostViewModel>
            {
                new PostViewModel(new Post
                {
                    Text = "Первый пост",
                    ImagePaths = new List<string>
                    {
                        "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\4kUltraHdMountainsWallpaper.jpg",
                        "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\catioRing7.jpg",
                        "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\door_multicolored_tokyo_121870_1920x1080.jpg",
                    }
                },
                new List<CommentModel>
                {
                    new CommentModel{Text = "First comment", UserId = "first user id comment"},
                    new CommentModel{Text = "Second comment", UserId = "second user id comment"},
                    new CommentModel{Text = "First comment", UserId = "first user id comment"},
                    new CommentModel{Text = "Second comment", UserId = "second user id comment"},
                    new CommentModel{Text = "First comment", UserId = "first user id comment"},
                    new CommentModel{Text = "Second comment", UserId = "second user id comment"},
                },
                new User
                {
                    Name = "Arseniy",
                    ImagePath = "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\defaultAvatar.png",
                    PublicationTime = DateTime.Now.ToShortDateString()
                }),
                new PostViewModel(new Post
                {
                    Text = "Второй пост",
                    ImagePaths = new List<string>
                    {
                        "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\4kUltraHdMountainsWallpaper.jpg",
                        "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\catioRing7.jpg",
                        "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\door_multicolored_tokyo_121870_1920x1080.jpg",
                    }

                },
                new List<CommentModel>
                {
                    new CommentModel{Text = "First comment", UserId = "first user id comment"},
                    new CommentModel{Text = "Second comment", UserId = "second user id comment"},
                    new CommentModel{Text = "First comment", UserId = "first user id comment"},
                    new CommentModel{Text = "Second comment", UserId = "second user id comment"},
                    new CommentModel{Text = "First comment", UserId = "first user id comment"},
                    new CommentModel{Text = "Second comment", UserId = "second user id comment"},
                },
                new User
                {
                    Name = "Arseniy",
                    ImagePath = "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\defaultAvatar.png",
                    PublicationTime = DateTime.Now.ToShortDateString()
                }),
            };

            DataContext = this;
            PostsList.ItemsSource = Posts;
        }

        private void PreviousImage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var postViewModel = button.DataContext as PostViewModel;
                postViewModel?.PreviousImage();
            }
        }

        private void NextImage_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var postViewModel = button.DataContext as PostViewModel;
                postViewModel?.NextImage();
            }
        }

        private void GenerateNewpost()
        {
            PostViewModel newPost = new PostViewModel
            (
                new Post
                {
                    Text = "newPost",
                    ImagePaths = new List<string>
                    {
                        "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\4kUltraHdMountainsWallpaper.jpg",
                        "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\catioRing7.jpg",
                        "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\door_multicolored_tokyo_121870_1920x1080.jpg",
                    }
                },
                new List<CommentModel>
                {
                    new CommentModel { Text = "First comment", UserId = "first user id comment" },
                    new CommentModel { Text = "Second comment", UserId = "second user id comment" },
                    new CommentModel { Text = "First comment", UserId = "first user id comment" },
                    new CommentModel { Text = "Second comment", UserId = "second user id comment" },
                    new CommentModel { Text = "First comment", UserId = "first user id comment" },
                    new CommentModel { Text = "Second comment", UserId = "second user id comment" },
                },
                new User
                {
                    Name = "Arseniy",
                    ImagePath = "C:\\Users\\gamer\\source\\repos\\MultiWindowWpfApp\\MultiWindowWpfApp\\Images\\defaultAvatar.png",
                    PublicationTime = DateTime.Now.ToShortDateString()
                }
            );
            Posts.Add(newPost);
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;

            if (scrollViewer.ExtentHeight - scrollViewer.ViewportHeight == scrollViewer.VerticalOffset)
                GenerateNewpost();
        }
    }

    public class PostViewModel : INotifyPropertyChanged
    {
        private Post _post;
        private int _currentImageIndex;

        public List<CommentModel> Comments { get; set; }
        public User User { get; set; }
        public string UserName => User.Name;
        public string ImageSrc => User.ImagePath;
        public string PublicationTime => User.PublicationTime;

        public PostViewModel(Post post, List<CommentModel> comments, User user)
        {
            _post = post;
            _currentImageIndex = 0;
            Comments = comments;
            User = user;
        }

        public string Text => _post.Text;

        public string CurrentImage
        {
            get
            {
                if (_post.ImagePaths.Count > 0)
                {
                    return _post.ImagePaths[_currentImageIndex];
                }
                return null;
            }
        }

        public void PreviousImage()
        {
            if (_post.ImagePaths.Count > 0)
            {
                _currentImageIndex = (_currentImageIndex - 1 + _post.ImagePaths.Count) % _post.ImagePaths.Count;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        public void NextImage()
        {
            if (_post.ImagePaths.Count > 0)
            {
                _currentImageIndex = (_currentImageIndex + 1) % _post.ImagePaths.Count;
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class User
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string PublicationTime { get; set; } = DateTime.Now.ToString();
    }

    public class Post
    {
        public string Text { get; set; }
        public List<string> ImagePaths { get; set; }   
    }
}
