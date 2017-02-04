IF not exist cache (mkdir cache)
IF not exist cache\jade (mkdir cache\jade)
IF not exist log (mkdir log)

call composer install
cd web
php -S localhost:8088