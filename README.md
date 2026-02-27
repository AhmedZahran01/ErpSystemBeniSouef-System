# ErpSystemBeniSouef-System

A Desktop Enterprise Resource Planning (ERP) application built with **WPF, C#, and SQL Server** for managing hospital and administrative operations. This system follows clean architecture principles and features modular layers for core business logic, data access, and infrastructure.

## 📌 Overview

This ERP system provides a foundation for managing key organizational operations including:

- Staff management (CRUD operations)
- Role-based access control
- Multiple business workflows
- Clean separation of concerns using layered architecture

The solution is structured into distinct projects representing the **Core**, **Infrastructure**, **Service**, and **UI** layers.

## 🧱 Project Structure

| Project | Description |
|---------|-------------|
| `ErpSystemBeniSouef.Core` | Business entities, domain models, and interfaces |
| `ErpSystemBeniSouef.Infrastructer` | Implementation of data access and infrastructure services |
| `ErpSystemBeniSouef.Service` | Business logic and service layer |
| `ErpSystemBeniSouef` | WPF UI project (desktop application) |

## 🚀 Features

- WPF Desktop Application with MVVM pattern
- Entity Framework Core integration
- SQL Server database support
- CRUD operations for staff and related entities
- Role management and access control
- Modular architecture separating UI, services, and data layers

## 🧠 Architecture

This system is organized following clean architecture principles with:

- **Core** layer containing domain models and business interfaces
- **Infrastructure** layer handling database access and external dependencies
- **Service** layer implementing business workflows & logic
- **UI** layer built with WPF for desktop interaction

## 💻 Tech Stack

- **C#**
- **.NET Framework / .NET Core** (depending on the solution configuration)
- **Windows Presentation Foundation (WPF)**
- **Entity Framework Core**
- **SQL Server**

## 📁 Getting Started

### Prerequisites

Make sure you have:

- [.NET SDK / .NET Framework](https://dotnet.microsoft.com/download)
- Visual Studio (recommended)
- SQL Server (LocalDB or full SQL Server)

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/AhmedZahran01/ErpSystemBeniSouef-System.git
