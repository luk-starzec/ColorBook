# ColorBook
A Blazor app for creating and managing color schemes. It can work offline as a PWA or synchronize data with a remote server. 

## Running locally
### Client
In Client\ColorBook directory:

Start webpack:
```
npm run dev
```

Start app:
 ```
dotnet run
 ```
 Default url: http://localhost:5000

 ### Server
 In Server directory:

 Set values in .env file:
  - DATABASE_URL - MongoDB connection string
  - APP_PORT - same port number in ServerApiUrl in client project appsettings
  - API_SECRET - same as ServerApiSecret in client project appsettings

Start server:
```
npm run dev
```