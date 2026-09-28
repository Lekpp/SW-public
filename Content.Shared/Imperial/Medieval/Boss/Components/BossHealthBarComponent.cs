using Robust.Shared.GameStates;

namespace Content.Shared.Imperial.Medieval.Boss;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class BossHealthBarComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Active = true;

    [DataField, AutoNetworkedField]
    public float CurrentHp = 100f;

    [DataField, AutoNetworkedField]
    public float MaxHp = 100f;

    [DataField, AutoNetworkedField]
    public Color Color = Color.Red;
}
