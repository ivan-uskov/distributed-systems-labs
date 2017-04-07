<?php declare(strict_types=1);

namespace Controller;

use Config\Routing;

use Web\Request;
use View\Renderer;

abstract class Action
{
    /** @var Renderer */
    private $renderer;
    /** @var Request */
    private $request;

    final public function __construct(Request $request)
    {
        $this->request = $request;
        $this->renderer = new Renderer();
    }

    final public function execute(): void
    {
        $this->doExecute($this->request);
    }

    final protected function render(string $templateName, array $options = []): void
    {
        $this->renderer->render($templateName, $options);
    }

    final protected function redirect($uri)
    {
        header("Location: {$uri}");
        die();
    }

    final protected function getUrl(string $key, array $params = [])
    {
        return Routing::buildUrl($key, $params);
    }

    abstract protected function doExecute(Request $request): void;
}