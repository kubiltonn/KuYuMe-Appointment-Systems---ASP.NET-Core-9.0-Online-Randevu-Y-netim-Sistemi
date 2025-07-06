FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AppointmentSystem/AppointmentSystem.csproj", "AppointmentSystem/"]
RUN dotnet restore "AppointmentSystem/AppointmentSystem.csproj"
COPY . .
WORKDIR "/src/AppointmentSystem"
RUN dotnet build "AppointmentSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppointmentSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppointmentSystem.dll"] 