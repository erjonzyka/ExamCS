#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ExamCS.Models;
public class Proficency
{
    [Key]    
    public int ProficencyId { get; set; } 
    public string Level {get;set;}
    public int? EnthusiastId { get; set; }    
    public int? HobbyId { get; set; }
    // Our navigation properties - don't forget the ?    
    public UserReg? Enthusiast { get; set; }    
    public Hobby? Hobby { get; set; }
}