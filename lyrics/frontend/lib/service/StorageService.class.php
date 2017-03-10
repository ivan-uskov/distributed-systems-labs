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
        $client = new Client();
        $response = $client->post(
            Config::GET_LYRIC_URL,
            [RequestOptions::JSON => ['id' => $id]]
        );

        $text = null;
        if ($response->getStatusCode() == 200)
        {
            $text = $response->getBody()->getContents();
            if (!empty($text))
            {
                $data = json_decode($text, true);
                $text = isset($data['text']) ? $data['text'] : null;
            }
        }

        return !empty($text) ? new Lyric($text, $id) : null;
    }

    public function getStatistics(): ?array
    {
        $client = new Client();
        $response = $client->get(Config::GET_STATISTICS_URL);

        $stats = null;
        if ($response->getStatusCode() == 200)
        {
            $text = $response->getBody()->getContents();

            if (!empty($text))
            {
                $stats = json_decode($text, true);
            }
        }

        return $stats;
    }
}