# Screen Map

## 1. Core Library Screens

1. **Library / Browse View**

   * Toggle between *List View* and *Cover Bookshelf View*
   * Search bar with filters (Category, C-Code, Owner, Reading Status)
   * Sort controls (Title, Author, Purchase Date)

2. **Book Detail**

   * Metadata (Title, Author, Publisher, Publish Date, ISBN, C-Code)
   * Category/tags
   * Reading status controls
   * Ownership & loan status
   * Notes section
   * Action buttons (Edit, Delete, Loan/Return, Change Category)

3. **Recently Added / Recently Read Dashboard**

   * Grid or carousel of books
   * Quick actions (Update Status, Open Details)

---

## 2. Book Registration

1. **Barcode Scan Screen**

   * Live camera preview
   * On successful scan: auto-fill book metadata from ISBN lookup
   * Confirm & save

2. **Manual Entry Form**

   * All book fields (ISBN, Title, Author, Publisher, Publish Date, Category, C-Code, Cover Image, Notes)

3. **External Search Results (Google Books API)**

   * Search box for unregistered books
   * Select book → pre-fill metadata → registration screen

---

## 3. Categorization & Tagging

1. **Category Management**

   * List of categories
   * Add/Edit/Delete
   * Assign C-Code automatically or manually override

2. **Bulk Actions Screen**

   * Multi-select books from library
   * Apply category, tag, ownership, or reading status in one action

---

## 4. Member & Bookshelf Management

1. **Bookshelf List / Selector**

   * Switch between bookshelves you belong to
   * Create/Edit/Delete bookshelves (Admin only)

2. **Bookshelf Detail / Members**

    * List of members with roles
    * Invite/remove members
    * Change member roles

3. **User Profile**

    * Name, email
    * Default bookshelf
    * Personal book stats (Owned, Reading, Completed, etc.)

---

## 5. Authentication & Access

1. **Login / Registration**
2. **Forgot Password**
3. **Guest View (Public Library View)**

    * Limited search & browse only

---

## 6. System & Notifications

1. **Settings**

    * PWA install prompt
    * Notification preferences
    * Loan reminder configuration

2. **Loan Tracking View**

    * List of books currently lent out / borrowed
    * Due date reminders

---

This would give **\~16 main screens**, with some having modal or overlay variants (e.g., edit forms, confirmation dialogs).
