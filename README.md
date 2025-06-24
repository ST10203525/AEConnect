Agri-Energy Connect is a role-based ASP.NET Core MVC web application designed to connect South African farmers with employees who assist with data management. The system allows farmers to manage product listings and employees to manage farmer profiles and analyze production data.

## Features
Identity & Authentication

ASP.NET Core Identity with Role-based Authorization

Two distinct roles: Farmer and Employee

Secure login, registration, and role seeding

## Farmer Role

Add products with name, category, and production date

View their own submitted products

Automatically links products to the currently logged-in farmer

## Employee Role

Create new farmer profiles and assign them to users

View a complete list of all farmer products

Filter product list by category and date range

## Data Management

SQL Server relational database

Entity Framework Core with code-first migrations

Seeded sample users and roles

## Data Validation & Error Handling

Model validation for all forms

Error handling for null references and unauthorized access

Database integrity protected with foreign key relationships

🧰 Technologies Used
ASP.NET Core MVC (.NET 8)
Entity Framework Core
ASP.NET Core Identity
SQL Server
Bootstrap 5
Visual Studio / VS Code


## Notes
Only authorized users can access features based on their roles.

Employee must enter the correct Identity UserId when creating a farmer.

You can customize email confirmation or account lockout in Identity options.

## Future Enhancements
Image upload support for products

Dashboard with charts for analytics

Notifications for farmer activities

API integration for external energy providers

## Contributors
Kiyashan Nadasen (Developer)
