using System.Threading.Tasks;

namespace PzomPatch.Services.Dialog;

public interface IDialogService
{
    Task<string?> ShowFolderAsync(string directory);
}