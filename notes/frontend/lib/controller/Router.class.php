<?php declare(strict_types=1);

namespace Controller;

use Config\Routing;
use \Utils\Request;

class Router
{
    public function getAction(Request $request): Action
    {
        $uri = $request->getUri();
        if (array_key_exists($uri, Routing::ROUTES))
        {
            $actionName = Routing::ROUTES[$uri];
        }
        else
        {
            $actionName = Routing::DEFAULT;
        }

        return new $actionName($request);
    }
}