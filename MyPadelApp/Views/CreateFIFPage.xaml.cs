namespace MyPadelApp.Views;

public partial class CreateFIFPage : ContentPage
{
	public CreateFIFPage()
	{
		InitializeComponent();
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(true);
    }
    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaymentScreenPage());
    }
    private async void OnCameraClicked(object sender, EventArgs e)
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                var storageStatus = await Permissions.RequestAsync<Permissions.StorageWrite>();

                if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                    await MediaPicker.Default.CapturePhotoAsync();
                else
                    await Shell.Current.DisplayAlert("Permission Denied", "Camera or Storage permission not granted.", "OK");
            }
            else
                await Shell.Current.DisplayAlert("Not Supported", "Capture is not supported on this device.", "OK");
        }
        catch { }
    }
}