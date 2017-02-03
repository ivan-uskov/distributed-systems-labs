<?php declare(strict_types=1);

namespace Controller;

use Utils\Request;
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

    abstract protected function doExecute(Request $request): void;
}