### **API To-Do List Documentation**


#### **Technologies Used**
- **.NET Core / ASP.NET Core**
- **Entity Framework Core (EF Core)**
- **Serilog**
- **Swagger/OpenAPI**
- **AutoMapper**
- **SQL Database**

#### **Architecture**
**Clean Architecture**:
- **Presentation Layer**
- **Application Layer**
- **Domain Layer**
- **Infrastructure Layer**

#### **Task Model**
The `Task` model includes:
- **Id** (Guid)
- **Title** (string)
- **Description** (string)
- **IsCompleted** (bool)
- **DueDate** (DateTime)

#### **API Endpoints**
- **GET /api/tasks**
- **GET /api/tasks/{id}**
- **POST /api/tasks**
- **PUT /api/tasks/{id}**
- **DELETE /api/tasks/{id}**

#### **Input Validation**
- **Required fields** like `Title` and `Description` are validated using data annotations (e.g., `[Required]`).
- **Error Handling**: Returns appropriate error responses, such as **400 Bad Request** for invalid data or **404 Not Found** if the task does not exist.

#### **Concurrency**
The API uses **async/await** for asynchronous programming.

#### **Error Management and Status Codes**
- **200 OK**: Successful request (GET/POST/PUT).
- **201 Created**: Task successfully created.
- **400 Bad Request**: Invalid input data.
- **404 Not Found**: Task not found.
- **500 Internal Server Error**: Server-side error.

#### **Logging**
**Serilog** is used to log requests and errors for troubleshooting and monitoring.

#### **Testing**
The project includes automated tests using **XUnit**, covering:
- **Input Validation**: Ensuring required fields are provided.
- **API Tests**: Verifying correct behavior of API endpoints.
- **Error Tests**: Checking how the API handles invalid input or errors.

#### **Setup and Running**
1. Clone the project.
2. Set up a SQL database (SQLite or SQL Server).
3. Run the project:
   ```bash
   dotnet build
   dotnet run
   ```
4. Access Swagger UI at `/swagger` for testing the API.