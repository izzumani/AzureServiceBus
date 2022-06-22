Designed solution that will process transactions in a Service Bus Topic, persist the transaction into a MSSQL DB. 

it perform the following:

Auditing of the message pulled from service bus topic
Request and Response Auditing
Auditing of the message stored into MSSQL
Application Insights monitoring for Worker Service is enabled
Application insights request response logging

It emphasis on SOLID design principles. 



Autofac
Azure Service Bus

Its a background worker service that will process all purchase transactions from a service bus Topic and persist into a DB

## Architecture


*Event-Driven
*Clean Architecture

## Design Patterns
*CQRS - WebApi's*
*Event Sourcing*
*Repository Pattern*



# Solution Architecture

**
Core Project (Domain) (The models of what this application represents)
	Entities
	Value Objects
	Aggregates (if doing DDD)
	Enumerations


Application Project  (The use cases or how to solve the user also needs our business logic)
	Application Interfaces
	View Models / DTOs
	Mappers
	Application Exceptions
	Validation
	Logic
	Commands / Queries (if doing CQRS)


Infrastructure Project  (This is external dependencies such as Repositories, controllers, and integrations to services (like external APIs, Stripe, etc.))
	Database
	Web services
	Files
	Message Bus
	Logging
	Configuration

Persistence Project (Different types of data stores. EF)

Presentation Project
	MVC Controllers
	Web API Controllers
	Swagger / NSwag
	Authentication / Authorisation

Common


Test
**

# Tech Stack
**Tech Stack / Libraries**

*.NET Core 6.0*
*Application Insights *
*SQL Server, *
*EntitiyFramework Core,*
*Docker, *
*NUnit Test, *
*Moq *
*AutoFac *
*MediatR - WebApi*
*Azure Service Bus - using Correlation Filter with the property Label*



#ToDo
*Cache Aside*
*Circuit-Breaker*
*Retry*

Solution is designed with Event-Driven architecture, this Worker Service will be consuming and producing events and follow a model of eventual consistency. 
When an underlying service is unavailable it would expect the requests to be processed once the service is back up and running 

We are be using Azure Service Bus to deliver events and messages. 

