using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppControllers;

[ApiController]
[Route("[controller]")]
public abstract class AppController<T> : ControllerBase where T : AppEntity
{
    private readonly AppDbContext db;
    private readonly ILogger<T> logger;
    private readonly DbSet<T>? table;

    public AppController(AppDbContext db, ILogger<T> logger)
    {
        this.db = db;
        this.logger = logger;
        var type = typeof(T);
        var name = type.ToString().Split('.').Last();
        table = (DbSet<T>)this.db[name]!;
    }

    [HttpGet]
    [EnableQuery]
    public virtual IQueryable<T>? Get(
        [FromQuery(Name = "$filter")] string? filter, [FromQuery(Name = "$top")] int? top, [FromQuery(Name = "$skip")] int? skip, [FromQuery(Name = "$orderby")] string? orderBy, [FromQuery(Name = "$select")] string? select, [FromQuery(Name = "$expand")] string? expand
    )
    {
        logger.LogInformation("Query for {type}, {filer}, {select}, {top}, {skip}", typeof(T), filter, select, top,
            skip);
        return table?.AsQueryable();
    }

    [HttpPost]
    public virtual T Post([FromBody] T obj)
    {
        if (obj.Id == 0)
        {
            table?.Add(obj);
        }
        else
        {
            db.Entry(obj).State = EntityState.Modified;
        }

        db.SaveChanges();
        return obj;
    }

    [HttpDelete("{id}")]
    public virtual ActionResult Delete(long id)
    {
        var obj = table?.FirstOrDefault(x => x.Id == id);
        if (obj == null) return NotFound();
        table?.Remove(obj);
        db.SaveChanges();
        return Ok();
    }

    [HttpGet("{id}")]
    [EnableQuery]
    public virtual ActionResult<T> GetOne(long id, [FromQuery(Name = "$select")] string? select, [FromQuery(Name = "$expand")] string? expand)
    {
        var obj = table?.FirstOrDefault(x => x.Id == id);
        if (obj == null) return NotFound();
        return obj;
    }
}
public static class OdataExtensions
{
    public static IMvcBuilder RegisterOdata(this IMvcBuilder builder)
    {
        builder.AddOData(o => { o.Select().Filter().OrderBy().EnableQueryFeatures(); });
        return builder;
    }
}