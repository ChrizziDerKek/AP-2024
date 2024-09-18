namespace Projektarbeit.Model
{
    /// <summary>
    /// Enumeration fuer die Kodierung eines Puzzleteils
    /// </summary>
    public enum Codierung : int
    {
        Loch = 0,
        Oben = 1,
        Unten = 2,
        Leer = 4,
        ObenUnten = Oben | Unten, //3
    }
}