using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class ProfileInformationPage : ContentPage
{
	public ProfileInformationPage(ProfileInformationViewModel profileInformationViewModel)
	{
		InitializeComponent();
		BindingContext = profileInformationViewModel;
    }
}