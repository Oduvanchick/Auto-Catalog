using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp_CourseProject2.Infrustructure;
using WpfApp_CourseProject2.Views;

namespace WpfApp_CourseProject2.ViewModels
{
    class CurrentUserViewModel : MainViewModel
    {
        UserControl currentUserView;
        UserControl generalUserView;
        UserControl favoriteUserView;
        public UserControl CurrentUserView
        {
            get => currentUserView;
            set
            {
                currentUserView = value;
                NotifyOfPropertyChanged();
            }
        }
        public CurrentUserViewModel()
        {
            CurrentUserView = new GeneralUserView();

            ShowGeneralUserPageCommand = new RelayCommand(x =>
            {
                CurrentUserView = generalUserView ?? (generalUserView = new GeneralUserView());
            });
            ShowFavoritesCommand = new RelayCommand(x =>
            {
                CurrentUserView = favoriteUserView ?? (favoriteUserView = new FavouriteCarsPage());
            });
        }
        public ICommand ShowGeneralUserPageCommand { get; set; }
        public ICommand ShowFavoritesCommand { get; set; }
        //public ICommand ShowSettingsPageCommand { get; set; }
    }
}
