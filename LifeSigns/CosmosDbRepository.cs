using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Configuration;

namespace LifeSigns
{
    internal class CosmosDbRepository
    {
        string? databaseName;

        string? containerName;

        string? accountEndpoint;

        string? key;
  
        public Container? Container { get; set; }

        private CosmosClient? client;

        public CosmosDbRepository()
        {
            databaseName = ConfigurationManager.AppSettings["databaseName"];

            containerName = ConfigurationManager.AppSettings["containername"];

            accountEndpoint = ConfigurationManager.AppSettings["accountendpoint"];

            key = ConfigurationManager.AppSettings["key"];

            client = new CosmosClient(accountEndpoint, key);
        }


        public async Task Init()
        {
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);

            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            Container = client.GetContainer(databaseName, containerName);

            return;
        }

        public Task UpsertItemAsync(Person person)
        {
            return Container.UpsertItemAsync<Person>(person, new PartitionKey(person.Id));
        }

        public async Task<Person> ReadItemAsync(string id)
        {
            return await Container.ReadItemAsync<Person>(id, new PartitionKey(id));
        }



    }
}
