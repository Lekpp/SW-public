using Content.Server.Temperature.Systems;
using Content.Shared.Imperial.Medieval.Kitchen;
using Content.Shared.Placeable;

namespace Content.Server.Imperial.Medieval.Kitchen;

/// <summary>
///     imperial medieval - heats whatever lies on an open fire with <see cref="MedievalFireGrillComponent"/>.
///     Raw meat then turns into a steak through its stock temperature construction step, same as on the
///     electric grill.
/// </summary>
public sealed class MedievalFireGrillSystem : EntitySystem
{
    [Dependency] private readonly TemperatureSystem _temperature = default!;

    // reused every tick: heating can finish a cooking step, and the finished item replaces the raw one,
    // which changes the placer's set; never iterate the live set while heating
    private readonly List<EntityUid> _onFire = new();

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<MedievalFireGrillComponent, ItemPlacerComponent>();
        while (query.MoveNext(out _, out var grill, out var placer))
        {
            if (placer.PlacedEntities.Count == 0)
                continue;

            _onFire.Clear();
            _onFire.AddRange(placer.PlacedEntities);

            var heat = grill.HeatPerSecond * frameTime;
            foreach (var item in _onFire)
            {
                if (TerminatingOrDeleted(item))
                    continue;

                _temperature.ChangeHeat(item, heat);
            }
        }
    }
}
