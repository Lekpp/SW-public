using Robust.Client.UserInterface.Controls;

namespace Content.Client.Imperial.Medieval.UserInterface.Windows;

/// <summary>
///     imperial medieval - one log of firewood under the campfire. Lit logs are the chosen cook time,
///     hover brightens whichever you point at; while cooking the menu drives the burn colour directly.
/// </summary>
public sealed class MedievalCampfireLogButton : TextureButton
{
    private static readonly Color LitColor = Color.White;
    private static readonly Color LitHoverColor = Color.FromHex("#fff1c8");
    private static readonly Color UnlitColor = Color.FromHex("#4a4038");
    private static readonly Color UnlitHoverColor = Color.FromHex("#8a7a68");

    private bool _lit;
    private Color? _burnColor;

    public bool Lit
    {
        get => _lit;
        set
        {
            _lit = value;
            UpdateColor();
        }
    }

    /// <summary>Set while the log is in the fire; overrides the lit/unlit look until cleared.</summary>
    public Color? BurnColor
    {
        get => _burnColor;
        set
        {
            _burnColor = value;
            UpdateColor();
        }
    }

    public MedievalCampfireLogButton()
    {
        UpdateColor();
    }

    protected override void DrawModeChanged()
    {
        base.DrawModeChanged();
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (_burnColor is { } burn)
        {
            ModulateSelfOverride = burn;
            return;
        }

        var hover = DrawMode == DrawModeEnum.Hover;
        ModulateSelfOverride = _lit
            ? hover ? LitHoverColor : LitColor
            : hover ? UnlitHoverColor : UnlitColor;
    }
}
