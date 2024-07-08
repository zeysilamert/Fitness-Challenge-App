using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitnessApp.Data;
using FitnessApp.Models;

namespace FitnessApp.Models
{
    public class TblJoinChallenge
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Challenge")]
        public int ChallengeId { get; set; }
        public TblAllChallenge? Challenge { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public UserDetails? User { get; set; }

        public DateTime JoinDate { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsCompleted { get; set; }
        public int CompletedChallengesCount { get; set; }

    }
}