﻿using MobilApp_Szakdolgozat.Views;
using MobilApp_Szakdolgozat.ViewModels;

namespace MobilApp_Szakdolgozat
{
    //lehet jól jön: https://stackoverflow.com/questions/77086642/how-to-update-the-tab-bar-in-net-maui
    //               https://dev.to/dotnet/hide-shell-flyout-items-and-tabs-in-xamarin-forms-1agi
    public partial class AppShell : Shell
    {
        public bool LoginVisible { get; set; }
        public bool LoggedInVisible { get; set; }
        public AppShell()
        {
            
            InitializeComponent();
            Proba_Tabbar.IsEnabled = false;
            Proba_Tabbar.IsVisible = false;
            //UpdateShellContentVisibility();
            ShellViewModel shellViewModel = new ShellViewModel();
            //shellViewModel.VisibilityLP();
            this.BindingContext  = shellViewModel;            
            Init();
            //Routing.RegisterRoute("loginDetails", typeof(LoginPage));
            //Routing.RegisterRoute("registerDetails", typeof(RegisterPage));
            //Routing.RegisterRoute("forgottenPwdDetails", typeof(ForgottenPwdPage));
            //Routing.RegisterRoute("profileDetails", typeof(ProfilePage));
            //Routing.RegisterRoute("ppCatalog", typeof(PPCatalogPage));
            //Routing.RegisterRoute(nameof(AdsPage), typeof(AdsPage));
            //Routing.RegisterRoute("messages", typeof(MessagesPage));
            //Routing.RegisterRoute("conversations", typeof(ConversationsPage));
            //Routing.RegisterRoute("searchDetails", typeof(SearchPage));
            //Routing.RegisterRoute(nameof(NewPage1), typeof(NewPage1));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ForgottenPwdPage), typeof(ForgottenPwdPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute("ppCatalog", typeof(PPCatalogPage));
            Routing.RegisterRoute(nameof(AdsPage), typeof(AdsPage));
            Routing.RegisterRoute("messages", typeof(MessagesPage));
            Routing.RegisterRoute("conversations", typeof(ConversationsPage));
            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute(nameof(NewPage1), typeof(NewPage1));


        }

        public void UpdateShellContentVisibility()
        {
            //Main_TabBar.Items.Clear();            
            
            string LoggedIn = SecureStorage.GetAsync("userId").Result;
            if (LoggedIn != null)
            {
                //Main_TabBar.IsEnabled = false;
                //Main_TabBar.IsVisible = false;
                //Proba_Tabbar.IsVisible = true;
                //Proba_Tabbar.IsEnabled = true;
                //Main_TabBar.Items.Add(new ShellContent()
                //{
                //    Title = "Profil",                    
                //    Icon = "profile.svg",
                //    ContentTemplate = new DataTemplate(() => new ProfilePage()),
                //});
            }
            else
            {
                //Main_TabBar.IsEnabled = true;
                //Main_TabBar.IsVisible = true;
                //Proba_Tabbar.IsVisible = false;
                //Proba_Tabbar.IsEnabled = false;
                
            }
            //OnPropertyChanged(nameof(LoginVisible));
            //OnPropertyChanged(nameof(LoggedInVisible));

        }        
        //    ShellViewModel shellViewModel = new ShellViewModel();
        //    this.BindingContext= shellViewModel;
        //    shellViewModel.VisibilityLP();
        

    private void Init()
        {
           //Main_TabBar.CurrentItem = MainSearch;
           Proba_Tabbar.CurrentItem = MainSearchP;
        }
    }
}