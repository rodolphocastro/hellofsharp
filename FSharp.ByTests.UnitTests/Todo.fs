module Todo

open System
open Xunit

// A "type" is a C# 9 record (aka an Immutable class)
type Todo = {
    Id: Guid
    Title: string
}

// We declare both functions and variables by using the let keyword
// In this case we're using XUnit's Fact attribute and creating a function that is able to create
// A Todo and asserts it has an ID.
[<Fact>]
let ``A TODO must have an ID`` () =
    let subject = { 
        Title = "An Awesome Todo"
        Id = Guid.NewGuid ()
    }   // Notice how we don't tip the compiler that this is a Todo at all!
    Assert.NotNull subject.Id
    Assert.IsType<Todo> subject |> ignore // Yet it *is* a TODO! Also not that |> operator? That's the "pipe forward" operators and it forwards the Assert.IsType result into "ignore", which is the equivalent of C#'s _
    // If we want to tip the compiler off we can use var:Type, like...
    let anotherSubject:Todo = {
        Title = "Another Todo"
        Id = Guid.NewGuid()
    }
    Assert.NotNull anotherSubject   // It just works.
    // Another fun thing about F# -> Everything is immutable by default! If we want to make a variable mutable we must be very specific!
    let expectedBogus = "bbbb"
    let mutable yetAnotherSubject = "aaaaa"
    yetAnotherSubject <- expectedBogus
    // expectedBogus <- "aaaa" fails wit
    Assert.Equal(expectedBogus, yetAnotherSubject)

// Here we're declaring a function that given a Todo returns a *new* Todo with the same fields except for Title! Which it replaces by whatever's on "newName"!
// Notice how we don't need to type anything, yet the compiler knows its a Todo and a String!
let RenameATodo aTodo newName = { aTodo with Title = newName }

// Here we're declaring a function that has no parameters and returns a Todo.
// We're hinting the Todo but it isn't required in order to compile.
let createATodo (): Todo = {
    Id = Guid.NewGuid()
    Title = "My Todo"
}

// Using that simple, paremeterless, function
[<Fact>]
let ``A TODO must have a Title`` () = 
    let subject = createATodo()
    Assert.NotNull subject.Title

// Using the function that renames a todo based on two parameters
// alongside the "pipe forward/backward" operator
[<Fact>]
let ``A TODO must be renameable`` () =
    let expected = "Brand new Name"
    let result = createATodo() |> RenameATodo <| expected
    let got = result.Title
    Assert.Equal(expected, got)
