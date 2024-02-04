FROM mcr.microsoft.com/dotnet/aspnet:7.0.13-alpine3.18

RUN apk update && apk upgrade

WORKDIR /r-vocabular

COPY . .

CMD [ "dotnet", "r-vocabular.dll" ]
