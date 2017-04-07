<?php declare(strict_types=1);

namespace Actions;

use Config\Routing;
use Controller\Action;
use Web\Request;

use Services\StorageService;

class ShowLyricAction extends Action
{
    protected function doExecute(Request $request): void
    {
        $lyricId = (string) $request->getParameter('lyricId');

        $storage = new StorageService();
        $lyric = $storage->get($lyricId);

        $this->render('show_lyric', ['noteText' => $lyric ? $lyric->getText() : '']);
    }
}