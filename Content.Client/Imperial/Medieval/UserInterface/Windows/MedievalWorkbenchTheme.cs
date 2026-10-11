using Content.Client.Imperial.Medieval.UserInterface.Elements;
using Content.Client.Resources;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;

namespace Content.Client.Imperial.Medieval.UserInterface.Windows;

/// <summary>
///     imperial medieval - look of <see cref="MedievalWorkbenchMenu"/>, picked by <c>uiTheme</c> on <c>CP14Workbench</c>.
///     New look: a case in <see cref="Get"/> plus a method like <see cref="Leather"/>;
///     textures in <c>/Textures/Imperial/Medieval/Interface/Workbench/&lt;Name&gt;/</c>.
/// </summary>
public sealed class MedievalWorkbenchTheme
{
    /// <summary>Null keeps the window's own header.</summary>
    public StyleBox? Header { get; private init; }

    public float HeaderHeight { get; private init; } = 32;

    public StyleBox List { get; private init; } = default!;

    public StyleBox Details { get; private init; } = default!;

    public StyleBox Preview { get; private init; } = default!;

    public StyleBox Material { get; private init; } = default!;

    public Color Selected { get; private init; }

    /// <summary>Drawn over both ends of the craft button when it can be pressed, null for none.</summary>
    public Texture? ButtonNail { get; private init; }

    // grey-blue so brown, pale and dark items all stand out
    private static readonly Color PreviewColor = Color.FromHex("#4e565e");

    public static MedievalWorkbenchTheme Get(string? id, IResourceCache cache)
    {
        return id switch
        {
            "Leather" => Leather(cache),
            "Wood" => Wood(cache),
            _ => Plain(),
        };
    }

    public static MedievalWorkbenchTheme Plain()
    {
        return new MedievalWorkbenchTheme
        {
            List = Flat(MedievalUiStyle.Inset, MedievalUiStyle.Border, 6),
            Details = Flat(MedievalUiStyle.Inset, MedievalUiStyle.Border, 12),
            Preview = Flat(PreviewColor, MedievalUiStyle.Border, 0, 2),
            Material = Flat(MedievalUiStyle.Background, MedievalUiStyle.Background, 4),
            Selected = MedievalUiStyle.BackgroundHover,
        };
    }

    public static MedievalWorkbenchTheme Leather(IResourceCache cache)
    {
        const string path = "/Textures/Imperial/Medieval/Interface/Workbench/Leather/";
        var edge = Color.FromHex("#140c07");

        // 128x40, seam and edge in the bottom 10 px
        var header = new StyleBoxTexture
        {
            Texture = cache.GetTexture(path + "header.png"),
            Mode = StyleBoxTexture.StretchMode.Tile,
            PatchMarginTop = 1,
            PatchMarginBottom = 10,
        };

        var list = new StyleBoxTexture
        {
            Texture = cache.GetTexture(path + "piece.png"),
            Mode = StyleBoxTexture.StretchMode.Tile,
        };
        list.SetPatchMargin(StyleBox.Margin.All, 12);
        list.SetContentMarginOverride(StyleBox.Margin.All, 6);

        // 40x36, the point takes the left 14 px
        var tag = new StyleBoxTexture
        {
            Texture = cache.GetTexture(path + "tag.png"),
            Mode = StyleBoxTexture.StretchMode.Tile,
            PatchMarginLeft = 14,
            PatchMarginRight = 4,
            PatchMarginTop = 4,
            PatchMarginBottom = 4,
        };
        tag.SetContentMarginOverride(StyleBox.Margin.Left, 16);
        tag.SetContentMarginOverride(StyleBox.Margin.Right, 8);
        tag.SetContentMarginOverride(StyleBox.Margin.Vertical, 2);

        return new MedievalWorkbenchTheme
        {
            Header = header,
            HeaderHeight = 40,
            List = list,
            Details = Flat(MedievalUiStyle.Inset, edge, 12),
            Preview = Flat(PreviewColor, edge, 0, 2),
            Material = tag,
            Selected = Color.FromHex("#4a2f1c"),
        };
    }

    public static MedievalWorkbenchTheme Wood(IResourceCache cache)
    {
        const string path = "/Textures/Imperial/Medieval/Interface/Workbench/Wood/";

        // 128x40, light top edge, seams between the boards, dark foot in the bottom 10 px
        var header = new StyleBoxTexture
        {
            Texture = cache.GetTexture(path + "header.png"),
            Mode = StyleBoxTexture.StretchMode.Tile,
            PatchMarginTop = 1,
            PatchMarginBottom = 10,
        };

        // 32x32, an iron nail in each corner
        var list = Panel(cache.GetTexture(path + "panel.png"), 10, 8);
        var details = Panel(cache.GetTexture(path + "panel.png"), 10, 12);

        // 24x24, the nails sit inside the corner patches
        var preview = Panel(cache.GetTexture(path + "preview.png"), 10, 0);

        // 40x36, the nail takes the left 14 px
        var plank = new StyleBoxTexture
        {
            Texture = cache.GetTexture(path + "tag.png"),
            Mode = StyleBoxTexture.StretchMode.Tile,
            PatchMarginLeft = 14,
            PatchMarginRight = 4,
            PatchMarginTop = 4,
            PatchMarginBottom = 4,
        };
        plank.SetContentMarginOverride(StyleBox.Margin.Left, 16);
        plank.SetContentMarginOverride(StyleBox.Margin.Right, 8);
        plank.SetContentMarginOverride(StyleBox.Margin.Vertical, 2);

        return new MedievalWorkbenchTheme
        {
            Header = header,
            HeaderHeight = 40,
            List = list,
            Details = details,
            Preview = preview,
            Material = plank,
            Selected = Color.FromHex("#4b3320"),
            ButtonNail = cache.GetTexture(path + "nail.png"),
        };
    }

    private static StyleBoxTexture Panel(Texture texture, float patch, float padding)
    {
        var box = new StyleBoxTexture
        {
            Texture = texture,
            Mode = StyleBoxTexture.StretchMode.Tile,
        };
        box.SetPatchMargin(StyleBox.Margin.All, patch);
        box.SetContentMarginOverride(StyleBox.Margin.All, padding);
        return box;
    }

    private static StyleBoxFlat Flat(Color background, Color border, float padding, float borderThickness = 1)
    {
        var box = new StyleBoxFlat
        {
            BackgroundColor = background,
            BorderColor = border,
            BorderThickness = new Thickness(borderThickness),
        };
        box.SetContentMarginOverride(StyleBox.Margin.All, padding);
        return box;
    }
}
