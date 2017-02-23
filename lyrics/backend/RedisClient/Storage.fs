namespace RedisClient

open System;
open System.Text;
open ServiceStack.Redis

module Storage =
    let store key value = 
        let redis = new RedisClient(Config.REDIS_HOST)
        redis.Set(key, value)
