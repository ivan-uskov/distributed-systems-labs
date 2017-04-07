<?php declare(strict_types=1);

namespace Actions;

use Controller\Action;
use Services\StorageService;
use Config\Routing;
use Web\Request;

class SaveLyricAction extends Action
{
    protected function doExecute(Request $request): void
    {
        $text = (string) $request->getParameter('lyric_text');
        if (!empty($text))
        {
            $storage = new StorageService();
            $lyricId = $storage->save($text);
            $url = $this->getUrl(Routing::SHOW_LYRIC, ['lyricId' => $lyricId]);
            $this->redirect($url);
        }
        else
        {
            $this->redirect(Routing::INDEX);
        }
    }
}