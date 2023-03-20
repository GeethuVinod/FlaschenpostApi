# FlaschenpostApi
This project is a .NET solution that provides a service for retrieving data about products and articles from a remote API. It also includes 4 api routes -
 * Get list of beer filtered by price
 * Get the most expensive and cheapest beers
 * Get the beers that comes in most bottles
 * Get all the above query results 

# Getting Started
To run the project, you will need .NET Core 6 installed on your machine. Once you have cloned the repository, you can open the solution in Visual Studio or another .NET IDE, build the project, and run the tests to verify that everything is working correctly.

# Installation

To clone the repository run this git command

git clone https://github.com/GeethuVinod/Flaschenpostapi.git

# Sample routes

https://<localhost:port>/api/Products/expensive-cheapest-per-litre?url=https://flapotest.blob.core.windows.net/test/ProductData.json

https://<localhost:port>/api/Products/cost?url=https://flapotest.blob.core.windows.net/test/ProductData.json&price=17.99

https://<localhost:port>/api/Products/expensive-cheapest-per-litre?url=https://flapotest.blob.core.windows.net/test/ProductData.json

https://<localhost:port>/api/Products/mostbottles?url=https://flapotest.blob.core.windows.net/test/ProductData.json


