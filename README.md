# Travel Express
Travel Express is an application for sharing cars between users.
Register your travels when you are driving, and get paid by other users with you.

# Setup the application
## Requirements
- PostgreSQL (v9 at least)
- C# with ASP.NET Core 3

## Setup database (PostgreSQL)
1. Install PostgreSQL (v9 at least)
2. Create a database called "TravelExpress"
3. If the database does not belong to Postgres user or if Postgres user has a password process as following then update the connection string in `appsettings.json`
4. Run the SQL scripts `Database/Scripts/tables.sql` to create tables

## Setup the application (C#)
1. Run `dotnet restore`
2. Run the application using dotnet command or VS directly
