<?php declare(strict_types=1);

namespace Model;

class Lyric
{
    /** @var string */
    private $text;
    /** @var $id */
    private $id;

    public function __construct(string $text, ?string $id = null)
    {
        $this->id = $id;
        $this->text = $text;
    }

    public function getId(): string
    {
        return $this->id;
    }

    public function getText(): string
    {
        return $this->text;
    }
}