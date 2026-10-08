using System.Text.Json.Serialization;
using SQLite;
namespace MauiAppPrimerParcial.Models;

// Entidad Principal: User
public class User
{
    [PrimaryKey]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }
}