using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPadelApp.Helpers;
using MyPadelApp.Models;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.MembershipUserServices;
using MyPadelApp.ViewModels.ViewBaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels
{
    public partial class FITCardViewModel : BaseViewModel
    {
        #region Services

        private readonly IMembershipUserService _membershipUserService;

        #endregion

        #region Properties

        [ObservableProperty]
        public Card _card;

        #endregion

        #region Commands

        public async void OnPageAppearing()
        {
            try
            {
                IsBusy = true;
                var response = await _membershipUserService.CardDetails(Utils.GetUser.email);
                if (response != null && response.code != null && response.code.Equals("0000"))
                    Card = JsonSerializer.Deserialize<Card>(response.data.ToString());
                else if (response != null && response.code != null)
                    await Shell.Current.DisplayAlert(AppResources.Error, response.message, AppResources.OK);
                else
                    await Shell.Current.DisplayAlert(AppResources.Error, AppResources.SomethingWrong, AppResources.OK);
            }
            catch { }
            IsBusy = false;
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

        #region Constructor
        public FITCardViewModel(IMembershipUserService membershipUserService)
        {
            _membershipUserService = membershipUserService;
            OnPageAppearing();
        }

        #endregion
    }
}