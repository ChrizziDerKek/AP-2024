using System;
using System.Collections.Generic;
using System.Linq;

namespace Projektarbeit.Model
{
    /// <summary>
    /// Stellt ein Puzzleteil dar
    /// </summary>
    public class Teil
    {
        //Codierung des Puzzleteils
        private Codierung[] codierung;

        //Bezeichnung
        private char bezeichnung;

        //Permutationen, die durch Drehen erzeugt werden
        private List<Teil> permutationen;

        /// <summary>
        /// Erstellt ein Puzzleteil mit der angegebenen Codierung und Bezeichnung
        /// </summary>
        /// <param name="code">Codierung</param>
        /// <param name="bez">Bezeichnung</param>
        public Teil(Codierung[] code, char bez)
        {
            codierung = code;
            bezeichnung = bez;
            //Permutationen sind zunaechst null
            //und werden durch Aufrufen von
            //GetPermutationen erzeugt
            permutationen = null;
        }

        /// <summary>
        /// Gibt die Bezeichnung aus
        /// </summary>
        /// <returns>Bezeichnung</returns>
        public char GetBezeichnung()
        {
            return bezeichnung;
        }

        /// <summary>
        /// Gibt die Codierung aus
        /// </summary>
        /// <returns>Codierung</returns>
        public Codierung[] GetCodierung()
        {
            return codierung;
        }

        /// <summary>
        /// Gibt die Permutationen aus, die durch Drehen erzeugt werden koennen
        /// </summary>
        /// <returns>Permutationen des Puzzleteils</returns>
        public Teil[] GetPermutationen()
        {
            //Permutationen muessen noch erstellt werden
            if (permutationen == null)
            {
                //Neue Liste erstellen und aktuelles Teil einfuegen
                permutationen = new List<Teil>() { this };
                //3 moegliche Permutationen erzeugen
                //Drehung nach unten
                Teil u = DreheU();
                //Drehung nach rechts
                Teil r = DreheR();
                //Drehung nach unten und rechts
                Teil ru = DreheRU();
                //Permutationen in Liste einfuegen, wenn sie unterschiedlich sind
                if (!IstGleich(u))
                    permutationen.Add(u);
                if (!IstGleich(r) && !u.IstGleich(r))
                    permutationen.Add(r);
                if (!IstGleich(ru) && !r.IstGleich(ru) && !u.IstGleich(ru))
                    permutationen.Add(ru);
            }
            //Fertige Liste in ein Array umwandeln und ausgeben
            return permutationen.ToArray();
        }

        /// <summary>
        /// Erzeugt eine Permutation des Puzzleteils durch eine Drehung nach unten
        /// </summary>
        /// <returns>Permutation</returns>
        private Teil DreheU()
        {
            Codierung[] code = new Codierung[codierung.Length];
            //Aktuelle Codierung durchgehen
            for (int i = 0; i < codierung.Length; i++)
            {
                //Oben wird zu unten und umgekehrt,
                //die restlichen Codierungen sind oben
                //und unten gleich und aendern sich somit nicht
                if (codierung[i] == Codierung.Oben)
                    code[i] = Codierung.Unten;
                else if (codierung[i] == Codierung.Unten)
                    code[i] = Codierung.Oben;
                else
                    code[i] = codierung[i];
            }
            //Neues Teil mit Codierung erstellen
            return new Teil(code, bezeichnung);
        }

        /// <summary>
        /// Erzeugt eine Permutation des Puzzleteils durch eine Drehung nach rechts
        /// </summary>
        /// <returns>Permutation</returns>
        private Teil DreheR()
        {
            //Codierung umkehren und neues Teil damit erstellen
            Codierung[] code = codierung.Reverse().ToArray();
            return new Teil(code, bezeichnung);
        }

        /// <summary>
        /// Erzeugt eine Permutation des Puzzleteils durch eine Drehung nach unten und rechts
        /// </summary>
        /// <returns>Permutation</returns>
        private Teil DreheRU()
        {
            //Nach unten gedrehtes Puzzleteil nach rechts drehen
            return DreheU().DreheR();
        }

        /// <summary>
        /// Gibt an, ob das aktuelle Teil mit dem uebergebenen Teil uebereinstimmt
        /// </summary>
        /// <param name="teil">Teil zum Vergleichen</param>
        /// <returns>Wahr wenn die Teile uebereinstimmen</returns>
        private bool IstGleich(Teil teil)
        {
            //Aktuelles Teil ist nie null
            if (teil == null)
                return false;
            //Bezeichnung muss gleich sein
            if (teil.bezeichnung != bezeichnung)
                return false;
            //Codierung muss gleiche Laenge haben
            if (teil.codierung.Length != codierung.Length)
                return false;
            //Codierungen durchlaufen und auf Unterschiede pruefen
            for (int i = 0; i < codierung.Length;i++)
                if (teil.codierung[i] != codierung[i])
                    return false;
            return true;
        }
    }
}