PG_CONTAINER_NAME ?= mass-postgres
PG_IMAGE ?= postgres:15.15-trixie
PG_USER ?= guest
PG_PASSWORD ?= guest
PG_DATABASE ?= mass
PG_PORT ?= 5432

export PG_CONTAINER_NAME PG_IMAGE PG_USER PG_PASSWORD PG_DATABASE PG_PORT

.PHONY: db-up db-iup db-down postgres migrate-up


db-up: migrate-up
	@test -f local-user-seed.sql || { echo "Missing local-user-seed.sql; restore it from your private backup." >&2; exit 1; }
	@docker exec -i "$$PG_CONTAINER_NAME" psql -v ON_ERROR_STOP=1 -1 -U "$$PG_USER" -d "$$PG_DATABASE" < local-user-seed.sql

db-down:
	@set -eu; \
	container="$$(docker ps -a --filter "name=^/$$PG_CONTAINER_NAME$$" --format '{{.ID}}')"; \
	if [ -z "$$container" ]; then \
		echo "Container $$PG_CONTAINER_NAME does not exist."; \
		exit 0; \
	fi; \
	managed="$$(docker inspect --format '{{index .Config.Labels "mass.local-postgres"}}' "$$container")"; \
	if [ "$$managed" != "true" ]; then \
		echo "Container $$PG_CONTAINER_NAME is not managed by this Makefile." >&2; \
		exit 1; \
	fi; \
	docker stop "$$container" >/dev/null; \
	echo "Stopped $$PG_CONTAINER_NAME."

postgres:
	@set -eu; \
	container="$$(docker ps -a --filter "name=^/$$PG_CONTAINER_NAME$$" --format '{{.ID}}')"; \
	if [ -n "$$container" ]; then \
		managed="$$(docker inspect --format '{{index .Config.Labels "mass.local-postgres"}}' "$$container")"; \
		if [ "$$managed" != "true" ]; then \
			echo "Container $$PG_CONTAINER_NAME already exists and is not managed by this Makefile." >&2; \
			exit 1; \
		fi; \
		config="$$(docker inspect --format '{{range .Config.Env}}{{println .}}{{end}}' "$$container")"; \
		for setting in "POSTGRES_USER=$$PG_USER" "POSTGRES_PASSWORD=$$PG_PASSWORD" "POSTGRES_DB=$$PG_DATABASE"; do \
			if ! printf '%s\n' "$$config" | grep -Fxq -- "$$setting"; then \
				echo "Container $$PG_CONTAINER_NAME has different PostgreSQL settings; use its original settings or a new container name." >&2; \
				exit 1; \
			fi; \
		done; \
		binding="$$(docker inspect --format '{{range (index .HostConfig.PortBindings "5432/tcp")}}{{.HostIp}}:{{.HostPort}}{{end}}' "$$container")"; \
		if [ "$$binding" != "127.0.0.1:$$PG_PORT" ]; then \
			echo "Container $$PG_CONTAINER_NAME has a different port mapping; use its original port or a new container name." >&2; \
			exit 1; \
		fi; \
		docker start "$$container" >/dev/null; \
	else \
		POSTGRES_PASSWORD="$$PG_PASSWORD" docker run -d \
			--name "$$PG_CONTAINER_NAME" \
			--label mass.local-postgres=true \
			--restart unless-stopped \
			-e POSTGRES_USER="$$PG_USER" -e POSTGRES_PASSWORD \
			-e POSTGRES_DB="$$PG_DATABASE" \
			-p "127.0.0.1:$$PG_PORT:5432" "$$PG_IMAGE" >/dev/null; \
	fi; \
	echo "Waiting for PostgreSQL..."; \
	ready=false; \
	for attempt in $$(seq 1 60); do \
		if docker exec "$$PG_CONTAINER_NAME" pg_isready -t 1 -h 127.0.0.1 -U "$$PG_USER" -d "$$PG_DATABASE" >/dev/null 2>&1; then \
			ready=true; \
			break; \
		fi; \
		sleep 1; \
	done; \
	if [ "$$ready" != true ]; then \
		echo "PostgreSQL did not become ready after 60 attempts." >&2; \
		exit 1; \
	fi

migrate-up: postgres
	@DB_CONNECTION="Host=127.0.0.1;Port=$$PG_PORT;Database=$$PG_DATABASE;Username=$$PG_USER;Password=$$PG_PASSWORD" \
		dotnet ef database update --project UI/mass.csproj
