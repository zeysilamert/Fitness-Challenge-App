using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitnessApp.Data;

namespace FitnessApp.Models
{
    public class TblComment
    {
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public UserDetails? User { get; set; }

        [Required]
        public int ChallengeId { get; set; }

        [ForeignKey("ChallengeId")]
        public TblAllChallenge? Challenge { get; set; }

        [Range(1, 5)] // Assuming rating is on a scale of 1 to 5
        public int Rating { get; set; }

        [Required]
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}