using Content.Client.Resources;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.Imperial.Medieval.UserInterface.Elements;

/// <summary>
///     imperial medieval - shared look for the reworked medieval windows (campfire and the ones after it):
///     lettering and colours in one place, so every window of the series looks the same.
/// </summary>
public static class MedievalUiStyle
{
    /// <summary>Medieval lettering for headings and buttons. Small print stays in the regular UI font to stay readable.</summary>
    public const string FontPath = "/Fonts/Imperial/Vinque/Vinque.otf";

    public const int TitleFontSize = 18;
    public const int TextFontSize = 14;

    public static readonly Color Gold = Color.FromHex("#d4af37");
    public static readonly Color TextMuted = Color.FromHex("#a89f91");
    public static readonly Color TextFaded = Color.FromHex("#7a6a58");

    public static Font TitleFont(IResourceCache cache) => cache.GetFont(FontPath, TitleFontSize);

    public static Font TextFont(IResourceCache cache) => cache.GetFont(FontPath, TextFontSize);

    /// <summary>
    ///     Puts the medieval lettering on a window title. The title label belongs to the shared MedievalWindow,
    ///     so it's looked up instead of changing that window for everyone.
    /// </summary>
    public static void ApplyTitleFont(Control window, IResourceCache cache)
    {
        if (FindLabel(window, "TitleLabel") is { } title)
            title.FontOverride = TitleFont(cache);
    }

    /// <summary>Medieval lettering on buttons and labels.</summary>
    public static void ApplyTextFont(IResourceCache cache, params Control[] controls)
    {
        var font = TextFont(cache);

        foreach (var control in controls)
        {
            switch (control)
            {
                case Button button:
                    button.Label.FontOverride = font;
                    break;
                case Label label:
                    label.FontOverride = font;
                    break;
            }
        }
    }

    private static Label? FindLabel(Control parent, string name)
    {
        foreach (var child in parent.Children)
        {
            if (child is Label label && child.Name == name)
                return label;

            if (FindLabel(child, name) is { } found)
                return found;
        }

        return null;
    }
}
