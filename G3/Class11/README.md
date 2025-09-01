# Workshop 
## Part 1
We need to create an API that keeps and manages our favorite movies. It should have the option to:
* keep data in a fixed static class

* get movie by id (two methods: route param and query string)
* get all movies 
* filter movies by genre and/or year
* create new movie
* update a movie
* delete a movie (two methods: get the id from body, get the id from route)

A movie contains:
* id
* title - required field
* description
* year - required field
* genre - required field

## Use DTOs

## All fields except description are required. If description is entered maximum length is 250 characters.


## Part 2

For Part 2, take the same API you built in Part 1 and refactor it to use N-tier architecture and Entity Framework Core instead of a static data store.

Requirements:

* Organize the project into layers (API/Controllers, Services, Data Access, Domain).
* Use Entity Framework Core with a database instead of the static class.
* Implement migrations and CRUD operations via EF Core.
* Keep using DTOs for requests and responses.
* The functionality stays the same as in Part 1 (get movies, filter, create, update, delete).

## Part 3

For Part 3, extend the API by adding User management.

Requirements:

Create a User class that has a relationship with Movie.

Implement a service for user registration and login.

Store passwords securely by applying hashing.

Implement authentication using JWT tokens.