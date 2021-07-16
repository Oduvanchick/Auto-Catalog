using DAL;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WpfApp_CourseProject2.Infrustructure;
using WpfApp_CourseProject2.Models;
using WpfApp_CourseProject2.ViewModels;
using WpfApp_CourseProject2.Views;

namespace WpfApp_CourseProject2
{
    class MainViewModel : BaseNotifyPropertyChanged
    {
        public ObservableCollection<Car> Cars { get; set; }
        public ObservableCollection<Images> Images1 { get; set; }
        public ObservableCollection<Transmissions> Transmissions { get; set; }
        public ObservableCollection<TypeOfDrive> TypeOfDrives { get; set; }

        public ObservableCollection<UserCar> UserCars { get; set; }
        public ObservableCollection<Car> CurrentUsersCar { get; set; }

        public CollectionView CarsView { get; set; }

        Car selectedCar;
        UserCar selectedUserCar;
        UserControl currentView;
        public UserControl CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                NotifyOfPropertyChanged();

            }

        }
        public ICommand SortCommand { get; set; }
        public ICommand DSortCommand { get; set; }
        public ICommand ShowSignUserViewCommand { get; set; }
        public ICommand SignInCommand { get; set; }
        public ICommand SignUpCommand { get; set; }
        public ICommand SignUpViewCommand { get; set; }
        public ICommand BackToSyteCommand { get; set; }
        public ICommand BackToUserCommand { get; set; }
        public ICommand BackToAdminCommand { get; set; }
        public ICommand AddNewUserCommand { get; set; }
        public ICommand AddNewCarCommand { get; set; }
        public ICommand ShowSignViewCommand { get; set; }
        public ICommand MouseDoubleClickCommand { get; set; }
        public ICommand UMouseDoubleClickCommand { get; set; }
        public ICommand AMouseDoubleClickCommand { get; set; }
        public ICommand AdminMouseDoubleClickCommand { get; set; }
        public ICommand UserMouseDoubleClickCommand { get; set; }
        public ICommand ChangeTheme { get; set; }
        public Car SelectedCar
        {
            get
            {
                return selectedCar;
            }
            set
            {
                selectedCar = value;
                NotifyOfPropertyChanged();          
            }
        }
        public UserCar SelectedUserCar
        {
            get
            {
                return selectedUserCar;
            }
            set
            {
                selectedUserCar = value;
                NotifyOfPropertyChanged();
            }
        }
        string userLogin;
        
        Users currentUser;
        public Users CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;

                NotifyOfPropertyChanged();
            }

        }
        Users newUser;
        Car newCar;
        public Car NewCar
        {
            get => newCar;
            set
            {
                newCar = value;

                NotifyOfPropertyChanged();
            }

        }
        public Users NewUser
        {
            get => newUser;
            set
            {
                newUser = value;

                NotifyOfPropertyChanged();
            }

        }


        public string UserLogin
        {
            get => userLogin;
            set
            {
                userLogin = value;

                NotifyOfPropertyChanged();
            }

        }
        Theme currentTheme;
        public Theme CurrentTheme
        {
            get => currentTheme;
            set
            {
                currentTheme = value;

                NotifyOfPropertyChanged();
            }

        }
        private async Task SendEmailAsync()
        {
            SmtpClient Smtp = new SmtpClient("gmail.com", 587);
            Smtp.Credentials = new NetworkCredential("mail.to.nata.vas@gmail.com", "satisfaction31052000");
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("mail.to.nata.vas@gmail.com");
            Message.To.Add(new MailAddress("mail.to.nata.vas@gmail.com"));
            Message.Subject = "Тема письма ";
            Message.Body = "Содержание";
            //Smtp.Send(Message);
            await Smtp.SendMailAsync(Message);
            //MailAddress from = new MailAddress("mail.to.nata.vas@gmail.com", "AutoCatalog");
            //MailAddress to = new MailAddress(NewUser.Mail);
            //MailMessage m = new MailMessage(from, to);
            //m.Subject = "Signing in AutoCatalog";
            //m.Body = $"Hello, {NewUser.UserName}! Wish you find CAR of your dream!";
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //smtp.Credentials = new NetworkCredential("mail.to.nata.vas@gmail.com", "satisfaction31052000");
            //smtp.EnableSsl = true;
            //await smtp.SendMailAsync(m);
            //Console.WriteLine("Письмо отправлено");
        }
        public MainViewModel()
        {
            List<Theme> themes = new List<Theme>();
            themes.Add(new Theme
            {
                Title = "Classic",
                Color = "Gray"
            });
            themes.Add(new Theme
            {
                Title = "Light",
                Color = "LightPink"
            });
            CurrentTheme = new Theme();
            CurrentTheme = themes[0];

            AutoCatalogContext1 context = new AutoCatalogContext1();
            CarRepository db = new CarRepository(context);
            Cars = new ObservableCollection<Car>(db.GetAll());
            CarsView = (CollectionView)CollectionViewSource.GetDefaultView(Cars);            
            Transmissions = new ObservableCollection<Transmissions>(context.Transmissions.ToList());
            TypeOfDrives = new ObservableCollection<TypeOfDrive>(context.TypeOfDrive.ToList());
            Images1 = new ObservableCollection<Images>(context.Images.ToList());

            CurrentView = new GeneralSyteView();
            newUser = new Users();
            newCar = new Car();
            CurrentUser = new Users();

            ChangeTheme = new RelayCommand(x =>
            {
                for (int i = 0; i < themes.Count; i++)
                {
                    if (themes[i].Title == x.ToString())
                    {
                        CurrentTheme = themes[i];
                        break;
                    }

                }
            });

            SortCommand = new RelayCommand(x =>
            {
                string property = x.ToString();
                if (property == "all")
                {
                    CarsView.SortDescriptions.Clear();
                    CarsView = (CollectionView)CollectionViewSource.GetDefaultView(Cars);
                    CarsView.Refresh();
                }
                else
                {
                    CarsView.SortDescriptions.Clear();
                    CarsView.SortDescriptions.Add(new SortDescription(property, ListSortDirection.Ascending));
                    CarsView.Refresh();
                }
            });
            DSortCommand = new RelayCommand(x =>
            {
                string property = x.ToString();
                CarsView.SortDescriptions.Clear();
                CarsView.SortDescriptions.Add(new SortDescription(property, ListSortDirection.Descending));
                CarsView.Refresh();
            });
            ShowSignUserViewCommand = new RelayCommand(x =>
            {
                CurrentView = new SignUserView();
            });

            MouseDoubleClickCommand = new RelayCommand(x =>
            {
                Images1 = new ObservableCollection<Images>(context.Images.ToList().Where(q => q.IDCar == SelectedCar.ID));
                NotifyOfPropertyChanged();
                CurrentView = new InfoCarView();
            });
            AdminMouseDoubleClickCommand = new RelayCommand(x =>
            {
                Images1 = new ObservableCollection<Images>(context.Images.ToList().Where(q => q.IDCar == SelectedCar.ID));
                CurrentAdminView = new InfoCarAdminView();

            });
            UserMouseDoubleClickCommand = new RelayCommand(x =>
            {
                Images1 = new ObservableCollection<Images>(context.Images.ToList().Where(q => q.IDCar == SelectedCar.ID));
                CurrentUserView = new InfoCarUserView();

            });

            BackToSyteCommand = new RelayCommand(x =>
            {
                CurrentUser = null;
                CurrentTheme = themes[0];
                NotifyOfPropertyChanged();
                CurrentView = new GeneralSyteView();
            });
            BackToUserCommand = new RelayCommand(x =>
            {
                CurrentUserView = new GeneralUserView();
            });
            BackToAdminCommand = new RelayCommand(x =>
            {
                CurrentAdminView = new GeneralAdminView();
            });
            SignInCommand = new RelayCommand(x =>
            {
                SendEmailAsync().GetAwaiter();
                var passwordBox = x as PasswordBox;

                CurrentUser = context.Users.ToList().FirstOrDefault(z => z.UserName == UserLogin && z.UserPassword == passwordBox.Password.ToString());
                if (CurrentUser != null)
                {
                    if (CurrentUser.Edition == false)
                    {
                        CurrentView = new CurrentUserView();
                        CurrentUserView = new GeneralUserView();
                        UserCars = new ObservableCollection<UserCar>(context.UserCar.ToList().Where(q => q.IDUser == CurrentUser.ID));
                        ShowGeneralUserPageCommand = new RelayCommand(z =>
                        {
                            NotifyOfPropertyChanged();
                            CurrentUserView = generalUserView ?? (generalUserView = new GeneralUserView());
                        });
                        ShowFavoritesCommand = new RelayCommand(z =>
                        {
                            NotifyOfPropertyChanged();

                            CurrentUserView = favoriteUserView ?? (favoriteUserView = new FavouriteCarsPage());
                            
                        });
                    }
                    else
                    {
                        CurrentView = new AdminView();
                        CurrentAdminView = new GeneralAdminView();
                        UserCars = new ObservableCollection<UserCar>(context.UserCar.ToList().Where(q => q.IDUser == CurrentUser.ID));
                        ShowGeneralAdminPageCommand = new RelayCommand(z =>
                        {
                            newCar = null;
                            NotifyOfPropertyChanged();
                            CurrentAdminView = generalAdminView ?? (generalAdminView = new GeneralAdminView());
                        });
                        ShowFavorites2Command = new RelayCommand(z =>
                        {
                            NotifyOfPropertyChanged();

                            CurrentAdminView = favAdminView ?? (favAdminView = new FavouriteCarsPage());
                        });

                        ShowNewCarCommand = new RelayCommand(z =>
                        {
                            NotifyOfPropertyChanged();
                            CurrentAdminView = newCarView ?? (newCarView = new NewCarView());
                        });
                        ShowEditCarCommand = new RelayCommand(z =>
                        {
                            
                            NotifyOfPropertyChanged();
                            CurrentAdminView = editCarView ?? (editCarView = new EditCarView());
                        });
                    }
                }
            });
            SignUpViewCommand = new RelayCommand(x =>
            {
                CurrentView = new NewUserView();
            });
            AddNewUserCommand = new RelayCommand(x =>
            {
                //SendEmailAsync().GetAwaiter();
                // доделать исключения
                UsersRepository uR = new UsersRepository(context);
                var passwordBox = x as PasswordBox;
                NewUser.UserPassword = passwordBox.Password.ToString();
                NewUser.Edition = false;
                uR.CreateOrUpdate(NewUser);
                uR.SaveChanges();
                //Users = db.Users.ToList();
                CurrentView = new SignUserView();
            });
            AddNewCarCommand = new RelayCommand(x =>
            {
                // доделать исключения
                CarRepository cR = new CarRepository(context);
                cR.CreateOrUpdate(NewCar);
                cR.SaveChanges();
                //Users = db.Users.ToList();
                NotifyOfPropertyChanged();
                CarRepository db1 = new CarRepository(context);
                Cars = new ObservableCollection<Car>(db1.GetAll());
                CarsView = (CollectionView)CollectionViewSource.GetDefaultView(Cars);
                CurrentAdminView = new GeneralAdminView();
            });
            ShowSignViewCommand = new RelayCommand(x =>
            {
                CurrentView = new SignUserView();
            });
            AddToFavoritesCommand = new RelayCommand(z =>
            {

                if (context.UserCar.ToList().FirstOrDefault(k => k.IDUser == CurrentUser.ID && k.IDCar == selectedCar.ID) == null)
                {
                    context.UserCar.AddOrUpdate(new UserCar
                    {
                        IDUser = CurrentUser.ID,
                        IDCar = selectedCar.ID
                    });
                    context.SaveChanges();
                    MessageBox.Show("Car successfully added to favorites!", "Message");
                }
                else
                    MessageBox.Show("Car already in favorites!", "Message");
                NotifyOfPropertyChanged();
                UserCars = new ObservableCollection<UserCar>(context.UserCar.ToList().Where(q => q.IDUser == CurrentUser.ID));
                
            });
            DelFromFavoritesCommand = new RelayCommand(z =>
            {
                context.UserCar.Remove(context.UserCar.ToList().FirstOrDefault(w => w.IDCar == SelectedUserCar.IDCar));
                context.SaveChanges();
                NotifyOfPropertyChanged();
                UserCars = new ObservableCollection<UserCar>(context.UserCar.ToList().Where(q => q.IDUser == CurrentUser.ID));
                CurrentUserView = new FavouriteCarsPage();
                CurrentAdminView = new FavouriteCarsPage();
            });
            DelCarCommand = new RelayCommand(z =>
            {
                MessageBoxResult result = MessageBox.Show("Do you want delete this item?", "Delete", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    context.Car.Remove(selectedCar);
                    context.SaveChanges();
                    NotifyOfPropertyChanged();
                    CarRepository db1 = new CarRepository(context);
                    Cars = new ObservableCollection<Car>(db1.GetAll());
                    CarsView = (CollectionView)CollectionViewSource.GetDefaultView(Cars);
                    CurrentAdminView = new GeneralAdminView();
                }
            });
            EditCarCommand = new RelayCommand(z =>
            {
                context.Car.AddOrUpdate(selectedCar);
                context.SaveChanges();
                NotifyOfPropertyChanged();
                CarRepository db1 = new CarRepository(context);
                Cars = new ObservableCollection<Car>(db1.GetAll());
                CarsView = (CollectionView)CollectionViewSource.GetDefaultView(Cars);
                CurrentAdminView = new GeneralUserView();
                CurrentUserView = new FavouriteCarsPage();
            });
        }

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
        UserControl currentAdminView;
        UserControl generalAdminView;
        UserControl favAdminView;
        UserControl newCarView;
        UserControl editCarView;
        public UserControl CurrentAdminView
        {
            get => currentAdminView;
            set
            {
                currentAdminView = value;
                NotifyOfPropertyChanged();
            }
        }
        public ICommand ShowGeneralUserPageCommand { get; set; }
        public ICommand ShowGeneralAdminPageCommand { get; set; }
        public ICommand ShowFavoritesCommand { get; set; }
        public ICommand ShowFavorites2Command { get; set; }
        public ICommand ShowNewCarCommand { get; set; }
        public ICommand ShowEditCarCommand { get; set; }
        public ICommand AddToFavoritesCommand { get; set; }
        public ICommand DelFromFavoritesCommand { get; set; }

        public ICommand EditCarCommand { get; set; }
        public ICommand DelCarCommand { get; set; }
       
    }
}

