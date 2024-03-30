using MySql.Data.MySqlClient;

namespace AnkiCloneApp.Data;

public class DataService
{
    private readonly string _connectionString;

    public DataService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    /* Get Flashcards from deck. Will need to alter this to get cards with specific dates. */
    public List<Flashcard> GetFlashCards()
    {
        var list = new List<Flashcard>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM CardInfo", connection);
            using (var reader = command.ExecuteReader())
            {
                try
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["ID"];
                        string frontData = (string)reader["FrontData"];
                        string backData = (string)reader["BackData"];
                        DateOnly creationDate = (DateOnly)reader["CreationDate"];
                        DateOnly revisionDate = (DateOnly)reader["RevisionDate"];
                        int revisions = (int)reader["Revisions"];

                        var flashcard = new Flashcard
                        {
                            FrontData = frontData, BackData = backData, ID = id, CreationDate = creationDate,
                            NextRevisionDate = revisionDate, Revisions = revisions
                        };
                        list.Add(flashcard);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        return list;
    }

    /* Add new card to deck */
    public void AddCard(Flashcard flashcard)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();

                using (var command =
                       new MySqlCommand(
                           "INSERT INTO CardInfo (ID, FrontData, BackData, CreationDate, RevisionDate, Revisions) VALUES (@id, @frontData, @backData, @creationDate, @revisionDate, @revisions)",
                           connection))
                {
                    /* Reformat Dates to fit SQL format */
                    string formattedCreationDated = flashcard.CreationDate.ToString("yy-MM-dd");
                    string formattedRevisionDate = flashcard.NextRevisionDate.ToString("yy-MM-dd");
                    
                    /* Add values to query parameters */
                    command.Parameters.AddWithValue("@id", flashcard.ID);
                    command.Parameters.AddWithValue("@frontData", flashcard.FrontData);
                    command.Parameters.AddWithValue("@backData", flashcard.BackData);
                    command.Parameters.AddWithValue("@creationDate", formattedCreationDated);
                    command.Parameters.AddWithValue("@revisionDate", formattedRevisionDate);
                    command.Parameters.AddWithValue("@revisions", flashcard.Revisions);
                    
                    command.ExecuteNonQuery(); // Execute query 

                    Console.WriteLine("Data Inserted succesfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
        }
    }
}