using Content.Shared.Imperial.Medieval.Trading;
using Content.Shared.Store;
using Robust.Shared.Prototypes;

namespace Content.Server.Imperial.Medieval.Trading;

[RegisterComponent]
public sealed partial class BidReceiptComponent : Component
{
    [DataField]
    public Guid OfferId;

    [DataField]
    public EntityUid Pit;

    [DataField]
    public string LotName = string.Empty;

    [DataField]
    public int Amount;

    [DataField]
    public ProtoId<CurrencyPrototype> Currency = "Revent";

    [DataField]
    public BidReceiptStatus Status;
}
