using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adonet_db_videogame
{
    public class VideogameManager
    {
        public static void InserisciVideogioco()
        {
            Console.WriteLine("Inserisci i dati del nuovo videogioco:");
            // Richiedo il nome del videogioco all'utente finché non viene inserito un valore non vuoto
            string name = "";
            while (string.IsNullOrEmpty(name))
            {
                Console.Write("Nome: ");
                 name = Console.ReadLine() ?? throw new ArgumentNullException(nameof(name));
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Il nome non può essere vuoto!");
                }
                
            }
            // Richiedo la descrizione del videogioco all'utente finché non viene inserito un valore non vuoto
            string overview = "";
            while (string.IsNullOrEmpty(overview))
            {
                Console.Write("Descrizione: ");
                overview = Console.ReadLine() ?? throw new ArgumentNullException(nameof(name));
                if (string.IsNullOrEmpty(overview))
                {
                    Console.WriteLine("La descrizione non può essere vuota!");
                }

            }
            // Richiedo la data di rilascio del videogioco all'utente finché non viene inserito un valore valido
            DateTime releaseDate;
            while (true)
            {
                Console.Write("Data di rilascio (formato: dd/mm/yyyy): ");
                if (DateTime.TryParse(Console.ReadLine(), out releaseDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("La data di rilascio non è in un formato valido!");
                }
            }
            // Richiedo l'ID della casa di sviluppo del videogioco all'utente finché non viene inserito un valore valido
            int softwareHouseId;
            while (true)
            {
                Console.Write("ID della casa di sviluppo: ");
                if (int.TryParse(Console.ReadLine(), out softwareHouseId))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("L'ID della casa di sviluppo non è un numero valido!");
                }
            }

            try
            {
                // Connetto al database e inserisco il nuovo videogioco
                string connectionString = "Data Source=localhost;Initial Catalog=db_videogames;Integrated Security=True";
                using SqlConnection conn = new(connectionString);
                conn.Open();

                string query = "INSERT INTO videogames (name, overview, release_date, software_house_id) VALUES (@name, @overview, @releaseDate, @softwareHouseId)";

                using SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@overview", overview);
                command.Parameters.AddWithValue("@releaseDate", releaseDate);
                command.Parameters.AddWithValue("@softwareHouseId", softwareHouseId);

                int result = command.ExecuteNonQuery();

                Console.WriteLine($"Inseriti {result} record nella tabella videogames");
            }
            catch (Exception ex)
            {
                // Gestisco eventuali errori durante l'inserimento del nuovo videogioco
                Console.Clear();
                Console.WriteLine($"Si è verificato un errore durante l'inserimento del nuovo videogioco: {ex.Message}");
            }

        }


        public static void CercaVideogiocoPerId()
        {
            Console.Write("Inserisci l'ID del videogioco: ");
            // Legge l'input dell'utente come una stringa e lo converte in un numero intero
            if (!int.TryParse(Console.ReadLine(), out int videogameId))
            {
                Console.WriteLine("L'ID del videogioco non è un numero valido!");
                return;
            }

            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=db_videogames;Integrated Security=True";
                using SqlConnection conn = new(connectionString);
                conn.Open();

                string query = "SELECT * FROM videogames WHERE id = @videogameId";

                using SqlCommand command = new(query, conn);
                command.Parameters.AddWithValue("@videogameId", videogameId);

                // Esegue la query e legge il risultato
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // Se è presente un record con l'ID specificato, stampa i dati del videogioco
                    Console.WriteLine($"ID: {reader["id"]}");
                    Console.WriteLine($"Nome: {reader["name"]}");
                    Console.WriteLine($"Descrizione: {reader["overview"]}");
                    Console.WriteLine($"Data di rilascio: {((DateTime)reader["release_date"]).ToString("dd/MM/yyyy")}");
                    Console.WriteLine($"ID casa di sviluppo: {reader["software_house_id"]}");
                }
                else
                {
                    Console.WriteLine($"Nessun videogioco trovato con ID {videogameId}");
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Si è verificato un errore durante la ricerca del videogioco: {ex.Message}");
            }
        }

        public static void CercaVideogiochiPerNome()
        {
        }

        public static void CancellaVideogioco()
        {
        }
    }
}
