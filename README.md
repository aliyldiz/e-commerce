# E-Commerce

## Overview

This project is a comprehensive e-commerce platform that allows users to shop online and sellers to manage their products. My goal while developing the project was to gain in-depth knowledge of software architecture, business logic and modern backend technologies. I chose the e-commerce project because it offered the opportunity to apply important concepts in a single project.

Throughout the process, I focused on topics such as layered architecture, database management, authentication, JWT, Dependency Injection, etc. within the framework of SOLID principles. I followed specific resources to guide me through these concepts and instead of just implementing them, I tried to learn how they work and how to use them effectively in real world applications.

## Features

- User authentication (Facebook and Google login)
- Two-factor authentication for enhanced security
- JWT Authentication for securing API endpoints (with refresh tokens for session management)
- Product management (add, edit, delete, view products)
- Shopping cart functionality (add, update, remove items)
- Order history and tracking
- Admin dashboard for managing products, orders, and users
- Detailed error tracking with logging
- Image storage in Azure Blob Storage
- Order notifications for customers
- Inventory management (stock tracking and low stock notifications)
- RabbitMQ integration for message queuing and asynchronous tasks
- SignalR for real-time communication
- Role-based authorization for user access control
- QR code scanning for product
- Azure deployment
- Product image management

## Technologies Used

### Backend

- **C#** with **ASP.NET Core** for API development
- **Entity Framework Core** for database management
- **PostgreSQL**
- **Identity Framework** for user authentication and authorization
- **Azure Storage** for media storage management
- **Seq** for centralized logging and event management

### Frontend
- **Angular**
- **TypeScript**
- **Bootstrap**

## API Endpoints

<img width="1446" alt="Screenshot 2025-02-20 at 4 56 43 PM" src="https://github.com/user-attachments/assets/b82a82fd-ee6d-487c-87c1-edfffdecd493" />
<img width="1446" alt="Screenshot 2025-02-20 at 4 57 14 PM" src="https://github.com/user-attachments/assets/67333c96-2842-4cb6-8772-1763d6085c9c" />
<img width="1446" alt="Screenshot 2025-02-20 at 4 57 27 PM" src="https://github.com/user-attachments/assets/bb18401e-ae50-4f7c-b2fa-33edd6d2fe9a" />
<img width="1446" alt="Screenshot 2025-02-20 at 4 57 48 PM" src="https://github.com/user-attachments/assets/78028b56-a207-4a04-a854-49af55193e1b" />

## Installation

### Backend Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/aliyldiz/e-commerce.git
   cd e-commerce/ECommerceApi
   ```
2. Install dependencies:
   ```bash
   dotnet restore
   ```
3. Set up the database connection in `appsettings.json`.
4. Apply database migrations:
   ```bash
   dotnet ef database update
   ```
5. Start the backend server:
   ```bash
   dotnet run
   ```
   The API will be available at `https://localhost:7092`.

### Frontend Setup

1. Navigate to the client directory:
   ```bash
   cd ../ECommerceClient
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Configure the API endpoint in `src/environments/environment.ts`.
4. Start the frontend server:
   ```bash
   ng serve
   ```
   The frontend will be available at `http://localhost:4200`.

## Usage

1. Register or log in as a user.
2. Browse and search for products.
3. Add items to your cart.
4. Admin users can manage products and view order details.

