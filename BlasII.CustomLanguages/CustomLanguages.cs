using BlasII.ModdingAPI;

namespace BlasII.CustomLanguages;

/// <summary>
/// Allows using custom translations made by the community
/// </summary>
public class CustomLanguages : BlasIIMod
{
    internal CustomLanguages() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    protected override void OnInitialize()
    {
        // Perform initialization here
    }
}
