using University.GraphQL.Schemas;
using University.GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddGraphQLServer()
    .AddQueryType<AppQuery>()
    .AddMutationType<AppMutation>()
    .AddType<StudentType>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseRouting();
//app.UseAuthorization();

// add the GraphQL server middleware
app.MapGraphQL();

app.Run();
