# **Calderilla.Services**

## Overview

**Calderilla.Services** is the business logic layer of the Calderilla solution. It acts as an intermediary between the API layer (`Calderilla.Api`) and the data access layer (`Calderilla.DataAccess`), encapsulating all business rules and service logic.

## Responsibilities

- Implements business logic and rules for the application.
- Coordinates data retrieval and persistence by interacting with repositories in the data access layer.
- Provides service interfaces for use by the API layer.
- Contains logic for processing and transforming data, such as parsing bank extracts and mapping them to domain models.

## Structure

The project is organized into the following main areas:

- **Operations**: Services for retrieving and managing account operations.
- **Banks**: Logic for parsing and importing bank extracts (e.g., ING, Sabadell).
- **Interfaces**: Service interfaces for dependency injection and abstraction.

## Dependencies

- Depends on `Calderilla.Domain` for domain models.
- Depends on `Calderilla.DataAccess` for data persistence and retrieval.

## Related Projects

- [Calderilla.Api](../Calderilla.Api/README.md)
- [Calderilla.DataAccess](../Calderilla.DataAccess/README.md)
- [Calderilla.Domain](../Calderilla.Domain/README.md)

---

