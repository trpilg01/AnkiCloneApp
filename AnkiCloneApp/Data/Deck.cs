namespace AnkiCloneApp.Data;

public class Deck
{
    private string _name;
    private int _deckId;
    public List<Flashcard> Cards;
    
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
    
    public Deck(string name)
    {
        _name = name;
        Cards = new List<Flashcard>();
    }

    public void AddCardToDeck(Flashcard flashcard)
    {
        Cards.Add(flashcard);
    }

}