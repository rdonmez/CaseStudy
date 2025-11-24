# BeymenCaseStudy

## Requirements
- Docker
- Docker Compose

## How to Run
1. Clone the repo
2. Navigate to repo directory
3. Run the following command
   - ``docker-compose up``
4. Wait for RabbitMQ to up and running. Service containers restart till RabbitMQ is ready.

## Accessing the APIs
   - StockService API: http://localhost:5001/swagger/index.html
   - OrderService API: http://localhost:5002/swagger/index.html
   - NotificationService API: http://localhost:5003/swagger/index.html

Note: Notification Service doest not send a real notification.
