using System.ComponentModel.DataAnnotations;
using AppControllers;
using Microsoft.EntityFrameworkCore;

namespace WebAPIUniversal.Models;

public class Device : AppEntity
{
    [StringLength(20)] public string? Name { get; set; }
    [StringLength(20)] public string? Category { get; set; }
    [Precision(18, 3)] public decimal? Price { get; set; }
}