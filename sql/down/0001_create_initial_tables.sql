-- Drop tables in reverse order of creation to avoid dependency issues
DROP TABLE IF EXISTS user_books;

DROP TABLE IF EXISTS bookshelf_users;

DROP TABLE IF EXISTS sessions;

DROP TABLE IF EXISTS user_identities;

DROP TABLE IF EXISTS books;

DROP TABLE IF EXISTS categories;

DROP TABLE IF EXISTS bookshelves;

DROP TABLE IF EXISTS users;

-- Remove extension
DROP EXTENSION IF EXISTS "uuid-ossp";

DELETE FROM db_versions WHERE version = '0001';
