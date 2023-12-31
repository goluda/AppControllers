# Universal Webapi with ef core 

Idea of this project is to provide simple library which provide helper classes to create **DbContext** and **Controllers** based on defined **Entity classes**

Library provides following abstract classes:
- **AppEntity** this is a sceleton of entity class which is using long as autogenerated key
- **AppDbContext** this is sceleton class of DbContext
- **AppController** this is sceleton of web api controller which provides basic crud operations for defined entity. Important note is that it uses POST method to upsert data.

## Example configuration

### Define database context

First step you must to do is to define your database context which inherites from **AppDbContext** abstract class. 

```c#
public class MyContext : AppDbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {
    }

    public DbSet<Person> Person => Set<Person>();
}
```
Then you need to define your entity class by inheriting from **AppEntity** abstract class. Here is an example of Person class

```c#
public class Person : AppEntity
{
    [StringLength(20)] public string? FirstName { get; set; }
    [StringLength(20)] public string? LstName { get; set; }
}
```
As you can see you don't add any key because AppEntity provides Id property as key field.

**Entity naming convention** while adding DbSet object please use the same name for Table as name of Entity class!!! e.g. Person class makes Person DbSet

```c#
public DbSet<Person> Person => Set<Person>();
```

In case you are using includes and want to have it returned in controler for one object you must configure this properties as **AutoInclude**. Here you can find example:

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Person>().Navigation(x => x.Address).AutoInclude();
    }
```

### Register db context

To register your database context in DI container please do following statement:

```c#
builder.Services.AddDbContext<AppDbContext, MyContext>(x => x.UseSqlite("Data Source=database.db"));
```
Remember to register yor context as type of **AppDbContext**

### Build controller

To build controller simply generate controller class which will inherit from **AppController** abstract class.

```c#
public class PersonController : AppController<Person>
{
    public PersonController(AppDbContext db, ILogger<Person> logger) : base(db, logger)
    {
    }
}
```
By default this class will provide for you follwing endpoints:
- GET - Odata endpoint to query object
- GET/{id} - endpoint to get object by id
- POST - endopint to upsert object in database
- DELETE/{id} - delete object

Each of this method can be overwrited because it is virtual.
If you need to add custom object you can do this at any moment you want.

### Odata query

Important for querying objects is to register **OData** in DI container.

By updating controllers registrations
```c#
builder.Services.AddControllers()
    .RegisterOdata();
```

When OData is registerd you have access to following query parameters on GET endpoint:
- $filter
- $select
- $orderby
- $top
- $skip
- $expand

This query params can give you possibility to easy make filters on default controller parameters available out of the box.