# CRYPTUNICS

![logo](https://cryptunicsst.z6.web.core.windows.net/assets/logo.png)

An application that gives you the current quotes for selected crypto currencies in a number of most used fiat currencies. 

## What's in a name?

Crypto Quotes -> Crypto Coats -> Crypto Tunics -> cryptunics

## Demo

You can find a live demo at [cryptunics demo](https://cryptunicsst.z6.web.core.windows.net/)

## Running on local machine

### Building and running the API

The latest .NET Core runtime is required for running the API from source. Download it from [here](https://dotnet.microsoft.com/en-us/download).

Before running the below steps, you should add the API keys for the ExchangeRates and CoinMarketCap APIs in the appsettings.json or in the secrets.json files.

Run the following steps from the terminal to build, test and run the API and then view the API explorer UI. 

```
dotnet build .\api\Cryptunics.sln
dotnet test .\api\Cryptunics.sln --collect "Code Coverage"
dotnet run --project .\api\src\Cryptunics.Web\Cryptunics.Web.csproj
start https://localhost:7209/swagger/index.html
```

### Building and running the UI

Run the following steps from the terminal to build, test and run the UI.

```
cd .\ui\
npm install
npm run test
npm run start
start http://localhost:4200/
```