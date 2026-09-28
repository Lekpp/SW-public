using System.Threading;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Imperial.Medieval.Boss;

[RegisterComponent]
public sealed partial class BossComponent : Component
{
    [DataField]
    public bool Active = false;

    [DataField]
    public ResPath MapPath = new ResPath("/Maps/Imperial/Medieval/boss-test-arena2.yml");

    [DataField(required: true)]
    public Dictionary<int, BossStageData> Stages = new();

    [DataField(required: true)]
    public float Health = 100f;

    [DataField]
    public ComponentRegistry ComponentsOnDefeat;

    [DataField]
    public SoundSpecifier? Song;

    [DataField]
    public float SongDuration = 240f;

    [DataField]
    public SoundSpecifier DefeatSound;

    [DataField]
    public SoundSpecifier LoseSound;

    [DataField]
    public string DefeatMessage = "boss-defeated-announce";

    [DataField]
    public string LoseMessage = "boss-won-announce";

    [ViewVariables(VVAccess.ReadWrite)]
    public EntityUid? SongEntity;

    [ViewVariables(VVAccess.ReadWrite)]
    public int Stage = 1;

    [ViewVariables(VVAccess.ReadWrite)]
    public List<EntityUid> Players = new List<EntityUid>();

    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan NextAttack = TimeSpan.Zero;

    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan NextSongPlay = TimeSpan.Zero;

    [DataField]
    public TimeSpan VictoryDelay = TimeSpan.FromSeconds(15); // Время до телепортации игроков обратно

    public TimeSpan? ReturnPlayersTime;

    [DataField]
    public string LinkId = "test"; // Сюда указывать такую же строку, что и в BossPlayerBackPoint. Это отправит игроков на неё в случае победы/поражения

    [DataField]
    public bool IsEndGame = true; // Начинать конец раунда при поражении/победе, как сейчас к морбиуса
}
