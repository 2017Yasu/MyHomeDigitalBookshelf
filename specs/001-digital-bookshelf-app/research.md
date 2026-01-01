# Technical Research: Digital Bookshelf Platform

**Branch**: `001-digital-bookshelf-app` | **Date**: 2026-01-01 | **Spec**: [spec.md](./spec.md)

This document outlines key technical areas requiring investigation to implement the application and client layers of the Digital Bookshelf Platform.

## 1. Google Books API Integration

**Objective**: Understand how to integrate with the Google Books API to fetch metadata for books using their ISBN. This service will be called from the Application layer.

**Key Questions**:

- **Authentication**: Does the API require an API key? What are the procedures for obtaining and securing this key? The key should be stored securely in database.
- **Rate Limits**: What are the usage limits? The Application service should implement caching if necessary.
- **Data Payload**: What is the structure of the data returned for a given ISBN? How does it map to our `Book` domain entity?
- **Error Handling**: How will the Application service handle cases where an ISBN is not found?

**Action Items**:

- **[ ] Task**: Create a proof-of-concept C# service that calls the Google Books API and deserializes the response.
- **[ ] Task**: Document the API's rate limits and the mapping between the API response and the `Book` entity.

## 2. Japanese C-Code Classification

**Objective**: Find a reliable method for automatically classifying books using the Japanese C-Code system.

**Key Questions**:

- **API Data**: Can the C-Code be retrieved directly from the Google Books API?
- **Fallback Strategy**: The Application layer must be designed to handle cases where a C-Code cannot be determined automatically, allowing it to be set manually via an API endpoint.

**Action Items**:

- **[ ] Task**: Analyze the Google Books API response for any fields related to C-Code.

## 3. Barcode Scanning in Client Applications

**Objective**: Identify a robust library for barcode scanning that works for both the web (PWA) and mobile (Expo) clients.

**Key Questions**:

- **Web (PWA)**: What JavaScript libraries are suitable for camera-based barcode scanning in a browser?
- **Mobile (Expo)**: What is the recommended library for barcode scanning within the Expo ecosystem? (e.g., `expo-camera`).
- **Performance & Integration**: How fast and accurate are the libraries, and how can they be integrated into our React components?

**Action Items**:

- **[ ] Task**: Build a small prototype in the web client to test camera access and scanning.
- **[ ] Task**: Build a similar prototype in the mobile client.

## 4. PWA Features (Offline Caching & Push Notifications)

**Objective**: Plan the implementation of key Progressive Web App features for the web client.

**Key Questions**:

- **Offline Caching**: What caching strategy should be used in the service worker for the React app?
- **Push Notifications**: What is the architecture for push notifications? (Client subscription -> Backend endpoint -> Push service).

**Action Items**:

- **[ ] Task**: Define caching strategies for the PWA.
- **[ ] Task**: Plan the necessary API endpoints on the backend to manage push notification subscriptions.