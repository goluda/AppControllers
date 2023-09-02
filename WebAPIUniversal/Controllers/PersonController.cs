using AppControllers;
using WebAPIUniversal.Models;

namespace WebAPIUniversal.Controllers;

public class PersonController : AppController<Person>
{
    public PersonController(AppDbContext db, ILogger<Person> logger) : base(db, logger)
    {
    }
}