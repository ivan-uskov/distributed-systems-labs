<?php declare(strict_types=1);

namespace Utils;

class ArrayUtils
{
    public static function get(array $arr, $key, $default = null)
    {
        return isset($arr[$key]) ? $arr[$key] : $default;
    }
}