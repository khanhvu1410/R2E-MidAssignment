# Library Management System

## Overview

A full-stack web application for managing books, users, and borrowing transactions with role-based access control (Admin & User), secure JWT authentication, and an intuitive interface.

## Tech stack

- Backend: ASP.NET Core WebAPI
- Frontend: React
- Authentication: Token-based (JWT)
- Database: SQL Server
- Testing: NUnit for unit tests

## Features

### Authentication & Authorization

1. Login functionality
2. Role-based access: Super User vs Normal User
3. Token-based JWT security

### Normal User

1. Borrow Books:

- Request up to 5 books per request
- Limit: 3 borrowing requests per month
- Status tracking: Approved / Rejected / Waiting

2. View Borrowed Books:

- See all books borrowed with statuses

### Super User

1. Manage Categories:

- Create, update, delete, and view categories

2. Manage Books:

- Create, update, delete, and view books

3. Manage Borrowing Requests:

- Approve or reject user requests
- View all borrowing history

## Prerequisites

- .NET 8.0 SDK
- Node.js & npm
- SQL Server
- Visual Studio 2022 or Visual Studio Code

## How to run

### Backend

1. Clone the repository:

```sh
git clone https://github.com/khanhvu1410/rookie-mid-assignment.git
cd rookie-mid-assignment/LibraryManagement
```

2. Update appsettings.json with your SQL Server connection in API project:

```json
"ConnectionStrings": {
  "LibraryManagementDBConnection": "Server=YourSQLServerName;Database=LibraryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. Apply database migrations:

```sh
dotnet ef database update --project LibraryManagement.Persistence --startup-project LibraryManagement.API
```

4. Create .env file in API project:

```sh
cd LibraryManagement.API
cp .env.example .env
```

5. Edit .env with your local values:

```sh
JWT_SECRET=your_secure_development_secret
JWT_ISSUER=https://localhost:7295
JWT_AUDIENCE=http://localhost:3000
JWT_EXPIRY_MINUTES=120
```

6. Run the backend:

```sh
cd LibraryManagement.API
dotnet run
```

7. Run unit tests:

```sh
dotnet test
```

### Frontend

1. Clone the repository:

```sh
git clone https://github.com/khanhvu1410/rookie-mid-assignment.git
cd rookie-mid-assignment/library-management-client
```

2. Install dependencies and run:

```sh
npm install
npm start
```
