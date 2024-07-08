using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
namespace FitnessApp.Models;

public class UserDetails : IdentityUser
{
    public string? ProfilePicture { get; set; }
    public string? Bio { get; set; }
    
}