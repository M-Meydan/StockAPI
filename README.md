**London Stock API**  

The London Stock API is an MVP system that provides basic functionality for trade notifications and retrieving stock data. This document provides an overview of the API's structure, endpoints, and the practices incorporated to ensure security, reliability, and scalability. 

**API Project Structure**

The API project follows a clean architectural practice and is organized into the following folders: 

- **App**: Contains application-specific business code. This folder is where the core logic of the API resides.  
- **Controllers**: Contains the API controllers responsible for orchestrating the execution of business logic and returning appropriate responses. 
- **Domain**: Contains the core domain entities of the system. 
- **Infrastructure**: Includes implementations of repositories and data access components. 

**Trades API**

Provides functionality for trade notifications from authorized brokers. It includes the following endpoint: 

- **POST /api/trades**: Receives the trade notifications from authorized brokers. This information stored in a SQLite database and a trade Id returned. Additionally, a notification is published, which triggers relevant handlers. Currently, the UpdateStockAveragePriceHandler picks up the notification to update the related stock price based on the trade average. 

**Stocks API**:  

Provides functionality for retrieving stock data. It includes the following endpoints: 

- **GET /api/stocks/tickers/{symbol}:** This endpoint returns stock data from the SQLite database for a specific ticker symbol.  
- **GET /api/stocks/tickers:** This endpoint returns a list of stock data from the SQLite database. If no symbols are provided in the query parameters, it returns all stocks. Otherwise, it filters the stocks based on the provided symbol/s (e.g. vod,shel). 

Currently, the API key for authentication is added in the appssettings.json (abc123). It has to be provided in Swagger to test the endpoints. 

The API project incorporates several practices and components that contribute to the security, reliability and scalability of the API: 

- **SQLite database**: Used for storing trade and stock data. 
- **Exception Handling**: All unhandled exceptions captured and logged. This helps in identifying and resolving issues, improving the reliability of the API. 
- **API Key Authentication**: Provides security and protects the API from unauthorized access. 
- **MediatR and CQRS**: The project follows the CQRS pattern using MediatR. This pattern separates commands and queries, simplifying the logic and enabling scalability by decoupling write operations (commands) from read operations (queries). 
- **Dependency Injection**: The project uses dependency injection extensively, allowing for the easy management and swapping of dependencies. This promotes modularity and makes it easier to test and maintain the codebase. 
- **AutoMapper**: Used for object-to-object mapping, reducing the manual effort required for mapping data between entities and DTOs. This simplifies development and reduces the risk of errors. 
- **FluentValidation**: Ensures data integrity and improves reliability by rejecting invalid requests. 
- **Swagger Documentation**: This helps developers understand and consume the API effectively, reducing integration issues and improving reliability. 

**Is this system scalable?** 

The current API project lays a foundation for scalability at app level by using (CQRS) pattern and the MediatR library. This allows scaling the write operations independently from the read queries, enabling better performance and scalability.  

**How can it cope with high traffic?** 

Using a message queue and scalable asynchronous processors for event/notification handling would help the API handle increased traffic loads efficiently. 

**Can you identify bottlenecks?** 

Initially, the relational database would handle the load. When the traffic load starts to increase the dataset will also grow along with it so potential optimizations like indexing, query optimization and caching could be introduced for better performance. However, concurrent inserts and updates, especially when updating trade data and calculating average prices concurrently, can stress the database and lead to synchronization issues. Careful consideration and synchronization mechanisms should be implemented to maintain a consistent data state. 

**Suggest an improved design and architecture** 

- **Scaling the Data Layer**: implementing a scalable and high-performance database solution, such RDBMS or a NoSQL database. 
- **Caching Mechanism**: a caching mechanism could be added, such as Redis or a distributed cache, to improve response times and reduce the load on the database for frequently accessed data. 
- **Message Queues & Asynchronous Processing**: introducing messaging and asynchronous processors for operations, such as database queries, notifications and events. This approach improves the overall throughput and responsiveness of the API. 

Note: The implementation and test coverage of this MVP system are limited due to time constraints. 
