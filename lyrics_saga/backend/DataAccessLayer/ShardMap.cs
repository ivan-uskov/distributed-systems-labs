using System.Collections.Generic;

namespace DataAccessLayer
{
    class ShardMap
    {
        private const int SHARD_MAX_SIZE = 100;
        private static int[] shards = new int[1000];
        private static Dictionary<string, int> contentIdMap = new Dictionary<string, int>();

        public static int getShardIdForStoredContent(string contentId)
        {
            return contentIdMap[contentId];
        }

        public static int assignContentId(string contentId)
        {
            int i = 0;
            for (; i < shards.Length; ++i)
            {
                if (shards[i] < SHARD_MAX_SIZE)
                {
                    break;
                }
            }

            assigndContentIdForShard(contentId, i);

            return i;
        }

        private static void assigndContentIdForShard(string contentId, int shardId)
        {
            ++shards[shardId];
            contentIdMap[contentId] = shardId;
        }
    }
}
