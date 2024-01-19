using System.Threading.Tasks;

namespace PzomPatch.Services.Zomboid;

public interface IZomboidService
{
    Task Initialize();

    Task SetMemorySize(int size);
    Task<IResult> WriteServerList(string name, string ip);
    Task ApplyPatch();
}