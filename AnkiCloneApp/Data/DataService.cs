using System.Data;
using MySql.Data.MySqlClient;

namespace AnkiCloneApp.Data;

public class DataService
{
    private readonly string _connectionString;

    public DataService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public async Task UpdateAllFlashcards(List<Flashcard> flashcards) // Used for editing all flashcards on review page
    {
        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                foreach (var flashcard in flashcards)
                {
                    using (var command = new MySqlCommand("UpdateFlashcards", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                
                        // Add parameters
                        command.Parameters.Add(new MySqlParameter("p_ID", flashcard.Id));
                        command.Parameters.Add(new MySqlParameter("p_FrontData", flashcard.FrontData));
                        command.Parameters.Add(new MySqlParameter("p_BackData", flashcard.BackData));
                        command.Parameters.Add(new MySqlParameter("p_Revisions", flashcard.Revisions));
                    
                        // Execute the command
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Flashcard>> GetAllCardsAsync() 
    {
        var list = new List<Flashcard>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("SELECT * FROM CardInfo WHERE isDeleted = 0", connection)) // Only gets cards that are not deleted
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        while (await reader.ReadAsync())
                        {
                            /* Read Variables */
                            int id = (int)reader["ID"];
                            string frontData = (string)reader["FrontData"];
                            string backData = (string)reader["BackData"];
                            DateOnly creationDate = DateOnly.FromDateTime((DateTime)reader["CreationDate"]);
                            DateOnly revisionDate = DateOnly.FromDateTime((DateTime)reader["RevisionDate"]);
                            int revisions = (int)reader["Revisions"];
    
                            /* Create Flashcard */
                            var flashcard = new Flashcard
                            {
                                FrontData = frontData, BackData = backData, Id = id, CreationDate = creationDate,
                                NextRevisionDate = revisionDate, Revisions = revisions
                            };
                        
                            /* Add flashcard to list */
                            list.Add(flashcard);
                        }
                        Console.WriteLine("Flashcards successfully loaded");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
        return list;
    }
    
    /* Get Flashcards by DeckId. Will need to alter function to search by review date */
    public async Task<List<Flashcard>> GetFlashCardsAsync(int deckID)
    {
        var list = new List<Flashcard>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new MySqlCommand("SELECT * FROM CardInfo WHERE DeckID = @deckId and isDeleted = 0", connection))
            {
                command.Parameters.AddWithValue("@deckId", deckID);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    try
                    {
                        while (await reader.ReadAsync())
                        {
                            /* Read Variables */
                            int id = (int)reader["ID"];
                            string frontData = (string)reader["FrontData"];
                            string backData = (string)reader["BackData"];
                            DateOnly creationDate = DateOnly.FromDateTime((DateTime)reader["CreationDate"]);
                            DateOnly revisionDate = DateOnly.FromDateTime((DateTime)reader["RevisionDate"]);
                            int revisions = (int)reader["Revisions"];
    
                            /* Create Flashcard */
                            var flashcard = new Flashcard
                            {
                                FrontData = frontData, BackData = backData, Id = id, CreationDate = creationDate,
                                NextRevisionDate = revisionDate, Revisions = revisions, DeckId = deckID
                            };
                        
                            /* Add flashcard to list */
                            list.Add(flashcard);
                        }
                        Console.WriteLine("Flashcards successfully loaded");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
        return list;
    }

    /* Add new card to deck */
    public void AddCard(Flashcard flashcard)
    {
        Console.WriteLine(flashcard.DeckId);
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();

                using (var command =
                       new MySqlCommand(
                           "INSERT INTO CardInfo (FrontData, BackData, CreationDate, RevisionDate, Revisions, DeckID) VALUES (@frontData, @backData, @creationDate, @revisionDate, @revisions, @deckID)",
                           connection))
                {
                    /* Reformat Dates to fit SQL format */
                    string formattedCreationDated = flashcard.CreationDate.ToString("yyyy-MM-dd");
                    string formattedRevisionDate = flashcard.NextRevisionDate.ToString("yyyy-MM-dd");
                    
                    /* Add values to query parameters */
                    command.Parameters.AddWithValue("@frontData", flashcard.FrontData);
                    command.Parameters.AddWithValue("@backData", flashcard.BackData);
                    command.Parameters.AddWithValue("@creationDate", formattedCreationDated);
                    command.Parameters.AddWithValue("@revisionDate", formattedRevisionDate);
                    command.Parameters.AddWithValue("@revisions", flashcard.Revisions);
                    command.Parameters.AddWithValue("@deckID", flashcard.DeckId);
                    
                    command.ExecuteNonQuery(); // Execute
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
        }
    }
    
    /* Add new Deck */
    public void AddDeck(string name)
    {
        string formattedDate = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd");
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                using (var command = new MySqlCommand("INSERT INTO Decks (Name, CreationDate) VALUES (@Name, @CreationDate)", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@CreationDate", formattedDate);
                    command.ExecuteNonQuery();
                    
                }
                Console.WriteLine("Decks successfully loaded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
        }
    }
    
    public async Task<List<Deck>> GetDecksAsync()
    {
        var deckList = new List<Deck>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM Decks WHERE isDeleted = 0", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var name = (string)reader["Name"];
                            var id = (int)reader["DeckID"];
                            var creationDate = DateOnly.FromDateTime((DateTime)reader["CreationDate"]);

                            DateOnly lastReviewed;
                            if (!reader.IsDBNull(2))
                            {
                                lastReviewed = DateOnly.FromDateTime((DateTime)reader["LastReviewed"]);
                                var deck = new Deck
                                    { Name = name, DeckId = id, CreationDate = creationDate, LastReviewed = lastReviewed };
                                deckList.Add(deck);
                            }
                            else
                            {
                                var deck = new Deck { Name = name, DeckId = id, CreationDate = creationDate };
                                deckList.Add(deck);
                            }
                        }
                    }
                }
                Console.WriteLine("Data successfully inserted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
        }
        return deckList;
    }
    
    
    /* Updates Card's Revision Date */
    public void UpdateCardDate(Flashcard flashcard)
    {
        string formattedRevisionDate = flashcard.NextRevisionDate.ToString("yyyy-MM-dd");
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                using (var command =
                       new MySqlCommand("UPDATE CardInfo " +
                                        "SET RevisionDate = @revisionDate, Revisions = @revisions " +
                                        "WHERE ID = @id",
                           connection))
                {
                    command.Parameters.AddWithValue("@revisionDate", formattedRevisionDate);
                    command.Parameters.AddWithValue("@revisions", flashcard.Revisions);
                    command.Parameters.AddWithValue("@id", flashcard.Id);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }

    public async Task DeleteDeck(int deckId)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            try
            {
                using (var command = new MySqlCommand("DeleteDeck", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    // Add parameters
                    command.Parameters.Add(new MySqlParameter("p_Id", deckId));
                    
                    // Execute the command
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public void DeleteCard(Flashcard flashcard)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            try
            {
                connection.Open();
                using (var command = new MySqlCommand("UPDATE CardInfo " +
                                                      "SET isDeleted = 1 " +
                                                      "WHERE ID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", flashcard.Id);
                    command.ExecuteNonQuery();
                    
                    Console.WriteLine($" Flashcard ID: {flashcard.Id} Deleted");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}