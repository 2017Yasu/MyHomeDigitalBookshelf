# Executive Summary – My Home Digital Bookshelf

## Purpose

A web-based platform for individuals and families to manage personal book collections, track reading progress, and coordinate lending.

## Key Capabilities

- **Book Management:** Search, filter, and sort by title, author, ISBN, category, owner, or reading status. Search unregistered books via Google Books API.
- **Categorization:** Automatic Japanese C-Code classification, with support for custom tags and manual overrides.
- **Registration:** Add books by barcode scanning or manual entry, with automatic metadata retrieval.
- **User Tracking:** Record per-user ownership, reading status, loan status, purchase date, and price.
- **Bookshelf Structure:** Each user belongs to at least one bookshelf; bookshelves can have multiple members; books belong to one bookshelf only.
- **Additional Tools:** Recently added/read dashboards, bulk actions, and visual “bookshelf view.”

## Technology & Platforms

- **Primary:** Responsive web app with PWA features (offline caching, push notifications, home screen install).
- **Roadmap:** Native iOS/Android apps with enhanced scanning and offline-first capabilities.

## Roles & Permissions

- **System Roles:** Administrator (full access), Member (manage owned books), Guest (view/search public lists).
- **Bookshelf Roles:** Administrator (manage members/books), Member (manage owned books).

## Data Model

Relational structure supporting bookshelves, categories, books, users, and many-to-many relationships for bookshelf membership and user-specific book statuses.

## User Interface

Key screens include library view, book detail, barcode scan, manual entry, external search results, category management, and member status tracking.

## C-Code Classification

Built-in support for the Japanese publishing industry’s standard Audience–Format–Genre coding system for automatic categorization.
