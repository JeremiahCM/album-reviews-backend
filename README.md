# Album Review Backend Project

This backend project uses
* .NET 6.0
* Deezer API

This developed API allows you to write an album review, passing in the details for
* Artist Name
* Album Name
* Album Review

## Section One

* Created __AlbumReviews__ controller that implements CRUD operations for a resource (Create, Read, Update, Delete)
* Create calls to Deezer API
    - API calling made through __DeezerService__.
    - __DeezerService__ retrieves the release date, genre and number of tracks of the album being reviewed.
* Created configuration files for 'Development' and 'Production' environments
    - The difference between these two files is that the first file __appsettings.Development.json__ has lower logging levels of _default as trace_ and _microsoft as debug_. The second file has higher logging levels of _default as information_ and _microsoft as warning_. These different logging levels of severity are appropriate because during development you would be more interested in detailed logs for debugging purposes compared to when the API is already put to production.

## Section Two

Demonstrate an understanding of how these middleware via DI (dependency injection) simplifies your code

* The middlewares via dependency injection helps simplify code because it utilizing pieces of code from other classes makes the web API more loosely coupled, giving our code better modularity and easier testability. An example of this would be adding my developed __Deezer Service__ to the app's services in the __Program.cs__ file, and the interface-based injection of the __Deezer Service__ into the constructor of the __Album Reviews Controller__. This dependency injection isolates the operations for directly calling the Deezer API into one file of code, making our project easier to manage, and allows for unit testing.

## Section Three

Demonstrate the use of NUnit to unit test your code

* NUnit has been utilized in the UnitTest1.cs, seen through the use of the NUnit attributes.

Use at least one substitute to test your code

* A mock database is made in the unit test during the arranging portion of __GetAll_ReturnsAllReviews()__. 

Demonstrate an understanding of why the middleware libraries made your code easier to test

* Being able to create a mock version of the database utilizing the __moq__ framework injected into the project made coding the test easier because it helps you deal with unit testing more compilcated objects without the need to write a lot of the boilerplate code required. It is also useful to be able to test the controller's __GetAllAlbumReviews()__ without having to interact with the live database.