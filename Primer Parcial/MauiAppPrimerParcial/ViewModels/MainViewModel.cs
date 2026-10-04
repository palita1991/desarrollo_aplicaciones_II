using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApiApp.Models;
using MauiApiApp.Services;

namespace MauiApiApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<Post> posts;

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private bool isBusy;

    public MainViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Posts = new ObservableCollection<Post>();
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        StatusMessage = "Cargando datos...";
        Posts.Clear();

        var (data, error) = await _apiService.GetPostsAsync();

        if (error != null)
        {
            StatusMessage = error;
        }
        else if (data != null && data.Any())
        {
            StatusMessage = $"Se cargaron {data.Count} elementos.";
            // Cargamos solo los primeros 10 para no saturar la UI en esta prueba
            foreach (var post in data.Take(10))
            {
                Posts.Add(post);
            }
        }

        IsBusy = false;
    }

    // Solución al Error #1: Navegamos a una ruta semántica ("PostDetail") y enviamos el parámetro
    [RelayCommand]
    private async Task GoToDetailsAsync(Post selectedPost)
    {
        if (selectedPost == null) return;

        var parameters = new Dictionary<string, object>
        {
            { "PostId", selectedPost.Id } // Pasamos el ID real
        };

        await Shell.Current.GoToAsync("PostDetail", parameters);
    }
}