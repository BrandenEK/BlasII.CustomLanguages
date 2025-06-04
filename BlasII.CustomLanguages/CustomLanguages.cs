using BlasII.ModdingAPI;
using System.IO;

namespace BlasII.CustomLanguages;

/// <summary>
/// Allows using custom translations made by the community
/// </summary>
public class CustomLanguages : BlasIIMod
{
    internal CustomLanguages() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    /// <summary>
    /// Loads all languages from the custom_languages folder
    /// </summary>
    protected override void OnAllInitialized()
    {
        string langDir = Path.Combine(FileHandler.ModdingFolder, "custom_languages");
        Directory.CreateDirectory(langDir);

        ModLog.Info($"Loading custom languages from {langDir}");
        foreach (string dir in Directory.GetDirectories(langDir))
        {
            LoadLanguage(dir);
        }
    }

    private void LoadLanguage(string dir)
    {
        string langCode = Path.GetFileName(dir);
        ModLog.Info($"Loading language: {langCode}");

    }
}
