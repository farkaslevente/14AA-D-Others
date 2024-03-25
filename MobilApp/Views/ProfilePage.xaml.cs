using MobilApp_Szakdolgozat.ViewModels;

namespace MobilApp_Szakdolgozat.Views;

public partial class ProfilePage : ContentPage
{    
    public ProfilePageViewModel vm { get; set; }
	public ProfilePage()
	{        
        this.BindingContext = new ShellViewModel();
        this.BindingContext = new ProfilePageViewModel();
        vm = new ProfilePageViewModel();
        vm.profileChangeVisibility = false;
        InitializeComponent();        

    }

    private void ProfileChangeBTN_Clicked(object sender, EventArgs e)
    {
        vm.profileChangeVisibility = true;
        ProfileChangeBTN.IsVisible = false;              
    }

    private void ProfileSaveBTN_Clicked(object sender, EventArgs e)
    {                        
    }

    private async void ProfilePicChangeBTN_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PPCatalogPage));
    }

    private void BTNLogout_Clicked(object sender, EventArgs e)
    {
        ShellViewModel ShellInstance = new ShellViewModel();
        DisplayAlert("Kijelentkez�s", "�n kijelentkezett", "Rendben");
        SecureStorage.Remove("userName");
        SecureStorage.Remove("userEmail");
        SecureStorage.Remove("userImage");
        SecureStorage.Remove("userLocation");
        SecureStorage.Remove("userId");
        SecureStorage.Remove("userRole");
        ShellInstance.VisibilityLP();
        Shell.Current.GoToAsync(nameof(MainPage));
    }

    private async void BTNProfile_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }

    private async void BTNLogin_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    private async void BTNMyAds_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AdsPage));
    }

    private async void BTNMessages_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MessagesPage));
    }

    private async void BTNAdmin_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NewPage1));
    }

    private async void BTNMainPage_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MainPage));
    }

    private async void BTNFav_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FavPage));
    }
}