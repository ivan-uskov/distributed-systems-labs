<?php declare(strict_types=1);

namespace Services;

use Config\Config;

use GuzzleHttp\Client;
use GuzzleHttp\RequestOptions;
use \Psr\Http\Message\ResponseInterface;

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
        $response = $this->postRequest(
            Config::SAVE_LYRIC_URL,
            [RequestOptions::JSON => ['text' => $text]]
        );

        if ($response && $response->getStatusCode() == 200)
        {
            $responseData = (array) json_decode((string) $response->getBody()->getContents(), true);
            return (string) ArrayUtils::get($responseData, 'id');
        }

        return '';
    }

    public function get(string $id): ?Lyric
    {
        $response = $this->postRequest(Config::GET_LYRIC_URL, [RequestOptions::JSON => ['id' => $id]]);
        $text = null;

        if ($response && $response->getStatusCode() == 200)
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
        $response = $this->getRequest(Config::GET_STATISTICS_URL);
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

    private function getRequest($url): ?ResponseInterface
    {
        return $this->createRequest(function(Client $client) use ($url) {
            return $client->get($url);
        });
    }

    private function postRequest($url, $urlParams): ?ResponseInterface
    {
        return $this->createRequest(function(Client $client) use ($url, $urlParams) {
            return $client->post($url, $urlParams);
        });
    }

    private function createRequest(callable $maker): ?ResponseInterface
    {
        $client = new Client();
        try
        {
            $response = $maker($client);
        }
        catch (\Exception $e)
        {
            $response = null;
        }

        return $response;
    }
}