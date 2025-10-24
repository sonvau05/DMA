using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseDefaultFiles();
app.UseStaticFiles();

var users = new ConcurrentDictionary<string, User>(new[] {
    new KeyValuePair<string,User>("alice", new User("alice","pass1")),
    new KeyValuePair<string,User>("bob", new User("bob","pass2")),
    new KeyValuePair<string,User>("carol", new User("carol","pass3"))
});

const string adminKey = "adminkey123";

app.MapPost("/api/auth/login", (Credential cred) => {
    if (cred == null) return Results.BadRequest();
    if (!users.TryGetValue(cred.Username, out var u)) return Results.Unauthorized();
    if (u.TokenCleared) return Results.Unauthorized();
    if (u.Password != cred.Password) return Results.Unauthorized();
    u.Token = Guid.NewGuid().ToString();
    return Results.Ok(new { token = u.Token });
});

app.MapGet("/api/admin/users", (HttpRequest req) => {
    if (!IsAdmin(req)) return Results.Unauthorized();
    var list = users.Values.Select(u => new { u.Username, u.Token, u.TokenCleared });
    return Results.Ok(list);
});

app.MapPost("/api/admin/clear/{username}", (string username, HttpRequest req) => {
    if (!IsAdmin(req)) return Results.Unauthorized();
    if (!users.TryGetValue(username, out var u)) return Results.NotFound();
    u.Token = null;
    u.TokenCleared = true;
    return Results.Ok(new { message = "cleared", username });
});

app.MapPost("/api/admin/reset/{username}", (string username, HttpRequest req) => {
    if (!IsAdmin(req)) return Results.Unauthorized();
    if (!users.TryGetValue(username, out var u)) return Results.NotFound();
    u.TokenCleared = false;
    return Results.Ok(new { message = "reset", username });
});

app.Run();

bool IsAdmin(HttpRequest req) => req.Query.TryGetValue("key", out var v) && v == adminKey;

record User(string Username, string Password)
{
    public string? Token { get; set; }
    public bool TokenCleared { get; set; }
}

record Credential(string Username, string Password);
