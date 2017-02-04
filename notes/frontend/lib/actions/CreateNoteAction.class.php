<?php declare(strict_types=1);

namespace Actions;

use Config\Routing;
use Controller\Action;
use Web\Request;

class CreateNoteAction extends Action
{
    protected function doExecute(Request $request): void
    {
        $this->render('create_note', ['formAction' => $this->getUrl(Routing::SAVE_NOTE)]);
    }
}