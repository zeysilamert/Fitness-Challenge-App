using FitnessApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data;

public class ApplicationDbContext : IdentityDbContext<UserDetails>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<TblAllChallenge> TblAllChallenges { get; set; }
    public virtual DbSet<TblJoinChallenge> TblJoinChallenges { get; set; }
    public virtual DbSet<TblComment> TblComments { get; set; }

}
