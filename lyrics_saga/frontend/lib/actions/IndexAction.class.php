<?php declare(strict_types=1);

namespace Actions;

use Config\Routing;
use Controller\Action;
use Web\Request;

class IndexAction extends Action
{
    protected function doExecute(Request $request): void
    {
        $this->render('index', ['formAction' => $this->getUrl(Routing::SAVE_LYRIC)]);
    }
}