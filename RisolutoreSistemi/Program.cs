using System;
using System.Runtime.Serialization;

namespace RisolutoreSistemi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // benvenuto
            Console.WriteLine("Benvenuto! Questo programma permette di risolvere un sistema di equazioni a N incognite.");

            // booleani per continuo o input falliti
            bool Continua = true;
            bool Errore = false;

            // dichiarazione nulla (per ora) di matrice, vettore e dimensione
            float[,] Matrice = null;
            float[] ValNoti = null;
            short Dimensione = 0;

            // ciclo che itera fino a quando Continua = false
            do
            {
                // stampa menu principale, con booleano errore (Se è vero viene stampata una string apposita)
                StampaMenuPrincipale(Errore);

                // azzeramento condizione errore
                Errore = false;

                // input utente
                string Scelta = Console.ReadLine();

                // switch per scelta utente
                switch (Scelta)
                {
                    case "1": // pulizia console, richiesta guidata dimensione, creazione matrice e vettore
                        Console.Clear();

                        Dimensione = RichiestaDimens();
                        Matrice = new float[Dimensione, Dimensione];
                        ValNoti = new float[Dimensione];

                        break;
                    case "2": // sottomenu, con propri booleani di continuo (per tornare indietro) ed errore 
                        bool ContinuaRiempimento = true;
                        bool ErroreRiempimento = false;

                        do
                        {
                            // la logica del sottomenu è identica a quella del menu principale
                            StampaSottomenuRiempimento(ErroreRiempimento);
                            ErroreRiempimento = false;

                            string SceltaRiempimento = Console.ReadLine();

                            switch (SceltaRiempimento)
                            {
                                case "1": // riempimento guidato  (I/O). Ritorno al menu principale perché non serve più riempire
                                    RiempimentoGuidato(Dimensione, Matrice, ValNoti);
                                    ContinuaRiempimento = false;
                                    break;
                                case "0": // Ritorno indietro
                                    ContinuaRiempimento = false;
                                    break;
                                case "/debug":
                                    DebugSysProvaS(Matrice, ValNoti); // Sistema di debug
                                    ContinuaRiempimento = false;
                                    break;
                                default: // Rifare la scelta
                                    ErroreRiempimento = true;
                                    break;

                                    // Sono da implementare: Input da file, input da stringa con parser
                            }
                        }
                        while (ContinuaRiempimento);

                        break;
                    case "3": // stampa sistema
                        StampaSistema(Dimensione, Matrice, ValNoti);
                        break;
                    case "4": // risoluzione
                        RisoluzioneSistema(Dimensione, Matrice, ValNoti);
                        break;
                    case "0": // pulizia console e uscita. Continua = false perché non si deve fare altro
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
            // Messaggio personalizzato a capo
            Console.Write(Environment.NewLine + Messaggio + ": ");

            // booleano input fallito
            bool Fail = false;

            // dichiarazione tempinput e assegnazione a 0 (temporanea)
            float Tempinput = 0;

            do
            {
                // azzeramento condizione fallimento
                Fail = false;

                // tentativo di float.parse, altrimenti segnalazione all'utente e obbligo di riprovare
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

        static short RichiestaDimens() // Questa funzione è come l'altra ma solo per la dimensione (short).
        {
            Console.Write("Si prega di inserire la dimensione della matrice: ");

            // booleano input fallito
            bool Fail = false;

            // inizializz. temporanea
            short Tempinput = 0;

            do // stessa logica di RichiestaInputSicuro, ma il catch viene triggerato anche con valore negativo
            {
                Fail = false;
                try
                {
                    Tempinput = short.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Write(Environment.NewLine + "Il valore inserito non è valido, Riprovare: ");
                    Fail = true;
                }
            } while (Fail);

            return Tempinput;
        }

        static string FormaEspressioneNecessaria(short NIncognite) // Composizione stringa della forma di espressione
        {
            // inizializzazione stringa nulla
            string TempString = "";

            // composizione stringa
            for (short Indice = 1; Indice <= NIncognite; Indice++)
            {
                // 64 è il char prededente ad 'a', ma dato che i parte da 1, il primo char è 65, ovvero A.
                TempString += $"{(char)(64 + Indice)}x{Indice} +/- ";
            }

            // sovrascrittura della stringa con la stessa ma senza il +/- di troppo alla fine
            TempString = TempString.Remove(TempString.Length - 4) + $" = {(char)(65 + NIncognite)}";

            return TempString; // Esempio: Ax1 +/- Bx2 +/- Cx3 = D se la dimensione è 3.

        }

        static bool DomandaChiusa(string Messaggio) // Semplice interazione I/O per domanda chiusa
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

        static void RiempimentoGuidato(short Dimensione, float[,] Matrice, float[] ValNoti)
        {
            Console.Clear();

            // Se il sistema è vuoto, segnalazione all'utente
            if (IlSistemaEVuoto(Matrice, ValNoti))
            {
                SegnalaSistemaVuoto();
            }
            else // altrimenti si procede
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

        static void StampaSistema(short Dimensione, float[,] Matrice, float[] ValNoti)
        {
            Console.Clear();

            // Se il sistema è vuoto, segnalazione all'utente
            if (IlSistemaEVuoto(Matrice, ValNoti))
            {
                SegnalaSistemaVuoto();
            }
            else // altrimenti si procede
            {
                Console.WriteLine("Il sistema è quanto segue: ");
            }
            Console.Write(Environment.NewLine);

            for (short Riga = 0; Riga < Dimensione; Riga++)
            {
                for (short Colonna = 0; Colonna < Dimensione; Colonna++)
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

            // pausa perché la stampa è una cosa che interessa l'utente
            Console.WriteLine(Environment.NewLine + "Premere qualunque tasto per tornare indietro...");
            Console.ReadKey();
        }

        static void StampaErroreMenu(bool Errore) // stringa da stampare a seconda del booleano errore
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
            // pausa perché questa cosa interessa l'utente
            Console.WriteLine("Il sistema è vuoto, premere qualunque tasto per tornare indietro...");
            Console.ReadKey();
        }
        static float[] RisoluzioneSistema(short Dimensione, float[,] Matrice, float[] ValNoti)
        {
            switch (Dimensione)
            {
                case 2:
                    return RisoluzioneCramer(Matrice, ValNoti);
                case 3:
                    return RisoluzioneSarrus(Matrice, ValNoti);
                default:
                    // pausa perché questa cosa interessa l'utente
                    Console.WriteLine("Avviso! Al momento non è possibile risolvere sistemi di {0} incognite!", Dimensione);
                    Console.ReadKey();
                    break;

                    // Da implementare Laplace
            }

            return null; // se con è possibile risolvere il sistema, il programma non beccherà gli altri return, arrivando qua
        }

        static float[] RisoluzioneCramer(float[,] Matrice, float[] ValNoti)
        {
            float[] Determinanti = new float[3];

            Determinanti[0] = (ValNoti[0] * Matrice[1, 1]) - (Matrice[0, 1] * ValNoti[1]); //Dx
            Determinanti[1] = (Matrice[0, 0] * ValNoti[1]) - (ValNoti[0] * Matrice[1, 0]); //Dy
            Determinanti[2] = (Matrice[0, 0] * Matrice[1, 1]) - (Matrice[0, 1] * Matrice[1, 0]); //D


            if (Determinanti[0] == 0)
            {
                return null; // Il sistema è irrisolvibile in questo caso
            }
            else
            {
                // Ritorna Dx/D, Dy/D
                return new float[] { (Determinanti[0] / Determinanti[2]), (Determinanti[1] / Determinanti[2]) };
            }

        }

        static float[] RisoluzioneSarrus(float[,] Matrice, float[] ValNoti)
        {
            float[] Vars = new float[4];


            for (short IndiceDeterm = 0; IndiceDeterm < 4; IndiceDeterm++)
            {

                float Accumulatore = 0;

                if (IndiceDeterm == 3) // Determinante generico
                {
                    for (short Offset = 0; Offset < 3; Offset++)
                    {
                        // Moltiplicatore = 1
                        float Moltiplicatore = 1;

                        for (short Spostamento = 0; Spostamento < 3; Spostamento++)
                        {
                            // Moltiplica il moltiplicatore per la cella considerata ora
                            Moltiplicatore *= Matrice[Spostamento, Periodo((short)(Spostamento + Offset), 3)];
                        }

                        Accumulatore += Moltiplicatore;
                        Moltiplicatore = 1;

                        for (short Spostamento = 0; Spostamento < 3; Spostamento++)
                        {
                            // Moltiplica il moltiplicatore per la cella considerata ora
                            Moltiplicatore *= Matrice[Spostamento, Periodo((short)(2 - Spostamento - Offset), 3)];
                        }

                        Accumulatore -= Moltiplicatore;
                    }
                }
                else
                {
                    for (short Offset = 0; Offset < 3; Offset++)
                    {
                        // Moltiplicatore = 1
                        float Moltiplicatore = 1;

                        for (short Spostamento = 0; Spostamento < 3; Spostamento++)
                        {
                            // Se la colonna selezionata è quella considerata da rimpiazzare
                            if (Periodo((short)(Spostamento+Offset),3) == IndiceDeterm)
                            {
                                Moltiplicatore *= ValNoti[Spostamento];
                            }
                            else
                            {
                                // Moltiplica il moltiplicatore per la cella considerata ora
                                Moltiplicatore *= Matrice[Spostamento, Periodo((short)(Spostamento + Offset), 3)];
                            }
                        }

                        Accumulatore += Moltiplicatore;
                        Moltiplicatore = 1;

                        for (short Spostamento = 0; Spostamento < 3; Spostamento++)
                        {
                            // Se la colonna selezionata è quella considerata da rimpiazzare
                            if (Periodo((short)(2-Spostamento-Offset),3) == IndiceDeterm)
                            {
                                Moltiplicatore *= ValNoti[Spostamento];
                            }
                            else
                            {
                                // Moltiplica il moltiplicatore per la cella considerata ora
                                Moltiplicatore *= Matrice[Spostamento, Periodo((short)(2 - Spostamento - Offset), 3)];
                            }
                        }

                        Accumulatore -= Moltiplicatore;
                    }
                }

                Vars[IndiceDeterm] = Accumulatore;
            }

            DebugStampaVett(Vars);
            Console.ReadKey();
            return null; // da levare
        }

        static short Periodo(short Numero, short Periodo)
        {
            while (Numero < 0)
            {
                Numero += Periodo;
            }

            return (short)(Numero % Periodo); // Se indico la cella 4 ma la dimensione è 3, mi riferisco alla cella 1 (la seconda)
        }
        static void DebugSysProvaS(float[,] Matrice, float[] ValNoti)
        {
            Matrice[0, 0] = 4;
            Matrice[0, 1] = 6;
            Matrice[0, 2] = -3;
            ValNoti[0] = 0;
            Matrice[1, 0] = 6;
            Matrice[1, 1] = 3;
            Matrice[1, 2] = -8;
            ValNoti[1] = 7;
            Matrice[2, 0] = -2;
            Matrice[2, 1] = -4;
            Matrice[2, 2] = 6;
            ValNoti[2] = -2;
        }

        static void DebugStampaVett(float[] Vett)
        {
            for (int i = 0; i < Vett.Length; i++)
            {
                Console.WriteLine(Vett[i]);
            }
        }
    }
}
