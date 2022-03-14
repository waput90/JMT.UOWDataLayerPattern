# Unit of Work with Repository Pattern Library for EF Core 3.X
this library was built to managed the opening and closing of the database, and only used for single database instance to make all the queries effecient instead of closing and opening the database everytime you have an query for DB


## Application and Usage

1. make sure you register the access layer web or api project

```
-- Startup.cs --

using JMT.UOWDataLayerPattern.Core;

public void ConfigureServices(IServiceCollection services)
{

    // you can choose either mysql or mssql for DB your choice as long as its supported with EF       
    services.AddDbContextPool<KCargoDbContext>(option =>
        option.UseMySql(CONNECTION_STRING_HERE);

    services.AddDbContext<KCargoDbContext>(option =>
        option.UseMySql(CONNECTION_STRING_HERE);

    services.RegisterDataAccess<YOUR_DB_CONTEXT_HERE>();
    
```

2. also create a DbContext that will generate all the entities to your project

```
-- YOUR_DB_CONTEXT.cs ---

using JMT.UOWDataLayerPattern.Core;

public class YOUR_DB_CONTEXT : BaseDbContext<YOUR_DB_CONTEXT>
{
    public YOUR_DB_CONTEXT(
        DbContextOptions<YOUR_DB_CONTEXT> contextOptions) : base(contextOptions)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
  
    // see to it you register all the entities here to be included in migration
    public virtual DbSet<ENTITY_MODEL> entity_name { get; set; }
```

3. create a factory so that you will generate your migration and set its connection

```
public class KCargoDbContextFactory : IDesignTimeDbContextFactory<KCargoDbContext>
{
    KCargoDbContext IDesignTimeDbContextFactory<KCargoDbContext>.CreateDbContext(string[] args)
    {
          var builder = new DbContextOptionsBuilder<KCargoDbContext>();

          builder.UseMySql(YOUR_CONNECTION_STRING_HERE);
          
          return new KCargoDbContext(builder.Options);
    }
}

```

3.1. you can run the following example command to do a migration

```
dotnet ef migrations add SOME_TXT_MIGRATION_HERE -c YOUR_DB_CONTEXT_HERE
dotnet ef database update -c YOUR_DB_CONTEXT_HERE 
``` 

4. these are the following method available on this library

```
// will return entity object value
TEntity FindAsync(params object[] keyValues)

// will still return entity object value but you can add more key on it
TEntity Find(params object[] keyValues)

// will return entity object by the used of expression
TEntity FindBy(Expression<Func<TEntity, bool>> predicate = null)

// will find and return true or false if the data found
bool ContainBy(Expression<Func<TEntity, bool>> predicate = null)

// will get all the list with connected FK to the table
Task<IEnumerable<TEntity>> ItemsWithAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)

// will get all the list on specified expression params
Task<IEnumerable<TEntity>> ItemsAsync(Expression<Func<TEntity, bool>> predicate = null)

// will get table entity of the object
IQueryable<TEntity> Table => _dbContext.Set<TEntity>();

// will insert item to specific entity passed
void Insert(TEntity newItem)

// will insert bulk data to specific entity
void InsertBulk(IEnumerable<TEntity> newItems)

// will update entity
void Update(TEntity item, params Expression<Func<TEntity, object>>[] excludeProperties)

// will delete using entity object
void Delete(TEntity item)

// will delete using entity id
void Delete(object id)

// will delete bulk data on entity object
void DeleteBulk(IEnumerable<TEntity> itemsToDelete)

// will delete by specific expression on the entity object
void DeleteBy(Expression<Func<TEntity, bool>> predicate = null)

// will execute an SQL command like SELECT * FROM Example table
IEnumerable<TEntity> ExecuteQuery(string query)

// will execute an SQL command like SELECT * FROM Example table but in formattable query string
IEnumerable<TEntity> ExecuteQuery(FormattableString query)

// will execute an SQL command like SELECT * FROM Example table but in queryable state
IQueryable<TEntity> ExecuteQueryable(string query)

// will execute an SQL command like SELECT * FROM Example table but in queryable state with FK tables passed
IQueryable<TEntity> ItemsWithAsyncQueryable(
  Expression<Func<TEntity, bool>> predicate = null, 
  params Expression<Func<TEntity, object>>[] includeProperties)
  
  
// will execute a non query like INSERT, UPDATE and DELETE statements or DDL, DML 
bool ExecuteNonQuery(string query)

// insert asynchronously
Task InsertAsync(TEntity newItem)

// will return a count of entity based on predicate passed
Task<int> GetTotal(Expression<Func<TEntity, bool>> predicate = null)

```

5. example usage
```
using (var uow = _unitOfWorkFactory.CreateUow())
{
    var sample_query = uow.GetEntityRepository<YOUR_ENTITY_MODEL>()
        .FindBy(_ => _.key_name == key_name);

    if (sample_query != null)
    {
        // create email send log
        var mapped_entity_model = new YOUR_ENTITY_MODEL
        {
            key_name = key_name,
            created_date = DateTime.Now.GetDate(),
            modified_date = DateTime.Now.GetDate(),
        };

        uow.GetEntityRepository<YOUR_ENTITY_MODEL>().Insert(mapped_entity_model);
        await uow.Commit();

        return _mapper.Map<ENTITY_RESPONSE_MODEL>(mapped_entity_model);
    }
    else
    {
        sample_query.modified_date = DateTime.Now.GetDate();

        uow.GetEntityRepository<BackgroundEmailSendLog>().Update(sample_query);
        await uow.Commit();

        return _mapper.Map<ENTITY_RESPONSE_MODEL>(sample_query);
    }
}
```
