<?php declare(strict_types=1);

namespace Actions;

use Config\Routing;
use Controller\Action;
use Web\Request;

use Services\StorageService;

class ShowNoteAction extends Action
{
    protected function doExecute(Request $request): void
    {
        $noteId = (string) $request->getParameter('noteId');

        $storage = new StorageService();
        $note = $storage->get($noteId);

        if (!$note)
        {
            $this->redirect($this->getUrl(Routing::CREATE_NOTE));
        }

        $this->render('show_note', ['noteText' => $note->getText()]);
    }
}