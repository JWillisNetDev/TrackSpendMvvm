using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using TrackSpendMvvm.Data;
using TrackSpendMvvm.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddTrackSpendService(builder.Configuration);
builder.Services.AddViewModels();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
 
using (var scope = app.Services.CreateScope()) // Apply migrations
{
	await using var db = await scope.ServiceProvider.GetRequiredService<IDbContextFactory<TrackSpendDbContext>>()
		.CreateDbContextAsync(); // Service added by AddTrackSpendService
	await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
