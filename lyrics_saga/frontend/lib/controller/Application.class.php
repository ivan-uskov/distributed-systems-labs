<?php declare(strict_types=1);

namespace Controller;

use Web\Request;
use Utils\ArrayUtils;

class Application
{
    private $router;

    public function __construct()
    {
        $this->router = new Router();
    }

    public function run(): void
    {
        try
        {
            $request = $this->buildRequest();
            $action = $this->router->getAction($request);
            $action->execute();
        }
        catch (\Throwable $e)
        {
            echo "{$e->getMessage()}\n";
            echo "{$e->getFile()} : {$e->getLine()}\n";
            echo "{$e->getTraceAsString()}\n";
        }
    }

    private function buildRequest(): Request
    {
        $uri = (string) ArrayUtils::get($_SERVER, 'REQUEST_URI');
        $uri = strtok($uri, '?');

        $request = new Request();
        $request->setUri($uri);
        $request->setPostVars($_POST);
        return $request;
    }
}