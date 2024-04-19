using Google.Protobuf.WellKnownTypes;

namespace AnkiCloneApp.Data;
public class Deck
{
    private string _name;
    private int _deckId;
    private DateOnly _creationDate { get; set; }
    private DateOnly _lastReviewed;
    public List<Flashcard> Cards;
    public List<Flashcard> DueToday;
    
    /* Getters and Setters */
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public int DeckId
    {
        get { return _deckId; }
        set { _deckId = value; }
    }

    public DateOnly CreationDate
    {
        get { return _creationDate; }
        set { _creationDate = value; }
    }

    public DateOnly LastReviewed
    {
        get { return _lastReviewed; }
        set { _lastReviewed = value; }
    }
    
    /* Methods */
    public Deck(string name) // Remove this?
    {
        _name = name;
        Cards = new List<Flashcard>();
        DueToday = GetDueToday();
    }
    public Deck(){}

    /* Get flashcards due today */
    public List<Flashcard> GetDueToday()
    {
        List<Flashcard> flashcards = new List<Flashcard>();
        foreach (var card in Cards)
        {
            if (card.NextRevisionDate <= (DateOnly.FromDateTime(DateTime.Now) ))
            {
                flashcards.Add(card);
            }
        }

        return flashcards;
    }
}