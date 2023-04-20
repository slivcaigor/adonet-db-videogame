using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            // Creo l'istanza della classe Videogioco con i dati inseriti dall'utente
            Videogame videogame = new(name, overview, softwareHouseId, releaseDate);

            try
            {
                // Connetto al database e inserisco il nuovo videogioco
                string connectionString = "Data Source=localhost;Initial Catalog=db_videogames;Integrated Security=True";
                using SqlConnection conn = new(connectionString);
                conn.Open();

                string query = "INSERT INTO videogames (name, overview, release_date, software_house_id) VALUES (@name, @overview, @releaseDate, @softwareHouseId)";

                using SqlCommand command = new(query, conn);
                command.Parameters.AddWithValue("@name", videogame.Name);
                command.Parameters.AddWithValue("@overview", videogame.Overview);
                command.Parameters.AddWithValue("@releaseDate", videogame.ReleaseDate);
                command.Parameters.AddWithValue("@softwareHouseId", videogame.IdSoftwareHouse);

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
                    // Se è presente un record con l'ID specificato, crea un'istanza della classe Videogame
                    string name = (string)reader["name"];
                    string overview = (string)reader["overview"];
                    DateTime releaseDate = (DateTime)reader["release_date"];
                    int softwareHouseId = Convert.ToInt32(reader["software_house_id"]);
                    Videogame videogame = new( name, overview, softwareHouseId, releaseDate);

                    // Stampa i dati del videogioco tramite il metodo della classe Videogame
                    videogame.PrintDetails();
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
            Console.Write("Inserisci il nome del videogioco da cercare: ");
            string name = Console.ReadLine() ?? throw new ArgumentNullException(nameof(name));

            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=db_videogames;Integrated Security=True";
                using SqlConnection conn = new(connectionString);
                conn.Open();

                string query = "SELECT * FROM videogames WHERE name LIKE @name";

                using SqlCommand command = new(query, conn);
                command.Parameters.AddWithValue("@name", $"%{name}%");

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Videogame videogame = new(
                        (string)reader["name"],
                        (string)reader["overview"],
                        Convert.ToInt32(reader["software_house_id"]),
                        (DateTime)reader["release_date"]
                    );

                    videogame.PrintDetails();
                }

                if (!reader.HasRows)
                {
                    Console.WriteLine($"Nessun videogioco trovato con nome contenente \"{name}\"");
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Si è verificato un errore durante la ricerca del videogioco: {ex.Message}");
            }
        }

        public static void CancellaVideogioco()
        {
            Console.Write("Inserisci l'ID del videogioco da cancellare: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("L'ID inserito non è valido!");
                return;
            }

            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=db_videogames;Integrated Security=True";
                using SqlConnection conn = new(connectionString);
                conn.Open();

                string query = "DELETE FROM videogames WHERE id = @id";

                using SqlCommand command = new(query, conn);
                command.Parameters.AddWithValue("@id", id);

                int result = command.ExecuteNonQuery();

                if (result == 0)
                {
                    Console.WriteLine($"Nessun videogioco trovato con ID {id}");
                }
                else
                {
                    Console.WriteLine($"Cancellati {result} record dalla tabella videogames");
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Si è verificato un errore durante la cancellazione del videogioco: {ex.Message}");
            }
        }

    }
}
