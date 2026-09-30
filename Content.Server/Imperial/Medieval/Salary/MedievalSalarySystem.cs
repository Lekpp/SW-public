using Content.Server.Imperial.Medieval.Trading;
using Content.Server.Stack;
using Content.Shared.Imperial.Medieval.Salary;
using Content.Shared.Interaction;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.SSDIndicator;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;

namespace Content.Server.Imperial.Medieval.Salary;

public sealed class MedievalSalarySystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly StackSystem _stack = default!;
    [Dependency] private readonly TradingItemDeliverySystem _delivery = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MedievalSalaryReceiverComponent, MapInitEvent>(OnReceiverMapInit);
        SubscribeLocalEvent<MedievalAtmComponent, ActivateInWorldEvent>(OnAtmActivate);
    }

    private void OnReceiverMapInit(Entity<MedievalSalaryReceiverComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.NextPayment = _timing.CurTime + ent.Comp.ReceiveTimer;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var curTime = _timing.CurTime;
        var query = EntityQueryEnumerator<MedievalSalaryReceiverComponent>();
        while (query.MoveNext(out var uid, out var receiver))
        {
            if (_mobState.IsDead(uid) || TryComp<SSDIndicatorComponent>(uid, out var ssd) && ssd.IsSSD)
            {
                receiver.NextPayment += TimeSpan.FromSeconds(frameTime);
                continue;
            }

            if (curTime < receiver.NextPayment)
                continue;

            receiver.Balance += receiver.PaymentAmount;
            receiver.NextPayment = curTime + receiver.ReceiveTimer;
        }
    }

    private void OnAtmActivate(Entity<MedievalAtmComponent> ent, ref ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        args.Handled = true;

        if (!TryComp<MedievalSalaryReceiverComponent>(args.User, out var receiver) || receiver.Balance <= 0)
        {
            _popup.PopupEntity(Loc.GetString("medieval-atm-empty"), ent, args.User);
            return;
        }

        var amount = receiver.Balance;
        receiver.Balance = 0;

        foreach (var cash in _stack.SpawnMultiple(ent.Comp.Cash.Id, amount, Transform(args.User).Coordinates))
        {
            _delivery.Deliver(cash, args.User);
        }

        _audio.PlayPvs(ent.Comp.WithdrawSound, ent);
        _popup.PopupEntity(Loc.GetString("medieval-atm-withdraw", ("amount", amount)), ent, args.User);
    }
}
