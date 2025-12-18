var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Root endpoint
app.MapGet("/", () => "Minimal API is running");

// Simple GET with parameter
app.MapGet("/hello/{name}", (string name) =>
{
    return $"Hello, {name}!";
});

// Simple POST
app.MapPost("/echo", (MessageRequest request) =>
{
    return Results.Ok(new
    {
        received = request.Message,
        length = request.Message.Length
    });
});

app.Run();

// Request model
record MessageRequest(string Message);
