using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using University.Domain.Entities;
using University.GraphQL.Schemas;
using University.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextFactory<UniversityContext>(options =>
    options.UseInMemoryDatabase("University"));

builder.Services.AddScoped<IPasswordHasher<Users_Accounts>, PasswordHasher<Users_Accounts>>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<AppQuery>()
    .AddMutationType<AppMutation>()
    .RegisterTypes()
    .AddMaxExecutionDepthRule(15);

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var contextFactory = service.GetRequiredService<IDbContextFactory<UniversityContext>>();
    using var context = contextFactory.CreateDbContext();
    var passwordHasher = service.GetRequiredService<IPasswordHasher<Users_Accounts>>();
    var configuration = service.GetRequiredService<IConfiguration>();

    UniversityContextSeed.Initialize(context, passwordHasher, configuration);
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();
//app.UseAuthorization();

// add the GraphQL server middleware
app.MapGraphQL();

app.Run();
