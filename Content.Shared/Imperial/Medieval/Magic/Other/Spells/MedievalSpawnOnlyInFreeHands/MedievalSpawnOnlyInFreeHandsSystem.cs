using Content.Shared.Actions;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Imperial.Medieval.Magic;
using Content.Shared.Imperial.Medieval.Magic.Mana;
using Content.Shared.Interaction;
using Content.Shared.Magic.Events;
using Content.Shared.Popups;
using Robust.Shared.Network;

namespace Content.Shared.Imperial.Medieval.Magic.MedievalSpawnOnlyInFreeHands;


public sealed partial class MedievalSpawnOnlyInFreeHandsSystem : EntitySystem
{
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!;
    [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly INetManager _net = default!;


    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MedievalSpawnOnlyInFreeHandsComponent, MedievalBeforeCastSpellEvent>(OnBeforeSpellCast, before: new[] { typeof(ManaSystem) });
    }

    private void OnBeforeSpellCast(EntityUid uid, MedievalSpawnOnlyInFreeHandsComponent component, ref MedievalBeforeCastSpellEvent args)
    {
        if (_handsSystem.CountFreeHands(args.Performer) >= component.Count) return;

        if (component.NotEnoughMessage != null && _net.IsServer)
            _popup.PopupEntity(Loc.GetString(component.NotEnoughMessage.Value), args.Performer, args.Performer);

        _actionsSystem.ClearCooldown(uid);
        args.Cancelled = true;
    }
}
