using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.Imperial.Medieval.Kitchen;

/// <summary>
///     imperial medieval - an open fire you can grill food on: click the fire with food in hand to lay it on
///     the coals, and everything the <see cref="Content.Shared.Placeable.ItemPlacerComponent"/> on the same entity
///     tracks gets heated. Unlike the stock electric grill it needs no power and has no settings.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class MedievalFireGrillComponent : Component
{
    /// <summary>
    ///     Heat given to each item on the fire every second, in joules.
    ///     The stock electric grill gives 2400 on its highest setting.
    /// </summary>
    [DataField]
    public float HeatPerSecond = 1200f;

    /// <summary>
    ///     What can be laid on the fire by clicking it.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    ///     What can never be laid on the fire, even if it passes the whitelist (e.g. live animals).
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;
}
