using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace PzomPatch.Services.Dialog;

public class DialogService : IDialogService
{
    public Task<string?> ShowFolderAsync(string directory)
    {
        if (Application.Current?.ApplicationLifetime is not
            IClassicDesktopStyleApplicationLifetime desktop)
            return Task.FromResult<string?>(null);

        var dialog = new OpenFolderDialog
        {
            Title = "위치 지정",
            Directory = directory
        };

        return dialog.ShowAsync(desktop.MainWindow);
    }
}