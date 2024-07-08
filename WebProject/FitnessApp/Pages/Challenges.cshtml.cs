using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessApp.Models;
using FitnessApp.Data;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Pages
{
    [Authorize]
    public class CreateChallengesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateChallengesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<TblAllChallenge>? Challenges { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }
        

        public async Task OnGetAsync(string category, string difficulty, DateTime? dateTime)
        {

            IQueryable<TblAllChallenge> query = _context.TblAllChallenges;

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(c => c.Category == category);
            }

            if (!string.IsNullOrEmpty(difficulty))
            {
                query = query.Where(c => c.DifficultyLevel == difficulty);
            }

            if (dateTime.HasValue)
            {
                query = query.Where(c => c.StartDate <= dateTime.Value && c.EndDate >= dateTime.Value);
            }


            if (!string.IsNullOrEmpty(SearchQuery))
            {
                var searchQuery = SearchQuery.ToLower();
                query = query.Where(c =>
                    c.Category.ToLower().Contains(searchQuery) ||
                    c.DifficultyLevel.ToLower().Contains(searchQuery) ||
                    c.Instruction.ToLower().Contains(searchQuery));
            }

            Challenges = await query.ToListAsync();
        }
    }
}