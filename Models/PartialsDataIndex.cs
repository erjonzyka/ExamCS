#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ExamCS.Models;

public class PartialsDataIndex{
    public List<Hobby>? Novice {get;set;}

    public List<Hobby>? Intermediate {get;set;}
    public List<Hobby>? Expert {get;set;}

    public List<Hobby>? AllHobbies {get;set;}
    
}