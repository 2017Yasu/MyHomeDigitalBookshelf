# My Home Digital Bookshelf

## Project Purpose
A comprehensive web-based platform for personal and family book collection management. The system allows users to track their books, manage reading progress, and coordinate lending within a shared bookshelf context.

## Tech Stack
- **Backend:**
  - .NET Core (Clean Architecture)
  - PostgreSQL Database
- **Frontend:**
  - React 19
  - TypeScript
  - Vite
  - Bootstrap 5
- **Infrastructure:**
  - Docker (for development database)
  - CI/CD with GitHub Actions

## Architecture
The solution follows Clean Architecture principles with the following projects:
- **Api**: ASP.NET Core Web API with React frontend
- **Domain**: Core business logic and entities
- **Application**: Application services and interfaces
- **Infrastructure**: Data access and external services
- **Infrastructure.Tests**: Integration tests

## Key Features
- Book management with metadata from Google Books API
- Japanese C-Code classification support
- Multi-user bookshelf sharing
- Reading progress tracking
- Book lending management
- PWA capabilities
