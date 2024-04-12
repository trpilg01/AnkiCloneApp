namespace AnkiCloneApp.Data;

public class FlashcardService
{
    private readonly IList<Flashcard> _flashcards;
    
    public FlashcardService()
    {
        _flashcards = new List<Flashcard>();
    }

    public void AddCard(Flashcard card) // Useless method. Could delete
    {
        _flashcards.Add(card);
    }

    public void UpdateFlashcard(Flashcard card, int quality)
    {
        if (quality < 0 || quality > 5)
            throw new AggregateException("Quality must be between 0 and 5.");
        
        // If the response is correct (quality >= 3), apply the SM-2 algorithm
        if (quality >= 3)
        {
            // First Revision
            if (card.Revisions == 0)
            {
                card.Interval = 1;
            }
            // Second Revision
            else if (card.Revisions == 1)
            {
                card.Interval = 6;
            }
            // Subsequent Revisions
            else
            {
                card.Interval *= card.EFactor;
            }

            card.Revisions++;
        }
        else
        {
            // Reset repetition for incorrect responses, but don't reset EFactor
            card.Revisions = 0;
            card.Interval = 1;
        }

        // Adjust EFactor, but ensure it's mpt less than 1.3
        card.EFactor = Math.Max(1.3, card.EFactor + (0.1 - (5 - quality) * (0.08 + (5 - quality) * 0.02)));
        
        // Set the next review date
        card.NextRevisionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(card.Interval));
    }
}