# Online Store Backend - Azure Integration

## Introduction

The Online Store Backend is a proof-of-concept project designed to showcase a scalable cloud database architecture using Azure services. It includes a robust API, Azure Functions for background tasks, and an N-Tier architecture for separation of concerns.

## Architecture Overview

- Cloud Database: Combines SQL for structured data, Cosmos DB for scalability, and Blob Storage for media.
- N-Tier Design:
  - Data Tier: Handles database operations.
  - Business Tier: Contains services implementing business logic.
- Azure Functions:
  - UpdateOrderProcessedDuration: Manages order metrics.
  - UpdateProductStockOnOrder: Handles inventory updates.

## Features

- Scalable cloud database architecture with Azure.
- CQRS implementation for clear separation of read/write operations.
- Loose coupling with dependency injection for testability.
- Integration with Azure Blob Storage for storing product images.
- Integration with Cosmos DB for storing product reviews.

## Azure Functions

1. UpdateOrderProcessedDuration
   - Trigger: Time-based or event-driven (order update).
   - Purpose: Calculates and updates `orderProcessed` metrics.
2. UpdateProductStockOnOrder
   - Trigger: Event-driven (order placement).
   - Purpose: Updates product inventory when an order is placed.

## Local Setup

1. Use the right project folder `cd OnlineStore`
2. Restore dependencies `dotnet restore`
3. Update the connection strings in `appsettings.Development.json` or use the existing ones
4. Run application `dotnet run`

## Azure Setup

1. Use the following URL as the default base https://onlinestore-h0h3ckeab4h3dzau.northeurope-01.azurewebsites.net/api/{ControllerName}
2. All database connections can be found in `appsettings.json`
3. API Documentation can be found in `DOCUMENTATION.md` file
