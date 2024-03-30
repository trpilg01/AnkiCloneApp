namespace AnkiCloneApp.Data;

public class FlashcardService
{
    private readonly IList<Flashcard> _flashcards;
    
    public FlashcardService()
    {
        _flashcards = new List<Flashcard>();
    }

    public void AddCard(Flashcard card)
    {
        _flashcards.Add(card);
    }

    public void SetEFactor(Flashcard flashcard, float responseQuality)
    {
        flashcard.EFactor = flashcard.EFactor + (0.1 - (5 - responseQuality) * (0.08 + (5 - responseQuality) * 2));
    }

    public void SetRevisionDate(Flashcard flashcard)
    {
        if (flashcard.Revisions == 0)
        {
            flashcard.NextRevisionDate = flashcard.CreationDate;
        }
        else if (flashcard.Revisions == 1)
        {
            flashcard.NextRevisionDate = flashcard.CreationDate.AddDays(1);
        }
        else if (flashcard.Revisions == 2)
        {
            flashcard.NextRevisionDate = DateOnly.FromDateTime(DateTime.Today.AddDays(6));
        }
        else if (flashcard.Revisions > 2)
        {
            flashcard.NextRevisionDate = DateOnly.FromDateTime(DateTime.Today.AddDays((flashcard.Revisions - 1) * flashcard.EFactor));
        }
    }
}