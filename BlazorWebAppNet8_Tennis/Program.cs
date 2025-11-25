using BlazorWebAppNet8_Tennis.Components;
using BlazorWebAppNet8_Tennis.Services.Backend;
using BlazorWebAppNet8_Tennis.Services.Frontend;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddHttpClient();
builder.Services.AddHttpClient("backend", client =>
{
    client.BaseAddress = new Uri("https://localhost:7033/");
});


builder.Services.AddControllers();

builder.Services.AddSingleton<MatchService>();
builder.Services.AddSingleton<ScoreBoardService>();
builder.Services.AddScoped<TennisFrontendService>();


var app = builder.Build();

app.UseRouting();


app.MapControllers();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
