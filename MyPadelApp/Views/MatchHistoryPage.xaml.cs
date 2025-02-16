using MyPadelApp.Models;
using MyPadelApp.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace MyPadelApp.Views;

public partial class MatchHistoryPage : ContentPage, INotifyPropertyChanged
{
    MatchHistoryViewModel _matchHistoryViewModel;
    public MatchHistoryPage(MatchHistoryViewModel matchHistoryViewModel)
	{
		InitializeComponent();
        BindingContext = _matchHistoryViewModel = matchHistoryViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _matchHistoryViewModel.OnPageAppearing();
    }

    private void OnTabsClicked(object sender, TappedEventArgs e)
    {
        FirstBorder.BackgroundColor = Colors.Transparent;
        SecondBorder.BackgroundColor = Colors.Transparent;
        ThirdBorder.BackgroundColor = Colors.Transparent;

        if (sender == FirstBorder)
        {
            FirstBorder.BackgroundColor = Color.FromArgb("#24a9ff");
            _matchHistoryViewModel.TabChanged(1);
        }
        else if (sender == SecondBorder)
        {
            SecondBorder.BackgroundColor = Color.FromArgb("#24a9ff");
            _matchHistoryViewModel.TabChanged(2);
        }
        else if (sender == ThirdBorder)
        {
            ThirdBorder.BackgroundColor = Color.FromArgb("#24a9ff");
            _matchHistoryViewModel.TabChanged(3);
        }
    }
}