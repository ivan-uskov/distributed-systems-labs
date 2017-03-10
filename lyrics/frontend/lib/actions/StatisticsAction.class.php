<?php declare(strict_types=1);

namespace Actions;

use Controller\Action;
use Web\Request;

use Services\StorageService;

class StatisticsAction extends Action
{
    protected function doExecute(Request $request): void
    {
        $storage = new StorageService();
        $stats = $storage->getStatistics();
        $this->render('statistics', ['stats' => $stats]);
    }

}