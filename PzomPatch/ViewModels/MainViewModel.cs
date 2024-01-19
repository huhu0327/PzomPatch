using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PzomPatch.Services.Dialog;
using PzomPatch.Services.Process;
using PzomPatch.Services.Settings;
using PzomPatch.Services.Zomboid;

namespace PzomPatch.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService = default!;
    private readonly IProcessService _processService = default!;
    private readonly ISettingsService _settingsService = default!;
    private readonly IZomboidService _zomboidService = default!;

    private bool _isCanExcuteGame = true;

    [ObservableProperty] private int _memory;
    [ObservableProperty] private string _nickName = default!;
    [ObservableProperty] private string _serverIp = default!;

    [ObservableProperty] private string _steamPath = default!;
    [ObservableProperty] private string _zomboidPath = default!;

    public MainViewModel()
    {
    }

    public MainViewModel(IDialogService dialogService, IZomboidService zomboidService, ISettingsService settingsService,
        IProcessService processService)
    {
        _dialogService = dialogService;
        _zomboidService = zomboidService;
        _settingsService = settingsService;
        _processService = processService;

        SteamPath = settingsService.SteamClientPath;
        ZomboidPath = settingsService.ZomboidClientPath;
    }

    private bool CanExcuteGame()
    {
        return _isCanExcuteGame;
    }

    [RelayCommand]
    private async Task LoadFilesAsync()
    {
        await _zomboidService.Initialize();
        Memory = _settingsService.ClientMemorySize / 1024;
    }

    [RelayCommand]
    private async Task SetPathAsync(int param)
    {
        var directory = param == 0 ? SteamPath : ZomboidPath;
        var result = await _dialogService.ShowFolderAsync(directory);

        if (string.IsNullOrEmpty(result)) return;

        if (param == 0)
        {
            SteamPath = result;
            _settingsService.SteamClientPath = result;
        }
        else
        {
            ZomboidPath = result;
            _settingsService.ZomboidClientPath = result;
        }
    }

    [RelayCommand(CanExecute = nameof(CanExcuteGame))]
    private async Task PlayAsync()
    {
        await _processService.Start();
    }

    [RelayCommand]
    private async Task KillAsync()
    {
        await _processService.Kill();
    }

    [RelayCommand]
    private async Task SetMemoryAsync()
    {
        await _zomboidService.SetMemorySize(Memory * 1024);
    }

    [RelayCommand]
    private async Task SetNickNameAsync()
    {
        await _zomboidService.WriteServerList(NickName, ServerIp);
    }

    [RelayCommand]
    private async Task ApplyPatchAsync()
    {
        _isCanExcuteGame = false;
        await Task.Delay(500);
        await _zomboidService.ApplyPatch();
        _isCanExcuteGame = true;
    }
}