# Tasks: Digital Bookshelf Platform

**Input**: Design documents from `specs/001-digital-bookshelf-app/`
**Prerequisites**: plan.md, spec.md

## Phase 1: Foundational Backend (Application & API)

**Purpose**: Create the core services and controllers that all user stories will depend on.

- [X] T001 [P] **[App]** Implement `CreateUserCommand` and `AuthenticateUserQuery` in `MyHomeDigitalBookshelf.Application/Users/`.
- [X] T002 [P] **[App]** Implement `CreateBookshelfCommand` and `GetBookshelfByIdQuery` in `MyHomeDigitalBookshelf.Application/Bookshelves/`.
- [X] T003 [P] **[App]** Create `IBookFinderService` interface for Google Books API in `MyHomeDigitalBookshelf.Application/Books/`.
- [X] T004 [P] **[App]** Create a concrete implementation of `IBookFinderService` in `MyHomeDigitalBookshelf.Infrastructure/Api/`.
- [X] T005 [P] **[App.Tests]** Write unit tests for `CreateUserCommand` and `AuthenticateUserQuery`.
- [X] T006 **[Api]** Implement `AuthController` in `MyHomeDigitalBookshelf.Api/Controllers/` with `/register` and `/login` endpoints.
- [X] T007 **[Api]** Implement `BookshelvesController` in `MyHomeDigitalBookshelf.Api/Controllers/` with `POST /` and `GET /{id}` endpoints.

---

## Phase 2: User Story 1 - Register a New Book via Barcode (P1)

**Goal**: A user can scan a barcode, see fetched book data, and add it to their collection.
**Independent Test**: Scan an ISBN, confirm the details are shown, save the book, and verify it appears in the user's library.

### Tests for User Story 1
- [X] T008 [P] [US1] **[App.Tests]** Write unit test for `AddBookFromIsbnCommand`.
- [X] T009 [P] [US1] **[Client]** Write E2E test for the barcode scanning and book addition flow.

### Implementation for User Story 1
- [X] T010 [US1] **[App]** Implement `AddBookFromIsbnCommand` in `MyHomeDigitalBookshelf.Application/Books/`. This command will use the `IBookFinderService` to get data from Google Books API and create a new `Book`.
- [X] T011 [US1] **[Api]** Add a `POST /api/v1/books/from-isbn` endpoint to `BooksController`.
- [X] T012 [P] [US1] **[Client/UI]** Create a `BarcodeScanner` component in `client/common/ui/src/`.
- [X] T013 [P] [US1] **[Client/UI]** Create an `AddBookForm` component in `client/common/ui/src/`.
- [X] T014 [US1] **[Client/Web & Mobile]** Implement the "Add Book" screen that uses the `BarcodeScanner` and `AddBookForm`. On scan, it should call the new API endpoint and populate the form.

---

## Phase 3: User Story 2 - Manage Book Details and Status (P2)

**Goal**: A user can find a book, view its details, and update their reading status.
**Independent Test**: Search for a book, open it, change the reading status, and verify the change is saved and displayed correctly.

### Tests for User Story 2
- [X] T015 [P] [US2] **[App.Tests]** Write unit tests for `GetBooksForBookshelfQuery` and `UpdateUserBookStatusCommand`.
- [X] T016 [P] [US2] **[Client]** Write E2E test for searching for a book and updating its status.

### Implementation for User Story 2
- [X] T017 [P] [US2] **[App]** Implement `GetBooksForBookshelfQuery` in `MyHomeDigitalBookshelf.Application/Books/` with filtering/sorting capabilities.
- [X] T018 [P] [US2] **[App]** Implement `UpdateUserBookStatusCommand` in `MyHomeDigitalBookshelf.Application/UserBooks/`.
- [X] T019 [US2] **[Api]** Add `GET /api/v1/bookshelves/{id}/books` endpoint to `BookshelvesController`.
- [X] T020 [US2] **[Api]** Add `PUT /api/v1/user-books/{bookId}/status` endpoint.
- [X] T021 [P] [US2] **[Client/UI]** Create a `BookList` and `BookListItem` component in `client/common/ui/src/`.
- [X] T022 [P] [US2] **[Client/UI]** Create a `FilterSort` component in `client/common/ui/src/`.
- [X] T023 [US2] **[Client/Web & Mobile]** Implement the main Library screen, using the `BookList` and `FilterSort` components.
- [X] T024 [US2] **[Client/Web & Mobile]** Implement the Book Detail screen where users can view details and trigger the status update.

---

## Phase 4: User Story 3 - Administer a Bookshelf (P3)

**Goal**: A bookshelf administrator can invite new members.
**Independent Test**: An admin invites a user via email. The invited user signs up and is automatically a member of the bookshelf.

### Tests for User Story 3
- [X] T025 [P] [US3] **[App.Tests]** Write unit test for `InviteUserToBookshelfCommand`.

### Implementation for User Story 3
- [X] T026 [P] [US3] **[App]** Implement `InviteUserToBookshelfCommand` in `MyHomeDigitalBookshelf.Application/Bookshelves/`. This will likely involve sending an email, so an `IEmailService` abstraction will be needed.
- [X] T027 [P] [US3] **[Infrastructure]** Implement a concrete `EmailService` in `MyHomeDigitalBookshelf.Infrastructure/Services/`.
- [X] T028 [US3] **[Api]** Add `POST /api/v1/bookshelves/{id}/members` endpoint to `BookshelvesController`.
- [X] T029 [US3] **[Client/Web & Mobile]** Implement a Member Management screen for bookshelf admins to send invitations.
