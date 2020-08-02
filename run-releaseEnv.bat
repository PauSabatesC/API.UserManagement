@ECHO OFF
docker rm UserManagement-apiRest UserManagement-sqlDB UserManagement-redis UserManagement-apiRestDev UserManagement-sqlDBDev UserManagement-redisDev
docker-compose -f docker-compose-Prod.yml build
docker-compose -f docker-compose-Prod.yml up