using MelonLoader;

namespace BlasII.CustomLanguages;

internal class Main : MelonMod
{
    public static CustomLanguages CustomLanguages { get; private set; }

    public override void OnLateInitializeMelon()
    {
        CustomLanguages = new CustomLanguages();
    }
}