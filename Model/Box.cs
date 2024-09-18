namespace Projektarbeit.Model
{
    /// <summary>
    /// Stellt die Puzzlebox dar
    /// </summary>
    public class Box
    {
        //Ebenen der Box
        private Ebene[] ebenen;

        //Laenge und Breite
        private int laenge;
        
        /// <summary>
        /// Erstellt eine leere Box mit der angegebenen Laenge und Hoehe
        /// </summary>
        /// <param name="laenge">Laenge und Breite der Box</param>
        /// <param name="hoehe">Hoehe der Box, Anzahl der Ebenen</param>
        public Box(int laenge, int hoehe)
        {
            this.laenge = laenge;
            ebenen = new Ebene[hoehe];
            //Laenge und Ebenennummer an Ebenen uebergeben
            for (int i = 0; i < ebenen.Length; i++)
                ebenen[i] = new Ebene(laenge, i + 1);
        }

        /// <summary>
        /// Gibt an ob ein Teil-Objekt an der angegebenen Stelle und Ebene passt
        /// </summary>
        /// <param name="teil">Teil-Objekt</param>
        /// <param name="ebene">Ebene</param>
        /// <param name="stelle">Position in der Ebene</param>
        /// <returns></returns>
        public bool IstErlaubt(Teil teil, int ebene, int stelle)
        {
            //Jedes Teil kann in der untersten Ebene eingefuegt werden
            if (ebene == 0)
                return true;
            Codierung[] teilCode = teil.GetCodierung();
            bool drehung = ebenen[ebene].IstGedreht();
            //Codierung des Puzzleteils durchgehen
            for (int i = 0; i < teilCode.Length; i++)
            {
                //Codierung des Puzzleteils
                Codierung cTeil = teilCode[i];
                //Codierungen unter dem Puzzleteil je nach Drehung speichern
                Codierung cEbeneUnten = ebenen[ebene - 1].GetPos(i, stelle);
                if (drehung)
                    cEbeneUnten = ebenen[ebene - 1].GetPos(stelle, i);
                //Codierung unter der Ebene speichern, um herauszufinden, ob Loecher belegt sind
                Codierung cEbeneWeiterUnten = Codierung.Loch;
                //In Ebene 1 gibt es noch keine untere Ebene
                if (ebene != 1)
                {
                    cEbeneWeiterUnten = ebenen[ebene - 2].GetPos(i, stelle);
                    if (drehung)
                        cEbeneWeiterUnten = ebenen[ebene - 2].GetPos(stelle, i);
                }
                //Codierungen mit Hilfsmethode vergleichen
                if (!IstKompatibel(cTeil, cEbeneUnten, cEbeneWeiterUnten))
                    return false;
            }
            //Wahr wenn alle Codierungen zusammenpassen
            return true;
        }

        /// <summary>
        /// Hilfsmethode fuer IstErlaubt, gibt an ob Codierungen uebereinander sein koennen
        /// </summary>
        /// <param name="oben">Obere Codierung</param>
        /// <param name="unten">Untere Codierung zum Vergleichen</param>
        /// <param name="weiterUnten">Codierung unter unterer Codierung, wird z.B. fuer Fall Unten-Loch-Oben benoetigt</param>
        /// <returns>Ob Codierung "oben" ueber die anderen beiden passt</returns>
        private bool IstKompatibel(Codierung oben, Codierung unten, Codierung weiterUnten)
        {
            switch (oben)
            {
                case Codierung.Oben:
                case Codierung.Leer:
                    //Oben und Leer kann nur auf Loch, Leer oder Unten gesetzt werden
                    return unten != Codierung.Oben && unten != Codierung.ObenUnten;
                case Codierung.Unten:
                case Codierung.ObenUnten:
                    //Unten und ObenUnten kann nur auf Loch gesetzt werden, wenn das Loch nicht belegt ist
                    return unten == Codierung.Loch && weiterUnten != Codierung.Oben && weiterUnten != Codierung.ObenUnten;
            }
            //Loch kann auf alle anderen Zellen gesetzt werden
            return true;
        }

        /// <summary>
        /// Fuegt ein Teil-Objekt in die Box ein
        /// </summary>
        /// <param name="teil">Teil-Objekt, das eingefuegt werden soll</param>
        /// <param name="ebene">Ebene zum einfuegen</param>
        /// <param name="stelle">Position in der Ebene</param>
        public void Einfuegen(Teil teil, int ebene, int stelle)
        {
            ebenen[ebene].Einfuegen(teil, stelle);
        }

        /// <summary>
        /// Gibt die Ebenen der Box aus
        /// </summary>
        /// <returns>Ebenen der Box</returns>
        public Ebene[] GetEbenen()
        {
            return ebenen;
        }

        /// <summary>
        /// Gibt die Laenge der Box aus
        /// </summary>
        /// <returns>Laenge der Box</returns>
        public int GetLaenge()
        {
            return laenge;
        }
    }
}