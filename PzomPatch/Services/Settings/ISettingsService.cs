namespace PzomPatch.Services.Settings;

public interface ISettingsService
{
    string SteamClientPath { get; set; }
    string ZomboidClientPath { get; set; }
    int ClientMemorySize { get; set; }
}