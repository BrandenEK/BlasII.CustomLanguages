
namespace BlasII.CustomLanguages;

/// <summary>
/// Contains data for a custom language
/// </summary>
public class LanguageInfo(string id, string name, string author, string version)
{
    /// <summary> The unique id of this language </summary>
    public string Id { get; } = id;

    /// <summary> The display name of this language </summary>
    public string Name { get; } = name;

    /// <summary> The creator of this language </summary>
    public string Author { get; } = author;

    /// <summary> The semantic version of this language </summary>
    public string Version { get; } = version;
}
