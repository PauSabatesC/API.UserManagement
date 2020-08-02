@ECHO OFF
docker rm UserManagement-apiRest UserManagement-sqlDB UserManagement-redis UserManagement-apiRestDev UserManagement-sqlDBDev UserManagement-redisDev
docker-compose -f docker-compose-debugEnv.yml build
docker-compose -f docker-compose-debugEnv.yml up