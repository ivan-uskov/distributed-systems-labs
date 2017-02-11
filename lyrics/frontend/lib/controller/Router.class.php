<?php declare(strict_types=1);

namespace Controller;

use Config\Routing;
use Utils\ArrayUtils;
use Web\Request;

class Router
{
    private $patterns;

    public function getAction(Request $request): Action
    {
        $uri        = $request->getUri();
        $actionName = Routing::DEFAULT;
        foreach ($this->getPatterns() as [$route, $pattern])
        {
            if (preg_match($pattern, $uri, $matches))
            {
                $actionName = Routing::ROUTES[$route];
                foreach ($this->getParameterKeys($route) as $key)
                {
                    $value = ArrayUtils::get($matches, $key);
                    $request->addParameter($key, $value);
                }

                break;
            }
        }

        return new $actionName($request);
    }

    private function getPatterns()
    {
        if (empty($this->patterns))
        {
            $this->patterns = array_map(function(string $route) {
                return [$route, $this->buildRegex($route)];
            }, array_keys(Routing::ROUTES));
        }

        return $this->patterns;
    }

    private function buildRegex(string $route)
    {
        if (array_key_exists($route, Routing::REQUIREMENTS))
        {
            foreach (Routing::REQUIREMENTS[$route] as $key => $value)
            {
                $route = str_replace("%{$key}%", "(?<{$key}>{$value})", $route);
            }
        }

        return '/^' . str_replace('/', '\/', $route) . '$/';
    }

    private function getParameterKeys(string $route): array
    {
        return array_keys(ArrayUtils::get(Routing::REQUIREMENTS, $route, []));
    }
}