## PublicFunction.Database

[README.md](https://github.com/rayanabniro/PublicFunction/blob/main/PublicFunction/DataBase/README.md "README.md")


# MongoDB

**MongoDB** is a source-available, cross-platform, document-oriented database program. Classified as a **NoSQL** database product, MongoDB utilizes **JSON-like** documents with optional schemas. MongoDB is developed by MongoDB Inc. and current versions are licensed under the Server Side Public License


## PublicFunction.Database

[README.md](https://github.com/rayanabniro/PublicFunction/blob/main/PublicFunction/DataBase/README.md "README.md")



# MongoDB Interaction in C#

This document provides a detailed explanation of the methods defined in the `Mongo` class which interact with a MongoDB database. Below are the functions and their descriptions:

## Class: `Mongo`

The `Mongo` class is responsible for connecting to the MongoDB database and performing CRUD operations.

### Constructor: `Mongo(IConfiguration configuration)`
Initializes a new instance of the `Mongo` class, setting up a connection to the MongoDB database.

- **Parameters**: 
  - `IConfiguration configuration`: Configuration object to read MongoDB connection details.

---

## Method: `bool Insert<T>(string CollectionName, T BaseCollection) where T : TBaseDocumentModel`

Inserts a single document into the specified collection.

- **Parameters**:
  - `string CollectionName`: The name of the collection where the document will be inserted.
  - `T BaseCollection`: The document to be inserted (inherits from `TBaseDocumentModel`).
  
- **Returns**: `true` if the insertion was successful, throws an exception otherwise.

---

## Method: `bool Insert(string CollectionName, List<Dictionary<string, object>> BaseCollection)`

Inserts multiple documents into the specified collection.

- **Parameters**:
  - `string CollectionName`: The name of the collection.
  - `List<Dictionary<string, object>> BaseCollection`: A list of documents, where each document is represented as a dictionary.
  
- **Returns**: `true` if the insertion was successful, throws an exception otherwise.

---

## Method: `bool Insert(string CollectionName, Dictionary<string, object> BaseCollection)`

Inserts a single document, represented as a dictionary, into the specified collection.

- **Parameters**:
  - `string CollectionName`: The name of the collection.
  - `Dictionary<string, object> BaseCollection`: The document to be inserted.
  
- **Returns**: `true` if the insertion was successful, throws an exception otherwise.

---

## Method: `bool Update<T>(string CollectionName, FilterDefinition<T> filter, UpdateDefinition<T> update) where T : TBaseDocumentModel`

Updates a document in the specified collection based on the given filter.

- **Parameters**:
  - `string CollectionName`: The name of the collection.
  - `FilterDefinition<T> filter`: The filter to select the document to be updated.
  - `UpdateDefinition<T> update`: The update definition to apply to the document.
  
- **Returns**: `true` if the update was successful, throws an exception otherwise.

---

## Method: `bool Delete<T>(string CollectionName, FilterDefinition<T> filter) where T : TBaseDocumentModel`

Deletes a document from the specified collection based on the given filter.

- **Parameters**:
  - `string CollectionName`: The name of the collection.
  - `FilterDefinition<T> filter`: The filter to select the document to be deleted.
  
- **Returns**: `true` if the deletion was successful, throws an exception otherwise.

---

## Method: `List<T> Query<T>(string CollectionName, FilterDefinition<T> filter) where T : TBaseDocumentModel`

Queries for documents that match the specified filter in the given collection.

- **Parameters**:
  - `string CollectionName`: The name of the collection.
  - `FilterDefinition<T> filter`: The filter to match documents.
  
- **Returns**: A list of documents matching the filter, or an empty list if no documents are found.

---

## Method: `T QueryOne<T>(string CollectionName, FilterDefinition<T> filter) where T : TBaseDocumentModel`

Queries for a single document that matches the specified filter in the given collection.

- **Parameters**:
  - `string CollectionName`: The name of the collection.
  - `FilterDefinition<T> filter`: The filter to match the document.
  
- **Returns**: The document matching the filter, or `null` if no document is found.


## Use in .netCore
- in **appsettings.json** define Connection string like this
    ```json
        {
          "PublicFunction": {
            "DataBase": {
              "Mongo": {
                "ConnectionString": "Mongo Connection String",
              }
            }
          }
        }
    ```