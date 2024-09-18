using Projektarbeit.Model;
using Projektarbeit.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Projektarbeit.Controller
{
    /// <summary>
    /// Klasse zum Finden einer Loesung mit der angegebenen Box und den Puzzleteilen
    /// </summary>
    public class Loesung
    {
        //Puzzlebox
        private Box box;

        //Puzzleteile
        private List<Teil> teile;

        /// <summary>
        /// Erstellt eine Loesung fuer das angegebene Puzzle
        /// </summary>
        /// <param name="box">Puzzlebox</param>
        /// <param name="teile">Verfuegbare Puzzleteile</param>
        public Loesung(Box box, List<Teil> teile)
        {
            this.box = box;
            this.teile = teile;

            //Loesung finden oder Ausnahme werfen,
            //wenn keine gefunden werden kann
            if (!Loesen(0, this))
                throw new InvalidDataException("Es wurde keine gueltige Loesung fuer das Puzzle gefunden");
        }

        /// <summary>
        /// Erstellt eine Kopie vom angegebenen Loesung-Objekt
        /// </summary>
        /// <param name="l">Objekt zum kopieren</param>
        public Loesung(Loesung l)
        {
            box = l.box;
            teile = new List<Teil>();
            foreach (Teil teil in l.teile)
                teile.Add(teil);
        }

        /// <summary>
        /// Backtracking-Algorithmus zum Suchen einer Loesung fuer das Puzzle
        /// </summary>
        /// <param name="teil">Gibt an, das wievielte Puzzleteil eingefuegt wird</param>
        /// <param name="loesung">Loesung-Objekt, das kopiert werden soll</param>
        /// <returns>Wahr wenn eine Loesung gefunden wurde</returns>
        private bool Loesen(int teil, Loesung loesung)
        {
            //Aktuelle Loesung kopieren, damit wir
            //bei einem fehlgeschlagenen Versuch
            //zurueckgehen koennen (Backtracking)
            Loesung kopie = new Loesung(loesung);
            //Zuletzt eingefuegtes Puzzleteil
            //wird aus Kopie entfernt
            if (teil > 0)
                kopie.EntferneTeil(teil - 1);
            //Wenn keine Puzzleteile mehr uebrig sind,
            //haben wir eine Loesung gefunden
            if (kopie.teile.Count <= 0)
                return true;
            //Alle verfuegbaren Teile und deren Permutationen durchgehen
            for (int i = 0; i < kopie.teile.Count; i++)
            {
                Teil[] permutationen = kopie.teile[i].GetPermutationen();
                for (int j = 0; j < permutationen.Length; j++)
                {
                    //Ebene und Stelle fuers aktuelle Teil finden
                    int ebene = GetTeilEbene(teil);
                    int stelle = GetTeilStelle(teil);
                    //Pruefen, ob das Teil an der aktuellen Stelle passt
                    if (box.IstErlaubt(permutationen[j], ebene, stelle))
                    {
                        //Wenn ja, einfuegen
                        box.Einfuegen(permutationen[j], ebene, stelle);
                        //Und den Ablauf fuer das naechste Teil
                        //wiederholen und die Kopie des Loesung-
                        //Objekts uebergeben, bis eine Teilloesung
                        //gefunden wurde
                        if (Loesen(teil + 1, kopie))
                            return true;
                    }
                }
            }
            //Alle Teile wurden geprueft und
            //keins davon hat gepasst
            return false;
        }

        /// <summary>
        /// Gibt an, in welche Ebene das n-te Puzzleteil gesetzt wird
        /// </summary>
        /// <param name="teil">Index des Puzzleteils</param>
        /// <returns>Ebene zum Einfuegen</returns>
        private int GetTeilEbene(int teil)
        {
            return teil / box.GetLaenge();
        }

        /// <summary>
        /// Gibt an, an welche Stelle in der Ebene das n-te Puzzleteil gesetzt wird
        /// </summary>
        /// <param name="teil">Index des Puzzleteils</param>
        /// <returns>Stelle in der Ebene</returns>
        private int GetTeilStelle(int teil)
        {
            return teil % box.GetLaenge();
        }

        /// <summary>
        /// Entfernt das n-te Puzzleteil aus der Liste, nachdem es eingefuegt wurde
        /// </summary>
        /// <param name="teil">Index des Puzzleteils</param>
        private void EntferneTeil(int teil)
        {
            //Ebene und Stelle von Teil Index holen
            int ebene = GetTeilEbene(teil);
            int stelle = GetTeilStelle(teil);
            //Bezeichnung aus Box bekommen
            char bezeichnung = box.GetEbenen()[ebene].GetTeil(stelle);
            //Teil-Liste durchgehen und Teil mit Bezeichnung entfernen
            for (int i = 0; i < teile.Count; i++)
            {
                if (teile[i].GetBezeichnung() == bezeichnung)
                {
                    teile.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Gibt die fertige Loesung ueber View aus
        /// </summary>
        public void LoesungAusgeben()
        {
            //Dimensionen der Puzzlebox ausgeben
            int laenge = box.GetLaenge();
            int hoehe = box.GetEbenen().Length;
            Ausgabe.Info("Dimension " + laenge + "," + laenge + "," + hoehe);
            Ausgabe.Info("Anordnung der Teile");
            //Die oberste Ebene soll zuerst ausgegeben werden, deshalb Ebenen umdrehen
            foreach (Ebene e in box.GetEbenen().Reverse())
            {
                Ausgabe.Info("Ebene " + e.GetNummer());
                string zeile;
                //Je nach Drehung die Ebene richtig ausgeben
                if (e.IstGedreht())
                {
                    //Bei Drehung Zeile fuer Zeile ausgeben und
                    //die Bezeichnung des Puzzleteils dran haengen
                    for (int i = 0; i < laenge; i++)
                    {
                        zeile = "";
                        for (int j = 0; j < laenge; j++)
                        {
                            int code = (int)e.GetPos(i, j);
                            zeile += " " + code;
                        }
                        zeile += " " + e.GetTeil(i);
                        Ausgabe.Info(zeile.Substring(1));
                    }
                }
                else
                {
                    //Ohne Drehung Zeile fuer Zeile ausgeben und
                    //extra Zeile mit Bezeichnungen ausgeben
                    for (int i = 0; i < laenge; i++)
                    {
                        zeile = "";
                        for (int j = 0; j < laenge; j++)
                        {
                            int code = (int)e.GetPos(i, j);
                            zeile += " " + code;
                        }
                        Ausgabe.Info(zeile.Substring(1));
                    }
                    zeile = "";
                    for (int i = 0; i < laenge; i++)
                        zeile += " " + e.GetTeil(i);
                    Ausgabe.Info(zeile.Substring(1));
                }
                //Ebenen durch leere Zeile trennen,
                //ausser bei letzter Ausgabe
                if (e.GetNummer() != 1)
                    Ausgabe.Info("");
            }
        }
    }
}