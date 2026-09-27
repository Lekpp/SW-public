using Content.Shared.Kitchen.Components;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;

namespace Content.Client.Imperial.Medieval.UserInterface.Windows;

/// <summary>
///     imperial medieval - drives <see cref="MedievalCampfireMenu"/>. Talks to the stock MicrowaveSystem
///     with the stock messages, so cooking itself is untouched.
/// </summary>
[UsedImplicitly]
public sealed class MedievalCampfireBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private MedievalCampfireMenu? _menu;

    public MedievalCampfireBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<MedievalCampfireMenu>();
        _menu.SetFire(Owner);
        _menu.StartButton.OnPressed += _ => SendPredictedMessage(new MicrowaveStartCookMessage());
        _menu.EjectButton.OnPressed += _ => SendPredictedMessage(new MicrowaveEjectMessage());
        _menu.OnIngredientPressed += uid =>
            SendPredictedMessage(new MicrowaveEjectSolidIndexedMessage(EntMan.GetNetEntity(uid)));

        _menu.OnCookTimeSelected += seconds =>
        {
            // button index is what the microwave expects: 0 = warm up, 1..6 = 5..30 s
            SendPredictedMessage(new MicrowaveSelectCookTimeMessage(
                (int) (seconds / MedievalCampfireMenu.SecondsPerLog), seconds));
            _menu.SetCookTime(seconds);
        };
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not MicrowaveUpdateUserInterfaceState cState || _menu == null)
            return;

        var empty = cState.ContainedSolids.Length == 0;
        var cookSeconds = cState.ActiveButtonIndex == 0 ? 0u : cState.CurrentCookTime;

        // the server sends its "done" state before it clears the busy flag; a zeroed end time means it's finished
        var busy = cState.IsMicrowaveBusy && cState.CurrentCookTimeEnd != TimeSpan.Zero;

        _menu.SetBusy(busy, empty, cState.CurrentCookTimeEnd, cookSeconds);
        _menu.SetContents(BuildContents(EntMan.GetEntityArray(cState.ContainedSolids)));
        _menu.SetCookTime(cookSeconds);
    }

    private List<(EntityUid, string, Texture?)> BuildContents(EntityUid[] solids)
    {
        var contents = new List<(EntityUid, string, Texture?)>();
        var sprites = EntMan.System<SpriteSystem>();

        foreach (var entity in solids)
        {
            if (EntMan.Deleted(entity))
                continue;

            Texture? texture = null;
            if (EntMan.TryGetComponent<IconComponent>(entity, out var icon))
                texture = sprites.GetIcon(icon);
            else if (EntMan.TryGetComponent<SpriteComponent>(entity, out var sprite))
                texture = sprite.Icon?.Default;

            contents.Add((entity, EntMan.GetComponent<MetaDataComponent>(entity).EntityName, texture));
        }

        return contents;
    }
}
