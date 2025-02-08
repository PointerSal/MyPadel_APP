using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class ResendEmailPage : ContentPage
{
    ResendEmailViewModel _resendEmailViewModel;
    public ResendEmailPage(ResendEmailViewModel resendEmailViewModel)
	{
		InitializeComponent();
        BindingContext = _resendEmailViewModel = resendEmailViewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        _resendEmailViewModel.OnBack();
        return true;
    }
    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry currentEntry)
        {
            if (!string.IsNullOrEmpty(currentEntry.Text))
            {
                currentEntry.Text = currentEntry.Text.Substring(0, 1);
                _resendEmailViewModel.UserData.otp = entry1.Text + entry2.Text + entry3.Text + entry4.Text + entry5.Text + entry6.Text;
                MoveToNextEntry(currentEntry);
            }
        }
    }
    private void MoveToNextEntry(Entry currentEntry)
    {
        if (currentEntry == entry1) entry2.Focus();
        else if (currentEntry == entry2) entry3.Focus();
        else if (currentEntry == entry3) entry4.Focus();
        else if (currentEntry == entry4) entry5.Focus();
        else if (currentEntry == entry5) entry6.Focus();
        else if (currentEntry == entry6)
        {
            entry6.Unfocus();
            entry6.HideSoftInputAsync(System.Threading.CancellationToken.None);
        }
    }
}