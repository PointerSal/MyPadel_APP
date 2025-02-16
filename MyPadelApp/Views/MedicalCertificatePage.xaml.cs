using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class MedicalCertificatePage : ContentPage
{
	public MedicalCertificatePage(MedicalCertificateViewModel medicalCertificateViewModel)
	{
		InitializeComponent();
        BindingContext = medicalCertificateViewModel;
    }
}