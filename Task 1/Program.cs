using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Task_1;

class Program
{
    static void Main()
    {

        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

        string connectionString = config.GetConnectionString("DefaultConnection");
        Console.WriteLine("Task # 1: Four level categories tree.\n\n");
        Console.WriteLine("Show Categories tree by using Entity Framework");
        var sw1 = Stopwatch.StartNew();
        var efTree = Processor.BuildTreeEF();
        Processor.PrintTree(efTree);
        sw1.Stop();
        Console.WriteLine($"EF Time: {sw1.ElapsedMilliseconds} ms\n");

        Console.WriteLine("Show Categories tree by using Stored Procedure");
        var sw2 = Stopwatch.StartNew();
        var spTree = Processor.BuildTreeSP();
        Processor.PrintTree(spTree);
        sw2.Stop();
        Console.WriteLine($"SP Time: {sw2.ElapsedMilliseconds} ms\n");

        Console.WriteLine("Task # 2: Write memory allocation free base64 url string to guid converter.\n\n");
        string base64Url = "bP0yS_RJTI6tIzY5q_3eEA";
        Guid guid = Processor.Base64UrlToGuid(base64Url);
        Console.WriteLine($"Base64URL Input: {base64Url}");
        Console.WriteLine($"Base64URL Output:{guid}");
    }
}
