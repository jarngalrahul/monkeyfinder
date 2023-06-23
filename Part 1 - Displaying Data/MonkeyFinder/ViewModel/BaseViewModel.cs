namespace MonkeyFinder.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    public BaseViewModel() { }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;
}

//public class BaseViewModel : INotifyPropertyChanged
//{
//    bool isBusy;
//    string title;

//    public bool IsBusy
//    {
//        get => isBusy;
//        set
//        {
//            if(isBusy == value)
//                return;

//            isBusy = value;
//            OnPropertyChanged(nameof(IsBusy));
//            OnPropertyChanged(nameof(IsNotBusy));
//        }
//    }

//    public string Title
//    {
//        get => title;
//        set
//        {
//            if(title == value) 
//                return;

//            title = value;
//            OnPropertyChanged(nameof(Title));
//        }
//    }

//    public bool IsNotBusy => !isBusy;

//    public event PropertyChangedEventHandler PropertyChanged;

//    public void OnPropertyChanged([CallerMemberName]string name=null)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
//    }
//}
