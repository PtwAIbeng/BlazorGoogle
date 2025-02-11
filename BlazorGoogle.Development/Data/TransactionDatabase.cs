using Microsoft.Extensions.Configuration;

namespace BlazorGoogle.Development.Data
{
    public class TransactionDatabase : IDatabaseSettings
    {
        public TransactionDatabase()
        {
            // Build configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            ConnectionString = configuration.GetSection("TransactionDatabase:ConnectionString").Value;
            Timeout = int.Parse(configuration.GetSection("TransactionDatabase:Timeout").Value);
        }

        public string ConnectionString { get; set; }

        public int Timeout { get; set; }
    }
}
