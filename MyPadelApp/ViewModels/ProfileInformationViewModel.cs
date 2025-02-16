using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class ProfileInformationViewModel : BaseViewModel
    {
        #region Properties

        [ObservableProperty]
        private User _userProfile;

        #endregion

        #region Commands

        [RelayCommand]
        public void Appearing()
        {
            try
            {
                UserProfile = Utils.GetUser;
            }
            catch { }
        }

        [RelayCommand]
        public async Task NavigateToPage(string PageName)
        {
            try
            {
                await Shell.Current.GoToAsync(PageName);
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
