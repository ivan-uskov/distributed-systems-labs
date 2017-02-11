<?php declare(strict_types=1);

namespace Config;

class Routing
{
    public const INDEX = '/';
    public const SHOW_LYRIC = '/show-lyric/%lyricId%';
    public const SAVE_LYRIC = '/save-lyric';

    public const ROUTES = [
        self::SHOW_LYRIC => '\Actions\ShowLyricAction',
        self::SAVE_LYRIC => '\Actions\SaveLyricAction'
    ];

    public const REQUIREMENTS = [
        self::SHOW_LYRIC => [
            'lyricId' => '[0-9a-zA-Z-]+'
        ]
    ];

    public const DEFAULT = '\Actions\IndexAction';

    public static function buildUrl($url, array $vars)
    {
        foreach ($vars as $key => $value)
        {
            $url = str_replace("%{$key}%", $value, $url);
        }

        return $url;
    }
}