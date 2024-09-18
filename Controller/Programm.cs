using System;
using System.IO;
using Projektarbeit.Model;
using Projektarbeit.View;
using System.Diagnostics;

namespace Projektarbeit.Controller
{
    /// <summary>
    /// Beinhaltet die Main-Methode des Programms
    /// </summary>
    public class Programm
    {
        /// <summary>
        /// Main-Methode des Programms, wird beim Start automatisch mit den Befehlszeilenargumenten aufgerufen
        /// </summary>
        /// <param name="args">Befehlszeilenargumente mit Ein- und Ausgabedatei</param>
        static void Main(string[] args)
        {
            try
            {
                //Hartkodierte Pfade, um das Debuggen zu erleichtern
                string inputpfad = "../../Input/beispiel_1.txt";
                string outputpfad = "../../Output/beispiel_1.txt";

                //Wenn kein Debugger angehaengt ist,
                //lesen wir die Befehlszeilenargumente aus
                //und ersetzen unsere hartkodierten Pfade
                if (!Debugger.IsAttached)
                {
                    if (args.Length == 1)
                        throw new ArgumentNullException("Keine Output-Datei in Kommandozeile gefunden");
                    else if (args.Length < 1)
                        throw new ArgumentNullException("Keine Input- und Output-Datei in Kommandozeile gefunden");
                    inputpfad = args[0];
                    outputpfad = args[1];
                }

                //Ausgabedatei setzen und ggf. leeren
                Ausgabe.SetDatei(outputpfad);
                //Eingabedatei einlesen
                Datei eingabe = new Datei(inputpfad);
                //Leere Box mit Laenge und Hoehe erstellen
                Box box = new Box(eingabe.GetLaenge(), eingabe.GetHoehe());
                //Box und Teileliste an Loesung uebergeben
                Loesung loesung = new Loesung(box, eingabe.GetTeile());

                //Kommentare aus Eingabedatei ausgeben
                foreach (string kommentar in eingabe.GetKommentare())
                    Ausgabe.Info(kommentar);
                //Loesung ausgeben, wenn es eine gibt
                loesung.LoesungAusgeben();
            }
            catch (Exception ex)
            {
                //Gibt Ausnahmen in die Konsole und Ausgabedatei aus
                Ausgabe.Fehler(ex);
            }
        }
    }
}