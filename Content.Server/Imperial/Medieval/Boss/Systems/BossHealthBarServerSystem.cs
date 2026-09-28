using Content.Shared.Imperial.Medieval.Boss;

namespace Content.Server.Imperial.Medieval.Boss;

public sealed class BossHealthBarServerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BossComponent, ComponentStartup>(OnBossStartup);
    }

    private void OnBossStartup(EntityUid uid, BossComponent boss, ComponentStartup args)
    {
        var bar = EnsureComp<BossHealthBarComponent>(uid);
        bar.MaxHp = boss.Health;
        bar.CurrentHp = boss.Health;
        bar.Active = boss.Active;
        Dirty(uid, bar);
    }

    public void SyncBossHealth(EntityUid uid, BossComponent boss, BossHealthBarComponent? bar = null)
    {
        if (!Resolve(uid, ref bar, false))
            return;

        bar.CurrentHp = Math.Max(0f, boss.Health);
        bar.Active = boss.Active;
        Dirty(uid, bar);
    }
}
