# 🌐 Web Programming Project

This is my **Web Programming Project**, developed as part of my Computer Engineering studies.  
It consists of an **ASP.NET MVC application** that consumes a **RESTful API**, implementing a clean architecture with services, DTOs, and Entity Framework Core.

---

## 🚀 Features
- Manage **Customers, Employees, Vehicles, and Car Wash Services**.
- **ASP.NET MVC** frontend with controllers and Razor views.
- **REST API** built with ASP.NET Core + EF Core.
- Data persistence with **SQL Server**.
- Error handling with `TempData` and HTTP status codes.
- Swagger documentation (POST/PUT only require IDs of related entities).

---

## 🛠️ Tech Stack
- **Languages:** C#, JavaScript, HTML5, CSS3
- **Frameworks:** ASP.NET MVC, ASP.NET Core Web API, Entity Framework Core
- **Database:** SQL Server
- **Tools:** Git, Postman, Swagger, Visual Studio, Azure (basic)

---

## 📂 Project Structure
/src
WebApp/ -> ASP.NET MVC frontend
Api/ -> RESTful API
Core/ -> Entities, DTOs, Interfaces
Infrastructure/ -> EF Core, Migrations, Repositories


---

## ⚙️ How to Run
1. Clone the repository:
   ```bash
   git clone https://github.com/YOUR_USERNAME/web-programming-project.git

    Configure the database connection string in:

Api/appsettings.json
WebApp/appsettings.json

Apply migrations:

dotnet ef database update

Run the API:

cd src/Api
dotnet run

Run the WebApp:

    cd src/WebApp
    dotnet run

The API will be available at: https://localhost:5001/swagger
The WebApp will be available at: https://localhost:5002

## 📸 Screenshots

### 🔐 API Documentation
![Swagger UI](assets/Screenshot 2025-08-25 at 21-54-44 Swagger UI.png)

### 🏠 Home Page
![Home Page](assets/Screenshot 2025-08-25 at 21-52-18 Inicio - Proyecto3.png)

### 👥 Customers
- List  
  ![Customers List](assets/Screenshot 2025-08-25 at 21-52-39 Listado de clientes - Proyecto3.png)  
- Edit  
  ![Edit Customer](assets/Screenshot 2025-08-25 at 21-53-24 Editar cliente - Proyecto3.png)

### 👨‍💼 Employees
- List  
  ![Employees List](assets/Screenshot 2025-08-25 at 21-52-50 Listado de empleados - Proyecto3.png)  
- Details  
  ![Employee Details](assets/Screenshot 2025-08-25 at 21-53-33 Detalles del empleado - Proyecto3.png)

### 🚗 Vehicles
- List  
  ![Vehicles List](assets/Screenshot 2025-08-25 at 21-52-57 Vehículos - Proyecto3.png)  
- Delete  
  ![Delete Vehicle](assets/Screenshot 2025-08-25 at 21-53-43 Eliminar vehículo - Proyecto3.png)

### 🚿 Car Wash Services
- Register  
  ![Register Car Wash](assets/Screenshot 2025-08-25 at 21-54-00 Registrar lavado - Proyecto3.png)  
- List  
  ![Car Washes List](assets/Screenshot 2025-08-25 at 21-53-06 Listado de lavados - Proyecto3.png)

