<?php declare(strict_types=1);

namespace Services;

use Model\Note;

class StorageService
{
    /**
     * @param string $noteText
     * @return string
     */
    public function save(string $noteText): string
    {
        return 'id';
    }

    public function get(string $id): ?Note
    {
        return new Note("Hello");
    }
}