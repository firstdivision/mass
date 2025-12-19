-include .env
export

PG_CONTAINER_NAME=localdb
PG_USER=guest
PG_PASSWORD=guest
PG_PORT=5432

postgres:
	docker network create mass-network || true
	docker run --network mass-network --name $(PG_CONTAINER_NAME) -e POSTGRES_USER=$(PG_USER) -e POSTGRES_PASSWORD=$(PG_PASSWORD) -d --restart unless-stopped -p $(PG_PORT):5432 postgres:15.15-trixie

migration:
	dotnet ef migrations add $(name) --output-dir Data/Migrations

migration-sql:
	dotnet ef migrations script --output migrate.sql --idempotent

watch:
	bash ./dotnet-watch.sh