using Platform;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Return Pipeline Path
// This middleware will be executed after all other middlewares

app.Use(async (context, next) =>
{
    await next();
    await context.Response
    .WriteAsync($"\nStatus Code: {context.Response.StatusCode}");
});

//custom midleware in Program.cs
app.Use(async (context, next) =>
{
    if (context.Request.Method == HttpMethods.Get &&
    context.Request.Query["custom"] == "true")
    {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("Custom Middleware \n");
    }
    await next();
});

//Short-Circuiting the Request Pipeline
app.Use(async (context, next) => {
    if (context.Request.Path == "/short")
    {
        await context.Response
        .WriteAsync($"Request Short Circuited");
    }
    else
    {
        await next();
    }
});

//calling class based middleware
app.UseMiddleware<QueryStringMiddleWare>();


//Creating Pipeline Branches
((IApplicationBuilder)app).Map("/branch", branch => {
    branch.UseMiddleware<Platform.QueryStringMiddleWare>();
    branch.Use(async (HttpContext context, Func<Task> next) => {
        await context.Response.WriteAsync($"Branch Middleware");
    });

    //Terminal middleware
    //ASP.NET Core supports the Run method as a convenience feature for creating terminal middleware
    branch.Run(async (context) => {
        await context.Response.WriteAsync($"Branch Middleware");
    });
});


app.MapGet("/", () => "Hello World!");



app.Run();
