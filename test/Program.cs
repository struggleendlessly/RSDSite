// See https://aka.ms/new-console-template for more information

using shared.DTO.Stripe;

using System.Text.Json;

var json = File.ReadAllText("H:\\Untitled-1.json");
var data = JsonSerializer.Deserialize<CustomerSubscriptionUpdated.Rootobject>(json);
Console.WriteLine("Hello, World!");

