var builder = WebApplication.CreateBuilder(args);

// MVC ekliyoruz
builder.Services.AddControllersWithViews();

// OpenAI Service ekliyoruz
builder.Services.AddSingleton<OpenAiService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var apiKey = config["OpenAI:ApiKey"];

    if (string.IsNullOrWhiteSpace(apiKey))
        throw new Exception("OpenAI API Key bulunamadı!");
    
    return new OpenAiService(apiKey);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Controller route ekliyoruz
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Index}/{id?}");

app.Run();