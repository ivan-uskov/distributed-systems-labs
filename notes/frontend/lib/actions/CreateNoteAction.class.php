<?php declare(strict_types=1);

namespace Actions;

use \Controller\Action;
use \Utils\Request;

class CreateNoteAction extends Action
{
    protected function doExecute(Request $request): void
    {
        $this->render('create_note');
    }
}