namespace Content.Shared.Imperial.ImperialStore;

/// <summary>
/// imperial medieval - essence slots of a grimoire. A slot is an entry of <see cref="ImperialStoreComponent.Bonuses"/>
/// holding one unit of a school currency, every balance update adds the sum of all slots to the balance.
/// </summary>
public static class ImperialStoreSlots
{
    /// <summary>
    /// Currencies that count as magic schools, only these can be put into a slot.
    /// </summary>
    public static readonly string[] Schools =
    [
        "MagicMedievalFire",
        "MagicMedievalLight",
        "MagicMedievalVodka",
        "MagicMedievalEarth",
        "MagicMedievalDarkness",
    ];

    /// <summary>
    /// How long putting a school into a slot takes.
    /// </summary>
    public const float ChangeSeconds = 3f;
}
