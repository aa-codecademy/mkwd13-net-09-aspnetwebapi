# Connecting to the database with other frameworks 🚠

## Dapper 🥨

Dapper is an ORM framework for .NET applications. It bridges the gap between an application and a relational database such as an SQL database. This ORM is built by the guys over at stack overflow and it is famous for being very simple and speedy.

### Benefits of Dapper 🔸

Dapper is built for speed and working with a lot of data. Dapper allows:

* High performance when manipulating data
* Working great with a lot of data
* Mapping DB to models without context
* Easy handling for stored procedures
* Support for sending multiple queries
* Short and simple syntax

Since it is a high-performance ORM it is used for applications where there is a lot of data, a lot of relations, or intensive usage. Dapper can be installed in an application that already has another ORM such as Entity Framework. For instance, if one part of the application is used frequently and is slow it can be migrated to dapper for communicating with the database. One of the main problems with dapper is that it works with ADO.NET very closely and we need to write naked queries. This means that we have to write SQL on the spot or call procedures, unlike entity framework where we just write LINQ and get our data instantly.

### Setup 🔽

Dapper works on top of the ADO.NET framework. Entity Framework does too but dapper uses ADO.NET features directly and more closely. This means that if we want to use Dapper we need to have ADO.NET installed. ASP.NET applications come with ADO.NET already in their dependencies and there is no need to install it. But if there is a need for Dapper installation on any other project type such as Class Library or Console Application the following packages should be installed:

* System.Data.SqlClient
* Dapper

### How does it work 🔸

Dapper does not have the option of Code First approach such as entity framework. We have to have a database first before we can work with dapper. Dapper can map the database with our models automatically. For this mapping we need to create our models exactly like our database, tables are classes and properties are columns just like Entity Framework. After that, we need to open a connection and write a query or call a procedure on that connection. After that, we close the connection. Because it is not a good practice to write naked queries it is better to write stored procedures for doing operations in the database.

#### Classes

```csharp
 public class Author
{
    public int ID { get; set; }
    public string Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfDeath { get; set; }
    public List<Novel> Novels { get; set; }
}
```

```csharp
public class Novel
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public List<Nomination> Nominations { get; set; }
}
```

```csharp
public class Nomination
{
    public int ID { get; set; }
    public int BookID { get; set; }
    public int AwardID
    {
        get
        {
            return (int)Award;
        }
        set
        {
            Award = (AwardType)value;
        }
    }
    public int YearNominated { get; set; }
    public bool IsWinner { get; set; }
    public AwardType Award { get; set; }
    public Novel Book { get; set; }
}

public enum AwardType
{
    Hugo = 1,
    Nebula = 2
}
```

#### Connection

```csharp
IDbConnection Connection = new SqlConnection(connectionString);
Connection.Open();
```

#### Simple Query

```csharp
List<Novel> novels = Connection.Query<Novel>("SELECT * FROM Novels").ToList();
Connection.Close();
```

#### Complex Query ( Multiple queries )

```csharp
List<Novel> novels = new List<Novel>();
using (var multi = Connection.QueryMultiple("SELECT * FROM Novels; SELECT * FROM Nominations"))
{
    novels = multi.Read<Novel>().ToList();
    foreach (var novel in novels)
    {
        novel.Nominations.Add(multi.Read<Nomination>()
        .Where(x => x.BookID == novel.ID).Single());
    }
}
Connection.Close();
```

#### Stored Procedure

```csharp
List<Author> authors = Connection.Query<Author>("dbo.getAuthors",
                new { authorName = nameFragment },
                commandType: CommandType.StoredProcedure).ToList();
```

## Extra Materials 📘

* [Introduction to ADO.NET](https://www.c-sharpcorner.com/uploadfile/puranindia/what-is-ado-net/)
* [Microsoft Examples of ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-code-examples)
* [Dapper Documentation](https://dapper-tutorial.net/dapper)
* [Article on how to use Dapper with C#](https://www.infoworld.com/article/3025784/how-to-work-with-dapper-in-c.html)
* [CRUD methods with Dapper](https://github.com/ericdc1/Dapper.SimpleCRUD/)
* [EF/Dapper/ADO.NET Comparison](https://exceptionnotfound.net/dapper-vs-entity-framework-vs-ado-net-performance-benchmarking/)
* [SQL Injection](https://www.w3schools.com/sql/sql_injection.asp)
