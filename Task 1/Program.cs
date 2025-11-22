using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Task_1;

class Program
{
    static void Main()
    {

        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

        string connectionString = config.GetConnectionString("DefaultConnection");
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


        string base64UrlGuid = "bP0yS_RJTI6tIzY5q_3eEA";
        Guid guid = Processor.Base64UrlToGuid(base64UrlGuid);
        Console.WriteLine(guid);
    }
}
