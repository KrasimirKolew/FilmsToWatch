namespace FilmsToWatch.Data.Models
{
    public class Actor
    {
        public int Id { get; set; }

        public string ActorName { get; set; } = string.Empty;

        public int FilmsInvolve { get; set; }
    }
}
