
using Microsoft.AspNetCore.SignalR.Client;
using RevSignalRClient.Models;
using System.Configuration;
using System.Text.Json;

string hubUrl = ConfigurationManager.AppSettings.Get("LOGGING_HUB_URL");
HubConnection connection = new HubConnectionBuilder()
                           .WithUrl(hubUrl)
                           .WithAutomaticReconnect()
                           .Build();

connection.Closed += async (error) =>
{
    await Task.Delay(new Random().Next(0, 5) * 1000);
    await connection.StartAsync();
};

connection.On<string>("TakeLog", (message) =>
{
    NeuronetInformation? ni = JsonSerializer.Deserialize<NeuronetInformation>(message);
    Console.WriteLine($"Id: {ni.Id}\n" +
                      $"Gender: {ni.Gender}\n" +
                      $"Age: {ni.Age}\n" +
                      $"Mood: {ni.Mood}");
    Console.WriteLine();
});

try
{
    await connection.StartAsync();
    Console.WriteLine("Connection to hub has been established!");
    while (true) { }
}
catch(Exception)
{
    Console.WriteLine("Can't connect to the hub");
}



