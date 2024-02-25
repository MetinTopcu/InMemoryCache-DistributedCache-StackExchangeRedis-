using StackExchange.Redis;

namespace StackExchangeRedis.Services
{
    public class RedisServices
    {

        private readonly string _redisHost;
        private readonly int _redisPort;
        private ConnectionMultiplexer _redis; // Redis Server ile haberleşir
        public IDatabase db { get; set; } // Redis databasesi
        public RedisServices(string host, int port) //Apsetting deki datayı alabilmek için IConfiguration 
        {
            _redisHost = host;
            _redisPort = port;
        }

        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";

            _redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDb(int db) // Redis deki 16 db den birini almak için
        {
            return _redis.GetDatabase(db);
        }
    }
}
