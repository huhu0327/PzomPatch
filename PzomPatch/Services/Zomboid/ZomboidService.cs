using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PzomPatch.Services.Settings;
using Serilog;

namespace PzomPatch.Services.Zomboid;

public class ZomboidService : IZomboidService
{
    private readonly ILogger _logger;
    private readonly ISettingsService _settingsService;
    private string _batchContents = string.Empty;
    private string _jsonContents = string.Empty;

    public ZomboidService(ILogger logger, ISettingsService settingsService)
    {
        _logger = logger;
        _settingsService = settingsService;
    }

    private string _zomboidUserPath => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Zomboid";

    public async Task Initialize()
    {
        DeleteLogFolder();
        await LoadClientFiles();

        _settingsService.ClientMemorySize = GetMemorySize();
    }

    public async Task ApplyPatch()
    {
        var tasks = new[] { CopyZombieFixMod(), CopyBetterFpsMod() };
        await Task.WhenAll(tasks);
    }

    public async Task SetMemorySize(int size)
    {
        ChangeBatchFile(size);

        ChangeJsonFile(size);

        await SaveClienFiles();
    }

    public async Task<IResult> WriteServerList(string name, string ip)
    {
        var path = _zomboidUserPath + @"\Lua\ServerListSteam.txt";

        var text = """
                   name=국밥서버
                   port=16261
                   description=연말 크리스마스 기념 좀보이드 국밥서버
                   password=password12@
                   usesteamrelay=true
                   """;

        try
        {
            await using var sw = new StreamWriter(path, false);
            await sw.WriteLineAsync(text);
            await sw.WriteLineAsync($"user={name}");
            await sw.WriteLineAsync($"ip={ip}");
        }
        catch (Exception exception)
        {
            _logger.Error(exception.Message);
            return Result.Failure(new Error("", ""));
        }

        return Result.Success();
    }

    private async Task<IResult> CopyZombieFixMod()
    {
        var modPath = @"workshop\content\108600\3115293671\mods\No Mo Culling";

        var targetPath = @"\zombie\popman";

        var files = new[] { @"\NetworkZombiePacker.class", @"\ZombieCountOptimiser.class" };

        var result = await CopyMod(modPath, targetPath, files);

        return result;
    }

    private async Task<IResult> CopyBetterFpsMod()
    {
        var modPath = @"workshop\content\108600\3022543997\mods\BetterFPS\media\4k\zombie\iso";

        var targetPath = @"\zombie\iso";

        var file = @"\IsoChunkMap.class";

        var result = await CopyMod(modPath, targetPath, file);

        return result;
    }

    private Task<IResult> CopyMod(string modPath, string targetPath, params string[] files)
    {
        var steamApps = $@"{_settingsService.ZomboidClientPath}..\..\..\";
        steamApps = Path.GetFullPath(steamApps);
        modPath = steamApps + modPath;
        targetPath = _settingsService.ZomboidClientPath + targetPath;
        if (!Path.Exists(modPath)) return Result.FailureFromResult(new Error("", ""));
        if (!Path.Exists(targetPath)) return Result.FailureFromResult(new Error("", ""));

        try
        {
            foreach (var file in files)
                File.Copy(modPath + file, targetPath + file, true);
        }
        catch (Exception exception)
        {
            _logger.Error(exception.Message);
            return Result.FailureFromResult(new Error("", ""));
        }

        return Result.SuccessFromResult();
    }

    private int GetMemorySize()
    {
        if (string.IsNullOrWhiteSpace(_batchContents))
            throw new Exception(_batchContents);

        var target = _batchContents.Split(" ")
            .LastOrDefault(s => s.Contains("-Xmx"));

        if (string.IsNullOrWhiteSpace(target))
            throw new Exception(target);

        target = Regex.Replace(target, @"\D", "");
        var result = int.Parse(target);

        return result;
    }

    private void DeleteLogFolder()
    {
        var path = _zomboidUserPath + @"\Logs";
        if (!Directory.Exists(path)) return;

        try
        {
            Directory.Delete(path, true);
        }
        catch (Exception exception)
        {
            _logger.Error(exception.ToString());
        }
    }

    private async Task LoadClientFiles()
    {
        var zomboPath = _settingsService.ZomboidClientPath;

        var batchPath = zomboPath + @"\ProjectZomboid64.bat";
        var jsonPath = zomboPath + @"\ProjectZomboid64.json";

        try
        {
            var tasks = new[] { File.ReadAllTextAsync(batchPath), File.ReadAllTextAsync(jsonPath) };
            var files = await Task.WhenAll(tasks);

            if (files.Length != 2) return;

            _batchContents = files[0];
            _jsonContents = files[1];
        }
        catch (Exception exception)
        {
            _logger.Error(exception.Message);
        }
    }

    private async Task SaveClienFiles()
    {
        var zomboPath = _settingsService.ZomboidClientPath;

        var batchPath = zomboPath + @"\ProjectZomboid64.bat";
        var jsonPath = zomboPath + @"\ProjectZomboid64.json";

        try
        {
            var tasks = new[]
            {
                File.WriteAllTextAsync(batchPath, _batchContents),
                File.WriteAllTextAsync(jsonPath, _jsonContents)
            };
            await Task.WhenAll(tasks);
        }
        catch (Exception exception)
        {
            _logger.Error(exception.Message);
        }
    }

    private void ChangeBatchFile(int size)
    {
        var splitedContents = _batchContents.Split(" ");

        splitedContents = splitedContents.Where(s => !s.Contains("Xms")).ToArray();

        for (var i = 0; i < splitedContents.Length; i++)
        {
            if (!splitedContents[i].Contains("-Xmx")) continue;
            splitedContents[i] = $"-Xmx{size}m -Xms{size}m";
        }

        _batchContents = string.Join(" ", splitedContents);
    }

    private void ChangeJsonFile(int size)
    {
        var splitedContents = _jsonContents.Split(Environment.NewLine);

        splitedContents = splitedContents.Where(s => !s.Contains("Xms")).ToArray();

        for (var i = 0; i < splitedContents.Length; i++)
        {
            if (!splitedContents[i].Contains("-Xmx")) continue;

            splitedContents[i] = Regex.Replace(splitedContents[i], @"\d{2,}", size.ToString());
            splitedContents[i] += Environment.NewLine + splitedContents[i].Replace("mx", "ms");
        }

        _jsonContents = string.Join(Environment.NewLine, splitedContents);
    }
}