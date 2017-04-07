namespace DataAccessLayer
{
    public class Storage
    {
        public static string get(string key)
        {
            var shardId = ShardMap.getShardIdForStoredContent(key);
            var storeKey = prepareStoreKey(key, shardId);
            return RedisClient.Storage.get(storeKey);
        }

        public static void store(string key, byte[] value)
        {
            var shardId = ShardMap.assignContentId(key);
            var storeKey = prepareStoreKey(key, shardId);
            RedisClient.Storage.store(storeKey, value);
        }

        private static string prepareStoreKey(string baseKey, int shardId)
        {
            return shardId + ":shard-" + baseKey;
        }
    }
}