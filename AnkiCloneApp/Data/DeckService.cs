namespace AnkiCloneApp.Data;


public class DeckService
{
    private List<Deck> _deckList { get; set; }

    public List<Deck> DeckList
    {
        get { return _deckList; }
        set { _deckList = value; }
    }

    public DeckService()
    {
        _deckList = new List<Deck>();
    }
}