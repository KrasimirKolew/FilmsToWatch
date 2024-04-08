namespace FilmsToWatch.Constants
{
    public static class DataConstants
    {
        //Films constants
        public const int TitleMinLen = 2;
        public const int TitleMaxLen = 50;

        public const int DirectorMinLen = 4;
        public const int DirectorMaxLen = 50;

        //Genre constants
        public const int GenreNameMinLen = 3;
        public const int GenreNameMaxLen = 20;

        //Actor constants
        public const int ActorNameMinLen = 4;
        public const int ActorNameMaxLen = 50;

        //Review constants
        public const int ContentMinLen = 15;
        public const int ContentMaxLen = 150;

        //massage constants
        public const string RequiredMesage = "The {0} field is required";
        public const string LenghtMessage = "The {0} field must be between {1} and {2} characters long";

    }
}
