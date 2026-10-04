using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApiApp.Models;
using MauiApiApp.Services;

namespace MauiApiApp.ViewModels;

[QueryProperty(nameof(PostId), "PostId")] 
public partial class DetailViewModel : ObservableObject
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private int postId;}

    [ObservableProperty]
    private Post currentPost;

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private bool isBusy;

    public DetailViewModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task LoadPostDetailsAsync()
    {
        if (PostId == 0) return;

        IsBusy = true;
        StatusMessage = "Cargando detalle del servidor...";

        var (data, error) = await _apiService.GetPostByIdAsync(PostId);

        if (error != null)
        {
            StatusMessage = error;
        }
        else
        {
            CurrentPost = data;
            StatusMessage = string.Empty;
        }

        IsBusy = false;
    }
}