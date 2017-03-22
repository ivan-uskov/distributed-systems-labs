namespace DataAccessLayer
{
    public class Storage
    {
        public static string get(string key)
        {
            return RedisClient.Storage.get(key);
        }

        public static void store(string key, byte[] value)
        {
            RedisClient.Storage.store(key, value);
        }
    }
}