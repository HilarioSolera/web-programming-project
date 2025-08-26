¡Perfecto Hilario! 🚀
Te dejo un **README completo, en inglés y de nivel profesional** para tu **Web Programming Project**. Está listo para que lo copies en tu `README.md`.

---

```md
# 🌐 Web Programming Project — ASP.NET MVC & RESTful API

This project was developed as part of my Computer Engineering studies.  
It is a **full-stack web application** built with **ASP.NET MVC** that consumes a custom **ASP.NET Core RESTful API**, providing a clean, scalable, and modular architecture.

The system manages **Customers, Employees, Vehicles, and Car Wash Services**, integrating both frontend and backend with database persistence using **Entity Framework Core** and **SQL Server**.

---

## 🚀 Features

- ✅ **Customer Management** (CRUD operations, search, details view).  
- ✅ **Employee Management** (registration, editing, duplication validation).  
- ✅ **Vehicle Management** (CRUD, delete with relational constraints).  
- ✅ **Car Wash Service Management** (register, update, assign customer/vehicle/employee).  
- ✅ **RESTful API** with Swagger documentation.  
- ✅ **Clean Architecture** with separation of concerns:
  - **Core:** Entities, DTOs, Interfaces.  
  - **Infrastructure:** EF Core, Repositories, Database migrations.  
  - **API:** Controllers, Endpoints, Validation.  
  - **WebApp:** ASP.NET MVC frontend with Razor Views.  
- ✅ **Error handling & notifications** using `TempData`.  
- ✅ **Swagger** configured to accept only IDs for related entities on POST/PUT.  

---

## 🛠️ Tech Stack

- **Languages:** C#, JavaScript, HTML5, CSS3  
- **Frameworks:** ASP.NET MVC, ASP.NET Core Web API, Entity Framework Core  
- **Database:** SQL Server  
- **Tools:** Git, Postman, Swagger, Visual Studio, Azure (basic deployment)  

---

## 📂 Project Structure

```

/web-programming-project
/src
WebApp/           -> ASP.NET MVC frontend (controllers + views)
Api/              -> RESTful API (ASP.NET Core)
Core/             -> Entities, DTOs, Interfaces
Infrastructure/   -> EF Core, Repositories, Migrations
/assets             -> Project screenshots
README.md
.gitignore
LICENSE

````

---

## ⚙️ Getting Started

### 1️⃣ Clone the repository
```bash
git clone https://github.com/YOUR_USERNAME/web-programming-project.git
cd web-programming-project
````

### 2️⃣ Configure the database

Update the **connection strings** in:

```
src/Api/appsettings.json
src/WebApp/appsettings.json
```

### 3️⃣ Apply migrations

```bash
cd src/Api
dotnet ef database update
```

### 4️⃣ Run the API

```bash
dotnet run
```

Available at: [https://localhost:5001/swagger](https://localhost:5001/swagger)

### 5️⃣ Run the Web Application

```bash
cd src/WebApp
dotnet run
```

Available at: [https://localhost:5002](https://localhost:5002)

---

## 📸 Screenshots

### 🔐 API Documentation

!\[Swagger UI]\(assets/Screenshot 2025-08-25 at 21-54-44 Swagger UI.png)

### 🏠 Home Page

!\[Home Page]\(assets/Screenshot 2025-08-25 at 21-52-18 Inicio - Proyecto3.png)

### 👥 Customers

* List
  !\[Customers List]\(assets/Screenshot 2025-08-25 at 21-52-39 Listado de clientes - Proyecto3.png)
* Edit
  !\[Edit Customer]\(assets/Screenshot 2025-08-25 at 21-53-24 Editar cliente - Proyecto3.png)

### 👨‍💼 Employees

* List
  !\[Employees List]\(assets/Screenshot 2025-08-25 at 21-52-50 Listado de empleados - Proyecto3.png)
* Details
  !\[Employee Details]\(assets/Screenshot 2025-08-25 at 21-53-33 Detalles del empleado - Proyecto3.png)

### 🚗 Vehicles

* List
  !\[Vehicles List]\(assets/Screenshot 2025-08-25 at 21-52-57 Vehículos - Proyecto3.png)
* Delete
  !\[Delete Vehicle]\(assets/Screenshot 2025-08-25 at 21-53-43 Eliminar vehículo - Proyecto3.png)

### 🚿 Car Wash Services

* Register
  !\[Register Car Wash]\(assets/Screenshot 2025-08-25 at 21-54-00 Registrar lavado - Proyecto3.png)
* List
  !\[Car Washes List]\(assets/Screenshot 2025-08-25 at 21-53-06 Listado de lavados - Proyecto3.png)

---

## 📜 License

This project is licensed under the [MIT License](LICENSE).
Feel free to use, modify, and share with proper attribution.

---

## 👤 Author

**Hilario David Solera Meza**

* 🎓 Computer Engineering Student
* 📧 Email: [hilario15@hotmail.es](mailto:hilario15@hotmail.es) | [solerahilario207@gmail.com](mailto:solerahilario207@gmail.com)
* 🌐 GitHub: [HilarioSolera](https://github.com/HilarioSolera)
* 💼 LinkedIn: https://www.linkedin.com/in/hilario-solera-6ba366174

