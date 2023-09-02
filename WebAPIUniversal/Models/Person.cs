using System.ComponentModel.DataAnnotations;
using AppControllers;

namespace WebAPIUniversal.Models;

public class Person : AppEntity
{
    [StringLength(20)] public string? FirstName { get; set; }
    [StringLength(20)] public string? LstName { get; set; }
}