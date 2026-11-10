using Microsoft.AspNetCore.SignalR;
using MlbbOverlay.Backend;
using System.IO;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Setup CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSignalR();

// Lokasi penyimpanan file JSON
var dbFilePath = Path.Combine(Directory.GetCurrentDirectory(), "overlay_data.json");
var appState = new MlStatsState();

// Baca file JSON saat project baru jalan
if (File.Exists(dbFilePath))
{
    try
    {
        var jsonContent = File.ReadAllText(dbFilePath);
        var savedData = JsonSerializer.Deserialize<MlStats>(jsonContent);
        if (savedData != null)
        {
            appState.Stats = savedData;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Gagal baca JSON: {ex.Message}");
    }
}

builder.Services.AddSingleton(appState);

var app = builder.Build();
app.UseCors("AllowReact");

app.UseDefaultFiles();
app.UseStaticFiles();

// Routing Hub SignalR
app.MapHub<OverlayHub>("/overlayHub");

// Endpoint API Get (Buat ditarik pas web awal dibuka)
app.MapGet("/api/stats", (MlStatsState state) => Results.Ok(state.Stats));

// Endpoint API Post (Buat update data dari Admin Panel & tulis ke JSON)
app.MapPost("/api/stats", async (MlStats newStats, MlStatsState state, IHubContext<OverlayHub> hubContext) =>
{
    state.Stats = newStats;

    // Simpan ke file JSON
    var jsonToSave = JsonSerializer.Serialize(state.Stats, new JsonSerializerOptions { WriteIndented = true });
    await File.WriteAllTextAsync(dbFilePath, jsonToSave);

    // Broadcast ke OBS yang lagi terhubung
    await hubContext.Clients.All.SendAsync("ReceiveStatsUpdate", state.Stats);

    return Results.Ok(state.Stats);
});

app.Run("http://localhost:5059");