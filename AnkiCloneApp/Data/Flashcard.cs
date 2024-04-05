using System.Runtime.InteropServices.JavaScript;
using System.Runtime;

namespace AnkiCloneApp.Data;

public class Flashcard
{
    private int _id { get; set; }
    private string _frontData { get; set; } 
    private string _backData { get; set; }
    private int _revisions { get; set; }
    private DateOnly _nextReviewDate { get; set; }
    private DateOnly _creationDate { get; set; }
    private double _eFactor { get; set; }

    private int _deckID { get; set; }

    public int DeckId
    {
        get { return _deckID; }
        set { _deckID = value; }
    }
    
    public double EFactor
    {
        get { return _eFactor; }
        set { _eFactor = value; }
    }
    
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public string FrontData
    {
        get { return _frontData; }
        set { _frontData = value; }
    }
    public string BackData
    {
        get { return _backData; }
        set { _backData = value; }
    }
    public int Revisions
    {
        get { return _revisions; }
        set { _revisions = value; }
    }
    public DateOnly NextRevisionDate
    {
        get { return _nextReviewDate; }
        set { _nextReviewDate = value; }
    }
    public DateOnly CreationDate
    {
        get { return _creationDate; }
        set { _creationDate = value; }
    }
    
    public string GetFront()
    {
        return _frontData;
    }

    public string GetBack()
    {
        return _backData;
    }
}