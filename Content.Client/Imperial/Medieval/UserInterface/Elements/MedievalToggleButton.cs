using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.Imperial.Medieval.UserInterface.Elements;

/// <summary>
///     imperial medieval - toggle button in the MedievalButton palette. MedievalButton has no pressed look,
///     so selected options (tabs, modes, amounts) use this one instead.
/// </summary>
[Virtual]
public class MedievalToggleButton : Button
{
    private static readonly StyleBoxFlat NormalBox = Box("#2a1f18", "#5c4a3d");
    private static readonly StyleBoxFlat HoverBox = Box("#3d2e24", "#d4af37");
    private static readonly StyleBoxFlat PressedBox = Box("#5a3b1c", "#d4af37");
    private static readonly StyleBoxFlat DisabledBox = Box("#1a1410", "#3d3530");

    public MedievalToggleButton()
    {
        StyleBoxOverride = NormalBox;
        Label.FontColorOverride = Color.FromHex("#a89f91");
    }

    protected override void DrawModeChanged()
    {
        base.DrawModeChanged();

        // can fire from the base constructor, before the label exists
        if (Label == null)
            return;

        (StyleBoxOverride, Label.FontColorOverride) = DrawMode switch
        {
            DrawModeEnum.Pressed => (PressedBox, Color.FromHex("#ffdf7a")),
            DrawModeEnum.Hover => (HoverBox, Color.White),
            DrawModeEnum.Disabled => (DisabledBox, Color.FromHex("#4a4540")),
            _ => (NormalBox, Color.FromHex("#a89f91")),
        };
    }

    private static StyleBoxFlat Box(string background, string border)
    {
        return new StyleBoxFlat
        {
            BackgroundColor = Color.FromHex(background),
            BorderColor = Color.FromHex(border),
            BorderThickness = new Thickness(1),
        };
    }
}
