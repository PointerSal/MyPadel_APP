using LocalizationResourceManager.Maui;
using Microsoft.Maui.Controls;
using MyPadelApp.Models;
using MyPadelApp.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyPadelApp.Views;

public partial class HomePage : ContentPage
{
    HomeViewModel _homeViewModel;
    private bool IsAppeared = false;
    public HomePage(HomeViewModel homeViewModel, ILocalizationResourceManager localizationResourceManager)
    {
        if (string.IsNullOrEmpty(Preferences.Default.Get("username", string.Empty)) || string.IsNullOrEmpty(Preferences.Default.Get("Password", string.Empty)))
            Navigation.PushAsync(new SelectionPage(localizationResourceManager));
       
        InitializeComponent();
        BindingContext = _homeViewModel = homeViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _homeViewModel.InitializeData();
        if(!IsAppeared)
        {
            _homeViewModel.GenerateCalendarForYears();
            IsAppeared=true;
        }
    }

    private async void OnBorderTapped(object sender, TappedEventArgs e)
    {
        PadelBorder.BackgroundColor = Colors.Transparent;
        TennisBorder.BackgroundColor = Colors.Transparent;
        SoccerBorder.BackgroundColor = Colors.Transparent;
        CricketBorder.BackgroundColor = Colors.Transparent;

        if (sender == PadelBorder)
        {
            PadelBorder.BackgroundColor = Color.FromArgb("#a4b0e7");
            _homeViewModel.CurrentField = 1;
        }
        else if (sender == TennisBorder)
        {
            TennisBorder.BackgroundColor = Color.FromArgb("#a4b0e7");
            _homeViewModel.CurrentField = 2;
        }
        else if (sender == SoccerBorder)
        {
            SoccerBorder.BackgroundColor = Color.FromArgb("#a4b0e7");
            _homeViewModel.CurrentField = 3;
        }
        else if (sender == CricketBorder)
        {
            CricketBorder.BackgroundColor = Color.FromArgb("#a4b0e7");
            _homeViewModel.CurrentField = 4;
        }
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            var selectedDate = (CalendarItem)e.CurrentSelection.FirstOrDefault();
            _homeViewModel.SelectedDate = selectedDate.Date;
            await _homeViewModel.GenerateTimeSlots(1);
            await _homeViewModel.GenerateTimeSlots(2);
            await Task.Delay(2000);
            CalendarCollectionView.ScrollTo(_homeViewModel.SelectedCalender, position: ScrollToPosition.Center, animate: false);
        }
        catch { }
    }
}