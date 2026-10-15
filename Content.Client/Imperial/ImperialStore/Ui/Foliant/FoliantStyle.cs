using Content.Client.Resources;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;

namespace Content.Client.Imperial.ImperialStore.Ui.Foliant;

/// <summary>
///     imperial medieval - colours, fonts and textures of the grimoire window. The textures are generated
///     (see <c>Resources/Textures/Imperial/Medieval/Interface/Foliant</c>) and tinted here, so a school needs no art of its own.
/// </summary>
public static class FoliantStyle
{
    private const string TexturePath = "/Textures/Imperial/Medieval/Interface/Foliant/";
    private const string FontPath = "/Fonts/Imperial/Vinque/Vinque.otf";

    // ink on the pages
    public static readonly Color Ink = Color.FromHex("#3a2a1c");
    public static readonly Color InkRed = Color.FromHex("#6b2a1a");
    public static readonly Color InkFaded = Color.FromHex("#7d6a4a");
    public static readonly Color RowHover = Color.FromHex("#d2bf96");
    public static readonly Color RowLine = Color.FromHex("#b39c72");

    // text on the dark parts: ribbons, buttons
    public static readonly Color Cloth = Color.FromHex("#f0e6d0");
    public static readonly Color Gold = Color.FromHex("#d4af37");
    public static readonly Color GoldBright = Color.FromHex("#ffdf7a");
    public static readonly Color Faded = Color.FromHex("#7a6a58");
    public static readonly Color Price = Color.FromHex("#ffb070");
    public static readonly Color PriceMissing = Color.FromHex("#e0664a");

    // currency colours that don't read on a dark socket or on cloth
    private static readonly Dictionary<string, Color> SchoolColors = new()
    {
        ["MagicMedievalEarth"] = Color.FromHex("#8a6a4a"),
    };

    // yellow turns olive green when it is darkened, so the light school gets its own cloth
    private static readonly Dictionary<string, Color> ClothColors = new()
    {
        ["MagicMedievalLight"] = Color.FromHex("#a67c24"),
    };

    // brightness of the ribbon texture, the tint is divided by it
    private const float RibbonBase = 0.8f;

    public static Font Vinque(IResourceCache cache, int size) => cache.GetFont(FontPath, size);

    public static Texture Tex(IResourceCache cache, string file) => cache.GetTexture(TexturePath + file);

    /// <summary>
    ///     The texture of a theme (a folder inside the Foliant one, e.g. <c>archmage</c>). A theme paints only what it needs,
    ///     the rest is taken from the common textures.
    /// </summary>
    public static Texture Tex(IResourceCache cache, string theme, string file)
    {
        if (TryTheme(cache, theme, file, out var texture))
            return texture;

        return Tex(cache, file);
    }

    /// <summary>The texture of a theme, if the theme has one of its own.</summary>
    public static bool TryTheme(IResourceCache cache, string theme, string file, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Texture? texture)
    {
        texture = null;
        if (string.IsNullOrEmpty(theme))
            return false;

        if (!cache.TryGetResource<TextureResource>(new ResPath(TexturePath + theme + "/" + file), out var resource))
            return false;

        texture = resource.Texture;
        return true;
    }

    /// <summary>Colour of a school in the window.</summary>
    public static Color School(string? currency, Color fallback)
    {
        return currency != null && SchoolColors.TryGetValue(currency, out var color) ? color : fallback;
    }

    /// <summary>Tint for the ribbon texture of a school.</summary>
    /// <summary>The colour of a school that is dark enough for lines and text on a page.</summary>
    public static Color Accent(string? currency, Color school)
    {
        if (currency == null)
            return Ink;

        if (currency == "MagicMedievalLight")
            return Color.FromHex("#a67c24");

        return new Color(school.R * 0.75f, school.G * 0.75f, school.B * 0.75f);
    }

    public static Color RibbonTint(string? currency, Color school)
    {
        var cloth = currency != null && ClothColors.TryGetValue(currency, out var color)
            ? color
            : new Color(school.R * 0.55f, school.G * 0.55f, school.B * 0.55f);

        return new Color(
            Math.Min(1f, cloth.R / RibbonBase),
            Math.Min(1f, cloth.G / RibbonBase),
            Math.Min(1f, cloth.B / RibbonBase));
    }

    public static StyleBoxTexture Patch(IResourceCache cache, string file, float patch, float content, string theme = "")
    {
        var box = new StyleBoxTexture
        {
            Texture = Tex(cache, theme, file),
            Mode = StyleBoxTexture.StretchMode.Tile,
        };
        box.SetPatchMargin(StyleBox.Margin.All, patch);
        box.SetContentMarginOverride(StyleBox.Margin.All, content);
        return box;
    }

