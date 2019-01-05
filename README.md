magic-card
==========
Demo virtual bank card solution written in C#.   The solution has been written using Visual Studio 2017 and has also been tested in VS Code.

The repo contains two projects:-

## 1. MagicCard.Library 
Contains the implementation of the virtual bank card code. 

Public interfaces

* `ICardRepository` - Allows a user to obtain instances the `ICard` interface.
* `ICard` - Allows the user to perform operations on the card.

Public classes

* `DefaultModule` - This is an AutoFac Module which allows the user to obtain the initial `ICardRepository` interface.  An example of how to do this is included in the `MagicCard.UnitTests` project .

## 2. MagicCard.UnitTests
Contains a number of xUnit tests which test the functionality of the `MagicCard.Library`.

