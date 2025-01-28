using CommunityToolkit.Mvvm.ComponentModel;
using MyPadelApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.ViewModels.ViewBaseModel
{
    public partial class BaseViewModel : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private User _userData = new User();

        #endregion
    }
}
