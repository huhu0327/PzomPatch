using System.Threading.Tasks;

namespace PzomPatch.Services.Process;

public interface IProcessService
{
    Task<IResult> Start();
    Task<IResult> Kill();
}