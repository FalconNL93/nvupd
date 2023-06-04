using System.Text.Json.Serialization;

namespace Nvupd.Core.Models;

public class OsData
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}