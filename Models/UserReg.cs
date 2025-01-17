#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ExamCS.Models;

public class UserReg
{

    [Key]
    public int id { get; set; }
    [Required(ErrorMessage ="Name is required")]
    [MinLength(2, ErrorMessage ="First Name must be at least 2 characters")]
    public string FirstName { get; set; }
    [Required(ErrorMessage ="Last Name is required")]
    [MinLength(2, ErrorMessage ="Last Name must be at least 2 characters")]
    public string LastName { get; set; }
    [Required(ErrorMessage ="Username is required")]
    [MinLength(3, ErrorMessage ="Username must be at least 3 characters")]
    [MaxLength(15, ErrorMessage ="Username cant be more than 15 characters")]
    [UniqueUsername]
    
    public string Username { get; set; }
    [Required]
    [MinLength(8, ErrorMessage ="Password must be at least 8 characters")]
    public string Password { get; set; }
    [NotMapped]
    [Required]
    [MinLength(8)]
    [Compare("Password",ErrorMessage ="Passwords must match!")]
    public string Pwconfirm { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    
}



public class UniqueUsernameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if (value == null)
        {

            return new ValidationResult("Username is required!");
        }


        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));

        if (_context.Users.Any(e => e.Username == value.ToString()))
        {

            return new ValidationResult("Username must be unique!");
        }
        else
        {

            return ValidationResult.Success;
        }
    }
}




