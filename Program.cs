using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Steeltoe.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.Placeholder;
using SvcService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    
    .AddYamlFile("application.yaml", false, true)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddPlaceholderResolver()
    ;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString(builder.Configuration["Dbms"] ?? "mysql");

builder.Services
    .AddDbContext<ServiceDbContext>(opt =>
        {
            opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    );

var app = builder.Build();

using (var dbEnsureScope = app.Services.CreateScope())
{
    dbEnsureScope.ServiceProvider.GetRequiredService<ServiceDbContext>()
        .Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/service/add", async ([FromBody]Service service ,[FromServices]ServiceDbContext db) =>
    {
        await db.Services.AddAsync(new()
        {
            Id= service.Id,
            DesiredCapability = service.DesiredCapability,
            Repo=service.Repo,
            VersionMajor=service.Version?.Major,
            VersionMinor=service.Version?.Minor,
            VersionPatch=service.Version?.Patch,
            DesiredCpu=service.DesiredResource.Cpu,
            DesiredDisk=service.DesiredResource.Disk,
            DesiredGpuCore=service.DesiredResource.GpuCore,
            DesiredGpuMem=service.DesiredResource.GpuMem,
            DesiredRam=service.DesiredResource.Ram,
            IdleDisk=service.IdleResource?.Disk,
            IdleCpu=service.IdleResource?.Cpu,  
            IdleGpuCore=service.IdleResource?.GpuCore,
            IdleGpuMem =service.IdleResource?.GpuMem,
            IdleRam=service.IdleResource?.Ram,
            Interfaces=service.Interfaces.Select(i=> new InterfaceEntity
            {
                Id = i.Id,
                Path= i.Path,
                InputSize= i.InputSize,
                OutputSize= i.OutputSize,
            }).ToList()
        });
        await db.SaveChangesAsync();
        return Results.NoContent();
    })
.WithName("AddService")
.WithOpenApi();

app.MapPost("/service/update", async ([FromBody] Service service, [FromServices] ServiceDbContext db) =>
    {
        var t= await db.Services.Include(s => s.Interfaces)
                .AsTracking().FirstOrDefaultAsync(s=>s.Id==service.Id);
        if (t is null) return Results.NotFound();
        t.DesiredCapability = service.DesiredCapability;
        t.Repo = service.Repo;
        t.VersionMajor = service.Version?.Major;
        t.VersionMinor = service.Version?.Minor;
        t.VersionPatch = service.Version?.Patch;
        t.DesiredCpu = service.DesiredResource.Cpu;
        t.DesiredDisk = service.DesiredResource.Disk;
        t.DesiredGpuCore = service.DesiredResource.GpuCore;
        t.DesiredGpuMem = service.DesiredResource.GpuMem;
        t.DesiredRam = service.DesiredResource.Ram;
        t.IdleDisk = service.IdleResource?.Disk;
        t.IdleCpu = service.IdleResource?.Cpu;
        t.IdleGpuCore = service.IdleResource?.GpuCore;
        t.IdleGpuMem = service.IdleResource?.GpuMem;
        t.IdleRam = service.IdleResource?.Ram;
        t.Interfaces = service.Interfaces.Select(i => new InterfaceEntity
        {
            Id = i.Id,
            Path = i.Path,
            InputSize = i.InputSize,
            OutputSize = i.OutputSize,
        }).ToList();
    
        await db.SaveChangesAsync();
        return Results.NoContent();
    })
    .WithName("UpdateService")
.WithOpenApi();

app.MapPost("service/delete", async ([FromBody] DeleteBean bean, [FromServices] ServiceDbContext db) =>
{
    await db.Services.Where(s=>s.Id==bean.ServiceId).DeleteFromQueryAsync();
    return Results.NoContent();
});

app.Run();

record DeleteBean(string ServiceId);
record Version(string Major, string Minor, string Patch);
record Interface(string Id, string Path, int InputSize, string OutputSize);
record Resource(decimal Cpu, decimal Ram, decimal Disk, decimal GpuCore, decimal GpuMem);
record Service(string Id, string Repo, Version? Version, List<Interface> Interfaces, Resource? IdleResource,
    Resource DesiredResource, int DesiredCapability);