using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Placeable;
using Content.Shared.Popups;
using Content.Shared.Whitelist;

namespace Content.Shared.Imperial.Medieval.Kitchen;

/// <summary>
///     imperial medieval - lays food on an open fire when the fire is clicked with it. Works like a table's
///     placeable surface, but only for what the fire's whitelist allows, so a torch clicked on the fire
///     still just gets lit and a bag doesn't get dumped into the flames.
/// </summary>
public sealed class MedievalFireGrillPlacementSystem : EntitySystem
{
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelist = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MedievalFireGrillComponent, AfterInteractUsingEvent>(OnAfterInteractUsing);
    }

    private void OnAfterInteractUsing(Entity<MedievalFireGrillComponent> ent, ref AfterInteractUsingEvent args)
    {
        if (args.Handled || !args.CanReach)
            return;

        if (_whitelist.IsWhitelistFail(ent.Comp.Whitelist, args.Used)
            || _whitelist.IsBlacklistPass(ent.Comp.Blacklist, args.Used))
            return;

        if (TryComp<ItemPlacerComponent>(ent, out var placer)
            && placer.MaxEntities > 0
            && placer.PlacedEntities.Count >= placer.MaxEntities)
        {
            _popup.PopupClient(Loc.GetString("medieval-fire-grill-full"), ent, args.User);
            args.Handled = true;
            return;
        }

        if (!_hands.TryDrop(args.User, args.Used))
            return;

        _transform.SetCoordinates(args.Used, args.ClickLocation);
        args.Handled = true;
    }
}
