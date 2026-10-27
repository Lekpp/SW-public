using System.Numerics;
using Content.Server.Weapons.Ranged.Systems;
using Content.Shared.Imperial.Medieval.Magic;
using Content.Shared.Imperial.Medieval.Magic.MedievalLaunchAfterSpawn;
using Content.Shared.Imperial.Medieval.Magic.MedievalTargetOverlayOnAiming;
using Robust.Shared.Timing;

namespace Content.Server.Imperial.Medieval.Magic.MedievalLaunchAfterSpawn;


public sealed class MedievalLaunchAfterSpawnSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly GunSystem _gun = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MedievalLaunchAfterSpawnComponent, MedievalAfterAimingSpawnBySpellEvent>(OnSpawn);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<MedievalLaunchAfterSpawnComponent>();

        while (query.MoveNext(out var uid, out var component))
        {
            if (component.LaunchAt == null || _timing.CurTime < component.LaunchAt) continue;

            Launch(uid, component);
        }
    }

    private void OnSpawn(EntityUid uid, MedievalLaunchAfterSpawnComponent component, MedievalAfterAimingSpawnBySpellEvent args)
    {
        var index = CountSameCast(uid, args.Performer);

        // The target list comes from the client and the spell code spawns an entity for each of them,
        // so the limit of the action has to be enforced here
        if (TryComp<MedievalTargetOverlayOnAimingComponent>(args.Action, out var aiming) && index >= aiming.MaxTargetCount)
        {
            QueueDel(uid);

            return;
        }

        component.Performer = args.Performer;
        component.Target = args.Target;
        component.SpawnedAt = _timing.CurTime;
        component.LaunchAt = _timing.CurTime + TimeSpan.FromSeconds(component.Delay + component.DelayStep * index);

        var origin = _transform.GetWorldPosition(args.Performer);
        var direction = GetTargetPosition(component) - origin;

        if (direction.LengthSquared() < 0.0001f) return;

        var slot = Angle.FromDegrees(component.SlotAngle * ((index + 1) / 2) * (index % 2 == 1 ? 1 : -1));

        _transform.SetWorldPosition(uid, origin + slot.RotateVec(Vector2.Normalize(direction)) * component.Radius);
    }

    /// <summary>
    /// How many entities of the same cast are already spawned
    /// </summary>
    private int CountSameCast(EntityUid self, EntityUid performer)
    {
        var count = 0;
        var query = EntityQueryEnumerator<MedievalLaunchAfterSpawnComponent>();

        while (query.MoveNext(out var uid, out var other))
        {
            if (uid == self || other.Performer != performer) continue;
            if (_timing.CurTime - other.SpawnedAt > TimeSpan.FromSeconds(0.5)) continue;

            count++;
        }

        return count;
    }

    private void Launch(EntityUid uid, MedievalLaunchAfterSpawnComponent component)
    {
        component.LaunchAt = null;

        var performerExists = Exists(component.Performer);
        var origin = _transform.GetWorldPosition(uid);
        var direction = GetTargetPosition(component) - origin;

        if (direction.LengthSquared() < 0.0001f)
            direction = performerExists ? _transform.GetWorldRotation(component.Performer).ToWorldVec() : Vector2.UnitY;

        var projectile = Spawn(component.Projectile, Transform(uid).Coordinates);
        var shooter = performerExists ? component.Performer : (EntityUid?) null;
        _gun.ShootProjectile(projectile, direction, Vector2.Zero, shooter, shooter, component.Speed);

        QueueDel(uid);
    }

    private Vector2 GetTargetPosition(MedievalLaunchAfterSpawnComponent component)
    {
        if (component.Target.TargetEntity is { } target && !TerminatingOrDeleted(target))
            return _transform.GetWorldPosition(target);

        return component.Target.CursorPosition.Position;
    }
}
