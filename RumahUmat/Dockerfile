FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base

WORKDIR /app

COPY ./RestAPI .

EXPOSE 2121

ENV ASPNETCORE_URLS=http://+:2121

ENTRYPOINT ["dotnet", "RumahUmat.dll"]
