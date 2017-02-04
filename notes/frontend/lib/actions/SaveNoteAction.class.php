<?php declare(strict_types=1);

namespace Actions;

use \Controller\Action;
use Services\StorageService;
use Config\Routing;
use Web\Request;

class SaveNoteAction extends Action
{
    protected function doExecute(Request $request): void
    {
        $text = (string) $request->getParameter('note_text');
        if (!empty($text))
        {
            $storage = new StorageService();
            $noteId = $storage->save($text);
            $url = $this->getUrl(Routing::SHOW_NOTE, ['noteId' => $noteId]);
            $this->redirect($url);
        }
        else
        {
            $this->redirect(Routing::CREATE_NOTE);
        }
    }
}