# TodoService
  
#### EF Core. Add migrations:
``` 
# dotnet ef migrations add Migration0001_Init -p ..\Todo.DAL\
```

#### Ef Core. Update database:
``` 
PS> $env:ASPNETCORE_ENVIRONMENT = 'Development'
PS> dotnet ef database update -p ..\Todo.DAL\
```
