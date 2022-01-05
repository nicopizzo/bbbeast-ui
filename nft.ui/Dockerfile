FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nft.ui/nft.ui.csproj", "nft.ui/"]
COPY ["nft.contract.encoding/nft.contract.csproj", "nft.contract.encoding/"]
RUN dotnet restore "nft.ui/nft.ui.csproj"
COPY . .
WORKDIR "/src/nft.ui"
RUN dotnet build "nft.ui.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "nft.ui.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nft.ui/nginx.conf /etc/nginx/nginx.conf