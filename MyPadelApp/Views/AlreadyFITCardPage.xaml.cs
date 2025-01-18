namespace MyPadelApp.Views;

public partial class AlreadyFITCardPage : ContentPage
{
	public AlreadyFITCardPage()
	{
		InitializeComponent();
	}
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(true);
    }

    private void OnAddCardClicked(object sender, EventArgs e)
    {

    }
    private async void OnNewCardClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BookedFieldPage());
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