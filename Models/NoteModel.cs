namespace TaskFlow.Models
{
    public class NoteModel
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Timestamp { get; set; }
        public bool IsStarred { get; set; }
    }
}