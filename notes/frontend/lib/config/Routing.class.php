<?php declare(strict_types=1);

namespace Config;

class Routing
{
    public const ROUTES = [
        '/show-note' => '\Actions\ShowNoteAction'
    ];

    public const DEFAULT = '\Actions\CreateNoteAction';
}