using System.Net.Http.Json;

var client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:7140"); 

while (true)
{
    var location = new
    {
        Name = "Truck-001",
        Latitude = -6.2 + Random.Shared.NextDouble() * 0.01,
        Longitude = 106.8 + Random.Shared.NextDouble() * 0.01
    };

    var res = await client.PostAsJsonAsync("/api/location", location);
    Console.WriteLine($"Sent: {location.Name} at {location.Latitude}, {location.Longitude}");
    await Task.Delay(3000);
}
