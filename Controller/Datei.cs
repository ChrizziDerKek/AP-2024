using System.Collections.Generic;
using System.IO;
using Projektarbeit.Model;

namespace Projektarbeit.Controller
{
    /// <summary>
    /// Klasse zum Auslesen einer Eingabedatei
    /// </summary>
    public class Datei
    {
        //Hoehe der Box
        private int hoehe;

        //Laenge und Breite der Box
        private int laenge;

        //Kommentare aus der Datei
        private List<string> kommentare;

        //Puzzleteile aus der Datei
        private List<Teil> teile;

        //Dateipfad
        private string pfad;

        /// <summary>
        /// Erstellt ein Objekt zum Auslesen einer Eingabedatei und gibt ggf. Fehler aus
        /// </summary>
        /// <param name="pfad">Dateipfad</param>
        public Datei(string pfad)
        {
            //Ausnahme werfen, wenn Datei nicht existiert
            if (!File.Exists(pfad))
                throw new FileNotFoundException("Datei \"" + pfad + "\" existiert nicht");

            //Datei auslesen
            string input = File.ReadAllText(pfad);

            //Ausnahme werfen, wenn Datei leer ist
            if (string.IsNullOrEmpty(input))
                throw new InvalidDataException("Datei \"" + pfad + "\" ist leer");

            //Variablen initialisieren
            hoehe = 0;
            laenge = 0;
            kommentare = new List<string>();
            teile = new List<Teil>();
            this.pfad = pfad;
            List<string> zeilen = new List<string>();
            int anzahlTeile = 0;

            //Eingabe Zeile fuer Zeile lesen, leere Zeilen ignorieren
            string[] temp = input.Split('\n');
            for (int i = 0; i < temp.Length; i++)
                if (!string.IsNullOrEmpty(temp[i]))
                    zeilen.Add(temp[i]);

            //Zeile fuer Zeile durchgehen, je nach Art behandeln
            int zeilenNummer = 0;
            bool dimensionGefunden = false;
            foreach (string zeile in zeilen)
            {
                //Zeilennummer fuer Fehlerbehandlung erhoehen
                zeilenNummer++;

                //Kommentare fangen mit "//" an
                if (zeile.StartsWith("//"))
                {
                    kommentare.Add(zeile);
                    continue;
                }

                //Die Dimensionsangabe faengt mit "Dimension " an
                if (zeile.StartsWith("Dimension "))
                {
                    int[] dim = LeseDimension(zeile, zeilenNummer);
                    laenge = dim[0];
                    hoehe = dim[1];
                    dimensionGefunden = true;
                    continue;
                }

                //Ansonsten Puzzleteil aus Zeile erstellen
                teile.Add(LeseTeil(zeile, zeilenNummer));
                anzahlTeile++;
            }

            //Ausnahme werfen, wenn keine Dimensionsangabe gefunden wurde
            if (!dimensionGefunden)
                throw new InvalidDataException("Datei \"" + pfad + "\" hat keine Dimensionsangabe");

            //Ausnahme werfen, wenn es zu viele Puzzleteile gibt
            if (anzahlTeile > laenge * hoehe)
                throw new InvalidDataException("Datei \"" + pfad + "\" hat zu viele Puzzleteile");

            //Ausnahme werfen, wenn es keine Puzzleteile gibt
            if (anzahlTeile <= 0)
                throw new InvalidDataException("Datei \"" + pfad + "\" hat keine Puzzleteile");
        }

        /// <summary>
        /// Gibt die Laenge und Hoehe aus der Dimensionszeile aus
        /// </summary>
        /// <param name="zeile">Dimensionszeile aus Eingabedatei</param>
        /// <param name="zeilennummer">Zeilennummer fuer Fehlerbehandlung</param>
        /// <returns>Laenge und Hoehe</returns>
        private int[] LeseDimension(string zeile, int zeilennummer)
        {
            int[] ausgabe = new int[2];
            //"Dimension " aus Zeile loeschen und mit Kommata trennen
            string[] dim = zeile.Substring(10).Split(',');
            //Laenge speichern
            ausgabe[0] = int.Parse(dim[0]);
            //Breite sollte immer gleich sein,
            //darum Ausnahme werfen wenn dem nicht so ist
            int breite = int.Parse(dim[1]);
            if (ausgabe[0] != breite)
                throw new InvalidDataException("Laenge " + ausgabe[0] + " und Breite " + breite + " in Datei \"" + pfad + "\" in Zeile " + zeilennummer + " sind nicht gleich");
            //Hoehe speichern
            ausgabe[1] = int.Parse(dim[2]);
            return ausgabe;
        }

        /// <summary>
        /// Erstellt ein Puzzleteil Objekt aus einer Zeile in der Eingabedatei
        /// </summary>
        /// <param name="zeile">Zeile aus Eingabedatei</param>
        /// <param name="zeilennummer">Zeilennummer fuer Fehlerbehandlung</param>
        /// <returns>Teil Objekt</returns>
        private Teil LeseTeil(string zeile, int zeilennummer)
        {
            //Daten mit Leerzeichen trennen,
            //der erste Buchstabe ist die Bezeichnung
            string[] daten = zeile.Split(' ');
            char bezeichnung = daten[0][0];
            //Ausnahme werfen, wenn Bezeichnung kein Grossbuchstabe ist
            if (!char.IsLetter(bezeichnung) || !char.IsUpper(bezeichnung))
                throw new InvalidDataException("Bezeichnung " + bezeichnung + " in Datei \"" + pfad + "\" in Zeile " + zeilennummer + " ist nicht zulaessig");
            //Daten nach dem Leerzeichen ist die Codierung, diese wird mit Kommata getrennt
            string[] codierungen = daten[1].Split(',');
            //Ausnahme werfen, wenn es zu viele Codierungen gibt
            if (codierungen.Length != laenge && laenge != 0)
                throw new InvalidDataException("Zu viele Codierungen in Datei \"" + pfad + "\" in Zeile " + zeilennummer);
            Codierung[] co = new Codierung[codierungen.Length];
            int i = 0;
            //Alle Codierungen durchgehen
            foreach (string code in codierungen)
            {
                //Codierung zu Int umwandeln
                int codierung = int.Parse(code);
                //Ausnahme bei unzulaessiger Codierung werfen
                if (codierung < 0 || codierung > 4)
                    throw new InvalidDataException("Codierung " + codierung + " in Datei \"" + pfad + "\" in Zeile " + zeilennummer + " ist nicht zulaessig");
                //Codierung in Array speichern
                co[i++] = (Codierung)codierung;
            }
            //Neues Teil mit Codierungsarray und Bezeichnung erstellen
            return new Teil(co, bezeichnung);
        }

        /// <summary>
        /// Gibt die Hoehe der Box aus
        /// </summary>
        /// <returns>Hoehe der Box</returns>
        public int GetHoehe()
        {
            return hoehe;
        }

        /// <summary>
        /// Gibt die Laenge der Box aus
        /// </summary>
        /// <returns>Laenge der Box</returns>
        public int GetLaenge()
        {
            return laenge;
        }

        /// <summary>
        /// Gibt die Kommentare aus
        /// </summary>
        /// <returns>Liste von Kommentaren</returns>
        public List<string> GetKommentare()
        {
            return kommentare;
        }

        /// <summary>
        /// Gibt die Puzzleteile aus
        /// </summary>
        /// <returns>Liste von Puzzleteilen</returns>
        public List<Teil> GetTeile()
        {
            return teile;
        }
    }
}