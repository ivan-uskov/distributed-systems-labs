<?php declare(strict_types=1);

namespace Services;

use Config\Config;

use GuzzleHttp\Client;
use GuzzleHttp\RequestOptions;

use Model\Lyric;
use Utils\ArrayUtils;

class StorageService
{
    /**
     * @param string $text
     * @return string
     */
    public function save(string $text): string
    {
        $client = new Client();
        $response = $client->post(
            Config::SAVE_LYRIC_URL,
            [RequestOptions::JSON => ['text' => $text]]
        );

        if ($response->getStatusCode() != 200)
        {
            return '';
        }

        $responseData = (array) json_decode((string) $response->getBody()->getContents(), true);

        return (string) ArrayUtils::get($responseData, 'id');
    }

    public function get(string $id): ?Lyric
    {
        if (class_exists('Redis'))
        {
            $redis = new \Redis(Config::REDIS_HOST, Config::REDIS_PORT);
            $text = $redis->get($id);
        }
        else
        {
            $text = exec('redis-cli get ' . escapeshellarg($id));
        }

        return !empty($text) ? new Lyric($text, $id) : null;
    }
}