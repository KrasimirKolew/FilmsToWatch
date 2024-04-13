using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FilmsToWatch.Constants.DataConstants;

namespace FilmsToWatch.Data.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ContentMaxLen)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int FilmId { get; set; }

        [ForeignKey(nameof(FilmId))]
        public Film Films { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;
    }
}
