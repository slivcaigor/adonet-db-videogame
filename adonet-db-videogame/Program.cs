namespace adonet_db_videogame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine("Seleziona un'opzione:");
                Console.WriteLine("1. Inserisci un nuovo videogioco");
                Console.WriteLine("2. Ricercare un videogioco per id");
                Console.WriteLine("3. Ricercare tutti i videogiochi attraverso il nome");
                Console.WriteLine("4. Cancella un videogioco");
                Console.WriteLine("5. Chiudi il programma");

                key = Console.ReadKey(true);

                switch (key.KeyChar)
                {
                    case '1':
                        VideogameManager.InserisciVideogioco();
                        break;
                    case '2':
                        VideogameManager.CercaVideogiocoPerId();
                        break;
                    case '3':
                        VideogameManager.CercaVideogiochiPerNome();
                        break;
                    case '4':
                        VideogameManager.CancellaVideogioco();
                        break;
                    case '5':
                        Console.WriteLine("Arrivederci!");
                        return;
                    default:
                        Console.WriteLine("Opzione non valida! Scegli tra quelle fornite");
                        break;
                }

                Console.WriteLine();
            } while (key.KeyChar != '5');

        }

    }
}