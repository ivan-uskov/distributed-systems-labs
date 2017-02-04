<?php declare(strict_types=1);

namespace Services;

use Config\Config;

use GuzzleHttp\Client;
use GuzzleHttp\RequestOptions;

use Model\Note;
use Utils\ArrayUtils;

class StorageService
{
    /**
     * @param string $noteText
     * @return string
     */
    public function save(string $noteText): string
    {
        $client = new Client();
        $response = $client->post(
            Config::SAVE_NOTE_URL,
            [RequestOptions::JSON => ['text' => $noteText]]
        );

        if ($response->getStatusCode() != 200)
        {
            return '';
        }

        $responseData = (array) json_decode((string) $response->getBody()->getContents(), true);

        return (string) ArrayUtils::get($responseData, 'id');
    }

    public function get(string $id): ?Note
    {
        if (class_exists('Redis'))
        {
            $redis = new \Redis(Config::REDIS_HOST, Config::REDIS_PORT);
            $noteText = $redis->get($id);
        }
        else
        {
            $noteText = exec('redis-cli get ' . escapeshellarg($id));
        }

        return !empty($noteText) ? new Note($noteText, $id) : null;
    }
}