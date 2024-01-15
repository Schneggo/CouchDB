using CouchDB.Connection;
using CouchDB.Driver.DatabaseApiMethodOptions;
using CouchDB.Driver.Extensions;
using CouchDB.Driver.Query.Extensions;
using CouchDB.Model;
using System.Runtime.InteropServices;


namespace CouchDB
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Läuft");

            //await DeleteData();

            //await CreateData();

            //await ChangeData();
            
            //var temp = await GetData();

            //Console.WriteLine(temp.Count());

            //foreach (var item in temp)
            //{
            //    Console.WriteLine("-------------------------");
            //    Console.WriteLine($"Tier:{item.Name}");
            //    Console.WriteLine($"Type:{item.Type}");
            //    Console.WriteLine($"Alter:{item.Alter}");
            //    Console.WriteLine($"Essen:{item.Essen}");

            //}

        }

        // -- Id vom Dokument muss bekannt sein
        // -- mit Find Async über die ID das Dokument erhalten
        // -- Mit Remove Async entfernen

        private static async Task DeleteData()
        {
            await using var context = new UserContext();

            var id = "8bfd4d4ca6d6d11d24a7c4491501021d";

            var find = await context.Tiere.FindAsync(id);

            await context.Tiere.RemoveAsync(find);
        }

        // -- Id vom Dokument muss bekannt sein
        // -- mit Find Async über die ID das Dokument erhalten
        // -- Mit Remove Async entfernen
        // -- Datei erneut mit geänderten Werten einfügen

        private static async Task ChangeData()
        {
            await using var context = new UserContext();

            var id = "8bfd4d4ca6d6d11d24a7c4491501021d";
            var find = await context.Tiere.FindAsync(id);

            var gorilla = new Tiere { Id= id, Alter = 10, Essen = "Banane", Name = "Gorilla", Type = "Tier" };
            
            context.Tiere.RemoveAsync(find);

            var gorilla2 = new Tiere { Id = id, Alter = 20, Essen = "Banane", Name = "Gorilla", Type = "Tier" };



            Console.WriteLine(find.Name);

            
            await context.Tiere.AddOrUpdateAsync(gorilla2);


        }

        // -- Erstellen einer Datei auf der Datenbank
        private static async Task CreateData()
        {
            //Verbindung herstellen
            await using var context = new UserContext();
            
            //Model erstellen und mit Daten füllen
            var animal = new Tiere { Name = "Kuh", Alter = 150, Essen = "Gras", Type = "Tier" };

            //Model beim Server hinzufügen
            animal = await context.Tiere.AddAsync(animal);
        }
        
        //-- Queryabfrage
        // Wichtig: Um die Daten Sortieren zu können muss ein Index erstellt werden
        // Der Index muss jedoch nur einmal erstell werden und wird automatisch auf dem Server gespeichert

        private static async Task<List<Tiere>> GetData()
        {
            await using var context = new UserContext();

            //Erstellen des Indexes mit allen Werten die für die Sortierung benötigt werden
            //Ansonsten wird eine Bad Request ausgegeben
            var index = await context.Tiere.CreateIndexAsync("animal_index", x => x.IndexBy(x => x.Name).ThenBy(x => x.Alter));

            //Ausgabe Sortieren
            var result = await context.Tiere
                .OrderBy(r => r.Name)
                .ThenBy(r => r.Alter)
                .Select(r => r.Name, r => r.Essen, r => r.Type, r => r.Alter).ToListAsync();

            return await Task.FromResult<List<Tiere>>(result);
        }
    }
}
