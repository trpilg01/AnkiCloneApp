using System.Diagnostics.CodeAnalysis;

namespace AnkiCloneApp.Data;


public class DeckService
{
    /* Should probably delete this class */
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

    public string GetDeckName(int deckId)
    {
        foreach (var deck in _deckList)
        {
            if (deck.DeckId == deckId)
            {
                return deck.Name;
            }
        }

        return "Error: Name not found!";
    }

    public void DeleteDeck(Deck deck)
    {
        _deckList.Remove(deck);
    }
}