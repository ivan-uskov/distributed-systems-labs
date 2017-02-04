<?php declare(strict_types=1);

namespace Utils;

use \Config\Config;

class Logger
{
    public static function debug(?string $message): void
    {
        self::log('debug', $message);
    }

    public static function debugExport($var): void
    {
        self::log('debug', var_export($var, true));
    }

    public static function log(string $fileName, ?string $message): void
    {
        file_put_contents(
            self::prepareFilePath($fileName),
            self::prepareMessage($message),
            FILE_APPEND
        );
    }

    private static function prepareMessage(?string $message): string
    {
        return "{$message}\n";
    }

    private static function prepareFilePath(string $fileName): string
    {
        return Config::LOG_DIR . $fileName . Config::LOG_EXT;
    }
}