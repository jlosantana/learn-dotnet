using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Middleware
app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/"));

// Custom Middleware
app.Use(async (context, next) => {
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path}]");
    await next(context);
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path}]");
});

var todos = new List<Todo>();

app.MapPost("/todos", (Todo task)=>{
    todos.Add(task);
    return TypedResults.Created("/todos/{id}", task);
}).AddEndpointFilter(async (contex, next) => {
    var taskArgument = contex.GetArgument<Todo>(0);
    var errors = new Dictionary<string, string[]>();
    if (taskArgument.IsCompleted)
    {
        errors.Add(nameof(Todo.IsCompleted), ["Cannot add complete TODO"]);
    }
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }
    return await next(contex);
});
app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id) => {
    var targetTodo = todos.SingleOrDefault(t => id == t.Id);
    return targetTodo is null ? TypedResults.NotFound() : TypedResults.Ok(targetTodo);
});
app.MapGet("/todos", () => todos);
app.MapDelete("/todos/{id}", Results<NoContent, NotFound> (int id) => {
    var countRemoved = todos.RemoveAll(t => id == t.Id);
    return countRemoved > 0 ? TypedResults.NotFound() : TypedResults.NoContent();
});

app.Run();

public record Todo(int Id, string Name, DateTime DueDate, bool IsCompleted);
