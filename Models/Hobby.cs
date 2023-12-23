#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ExamCS.Models;

public class Hobby
{

    [Key]
    public int HobbyId { get; set; }
    [Required(ErrorMessage ="Name is required")]
    [MinLength(2, ErrorMessage ="Name must be at least 2 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage ="Description is required")]
    public string Description { get; set; }
  
     public int CreatorId {get;set;}
    public UserReg? Creator { get; set; }    

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Proficency> Proficencies { get; set; } = new List<Proficency>();
    
}
