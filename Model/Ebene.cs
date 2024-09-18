namespace Projektarbeit.Model
{
    /// <summary>
    /// Stelle eine einzelne Ebene in der Puzzlebox dar
    /// </summary>
    public class Ebene
    {
        //Ebenennummer, also Index + 1
        private int nummer;
        
        //n x n Matrix zum Speichern der Codierungen
        private Codierung[,] daten;

        //Array mit den Bezeichnern der Teile in der Ebene
        private char[] teile;

        /// <summary>
        /// Erstellt eine leere Ebene mit der angegebenen Laenge
        /// </summary>
        /// <param name="laenge">Laenge und Breite der Ebene</param>
        /// <param name="nummer">Ebenennummer</param>
        public Ebene(int laenge, int nummer)
        {
            teile = new char[laenge];
            daten = new Codierung[laenge, laenge];
            this.nummer = nummer;
        }

        /// <summary>
        /// Gibt an, ob die Ebene gedreht ist. Jede zweite Ebene ist gedreht
        /// </summary>
        /// <returns>Wahr wenn die Ebene gedreht ist</returns>
        public bool IstGedreht()
        {
            return nummer % 2 == 0;
        }

        /// <summary>
        /// Gibt die Codierung an der angegebenen Position aus
        /// </summary>
        /// <param name="a">X Position</param>
        /// <param name="b">Y Position</param>
        /// <returns>Codierung an der Position</returns>
        public Codierung GetPos(int x, int y)
        {
            return daten[x, y];
        }

        public void Einfuegen(Teil teil, int stelle)
        {
            Codierung[] teilCode = teil.GetCodierung();
            for (int i = 0; i < teilCode.Length; i++)
            {
                if (IstGedreht())
                    daten[stelle, i] = teilCode[i];
                else
                    daten[i, stelle] = teilCode[i];
            }
            teile[stelle] = teil.GetBezeichnung();
        }

        /// <summary>
        /// Gibt die Nummer der Ebene aus
        /// </summary>
        /// <returns>Ebenennummer</returns>
        public int GetNummer()
        {
            return nummer;
        }

        /// <summary>
        /// Gibt den Bezeichner des Teils an der angegebenen Stelle aus
        /// </summary>
        /// <param name="stelle">Stelle in der Ebene</param>
        /// <returns>Teil-Bezeichner</returns>
        public char GetTeil(int stelle)
        {
            return teile[stelle];
        }
    }
}