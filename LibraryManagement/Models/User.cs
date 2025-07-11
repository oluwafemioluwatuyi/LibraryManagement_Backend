using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiration { get; set; }
    public string EmailVerificationToken { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime? EmailVerificationTokenExpiration { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    //Navigation properties

    public ICollection<Book> Books { get; set; }
}

