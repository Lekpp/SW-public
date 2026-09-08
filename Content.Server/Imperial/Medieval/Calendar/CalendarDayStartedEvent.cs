using Robust.Shared.Prototypes;

namespace Content.Shared.Imperial.Medieval.Calendar;

public sealed class CalendarDayStartedEvent : EntityEventArgs
{
    public int DayNumber { get; }
    public ProtoId<CalendarEventPrototype> EventId { get; }
    public CalendarEventPrototype Prototype { get; }

    public CalendarDayStartedEvent(int dayNumber, ProtoId<CalendarEventPrototype> eventId, CalendarEventPrototype prototype)
    {
        DayNumber = dayNumber;
        EventId = eventId;
        Prototype = prototype;
    }
}
