using CommunityToolkit.Mvvm.Input;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class ResendEmailViewModel : BaseViewModel
    {
        #region Commands

        [RelayCommand]
        private async void Appearing()
        {
            try
            {
                await Task.Delay(4000);
                await Shell.Current.GoToAsync("RegistrationResendOTPPage");
            }
            catch { }
        }

        [RelayCommand]
        private void ResendEmail()
        {
            try
            {
            }
            catch { }
        }

        [RelayCommand]
        public async Task Back()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch { }
        }

        #endregion
    }
}
