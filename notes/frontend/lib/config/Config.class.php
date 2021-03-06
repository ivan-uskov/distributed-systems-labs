<?php declare(strict_types=1);

namespace Config;

class Config
{
    public const ROOT_DIR = __DIR__ . '/../../';
    public const JADE_CACHE_DIR = self::ROOT_DIR . '/cache/jade';
    public const TEMPLATES_DIR = self::ROOT_DIR . 'templates/';
    public const LOG_DIR = self::ROOT_DIR . 'log/';
    public const DIR_SEPARATOR = '/';
    public const LOG_EXT = '.log';
    public const TEMPLATE_EXT  = '.jade';

    public const SAVE_NOTE_URL = 'http://localhost:8080/save-note';
    public const REDIS_HOST = 'localhost';
    public const REDIS_PORT = 6479;
}