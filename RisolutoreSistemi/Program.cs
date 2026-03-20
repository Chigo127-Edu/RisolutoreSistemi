using System;

namespace RisolutoreSistemi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Benvenuto! Questo programma permette di risolvere un sistema di equazioni a N incognite.");
            bool Continua = true;
            bool Errore = false;

            float[,] Matrice = null;
            float[] ValNoti = null;
            ushort Dimensione = 0;

            do
            {
                StampaMenuPrincipale(Errore);
                Errore = false;
                string Scelta = Console.ReadLine();
                switch (Scelta)
                {
                    case "1":
                        Console.Clear();

                        Dimensione = RichiestaDimens();
                        Matrice = new float[Dimensione, Dimensione];
                        ValNoti = new float[Dimensione];

                        break;
                    case "2":
                        bool ContinuaRiempimento = true;
                        bool ErroreRiempimento = false;

                        do
                        {
                            StampaSottomenuRiempimento(ErroreRiempimento);
                            ErroreRiempimento = false;
                            string SceltaRiempimento = Console.ReadLine();
                            switch (SceltaRiempimento)
                            {
                                case "1":
                                    RiempimentoGuidato(Dimensione, Matrice, ValNoti);
                                    ContinuaRiempimento = false;
                                    break;
                                case "0":
                                    ContinuaRiempimento = false;
                                    break;
                                default:
                                    ErroreRiempimento = true;
                                    break;
                            }
                        }
                        while (ContinuaRiempimento);

                        // vai al sottomenu
                        break;
                    case "3":
                        StampaSistema(Dimensione, Matrice, ValNoti);
                        break;
                    /*case "4":
                        // fx risolvi
                        break;*/
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Uscita...");
                        Continua = false;
                        break;
                    default:
                        Errore = true;
                        break;
                }
            }
            while (Continua);
        }

        static float RichiestaInputSicuro(string Messaggio)
        {
            Console.Write(Environment.NewLine + Messaggio + ": ");
            bool Fail = false;
            float Tempinput = 0;

            do
            {
                Fail = false;
                try
                {
                    Tempinput = float.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Write(Environment.NewLine + "Il valore inserito non è valido, Riprovare: ");
                    Fail = true;
                }
            } while (Fail);

            return Tempinput;
        }

        static ushort RichiestaDimens()
        {
            Console.Write("Si prega di inserire la dimensione della matrice: ");
            bool Fail = false;
            ushort Tempinput = 0;

            do
            {
                Fail = false;
                try
                {
                    Tempinput = ushort.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Write(Environment.NewLine + "Il valore inserito non è valido, Riprovare: ");
                    Fail = true;
                }
            } while (Fail);

            return Tempinput;
        }

        static string FormaEspressioneNecessaria(ushort NIncognite)
        {
            string TempString = "";
            for (ushort Indice = 1; Indice <= NIncognite; Indice++)
            {
                TempString += $"{(char)(64 + Indice)}x{Indice} +/- ";
            }

            TempString = TempString.Remove(TempString.Length - 2) + $" = {(char)(65 + NIncognite)}";
            return TempString;
        }

        static bool DomandaChiusa(string Messaggio)
        {
            Console.Write(Messaggio + " (Digitare \"y\"/\"Y\" per accettare, qualunque altro tasto per rifiutare): ");

            string Tempinput = Console.ReadLine();

            if (Tempinput == "y" || Tempinput == "Y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void RiempimentoGuidato(ushort Dimensione, float[,] Matrice, float[] ValNoti)
        {
            Console.Clear();

            if (IlSistemaEVuoto(Matrice, ValNoti))
            {
                SegnalaSistemaVuoto();
            }
            else
            {
                for (int Riga = 0; Riga < Dimensione; Riga++)
                {
                    Console.WriteLine($"Verranno ora richiesti, passo-passo, i parametri della {Riga + 1}^ equazione" + Environment.NewLine + $"La forma deve essere {FormaEspressioneNecessaria(Dimensione)}.");
                    for (int Colonna = 0; Colonna < Dimensione; Colonna++)
                    {
                        Matrice[Riga, Colonna] = RichiestaInputSicuro($"Inserire il parametro {(char)(65 + Colonna)}");
                    }
                    ValNoti[Riga] = RichiestaInputSicuro($"Inserire il parametro {(char)(65 + Dimensione)}");
                }
            }
        }

        static void StampaSistema(ushort Dimensione, float[,] Matrice, float[] ValNoti)
        {
            Console.Clear();

            if (IlSistemaEVuoto(Matrice, ValNoti))
            {
                SegnalaSistemaVuoto();
            }
            else
            {
                Console.WriteLine("Il sistema è quanto segue: ");
            }
            Console.Write(Environment.NewLine);

            for (ushort Riga = 0; Riga < Dimensione; Riga++)
            {
                for (ushort Colonna = 0; Colonna < Dimensione; Colonna++)
                {
                    if (Matrice[Riga, Colonna] >= 0)
                    {
                        Console.Write($"\t+{Matrice[Riga, Colonna]}(x{Colonna + 1})");
                    }
                    else
                    {
                        Console.Write($"\t{Matrice[Riga, Colonna]}(x{Colonna + 1})");
                    }
                }
                if (ValNoti[Riga] >= 0)
                {
                    Console.Write($"\t=\t+{ValNoti[Riga]}" + Environment.NewLine);

                }
                else
                {
                    Console.Write($"\t=\t{ValNoti[Riga]}" + Environment.NewLine);
                }
            }

            Console.WriteLine(Environment.NewLine + "Premere qualunque tasto per tornare indietro...");
            Console.ReadKey();
        }

        static void StampaErroreMenu(bool Errore)
        {
            if (Errore)
            {
                Console.WriteLine("L'opzione inserita non è valida. Riprovare." + Environment.NewLine);
            }
            else
            {
                Console.WriteLine("Si prega di scegliere un'opzione." + Environment.NewLine);
            }
        }

        static void StampaMenuPrincipale(bool Errore)
        {
            Console.Clear();

            StampaErroreMenu(Errore);

            Console.WriteLine("------======MENU======------");
            Console.WriteLine("1. Crea sistema di N incognite");
            Console.WriteLine("2. Riempi il sistema");
            Console.WriteLine("3. Stampa il sistema");
            Console.WriteLine("4. Risolvi il sistema");
            Console.WriteLine("0. Esci");
            Console.Write(Environment.NewLine + "Opzione scelta: ");
        }

        static void StampaSottomenuRiempimento(bool Errore)
        {
            Console.Clear();

            StampaErroreMenu(Errore);

            Console.WriteLine("------=====RIEMPI=====------");
            Console.WriteLine("1. Riempi il sistema in modo guidato");
            Console.WriteLine("2. [Non disponibile] Riempi il sistema inserendo direttamente le equazioni");
            Console.WriteLine("3. [Non disponibile] Importa il sistema riempito tramite file");
            Console.WriteLine("0. Torna indietro");
            Console.Write(Environment.NewLine + "Opzione scelta: ");
        }

        static bool IlSistemaEVuoto(float[,] Matrice, float[] ValNoti)
        {
            if (Matrice == null || ValNoti == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void SegnalaSistemaVuoto()
        {
            Console.WriteLine("Il sistema è vuoto, premere qualunque tasto per tornare indietro...");
            Console.ReadKey();
        }
        static float[] RisoluzioneSistema(ushort Dimensione, float[,] Matrice, float[] ValNoti)
        {
            switch (Dimensione)
            {
                case 2:
                    return RisoluzioneCramer(Matrice, ValNoti);
                case 3:
                    return RisoluzioneSarrus(Matrice, ValNoti);
                default:
                    Console.WriteLine("Avviso! Al momento non è possibile risolvere sistemi di {0} incognite!", Dimensione);
                    Console.ReadKey();
                    break;
            }

            return null;
        }

        static float[] RisoluzioneCramer(float[,] Matrice, float[] ValNoti)
        {
            float[] Coordinate = new float[2];

            // DA IMPLEMENTARE

            return null;
        }

        static float[] RisoluzioneSarrus(float[,] Matrice, float[] ValNoti)
        {
            float[] Coordinate = new float[3];

            // DA IMPLEMENTARE 

            return null;
        }
    }
}
