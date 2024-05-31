Remo Torn
retorn
223199IADB


dotnet ef migrations --project App.DAL.EF --startup-project WebApp add Initial
dotnet ef database   --project App.DAL.EF --startup-project WebApp update
dotnet ef database   --project App.DAL.EF --startup-project WebApp drop



cd WebApp
dotnet aspnet-codegenerator controller -name GamesController        -actions -m  App.Domain.Game        -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
# use area
dotnet aspnet-codegenerator controller -name GamesController        -actions -m  App.Domain.Game        -dc AppDbContext -outDir Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
cd ..


Docker:
docker buildx build --progress=plain --force-rm --push -t akaver/webapp:latest .

docker buildx build --force-rm -t retorn0/webapp:latest --push . 