    public static StyleBoxTexture Cover(IResourceCache cache, string theme = "") => Patch(cache, "cover.png", 16, 16, theme);

    public static StyleBoxTexture Page(IResourceCache cache, bool left, string theme = "") => Patch(cache, left ? "page_l.png" : "page_r.png", 20, 18, theme);

    /// <summary>The fold between the pages, repeats down the book.</summary>
    public static StyleBoxTexture Gutter(IResourceCache cache, string theme = "")
    {
        // the top and the bottom are the edge of a page, only the middle is tiled
        var box = new StyleBoxTexture
        {
            Texture = Tex(cache, theme, "gutter.png"),
            Mode = StyleBoxTexture.StretchMode.Tile,
        };
        box.SetPatchMargin(StyleBox.Margin.Vertical, 20);
        return box;
    }

    /// <summary>A ribbon. The middle repeats, so a taller (chosen) ribbon is just longer.</summary>
    public static StyleBoxTexture Ribbon(IResourceCache cache, Color tint)
    {
        var box = new StyleBoxTexture
        {
            Texture = Tex(cache, "ribbon.png"),
            Mode = StyleBoxTexture.StretchMode.Tile,
            Modulate = tint,
        };
        box.SetPatchMargin(StyleBox.Margin.Top, 8);
        box.SetPatchMargin(StyleBox.Margin.Bottom, 13);
        box.SetContentMarginOverride(StyleBox.Margin.Left, 4);
        box.SetContentMarginOverride(StyleBox.Margin.Right, 6);
        box.SetContentMarginOverride(StyleBox.Margin.Top, 6);
        box.SetContentMarginOverride(StyleBox.Margin.Bottom, 15);
        return box;
    }

    /// <summary>
    ///     The trim of a ribbon of a theme (stitches, a hem), drawn over the tinted ribbon so that it keeps its own colour.
    ///     Null when the theme has none.
    /// </summary>
    public static StyleBoxTexture? RibbonTrim(IResourceCache cache, string theme)
    {
        if (!TryTheme(cache, theme, "ribbon_trim.png", out var texture))
            return null;

        var box = new StyleBoxTexture
        {
            Texture = texture,
            Mode = StyleBoxTexture.StretchMode.Tile,
        };
        box.SetPatchMargin(StyleBox.Margin.Top, 8);
        box.SetPatchMargin(StyleBox.Margin.Bottom, 13);
        return box;
    }

    public static StyleBoxTexture Button(IResourceCache cache, string file) => Patch(cache, file, 5, 4);

    /// <summary>A flat tinted panel, e.g. a row of a list.</summary>
    public static StyleBoxFlat Flat(Color background, Color border, float thickness)
    {
        return new StyleBoxFlat
        {
            BackgroundColor = background,
            BorderColor = border,
            BorderThickness = new Thickness(thickness),
        };
    }

    // sign of every school, grey textures tinted with the colour of the school
    private static readonly Dictionary<string, string> Runes = new()
    {
        ["MagicMedievalFire"] = "rune_fire.png",
        ["MagicMedievalLight"] = "rune_light.png",
        ["MagicMedievalVodka"] = "rune_water.png",
        ["MagicMedievalEarth"] = "rune_earth.png",
        ["MagicMedievalDarkness"] = "rune_dark.png",
    };

    public static string RuneFile(string school) => Runes.TryGetValue(school, out var file) ? file : "rune_none.png";

    /// <summary>The sign of a school (13x13 pixels, <paramref name="scale"/> times bigger), a plain diamond for anything else.</summary>
    public static TextureRect Rune(IResourceCache cache, string? school, Color color, float scale)
    {
        return new TextureRect
        {
            Texture = Tex(cache, school == null ? "rune_none.png" : RuneFile(school)),
            TextureScale = new System.Numerics.Vector2(scale, scale),
            Stretch = TextureRect.StretchMode.Keep,
            ModulateSelfOverride = color,
            VerticalAlignment = Control.VAlignment.Center,
        };
    }

    /// <summary>A wax seal, one for every point of archmage.</summary>
    public static TextureRect Seal(IResourceCache cache)
    {
        return new TextureRect
        {
            Texture = Tex(cache, "seal.png"),
            TextureScale = new System.Numerics.Vector2(2, 2),
            Stretch = TextureRect.StretchMode.Keep,
        };
    }

    public static string SchoolName(string school) => Loc.GetString("foliant-school-" + school);

    public static Label Text(string text, Font? font, Color color)
    {
        var label = new Label { Text = text, FontColorOverride = color };
        if (font != null)
            label.FontOverride = font;

        return label;
    }
}
