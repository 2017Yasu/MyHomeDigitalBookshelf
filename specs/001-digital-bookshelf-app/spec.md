# Feature Specification: Digital Bookshelf Platform

**Feature Branch**: `001-digital-bookshelf-app`
**Created**: 2026-01-01
**Status**: Draft
**Input**: User description: "Build the following application:## Purpose A platform for individuals to manage personal book collections, track reading progress, and coordinate lending. ## Key Capabilities - **Book Management:** Search, filter, and sort by title, author, ISBN, category, owner, or reading status. Search unregistered books via Google Books API. - **Categorization:** Automatic Japanese C-Code classification, with support for custom tags and manual overrides. - **Registration:** Add books by barcode scanning or manual entry, with automatic metadata retrieval. - **User Tracking:** Record per-user ownership, reading status, read book histories, loan status, purchase date, and price. - **Bookshelf Structure:** Each user belongs to at least one bookshelf; bookshelves can have multiple members; books belong to one bookshelf only. ## Technology & Platforms - **Primary:** Responsive web app with PWA features (offline caching, push notifications, home screen install). - **Roadmap:** Native iOS/Android apps with enhanced scanning and offline-first capabilities. ## Roles & Permissions - **System Roles:** Administrator (full access), Member (manage owned books), Guest (view/search public lists). - **Bookshelf Roles:** Administrator (manage members/books), Member (manage owned books). ## User Interface Key screens include library view, book detail, barcode scan, manual entry, external search results, category management, and member status tracking. ## C-Code Classification Built-in support for the Japanese publishing industry’s standard Audience–Format–Genre coding system for automatic categorization."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Register a New Book via Barcode (Priority: P1)

As a Member, I want to scan a book's barcode to automatically add it to my collection with all its details fetched, so I can quickly register my books.

**Why this priority**: This is the fastest and most common way users will want to add books, representing a core value proposition.

**Independent Test**: Can be tested by scanning a valid ISBN barcode. The system should create a new book record associated with the user and bookshelf, populated with metadata.

**Acceptance Scenarios**:

1.  **Given** a Member is logged in and on the 'Add Book' screen, **When** they scan a valid book barcode, **Then** the system retrieves metadata (title, author, ISBN, C-Code) and displays it for confirmation.
2.  **Given** the metadata is confirmed, **When** the user saves the book, **Then** the book is added to their bookshelf and associated with them as the owner.

---

### User Story 2 - Manage Book Details and Status (Priority: P2)

As a Member, I want to find a book in my library, view its details, and update my reading status or loan it to someone, so I can keep my collection up-to-date.

**Why this priority**: Tracking reading progress and lending is a primary function of the application.

**Independent Test**: Can be tested by searching for an existing book, opening its detail view, and changing the reading status. The change should be persistent.

**Acceptance Scenarios**:

1.  **Given** a Member is viewing their library, **When** they search for a book by title, **Then** the book appears in the results.
2.  **Given** a Member is on the book detail page, **When** they change the reading status from "To Read" to "Reading", **Then** the new status is saved and displayed.
3.  **Given** a Member is on the book detail page for a book they own, **When** they mark the book as "Loaned" to a friend, **Then** the loan status is updated and visible.

---

### User Story 3 - Administer a Bookshelf (Priority: P3)

As a Bookshelf Administrator, I want to invite new members to my bookshelf so we can share a collective library.

**Why this priority**: Multi-user collaboration within bookshelves is a key social feature of the platform.

**Independent Test**: Can be tested by an existing Bookshelf Administrator sending an invitation to a new user's email.

**Acceptance Scenarios**:

1.  **Given** a Bookshelf Administrator is on the member management screen, **When** they enter a valid email address and send an invitation, **Then** the system sends an invite and lists the user as "Pending".
2.  **Given** a new user receives an invitation email and accepts, **When** they create an account, **Then** they are automatically added as a Member to the corresponding bookshelf.

---

### Edge Cases

- What happens when a scanned barcode (ISBN) is not found via the Google Books API?
- How does the system handle a user trying to add a book that already exists on the bookshelf?
- What happens when an invited user is already a member of the platform?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST allow users to register by creating an account.
- **FR-002**: Every user MUST belong to at least one bookshelf upon registration.
- **FR-003**: A book MUST belong to only one bookshelf.
- **FR-004**: Users MUST be able to add books via barcode scanning (ISBN).
- **FR-005**: Users MUST be able to add books by manual data entry.
- **FR-006**: System MUST retrieve book metadata (title, author, etc.) from the Google Books API when a valid ISBN is provided.
- **FR-007**: System MUST automatically assign a Japanese C-Code category to books where applicable data is available.
- **FR-008**: Users MUST be able to add custom tags to books and manually override automatic categorization.
- **FR-009**: The system MUST track per-user data for each book: ownership, reading status (e.g., To Read, Reading, Read), read history, loan status, purchase date, and price.
- **FR-010**: Users MUST be able to search and filter the library by title, author, ISBN, category, owner, and reading status.
- **FR-011**: The system MUST support three system-level roles: Administrator, Member, and Guest, with distinct permissions.
- **FR-012**: The system MUST support two bookshelf-level roles: Administrator and Member, with distinct permissions.
- **FR-013**: Bookshelf Administrators MUST be able to manage members and books within their bookshelf.
- **FR-014**: The system MUST provide a unified library view that displays all books from all bookshelves a user is a member of.
- **FR-015**: The unified library view MUST include controls to filter the library by one or more bookshelves.

### Key Entities

- **User**: Represents an individual with an account. Attributes include name, email, and system role.
- **Bookshelf**: A collection of books and members. Every book belongs to exactly one bookshelf.
- **Book**: Represents a single book. Attributes include title, author, ISBN, and categories. It is owned by a User and belongs to a Bookshelf.
- **Category**: A classification for a book (e.g., Japanese C-Code or custom tags).
- **BookshelfUser**: A join entity representing a User's membership and role within a Bookshelf.
- **UserBook**: A join entity tracking a User's relationship with a Book (e.g., reading status, loan status, purchase date).

### Assumptions

- **A-001**: Users who are members of multiple bookshelves will be best served by a hybrid approach to visibility. By default, they will see a unified "global" library of all their books, but they will have tools to filter this view to one or more specific bookshelves.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: A user can add a new book via barcode scan in under 30 seconds.
- **SC-002**: 95% of book searches within a user's library must return results in under 2 seconds.
- **SC-003**: The system must correctly classify 90% of books with available Japanese C-Code data automatically.
- **SC-004**: A new user must be able to register, join a bookshelf, and add their first book in under 5 minutes.
- **SC-005**: The platform must support 1,000 concurrent users viewing and managing their libraries without performance degradation.
- **SC-006**: All primary features (viewing library, searching, viewing book details) must be available offline in the PWA.