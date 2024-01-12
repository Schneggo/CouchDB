using CouchDB.Driver;
using CouchDB.Driver.Extensions;
using CouchDB.Driver.Options;
using CouchDB.Driver.Query.Extensions;
using CouchDB.Model;

namespace CouchDB.Connection
{
    internal class UserContext : CouchContext
    {
        public CouchDatabase<Tiere> Tiere { get; set; }


        //Erstellen einer Fixen Connection
        protected override void OnConfiguring(CouchOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseEndpoint("http://localhost:5984/")
                .EnsureDatabaseExists()
                .DisableDocumentPluralization()
                .UseBasicAuthentication(username: "admin", password: "admin");
        }

        public async void GetRead()
        {
            await using var context = new UserContext();
            var skywalkers = await context.Tiere
                .Where(r =>
                    r.Name == "Hund"

                )
                .OrderByDescending(r => r.Name)
                .ThenByDescending(r => r.Alter)
                .Take(2).Select(
                    r => r.Name,
                    r => r.Alter).ToListAsync();

        }
    }
}
