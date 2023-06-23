using MonkeyFinder.Services;
using System.Windows.Input;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    MonkeyService monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = new();

    IConnectivity connectivity;
    IGeolocation geolocation;
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }

    [RelayCommand]
    async Task GetClosestMonkeyAsync()
    {
        if (IsBusy || Monkeys.Count == 0)
            return;

        try
        {
            var location = await geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await geolocation.GetLocationAsync(
                    new GeolocationRequest
                    {
                        Timeout = TimeSpan.FromSeconds(30),
                        DesiredAccuracy = GeolocationAccuracy.Medium
                    });
            }

            if (location == null)
                return;

            var first = Monkeys.OrderBy(m=>
                location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Kilometers))
                .FirstOrDefault();

            await Shell.Current.DisplayAlert("Closest monkey",
                $"{first.Name} in {first.Location}", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", 
                $"Unable to get monkeys:{ex.Message}", "OK");
        }
    }

    [RelayCommand]
    async Task GoToDetails(Monkey monkey)
    {
        if (monkey is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true,
            new Dictionary<string, object>
            {
                {"Monkey", monkey }
            });
    }

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if(connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet issue",
                    $"Check your internet and try again", "OK");
                return;
            }

            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();

            if(Monkeys.Count!=0)
                Monkeys.Clear();

            foreach(var monkey in monkeys)
                Monkeys.Add(monkey);
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get monkeys:{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
