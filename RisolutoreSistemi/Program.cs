using System;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace RisolutoreSistemi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Benvenuto! Questo programma permette di risolvere un sistema di equazioni a N incognite.");

            int Dimensione = RichiestaInputSicuro("Si prega di inserire la dimensione della matrice");

            int[,] Matrice = new int[Dimensione, Dimensione];
            int[] ValNoti = new int[Dimensione];

            RiempiValori(Dimensione, Matrice, ValNoti);

            if (DomandaChiusa("Si desidera stampare la matrice?"))
            {
                StampaSistema(Dimensione, Matrice, ValNoti);
            }
        }

        static int RichiestaInputSicuro(string Messaggio)
        {
            Console.Write(Environment.NewLine + Messaggio + ": ");
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
                    Console.WriteLine(Environment.NewLine + "Il valore inserito non è valido, Riprovare: ");
                    Fail = true;
                }
            } while (Fail);

            return Tempinput;
        }

        static string FormaEspressioneNecessaria(int NIncognite)
        {
            string TempString = "";
            for (int Indice = 1; Indice <= NIncognite; Indice++)
            {
                TempString += $"{(char)(64 + Indice)}x{Indice}, ";
            }

            TempString = TempString.Remove(TempString.Length - 2) + $" = {(char)(65 + NIncognite)}";
            return TempString;
        }

        static bool DomandaChiusa(string Messaggio)
        {
            Console.Write(Environment.NewLine + Messaggio + " (Digitare \"y\"/\"Y\" per accettare, qualunque altro tasto per rifiutare): ");

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

        static void RiempiValori(int Dimensione, int[,] Matrice, int[] ValNoti)
        {
            for (int Riga = 0; Riga < Dimensione; Riga++)
            {
                Console.WriteLine($"Verranno ora richiesti, passo-passo, i parametri della {Riga + 1}^ equazione. in forma {FormaEspressioneNecessaria(Dimensione)} ");
                for (int Colonna = 0; Colonna < Dimensione; Colonna++)
                {
                    Matrice[Riga, Colonna] = RichiestaInputSicuro($"Inserire il parametro {(char)(65 + Colonna)}");
                }
                ValNoti[Riga] = RichiestaInputSicuro($"Inserire il parametro {(char)(65 + Dimensione)}");
            }
        }

        static void StampaSistema(int Dimensione, int[,] Matrice, int[] ValNoti)
        {
            Console.Write(Environment.NewLine);
            
            for (int Riga = 0; Riga < Dimensione; Riga++)
            {
                for (int Colonna = 0; Colonna < Dimensione; Colonna++)
                {
                    Console.Write($"\t{Matrice[Riga,Colonna]}(x{Colonna+1})");
                }
                Console.Write($" = {ValNoti[Riga]}" + Environment.NewLine);
            }
        }
    }
}