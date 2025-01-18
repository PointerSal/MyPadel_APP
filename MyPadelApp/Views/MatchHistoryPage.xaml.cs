using MyPadelApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace MyPadelApp.Views;

public partial class MatchHistoryPage : ContentPage, INotifyPropertyChanged
{
    private ObservableCollection<Item> _Items;
    public ObservableCollection<Item> Items
    {
        get => _Items;
        set
        {
            _Items = value;
            OnPropertyChanged();
        }
    }
    public MatchHistoryPage()
	{
		InitializeComponent();
        BindingContext = this;

        Items = new ObservableCollection<Item>
        {
            new Item
            {
                Title = "PADEL",
                Subtitle = "Campo Padel 2",
                Status = "Partita da giocare",
                Date = "15 nov",
                Time = "9:30",
                Duration = "90 min",
                Price = "44€"
            },
            new Item
            {
                Title = "TENNIS",
                Subtitle = "Campo tennis terra rossa",
                Status = "Partita da giocare",
                Date = "18 nov",
                Time = "20:00",
                Duration = "60 min",
                Price = "30€"
            }
        };
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        FirstBorder.BackgroundColor = Colors.Transparent;
        SecondBorder.BackgroundColor = Colors.Transparent;
        ThirdBorder.BackgroundColor = Colors.Transparent;

        if (sender == FirstBorder)
        {
            FirstBorder.BackgroundColor = Color.FromArgb("#24a9ff");
        }
        else if (sender == SecondBorder)
        {
            SecondBorder.BackgroundColor = Color.FromArgb("#24a9ff");
        }
        else if (sender == ThirdBorder)
        {
            ThirdBorder.BackgroundColor = Color.FromArgb("#24a9ff");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new HistoryBookingSummaryPage());
    }
}