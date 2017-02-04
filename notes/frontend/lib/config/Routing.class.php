<?php declare(strict_types=1);

namespace Config;

class Routing
{
    public const CREATE_NOTE = '/';
    public const SHOW_NOTE = '/show-note/%noteId%';
    public const SAVE_NOTE = '/save-note';

    public const ROUTES = [
        self::SHOW_NOTE => '\Actions\ShowNoteAction',
        self::SAVE_NOTE => '\Actions\SaveNoteAction'
    ];

    public const REQUIREMENTS = [
        self::SHOW_NOTE => [
            'noteId' => '[0-9a-zA-Z-]+'
        ]
    ];

    public const DEFAULT = '\Actions\CreateNoteAction';

    public static function buildUrl($url, array $vars)
    {
        foreach ($vars as $key => $value)
        {
            $url = str_replace("%{$key}%", $value, $url);
        }

        return $url;
    }
}