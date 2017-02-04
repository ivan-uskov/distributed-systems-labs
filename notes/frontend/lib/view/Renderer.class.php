<?php declare(strict_types=1);

namespace View;

use Config\Config;
use Jade\Jade;

class Renderer
{
    private const JADE_CONFIG = [
        'prettyprint' => true,
        'extension'   => Config::TEMPLATE_EXT,
        'basedir'     => Config::TEMPLATES_DIR,
        //'cache'       => Config::JADE_CACHE_DIR
    ];

    private $pugClient;

    public function __construct()
    {
        $this->pugClient = new Jade(self::JADE_CONFIG);
    }

    public function render(string $templateName, array $options): void
    {
        $fileName = $this->prepareTemplatePath($templateName);
        $fileData = file_get_contents($fileName);
        echo $this->pugClient->render($fileData, $fileName, $options);
    }

    private function prepareTemplatePath(string $templateName): string
    {
        return Config::TEMPLATES_DIR . $templateName . Config::TEMPLATE_EXT;
    }
}