# AuthenticationApp

JSON Web Token authentication template for ASP.NET Core applications. It follows a layered architecture.

This specific project uses SQL Server as database and EF Core as ORM, but that details are isolated in the repository layer. If you want to use a different relational database, just change DB connection string and add necessary EF Core package. If you use a non-relational database, write a new repository layer.

## TODO
* Handle the exceptions related to the database in the repository layer. 
