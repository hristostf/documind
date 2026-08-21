api:
	dotnet run --project apps/api/src/DocuMind.Api

api-watch:
	dotnet watch --project apps/api/src/DocuMind.Api

api-build:
	dotnet build apps/api
db-up:
	docker compose up -d

db-down:
	docker compose down

web:
	cd apps/web && npm run dev


migration:
	dotnet ef migrations add $(name) \
		--project apps/api/src/DocuMind.Infrastructure \
		--startup-project apps/api/src/DocuMind.Api

db-update:
	dotnet ef database update \
		--project apps/api/src/DocuMind.Infrastructure \
		--startup-project apps/api/src/DocuMind.Api