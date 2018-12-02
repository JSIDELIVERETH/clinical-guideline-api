FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers

COPY  . ./
RUN dotnet restore cg.Api/


# Copy everything else and build
COPY . ./
RUN dotnet publish cg.Api/ -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build-env /app/cg.Api/out/ .
ENTRYPOINT ["dotnet", "cg.Api.dll"]
