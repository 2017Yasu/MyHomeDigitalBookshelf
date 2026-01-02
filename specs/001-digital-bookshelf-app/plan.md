# Implementation Plan: Digital Bookshelf Platform

**Branch**: `001-digital-bookshelf-app` | **Date**: 2026-01-01 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `specs/001-digital-bookshelf-app/spec.md`

## Summary

This plan outlines the implementation of the Digital Bookshelf Platform, focusing on the .NET Application and API layers, and the React/Expo client applications. The data model and domain layers are considered pre-existing. The plan will follow Clean Architecture principles, creating application services, exposing them via controllers, and building a corresponding user interface.

## Technical Context

**Language/Version**: C# (NET 8), TypeScript
**Primary Dependencies**: ASP.NET Core, xUnit, React, Expo (React Native)
**Storage**: PostgreSQL (via existing migrations)
**Testing**: xUnit for backend, React Testing Library / Jest for frontend
**Target Platform**: Web (Modern Browsers), iOS, Android
**Project Type**: Multi-project .NET Solution with Web and Mobile clients

## Implementation Phases

### Phase 1: Backend - Application Services Core

This phase focuses on creating the business logic within the `MyHomeDigitalBookshelf.Application` project.

- **Task 1.1: User Services**:
  - Implement `CreateUser` command.
  - Implement `GetUserByEmail` query.
  - Implement `AuthenticateUser` query (including password verification).
- **Task 1.2: Bookshelf Services**:
  - Implement `CreateBookshelf` command.
  - Implement `GetBookshelfById` query.
  - Implement `AddUserToBookshelf` command.
- **Task 1.3: Book Services**:
  - Implement `AddBook` command (for manual entry).
  - Implement `GetBookByIsbn` query.
  - Implement `GetBooksForBookshelf` query (with filtering and sorting).
- **Task 1.4: Unit Tests**:
  - Create unit tests for all new application services and commands/queries.

### Phase 2: Backend - API Controllers

This phase exposes the application services as RESTful endpoints in the `MyHomeDigitalBookshelf.Api` project.

- **Task 2.1: Auh Controller**:
  - Create `POST /api/v1/auth/register` endpoint.
  - Create `POST /api/v1/auth/login` endpoint.
- **Task 2.2: Bookshelves Controller**:
  - Create `POST /api/v1/bookshelves` endpoint.
  - Create `GET /api/v1/bookshelves/{id}` endpoint.
  - Create `POST /api/v1/bookshelves/{id}/members` endpoint.
- **Task 2.3: Books Controller**:
  - Create `POST /api/v1/books` endpoint for adding books.
  - Create `GET /api/v1/bookshelves/{bookshelfId}/books` endpoint with query parameters for filtering/sorting.
- **Task 2.4: API Contracts**:
  - Define OpenAPI (Swagger) specifications for all new endpoints. This will be generated automatically by Swashbuckle. I will create a static export of the contract.

### Phase 3: Frontend - Core & Authentication

This phase sets up the client applications and implements user authentication screens.

- **Task 3.1: Project Setup**:
  - Configure shared UI components in `client/common/ui`.
  - Set up routing (e.g., React Router) in `client/apps/web` and `client/apps/mobile`.
  - Create an API client service to communicate with the backend.
- **Task 3.2: Auth Screens**:
  - Implement Registration screen.
  - Implement Login screen.
  - Implement state management for user authentication (e.g., storing tokens).

### Phase 4: Frontend - Bookshelf & Book Management

This phase builds the core functionality for viewing and managing books.

- **Task 4.1: Library View**:
  - Implement the main library screen to display a user's books from their bookshelves.
  - Implement filtering and sorting UI controls.
  - Implement "infinite scroll".
- **Task 4.2: Book Details**:
  - Implement a screen to show detailed information for a single book.
  - Allow users to update reading status, loan status, etc.
- **Task 4.3: Add Book Flow**:
  - Implement a form for manually adding a book.
  - Integrate a barcode scanning component (from research in `research.md`). When a barcode is scanned, call the backend to get data from Google Books API and pre-fill the form.

### Phase 5: Advanced Features & PWA

- **Task 5.1: PWA Features**:
  - Implement a service worker for offline caching in the web app.
- **Task 5.2: C-Code and Categories**:
  - Implement UI for displaying and managing book categories.
- **Task 5.3: Push Notifications**:
  - Implement UI for subscribing to push notifications.
  - Implement client-side logic to handle incoming notifications.