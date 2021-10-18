FROM mcr.microsoft.com/dotnet/aspnet:2.1

MAINTAINER Stanislav Nekrasov <snekrasov@ussc.ru, pitchcontrol@yandex.ru>
EXPOSE 5000

WORKDIR /app
COPY ./out ./
ENTRYPOINT ["dotnet", "Agency.Web.dll"]
