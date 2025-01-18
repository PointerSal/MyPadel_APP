using Microsoft.Maui.Controls;
using MyPadelApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyPadelApp.Views;

public partial class HomePage : ContentPage, INotifyPropertyChanged
{
    private bool IsClicked { get; set; } = false;
    private ObservableCollection<CalendarItem> _CalendarItems;
    public ObservableCollection<CalendarItem> CalendarItems
    {
        get => _CalendarItems;
        set
        {
            _CalendarItems = value;
            OnPropertyChanged();
        }
    }
    private ObservableCollection<TimeSlot> _TimeSlots;
    public ObservableCollection<TimeSlot> TimeSlots
    {
        get => _TimeSlots;
        set
        {
            _TimeSlots = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<TimeSlot> _TimeSlots2;
    public ObservableCollection<TimeSlot> TimeSlots2
    {
        get => _TimeSlots2;
        set
        {
            _TimeSlots2 = value;
            OnPropertyChanged();
        }
    }
    public HomePage()
    {
        InitializeComponent();
        BindingContext = this;
        CalendarItems = new ObservableCollection<CalendarItem>();
        GenerateCalendarForYear();
        GenerateTimeSlots();
    }

    private void GenerateTimeSlots()
    {
        TimeSlots = new ObservableCollection<TimeSlot>();

        DateTime startTime = new DateTime(1, 1, 1, 1, 0, 0); 
        DateTime endTime = new DateTime(1, 1, 1, 23, 0, 0); 
        int a = 0;

        while (startTime <= endTime)
        {
            a++;
            TimeSlots.Add(new TimeSlot
            {
                Time = startTime, 
                Status = a==5? "Prenotato" : "",
            });
            startTime = startTime.AddMinutes(30);
            if (a == 5)
                a = 0;
        }

        TimeSlots2 = TimeSlots;
    }

    private void GenerateCalendarForYear()
    {
        try
        {
            CalendarItems.Clear();
            var startDate = DateTime.Today;
            var endDate = startDate.AddYears(1);

            for (var date = startDate; date < endDate; date = date.AddDays(1))
            {
                CalendarItems.Add(new CalendarItem
                {
                    Date = date,
                    Day = date.ToString("ddd"),
                    MonthName = date.ToString("MMM")
                });
            }
        }
        catch
        {
        }
    }
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        PadelBorder.BackgroundColor = Colors.Transparent;
        TennisBorder.BackgroundColor = Colors.Transparent;

        if (sender == PadelBorder)
        {
            PadelBorder.BackgroundColor = Color.FromArgb("#a4b0e7");
        }
        else if (sender == TennisBorder)
        {
            TennisBorder.BackgroundColor = Color.FromArgb("#a4b0e7");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (!IsClicked)
        {
            IsClicked = true;
            var tappedSlot = (e as TappedEventArgs)?.Parameter as TimeSlot;

            if (tappedSlot == null)
                return;

            if (!tappedSlot.Status.Equals("Prenotato"))
                await Navigation.PushAsync(new BookingSummaryPage());

            IsClicked = false;
        }
    }
}