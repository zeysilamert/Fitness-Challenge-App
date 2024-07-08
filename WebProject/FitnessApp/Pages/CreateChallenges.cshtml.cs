using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessApp.Data;
using FitnessApp.Models;
using System.Threading.Tasks;

namespace FitnessChallengeApp.Pages.Challenges
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TblAllChallenge Challenges { get; set; } = new TblAllChallenge();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TblAllChallenges.Add(Challenges);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Challenges");
        }
    }
}
