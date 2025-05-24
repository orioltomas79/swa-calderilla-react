# **Calderilla.DataAccess**

## Overview

`Calderilla.DataAccess` is a .NET class library responsible for data persistence and retrieval in the Calderilla application. It provides repository implementations for storing and accessing domain entities, primarily using Azure Blob Storage as the backend. The project abstracts storage operations, allowing other layers of the application to interact with data in a consistent and testable manner.

## Project Structure

- **BlobRepository.cs**: Implements low-level operations for reading and writing lists of objects to Azure Blob Storage.
- **OperationsRepository.cs**: Provides higher-level operations for managing `Operation` entities, including querying by month, year, or the last 12 months.

---

## BlobRepository.cs

`BlobRepository` implements the `IBlobRepository` interface and provides generic methods to read and write lists of objects to Azure Blob Storage. It uses the Azure.Storage.Blobs SDK and serializes data as JSON. The repository ensures the target container exists and handles connection string configuration via environment variables.

**Key responsibilities:**
- Reading lists of objects from blobs (`ReadListAsync<T>`)
- Writing lists of objects to blobs (`WriteListAsync<T>`)
- Managing blob client creation and container existence

---

## OperationsRepository.cs

`OperationsRepository` implements the `IOperationsRepository` interface and provides methods to manage `Operation` entities for users and accounts. It leverages `BlobRepository` for storage and organizes data by user, account, year, and month.

**Key responsibilities:**
- Retrieving operations for a specific month or year
- Retrieving operations for the last 12 months
- Saving operations for a given period
- Generating blob names based on user, account, year, and month

---

