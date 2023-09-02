using AppControllers;
using Microsoft.AspNetCore.Mvc;
using WebAPIUniversal.Models;

namespace WebAPIUniversal.Controllers;

public class DeviceController : AppController<Device>
{
    private readonly ILogger<Device> logger;
    public DeviceController(AppDbContext db, ILogger<Device> logger) : base(db, logger)
    {
        this.logger = logger;
    }
    public override IQueryable<Device>? Get([FromQuery(Name = "$filter")] string? filter, [FromQuery(Name = "$top")] int? top, [FromQuery(Name = "$skip")] int? skip, [FromQuery(Name = "$orderby")] string? orderBy, [FromQuery(Name = "$select")] string? select)
    {
        var temp = base.Get(filter, top, skip, orderBy, select)?.ToList();
        logger.LogInformation("Found {n} rows", temp.Count());
        return temp.AsQueryable();
    }
}