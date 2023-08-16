# csharp-todo-list

## DB
### connection strings
for connecting the app to the DB, instead of:
```json
{
  // ...
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1234;Database=TodoListDB;User Id=the_user;Password=the_pwd"
  }
}
```

in the CLI, use for dev mode: 
```
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,the_port;Database=TodoListDB;User Id=the_user;Password=the_pwd"
```

### db deletion
```azure
USE master;
GO
ALTER DATABASE TodoListDB
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE TodoListDB;
GO
```
