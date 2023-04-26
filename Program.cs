using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.Placeholder;
using SvcService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddYamlFile("application.yaml", false, true)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddPlaceholderResolver();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString(builder.Configuration["Dbms"] ?? "mysql");

builder.Services
    .AddMySql<ServiceDbContext>(connectionString, ServerVersion.AutoDetect(connectionString));

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
        db.Services.Update(new()
        {
            Id = service.Id,
            DesiredCapability = service.DesiredCapability,
            Repo = service.Repo,
            VersionMajor = service.Version?.Major,
            VersionMinor = service.Version?.Minor,
            VersionPatch = service.Version?.Patch,
            DesiredCpu = service.DesiredResource.Cpu,
            DesiredDisk = service.DesiredResource.Disk,
            DesiredGpuCore = service.DesiredResource.GpuCore,
            DesiredGpuMem = service.DesiredResource.GpuMem,
            DesiredRam = service.DesiredResource.Ram,
            IdleDisk = service.IdleResource?.Disk,
            IdleCpu = service.IdleResource?.Cpu,
            IdleGpuCore = service.IdleResource?.GpuCore,
            IdleGpuMem = service.IdleResource?.GpuMem,
            IdleRam = service.IdleResource?.Ram,
            Interfaces = service.Interfaces.Select(i => new InterfaceEntity
            {
                Id = i.Id,
                Path = i.Path,
                InputSize = i.InputSize,
                OutputSize = i.OutputSize,
            }).ToList()
        });
        await db.SaveChangesAsync();
        return Results.NoContent();
    })
    .WithName("AddService")
    .WithOpenApi();

app.Run();

record Version(string Major, string Minor, string Patch);
record Interface(string Id, string Path, int InputSize, string OutputSize);
record Resource(decimal Cpu, decimal Ram, decimal Disk, decimal GpuCore, decimal GpuMem);
record Service(string Id, string Repo, Version? Version, List<Interface> Interfaces, Resource? IdleResource,
    Resource DesiredResource, int DesiredCapability);