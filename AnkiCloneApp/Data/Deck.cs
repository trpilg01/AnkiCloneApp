using Google.Protobuf.WellKnownTypes;

namespace AnkiCloneApp.Data;
public class Deck
{
    private string _name;
    private int _deckId;
    private DateOnly _creationDate { get; set; }
    private DateOnly _lastReviewed;
    public List<Flashcard> Cards;
    
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
    public Deck(string name)
    {
        _name = name;
        Cards = new List<Flashcard>();
    }
    public Deck(){}

    public void AddCardToDeck(Flashcard flashcard)
    {
        Cards.Add(flashcard);
    }

}