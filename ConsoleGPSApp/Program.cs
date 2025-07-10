using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static async Task Main()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7140");

        var osrmUrl = "https://router.project-osrm.org/route/v1/driving/106.8272,-6.1754;106.8133,-6.1352?overview=full&geometries=geojson&steps=true";
        var osrmResp = await client.GetStringAsync(osrmUrl);
        using var doc = JsonDocument.Parse(osrmResp);

        var coordsJson = doc.RootElement
          .GetProperty("routes")[0]
          .GetProperty("geometry")
          .GetProperty("coordinates");

        var routeCoords = new List<(double Lat, double Lng)>();
        foreach (var coord in coordsJson.EnumerateArray())
        {
            var lng = coord[0].GetDouble();
            var lat = coord[1].GetDouble();
            routeCoords.Add((lat, lng));
        }

        Console.WriteLine($"Route contains {routeCoords.Count} street-following waypoints.\n");

        foreach (var (lat, lng) in routeCoords)
        {
            var location = new { Name = "Truck-001", Latitude = lat, Longitude = lng };
            var res = await client.PostAsJsonAsync("/api/location", location);
            Console.WriteLine($"Sent: {location.Name} at {lat:F6}, {lng:F6}");
            await Task.Delay(3000);
        }
    }
}


//using System;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//class Program
//{
//    static async Task Main()
//    {
//        var client = new HttpClient();
//        client.BaseAddress = new Uri("https://localhost:7140");

//        var routeCoordinates = new List<(double Latitude, double Longitude)>
//        {
//            (-6.1754, 106.8272),
//            (-6.1742, 106.8263),
//            (-6.1734, 106.8258),
//            (-6.1722, 106.8251),
//            (-6.1707, 106.8247),
//            (-6.1693, 106.8244),
//            (-6.1676, 106.8240),
//            (-6.1658, 106.8235),
//            (-6.1642, 106.8231),
//            (-6.1625, 106.8227),
//            (-6.1609, 106.8225),
//            (-6.1593, 106.8223),
//            (-6.1577, 106.8218),
//            (-6.1563, 106.8215),
//            (-6.1547, 106.8210),
//            (-6.1531, 106.8206),
//            (-6.1516, 106.8203),
//            (-6.1500, 106.8199),
//            (-6.1485, 106.8195),
//            (-6.1470, 106.8190),
//            (-6.1455, 106.8185),
//            (-6.1441, 106.8181),
//            (-6.1426, 106.8177),
//            (-6.1410, 106.8173),
//            (-6.1396, 106.8168),
//            (-6.1381, 106.8164),
//            (-6.1367, 106.8158),
//            (-6.1352, 106.8133)
//        };

//        int i = 0;
//        while (true)
//        {
//            var coord = routeCoordinates[i];

//            var location = new
//            {
//                Name = "Truck-001",
//                Latitude = coord.Latitude,
//                Longitude = coord.Longitude
//            };

//            var res = await client.PostAsJsonAsync("/api/location", location);
//            Console.WriteLine($"Sent: {location.Name} at {location.Latitude}, {location.Longitude}");

//            i = (i + 1) % routeCoordinates.Count;
//            await Task.Delay(3000); // every 3 seconds
//        }
//    }
//}
