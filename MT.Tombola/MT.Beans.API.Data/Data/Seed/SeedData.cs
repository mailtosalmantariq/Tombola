using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MT.Tombola.Api.Data.Data;
using MT.Tombola.Api.Data.Models;
using MT.Tombola.Api.Data.Static;
using System.Text.Json;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        Console.WriteLine("Seeding started...");

        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BeansDbContext>();

        if (await context.Beans.AnyAsync())
        {
            Console.WriteLine("Beans already exist. Skipping seeding.");
            return;
        }

        var path = Path.Combine(AppContext.BaseDirectory, "Data", "beans.json");
        Console.WriteLine($"Looking for file at: {path}");

        if (!File.Exists(path))
        {
            Console.WriteLine("File not found.");
            return;
        }

        var json = await File.ReadAllTextAsync(path);
        Console.WriteLine($"JSON content length: {json.Length}");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new StringToDecimalConverter());

        var beans = JsonSerializer.Deserialize<List<Bean>>(json, options);
        if (beans is null || beans.Count == 0)
        {
            Console.WriteLine("No beans found in JSON.");
            return;
        }

        foreach (var bean in beans)
        {
            if (string.IsNullOrWhiteSpace(bean.ExternalId))
                bean.ExternalId = $"bean-{Guid.NewGuid()}";
        }

        Console.WriteLine($"Seeding {beans.Count} beans...");
        context.Beans.AddRange(beans);
        await context.SaveChangesAsync();
        Console.WriteLine("Seeding complete.");
    }

}
