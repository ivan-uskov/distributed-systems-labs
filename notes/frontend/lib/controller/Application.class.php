<?php declare(strict_types=1);

namespace Controller;

use Web\Request;
use \Utils\ArrayUtils;

class Application
{
    private $router;

    public function __construct()
    {
        $this->router = new Router();
    }

    public function run(): void
    {
        $request = $this->buildRequest();
        $action = $this->router->getAction($request);
        $action->execute();
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