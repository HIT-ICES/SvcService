using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Extensions.Configuration.Placeholder;
using SvcService;
using SvcService.Data;
using Service = SvcService.Service;
using static Microsoft.AspNetCore.Http.Results;
using System.Text.Json;
using Steeltoe.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddPlaceholderResolver();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var connectionString = builder.Configuration.GetConnectionString(builder.Configuration["Dbms"] ?? "mysql");

//Console.WriteLine($"Connection String: {connectionString}");
//foreach (var key in new[] { "MYSQL_UID", "MYSQL_IP", "MYSQL_PWD" })
//{
//    Console.WriteLine($"ENV:{key}={Environment.GetEnvironmentVariable(key)}");
//}



builder.Services
    .AddDbContext<ServiceDbContext>(opt =>
        {
            opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    );

var app = builder.Build();

using (var dbEnsureScope = app.Services.CreateScope())
{
    var db = dbEnsureScope.ServiceProvider.GetRequiredService<ServiceDbContext>().Database;
    //await db.EnsureCreatedAsync();
    //var pendingMigrations=await db.GetPendingMigrationsAsync();
    await db.MigrateAsync();

}

app.UseCors(cors => cors.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());

app.UseSwagger();
app.UseSwaggerUI();


app.MapPost("/service/add", async ([FromBody] Service service, [FromServices] ServiceDbContext db) =>
{
    var serviceEntity = new ServiceEntity();
    service.CopyToEntity(serviceEntity);
    await db.Services.AddAsync(serviceEntity);
    await db.SaveChangesAsync();
    return MResponse.Successful();
}).WithName("AddService").WithOpenApi();

app.MapPost("/service/update", async ([FromBody] Service service, [FromServices] ServiceDbContext db) =>
{
    var t = await db.Services.Include(s => s.Interfaces)
            .AsTracking().FirstOrDefaultAsync(s => s.Id == service.Id);
    if (t is null) return MResponse.Failed($"Service with Id {service.Id} not found");
    service.CopyToEntity(t);
    await db.SaveChangesAsync();
    return MResponse.Successful();
}).WithName("UpdateService").WithOpenApi();

app.MapPost("service/delete", async ([FromBody] ByServiceIdBean bean, [FromServices] ServiceDbContext db) =>
{
    return await db.Services.Where(s => s.Id == bean.ServiceId).DeleteFromQueryAsync() >= 1
        ? MResponse.Successful()
        : MResponse.Failed($"Service with Id {bean.ServiceId} not found");
}).WithName("DeleteService").WithOpenApi();

app.MapPost("service/getById", async ([FromBody] ByServiceIdBean bean, [FromServices] ServiceDbContext db) =>
{
    var entity = await db.Services
        .Include(s => s.Interfaces)
        .Where(s => EF.Functions.Like(s.Id, $"%{bean.ServiceId}%"))
            .ToArrayAsync();
    return entity.Length == 0 ?
            Ok(MResponse.Failed($"Service with Id {bean.ServiceId} not found")) :
            Ok(MResponse.Successful(entity.Select(Service.FromEntity).ToArray()));
}).WithName("GetServiceById").WithOpenApi();

app.MapPost("service/getServiceByExactId", async ([FromBody] ByServiceIdBean bean, [FromServices] ServiceDbContext db) =>
{
    var entity = await db.Services
        .Include(s => s.Interfaces)
        .Where(s => s.Id==bean.ServiceId)
        .ToArrayAsync();
    return entity.Length == 0 ?
        Ok(MResponse.Failed($"Service with Id {bean.ServiceId} not found")) :
        Ok(MResponse.Successful(entity.Select(Service.FromEntity).ToArray()));
}).WithName("GetServiceByExactId").WithOpenApi();

app.MapPost("/service/getByNameVersion", async ([FromBody] ByNameVersionBean bean, [FromServices] ServiceDbContext db) =>
{

    var entity = await (bean.Version is null ?
        db.Services
            .Include(s => s.Interfaces)
            .Where(s => EF.Functions.Like(s.Name, $"%{bean.Name}%")) :
        db.Services
            .Include(s => s.Interfaces)
            .Where(s =>
                EF.Functions.Like(s.Name, $"%{bean.Name}%") &&
                EF.Functions.Like(s.VersionMajor, $"%{bean.Version.Major}%") &&
                EF.Functions.Like(s.VersionMinor, $"%{bean.Version.Minor}%") &&
                EF.Functions.Like(s.VersionPatch, $"%{bean.Version.Patch}%"))).ToArrayAsync();
    return entity.Length == 0 ?
        Ok(MResponse.Failed($"Service with Name {bean.Name} and Version {bean.Version} not found")) :
        Ok(MResponse.Successful(entity.Select(Service.FromEntity).ToArray()));
}).WithName("GetServiceByNameVersion").WithOpenApi();

app.MapPost("/service/addDependencies", async ([FromBody] List<DependencyDescription> bean, [FromServices] ServiceDbContext db) =>
{
    async Task _add(DependencyDescription depd)
    {
        var entity = new DependencyEntity();
        depd.CopyToEntity(entity);
        await db.Dependencies.AddAsync(entity);

    }

    await Task.WhenAll(bean.Select(_add));
    await db.SaveChangesAsync();
    return Ok(MResponse.Successful());
}).WithName("AddDependencies").WithOpenApi();

app.MapPost("/service/updateDependencies", async ([FromBody] List<DependencyDescription> bean, [FromServices] ServiceDbContext db) =>
{
    void _update(DependencyDescription depd)
    {
        var entity = new DependencyEntity();
        depd.CopyToEntity(entity);
        db.Dependencies.Update(entity);

    }

    bean.ForEach(_update);
    await db.SaveChangesAsync();
    return Ok(MResponse.Successful());
}).WithName("UpdateDependencies").WithOpenApi();

app.MapPost("/service/deleteDependencies", async ([FromBody] List<DependencyDescription> bean, [FromServices] ServiceDbContext db) =>
{
    void _delete(DependencyDescription depd)
    {
        var entity = new DependencyEntity();
        depd.CopyToEntity(entity);
        db.Dependencies.Remove(entity);

    }

    bean.ForEach(_delete);
    await db.SaveChangesAsync();
    return Ok(MResponse.Successful());
}).WithName("DeleteDependencies").WithOpenApi();

app.MapPost("/service/getInterfaceDependencies", handler: ([FromServices] ServiceDbContext db) =>
{
    var all= new List<InterfaceDependencyGraphNode>();
    foreach (var depdGroup in db.Dependencies.ToList().GroupBy(d => d.CallerId))
    {
        all.Add(
        new InterfaceDependencyGraphNode(depdGroup.Key,
            depdGroup.ToDictionary(d => d.CalleeId, d =>
                JsonSerializer.Deserialize<JsonElement>(d.SerilizedData))));
    }
    return Ok(MResponse.Successful(all));
}).WithName("GetInterfaceDependencies").WithOpenApi();

app.MapPost("/service/getServiceDependencies",  ( [FromServices] ServiceDbContext db) =>
{
    var all = new List<ServiceDependencyGraphNode>();
    foreach (var depdGroup in 
             db.Dependencies.GroupBy(d => d.CallerServiceId))
    {
        all.Add(
            new ServiceDependencyGraphNode(depdGroup.Key,
                depdGroup.GroupBy(d=>d.CalleeServiceId)
                    .ToDictionary(d => d.Key, 
                        d=>d.Select(DependencyDescription.FromEntity).ToArray())));
    }
    return Ok(MResponse.Successful(all));
}).WithName("GetServiceDependencies").WithOpenApi();

app.Run();

namespace SvcService
{

    public record ByServiceIdBean(string ServiceId);

    public record ByInterfaceIdBean(string Id)
    {
        public string IdSuffix =>Id.Split("::")[1];
        public string ServiceId => Id.Split("::")[0];
    }
    public record ByNameVersionBean(string Name, Version? Version);

    public record Version(string Major, string Minor, string Patch);

    public record Interface(string Id, string Path, int InputSize, string OutputSize)
    {
        public InterfaceEntity ToEntity()
        {
            return new()
            {
                Id = Id,
                Path = Path,
                InputSize = InputSize,
                OutputSize = OutputSize,
            };
        }

        public static Interface FromEntity(InterfaceEntity entity)
        {
            return new(entity.Id, entity.Path, entity.InputSize, entity.OutputSize);
        }
    }

    public record Resource(decimal Cpu, decimal Ram, decimal Disk, decimal GpuCore, decimal GpuMem);

    public record Service(string Id, string Name, string Repo, string ImageUrl, Version? Version, List<Interface> Interfaces,
        Resource? IdleResource,
        Resource DesiredResource, int DesiredCapability)
    {
        public void CopyToEntity(ServiceEntity entity)
        {
            entity.Id = Id;
            entity.Name = Name;
            entity.DesiredCapability = DesiredCapability;
            entity.Repo = Repo;
            entity.ImageUrl = ImageUrl;
            entity.Version = Version;
            entity.DesiredResource = DesiredResource;
            entity.IdleResource = IdleResource;
            entity.Interfaces = Interfaces.Select(i => i.ToEntity()).ToList();
        }
        public static Service FromEntity(ServiceEntity entity)
        {
            return new(
                entity.Id,
                entity.Name,
                entity.Repo,
                entity.ImageUrl,
                entity.Version,
                entity.Interfaces.Select(Interface.FromEntity).ToList(),
                entity.IdleResource,
                entity.DesiredResource,
                entity.DesiredCapability);
        }
    }

    public record DependencyDescription(string Caller, string Callee, JsonElement ExtraData)
    {
        public void CopyToEntity(DependencyEntity entity)
        {
            entity.CallerId = Caller;
            entity.CalleeId = Callee;
            entity.SerilizedData = ExtraData.GetRawText();
        }
        public static DependencyDescription FromEntity(DependencyEntity entity)
            => new(entity.CallerId, entity.CalleeId,
                JsonSerializer.Deserialize<JsonElement>(entity.SerilizedData));

    }

    public record InterfaceDependencyGraphNode(string Caller, Dictionary<string, JsonElement> Callees);
    public record ServiceDependencyGraphNode(string Caller, Dictionary<string, DependencyDescription[]> Callees);
}
