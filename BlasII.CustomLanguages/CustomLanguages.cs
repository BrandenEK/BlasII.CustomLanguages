using BlasII.ModdingAPI;
using Il2CppI2.Loc;
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

        // Register in the I2.Loc LocalizationManager
        AddLanguageToManager(info, text, font);
    }

    private void AddLanguageToManager(LanguageInfo info, Dictionary<string, string> text, TMP_FontAsset font)
    {
        // Register text
        AddText(info, text);

        // Register font
        if (font != null)
            AddFont(info, font);
    }

    private void AddText(LanguageInfo info, Dictionary<string, string> text)
    {
        int count = 0;

        foreach (var source in LocalizationManager.Sources)
        {
            source.AddLanguage(info.Name, info.Id);
            int langIdx = source.GetLanguageIndexFromCode(info.Id);

            foreach (string term in source.GetTermsList())
            {
                if (text.TryGetValue(term, out string newText))
                {
                    source.GetTermData(term).Languages[langIdx] = newText;
                    count++;
                }
            }
        }

        ModLog.Info($"Updating {count} terms for {info.Name} translation");
    }

    private void AddFont(LanguageInfo info, TMP_FontAsset font)
    {
        LocalizationManager_FindAsset_Patch.FontAssets.Add(info.Id, font);

        TermData data = LocalizationManager.GetTermData("FONT", out var source);
        data.SetTranslation(source.GetLanguageIndexFromCode(info.Id), info.Id);

        ModLog.Info($"Updating font for {info.Name} translation");
    }

    private LanguageInfo LoadInfo(string path)
    {
        if (!File.Exists(path))
            throw new Exception($"No info.json file present!");

        string text = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<LanguageInfo>(text);
    }

    private Dictionary<string, string> LoadText(string path)
    {
        if (!File.Exists(path))
            return [];

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
        if (!File.Exists(path))
            return null;

        AssetBundle bundle = AssetBundle.LoadFromFile(path);
        UnityEngine.Object obj = bundle.LoadAsset("assets/font.asset", Il2CppType.Of<TMP_FontAsset>());
        TMP_FontAsset font = obj.Cast<TMP_FontAsset>();
        font.hideFlags = HideFlags.DontUnloadUnusedAsset;

        return font;
    }
}
