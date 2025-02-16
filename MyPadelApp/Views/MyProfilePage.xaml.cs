using LocalizationResourceManager.Maui;
using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class MyProfilePage : ContentPage
{
	public MyProfilePage(MyProfileViewModel myProfileViewModel)
	{
		InitializeComponent();
        BindingContext = myProfileViewModel;
    }
}