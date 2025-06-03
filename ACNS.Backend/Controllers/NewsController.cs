using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ACNS.Backend.Models;

namespace ACNS.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly string filePath = Path.Combine("wwwroot", "data", "newsMain.json");

    [HttpGet]
    public async Task<IActionResult> GetNews()
    {
        if (!System.IO.File.Exists(filePath))
            return NotFound("File not found.");

        var json = await System.IO.File.ReadAllTextAsync(filePath);
        var newsList = JsonSerializer.Deserialize<NewsItem[]>(json);
        return Ok(newsList);
    }

    [HttpPost]
    public async Task<IActionResult> AddNews([FromBody] NewsItem item)
    {
        var list = new List<NewsItem>();

        if (System.IO.File.Exists(filePath))
        {
            var existingJson = await System.IO.File.ReadAllTextAsync(filePath);
            list = JsonSerializer.Deserialize<List<NewsItem>>(existingJson) ?? new List<NewsItem>();
        }

        list.Insert(0, item); // Add at the top
        var updatedJson = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        await System.IO.File.WriteAllTextAsync(filePath, updatedJson);

        return Ok(item);
    }
}