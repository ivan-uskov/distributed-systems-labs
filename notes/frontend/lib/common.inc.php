<?php declare(strict_types=1);

require_once '../vendor/autoload.php';
require_once 'config/Config.class.php';
require_once 'config/Routing.class.php';

require_once 'view/Renderer.class.php';

require_once 'utils/ArrayUtils.class.php';
require_once 'utils/Logger.class.php';

require_once 'web/Request.class.php';

require_once 'controller/Router.class.php';
require_once 'controller/Action.class.php';
require_once 'controller/Application.class.php';

require_once 'service/StorageService.class.php';

require_once 'model/Note.class.php';

require_once 'actions/CreateNoteAction.class.php';
require_once 'actions/ShowNoteAction.class.php';
require_once 'actions/SaveNoteAction.class.php';

