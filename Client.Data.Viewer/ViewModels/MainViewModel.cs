using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Data.Viewer.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    int count = 0;

    [RelayCommand]
    void Increase()
    {
        Count++;
    }

    [RelayCommand]
    void Decrease()
    {
        Count--;
    }
}