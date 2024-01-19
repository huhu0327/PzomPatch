using System;
using Salaros.Configuration;

namespace PzomPatch.Services.Settings;

public class SettingsService : ISettingsService
{
    private readonly ConfigParser _config;
    private readonly string _section = "PzomSettings";

    public SettingsService()
    {
        var configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                         @"\PzomPatch\PzomPatch.dat";
        _config = new ConfigParser(configPath);
    }

    public string SteamClientPath
    {
        get => Get(nameof(SteamClientPath), @"");
        set => Set(nameof(SteamClientPath), value);
    }

    public string ZomboidClientPath
    {
        get => Get(nameof(ZomboidClientPath), @"");
        set => Set(nameof(ZomboidClientPath), value);
    }

    public int ClientMemorySize
    {
        get => int.Parse(Get(nameof(ClientMemorySize), 3072.ToString()));
        set => Set(nameof(ClientMemorySize), value.ToString());
    }

    private string Get(string keyName, string? deafaultValue = null)
    {
        return _config.GetValue(_section, keyName, deafaultValue);
    }

    private void Set(string keyName, string value)
    {
        _config.SetValue(_section, keyName, value);
        _config.Save();
    }
}