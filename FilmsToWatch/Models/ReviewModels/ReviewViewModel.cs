﻿using System.ComponentModel.DataAnnotations;


namespace FilmsToWatch.Models.ReviewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int FilmId { get; set; }
    }
}
