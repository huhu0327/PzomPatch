using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PzomPatch.Services.Settings;
using Serilog;

namespace PzomPatch.Services.Process;

public class ProcessService : IProcessService
{
    private readonly ILogger _logger;
    private readonly ISettingsService _settingsService;

    public ProcessService(ILogger logger, ISettingsService settingsService)
    {
        _logger = logger;
        _settingsService = settingsService;
    }

    public Task<IResult> Start()
    {
        var path = _settingsService.SteamClientPath;
        var memory = _settingsService.ClientMemorySize;

        var info = new ProcessStartInfo
        {
            FileName = @$"{path}\steam.exe",
            Arguments =
                $"-applaunch 108600 CMDLINE -Xmx{memory}m -Xms{memory}m",
            UseShellExecute = false,
            CreateNoWindow = false
        };

        try
        {
            using var process = System.Diagnostics.Process.Start(info);
        }
        catch (Exception exception)
        {
            _logger.Error(exception.Message);
            return Result.FailureFromResult(new Error("", ""));
        }

        return Result.SuccessFromResult();
    }

    public Task<IResult> Kill()
    {
        var process = System.Diagnostics.Process.GetProcessesByName("ProjectZomboid64").FirstOrDefault();

        if (process is null) return Result.FailureFromResult(new Error("", ""));

        try
        {
            process.Kill();
        }
        catch (Exception exception)
        {
            _logger.Error(exception.Message);
            return Result.FailureFromResult(new Error("", ""));
        }

        return Result.SuccessFromResult();
    }
}