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
    private static readonly StyleBoxFlat NormalBox = Box(MedievalUiStyle.Background, MedievalUiStyle.Border);
    private static readonly StyleBoxFlat HoverBox = Box(MedievalUiStyle.BackgroundHover, MedievalUiStyle.Gold);
    private static readonly StyleBoxFlat PressedBox = Box(MedievalUiStyle.BackgroundPressed, MedievalUiStyle.Gold);
    private static readonly StyleBoxFlat DisabledBox = Box(MedievalUiStyle.BackgroundDisabled, MedievalUiStyle.BorderDisabled);

    public MedievalToggleButton()
    {
        StyleBoxOverride = NormalBox;
        Label.FontColorOverride = MedievalUiStyle.Text;
    }

    protected override void DrawModeChanged()
    {
        base.DrawModeChanged();

        // can fire from the base constructor, before the label exists
        if (Label == null)
            return;

        (StyleBoxOverride, Label.FontColorOverride) = DrawMode switch
        {
            DrawModeEnum.Pressed => (PressedBox, MedievalUiStyle.GoldBright),
            DrawModeEnum.Hover => (HoverBox, Color.White),
            DrawModeEnum.Disabled => (DisabledBox, MedievalUiStyle.TextDisabled),
            _ => (NormalBox, MedievalUiStyle.Text),
        };
    }

    private static StyleBoxFlat Box(Color background, Color border)
    {
        return new StyleBoxFlat
        {
            BackgroundColor = background,
            BorderColor = border,
            BorderThickness = new Thickness(1),
        };
    }
}
