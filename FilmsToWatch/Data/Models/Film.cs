﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FilmsToWatch.Constants.DataConstants;

namespace FilmsToWatch.Data.Models
{
    public class Film
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLen)]
        public string Title { get; set; } = string.Empty;

        public string MovieImage { get; set; } = string.Empty;

        [Required]
        public string ReleaseYear { get; set; } = string.Empty;

        [Required]
        [MaxLength(DirectorMaxLen)]
        public string Director { get; set; } = string.Empty;

        [Required]
        public int GenreId { get; set; }

        [Required]
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = null!;

        [Required]
        public int ActorId { get; set; }

        [Required]
        [ForeignKey(nameof(ActorId))]
        public Actor Actor { get; set; } = null!;

        [Required]
        public string FilmAdderId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(FilmAdderId))]
        public IdentityUser FilmAdder { get; set; } = null!;

        public IList<FilmWatcher> FilmsWatchers { get; set; } = new List<FilmWatcher>();

        public IList<Review> Reviews { get; set; } = new List<Review>();

    }
}
