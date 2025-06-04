using HarmonyLib;
using Il2CppI2.Loc;
using Il2CppTMPro;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.CustomLanguages;

/// <summary>
/// Locates a custom font asset by ID
/// </summary>
[HarmonyPatch(typeof(LocalizationManager), nameof(LocalizationManager.FindAsset))]
class LocalizationManager_FindAsset_Patch
{
    public static Dictionary<string, TMP_FontAsset> FontAssets { get; } = [];

    public static bool Prefix(string value, ref Object __result)
    {
        if (!FontAssets.TryGetValue(value, out TMP_FontAsset font))
            return true;

        __result = font;
        return false;
    }
}
