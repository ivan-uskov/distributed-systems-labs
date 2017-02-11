<?php declare(strict_types=1);

namespace Web;

use Utils\ArrayUtils;

class Request
{
    /** @var string */
    private $uri;

    /** @var array */
    private $postVars = [];

    /** @var array */
    private $parameters = [];

    /**
     * @return array
     */
    public function getPostVars(): array
    {
        return $this->postVars;
    }

    public function getParameter($key, $default = null)
    {
        return ArrayUtils::get($this->parameters, $key, $default) ?:
               ArrayUtils::get($this->postVars, $key, $default);
    }

    public function addParameter($key, $value)
    {
        $this->parameters[$key] = $value;
    }

    /**
     * @param array $postVars
     */
    public function setPostVars(array $postVars)
    {
        $this->postVars = $postVars;
    }

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