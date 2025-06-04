using BlasII.ModdingAPI;
using Il2CppInterop.Runtime;
using Il2CppTMPro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
            try
            {
                LoadLanguage(dir);
            }
            catch (Exception ex)
            {
                ModLog.Error(ex);
            }
        }
    }

    private void LoadLanguage(string dir)
    {
        ModLog.Info($"Loading language: {Path.GetFileName(dir)}");

        // Load info
        string infoPath = Path.Combine(dir, "info.json");
        LanguageInfo info = LoadInfo(infoPath);

        // Load text
        string textPath = Path.Combine(dir, "language.txt");
        Dictionary<string, string> text = LoadText(textPath);

        // Load font
        string fontPath = Path.Combine(dir, "font.bundle");
        TMP_FontAsset font = LoadFont(fontPath);
    }

    private LanguageInfo LoadInfo(string path)
    {
        string text = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<LanguageInfo>(text);
    }

    private Dictionary<string, string> LoadText(string path)
    {
        string[] lines = File.ReadAllLines(path);
        Dictionary<string, string> language = [];

        foreach (string line in lines)
        {
            int colonIdx = line.IndexOf(':');
            string key = line.Substring(0, colonIdx).Trim();
            string value = line.Substring(colonIdx + 1).Trim().Replace('@', '\n');

            if (value != string.Empty)
                language.Add(key, value);
        }

        return language;
    }

    private TMP_FontAsset LoadFont(string path)
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(path); // change to font.asset
        UnityEngine.Object obj = bundle.LoadAsset("assets/NotoSerif-PL SDF.asset", Il2CppType.Of<TMP_FontAsset>());
        TMP_FontAsset font = obj.Cast<TMP_FontAsset>();
        font.hideFlags = HideFlags.DontUnloadUnusedAsset;

        return font;
    }
}
