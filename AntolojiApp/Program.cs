using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// HTML dosyamızı (wwwroot içindekileri) sunmak için gerekli ayarlar
app.UseDefaultFiles(); 
app.UseStaticFiles();

string veritabani = "kelimeler.json";

// GET: Sayfa yüklendiğinde verileri getirir
app.MapGet("/api/kelimeler", async () => {
    if (File.Exists(veritabani))
    {
        var json = await File.ReadAllTextAsync(veritabani);
        return Results.Content(json, "application/json");
    }
    return Results.Ok(new object[0]);
});

// POST: Yeni kelime eklendiğinde veya silindiğinde verileri kaydeder
app.MapPost("/api/kaydet", async (HttpContext context) => {
    using var reader = new StreamReader(context.Request.Body);
    var gelenVeri = await reader.ReadToEndAsync();
    
    try {
        JsonDocument.Parse(gelenVeri);
        await File.WriteAllTextAsync(veritabani, gelenVeri);
        return Results.Ok(new { status = "success" });
    }
    catch {
        return Results.BadRequest(new { status = "error" });
    }
});

app.Run();