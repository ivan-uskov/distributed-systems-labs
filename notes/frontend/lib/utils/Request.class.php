<?php declare(strict_types=1);

namespace Utils;

class Request
{
    /** @var string */
    private $uri;

    /**
     * @param string $uri
     */
    public function setUri(string $uri): void
    {
        $this->uri = $uri;
    }

    /**
     * @return string
     */
    public function getUri(): string
    {
        return $this->uri;
    }
}