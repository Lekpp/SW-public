namespace Content.Server.Imperial.Medieval.Boss;

[RegisterComponent]
public sealed partial class BossPlayersBackPointComponent : Component
{
    [DataField]
    public string LinkId = "test";

    [DataField]
    public float RandomOffset = 0f; // Случайное отклонение от центра, чтобы игроки не спавнились в одной точке
}
