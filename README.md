# 🐾 Pet Adoption Portal – Blazor Server (.Net 9)

Welcome to the **Pet Adoption Portal**, a full-stack web application for managing pet adoptions, built with **Blazor Server** and **ASP.NET Core Web API**. This modern and responsive platform bridges the gap between animal shelters and loving adopters, offering a seamless pet adoption experience.

---

## 📌 Table of Contents

- [About the Project](#about-the-project)
- [Technologies Used](#technologies-used)
- [Architecture](#architecture)
- [Setup Instructions](#setup-instructions)
- [Key Features](#key-features)
- [Future Enhancements](#future-enhancements)
- [Screenshots](#screenshots)
- [License](#license)

---

## 🐶 About the Project

The **Pet Adoption Portal** is designed for users to:

- Browse and search adoptable pets
- View detailed pet profiles with images and descriptions
- Submit adoption requests securely
- Register, log in, and manage user profiles

### 🎯 Goals
This project demonstrates best practices in **Blazor Server development**, clean architecture, and real-world integration with a **.NET Web API backend**.

It’s ideal for developers exploring:
- Full-stack development with Blazor
- ASP.NET Core architecture
- Real-time updates with SignalR
- Clean code and separation of concerns

---

## 💻 Technologies Used

### Frontend – Blazor Server
- **.NET 8 / .NET 7**
- **Razor Components**
- **SignalR** (real-time notifications)
- **Dependency Injection**
- **JWT Authentication** or **ASP.NET Identity** (planned)

### Backend – ASP.NET Core Web API
- **.NET Core Web API**
- **Entity Framework Core**
- **SQL Server / Azure SQL**
- **Repository + Unit of Work pattern**

---

## 🧠 Architecture

The system follows a **Clean Architecture** pattern:
- **Presentation Layer**: Blazor UI
- **API Layer**: ASP.NET Core Web API
- **Data Access Layer**: EF Core, SQL Server
- **Business Logic**: Services injected into both layers

Data is passed via DTOs and models, with separation between client and server concerns.

---

## 🚀 Setup Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/pet-adoption-portal.git
