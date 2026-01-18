using System.Text.Json.Serialization;

public class BeanDto
{
    public string ExternalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Colour { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("Image")]
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public bool IsBOTD { get; set; }
}
