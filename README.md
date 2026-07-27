# Employee23BITV01

## Members

 Student ID 
| 2311554285 - Nguyễn Lê Huy Hoàng | Implemented the Order module including Order listing, Create Order with transaction handling, automatic total calculation, stock update, search functionality, statistics dashboard, and integration/testing |
| 2311556183 - Nguyễn Thanh Danh | Database setup, Entity Framework scaffolding, initial project setup, and supporting project structure. |

---

## Technologies

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap



## Features

### Order Management

- Display all orders
- Display product type count per order

### Create Order

- Customer Name
- Product selection
- Quantity input
- Automatic Unit Price
- Automatic Total Amount
- Automatic Stock Update
- Database Transaction
- Rollback when stock is insufficient

### Search

- Search by Customer Name
- Filter by Minimum Total Amount
- Filter by Maximum Total Amount

### Statistics

- Total Revenue
- Top 3 Best Selling Products


## Database

- Categories
- Products
- Orders
- OrderDetails


## How to Run

1. Restore the SQL Server database.
2. Update the connection string in appsettings.json.
3. Run the application.
4. Open /Orders.
