using System;
using System.IO;

namespace Projektarbeit.View
{
    /// <summary>
    /// Klasse mit Methoden zum Ausgeben von Informationen
    /// </summary>
    public class Ausgabe
    {
        //Ausgabedateipfad
        private static string datei = "";

        /// <summary>
        /// Legt die Ausgabedatei fest
        /// </summary>
        /// <param name="pfad">Dateipfad</param>
        public static void SetDatei(string pfad)
        {
            datei = pfad;
            //Leeren String in Datei schreiben,
            //um alte Daten zu ueberschreiben
            if (File.Exists(datei))
                File.WriteAllText(datei, "");
        }

        /// <summary>
        /// Fehlerausgabe in Konsole und Datei
        /// </summary>
        /// <param name="e">Exception Objekt zum Ausgeben</param>
        public static void Fehler(Exception e)
        {
            //Ausnahme ausgeben
            Info(e.Message);
            //Programm bei Fehler beenden
            Environment.Exit(0);
        }

        /// <summary>
        /// Methode zum Ausgeben von Informationen in die Konsole und eine Ausgabedatei
        /// </summary>
        /// <param name="info">Information zum ausgeben</param>
        public static void Info(string info)
        {
            Console.WriteLine(info);
            //Falls keine Ausgabedatei angegeben wurde,
            //geben wir die Informationen nur in die
            //Konsole aus
            if (datei == "")
                return;
            //Datei erstellen, wenn sie nicht existiert
            if (!File.Exists(datei))
                File.CreateText(datei).Close();
            //Alte Daten aus Datei lesen
            string temp = File.ReadAllText(datei);
            //Alte Daten und neue Daten in die Datei schreiben
            File.WriteAllText(datei, temp + info + "\n");
        }
    }
}