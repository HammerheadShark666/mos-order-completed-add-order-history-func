# mos-order-completed-add-order-history-func

Microservice Order System - Order History Microservice

[See Wiki for details about the Order Completed Add Order History Function](https://github.com/HammerheadShark666/mos-order-completed-add-order-history-func/wiki)

This project is an **Azure Function** designed to add order history records. It is built using **.NET 8**, interacts with an **SQL Server** database for storage, and sends order ID messages to an **Azure Service Bus** for further processing. The function is set up with a **CI/CD pipeline** for seamless deployment.

## Features

- **Order History**: Listens for order messages from Azure Service Bus and adds corresponding order history records to the SQL Server database.
- **SQL Server Database**: Stores order history details such as order ID, customer ID, product details, and timestamps.
- **Azure Service Bus Integration**: Publishes messages to the service bus for processing order records.
- **Scalable Serverless Architecture**: Utilizes Azure Functions for on-demand execution and scaling.
- **CI/CD Pipeline**: Automated build and deployment using **GitHub Actions**.

---

## Technologies Used

- **.NET 8**
- **C#**
- **Azure Functions**
- **SQL Server** (Azure SQL)
- **Azure Service Bus**
- **GitHub Actions** for CI/CD

---
