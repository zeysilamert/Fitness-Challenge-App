using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MyApp.Namespace
{
    [Authorize]
    public class ChallengeDetailsModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public ChallengeDetailsModel(ApplicationDbContext context)
        {
            _context = context;
            JoinChallenges = new List<TblJoinChallenge>();
        }

        public TblAllChallenge? Challenges { get; set; }
        public List<TblComment>? Comments { get; set; }
        public List<TblJoinChallenge>? JoinChallenges { get; set; }
        public double AverageRating { get; set; }
        public bool hasJoined { get; set; }
        public bool isFavorite { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Challenges = await _context.TblAllChallenges.FindAsync(id);

            if (Challenges == null)
            {
                return NotFound();
            }

            Comments = await _context.TblComments
                .Where(c => c.ChallengeId == id)
                .Include(c => c.User)
                .ToListAsync();

            if (Comments.Any())
            {
                AverageRating = Comments.Average(c => c.Rating);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                JoinChallenges = await _context.TblJoinChallenges
                    .Where(jc => jc.ChallengeId == id && jc.UserId == userId)
                    .ToListAsync();
            }

            var joinedChallenge = await _context.TblJoinChallenges
                .FirstOrDefaultAsync(jc => jc.ChallengeId == id && jc.UserId == userId);

            hasJoined = joinedChallenge != null;
            isFavorite = joinedChallenge?.IsFavorite ?? false;

            return Page();
        }

        public async Task<IActionResult> OnPostJoinChallengeAsync(int challengeId)
        {
            if (User.Identity != null)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            hasJoined = await _context.TblJoinChallenges
                .AnyAsync(jc => jc.ChallengeId == challengeId && jc.UserId == userId);

            if (hasJoined)
            {
                TempData["ErrorMessage"] = "You have already joined this challenge.";
                return RedirectToPage("/ChallengeDetails", new { id = challengeId });
            }

            var joinedChallenge = new TblJoinChallenge
            {
                UserId = userId,
                ChallengeId = challengeId,
                JoinDate = DateTime.Now
            };

            _context.TblJoinChallenges.Add(joinedChallenge);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "You have successfully joined the challenge!";

            return RedirectToPage("/ChallengeDetails", new { id = challengeId });
        }

        public async Task<IActionResult> OnPostAddCommentAndRatingAsync(int challengeId, int rating, string content)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            hasJoined = await _context.TblJoinChallenges
                .AnyAsync(jc => jc.ChallengeId == challengeId && jc.UserId == userId);

            if (!hasJoined)
            {
                TempData["ErrorMessage"] = "You must join the challenge to rate it.";
                return RedirectToPage("/ChallengeDetails", new { id = challengeId });
            }

            var hasCommented = await _context.TblComments
                .FirstOrDefaultAsync(c => c.ChallengeId == challengeId && c.UserId == userId);

            if (hasCommented != null)
            {
                TempData["ErrorMessage"] = "You already commented and rated this challenge before!";
                return RedirectToPage("/ChallengeDetails", new { id = challengeId });
            }
            else
            {
                var newComment = new TblComment
                {
                    UserId = userId,
                    ChallengeId = challengeId,
                    Rating = rating,
                    Content = content,
                    CreatedAt = DateTime.Now
                };

                _context.TblComments.Add(newComment);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/ChallengeDetails", new { id = challengeId });
        }

        public async Task<IActionResult> OnPostSaveAsFavoriteAsync(int challengeId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var joinedChallenge = await _context.TblJoinChallenges
                .FirstOrDefaultAsync(jc => jc.ChallengeId == challengeId && jc.UserId == userId);

            if (joinedChallenge != null)
            {
                joinedChallenge.IsFavorite = true;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Challenge saved as favorite!";
            }
            else
            {
                TempData["ErrorMessage"] = "You need to join the challenge first!";
            }

            return RedirectToPage();
        }
    }
}
