# SimpleEcommerce

A simple eCommerce application built using plain HTML, CSS, and JavaScript. This application allows users to register, log in, add products to a cart, and place orders. 

## Features
- **User Authentication**: Simple login and registration functionality.
- **Product Ordering**: Users can add products to the cart and place orders.
- **Dynamic Order Management**: Orders are created dynamically when products are added to the cart with a "Pending" status.
- **Admin Workflow**: Once the order is completed by the user, an admin will manage the workflow for order processing.
- **Soft Deletion**: Orders can be canceled by the user by changing the status to "Cancelled".
- **Product Addition** : Only Admins can add products and the user will be able to view them and add to cart whatever he needs

## Frontend Setup
The frontend uses simple HTML, CSS, and JavaScript. For best functionality, it is recommended to run the frontend on a live server (e.g., using [Live Server extension](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer) for VS Code).

### Running the Frontend
1. Install the [Live Server extension](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer) in your code editor (VS Code recommended).
2. Open the project in VS Code and start the Live Server.
3. The app will be available at `http://localhost:5500` (or another port depending on your setup).
4. The Frontend is in folder MyEcommerceWebPage

## Backend Setup

### Database Configuration
The backend does not require a manual database update. Follow these steps to get started:

1. **Configure the Connection String**: 
   - Open the `appsettings.json` file in the backend project.
   - Update the `ConnectionStrings` property with the correct server details for your database.

2. **Run the Project**: 
   - After configuring the connection string, run the backend project.
   - The application will automatically create the necessary database and tables.
3. **Add Admin User**
   - Register any user to be admin then update the role in the database of that user to be 0 (Corresponding to the "Admin" in the enum)

### Order Workflow
- **Add Products**: When a product is added to the cart, an order with the status "Pending" is automatically created.
- **Complete Order**: Once the user is finished adding products, they can complete the order. This triggers the workflow for admin management.
- **Order Delete**: The user can cancel their order by changing the status to "Cancelled". This action is a soft delete, ensuring that the order remains in the system for tracking purposes.

## Technologies Used
- **Frontend**: HTML, CSS, JavaScript
- **Backend**: ASP.NET Core (.Net Core 6)
- **Database**: Microsoft SQL Server

## Contributing
Feel free to request any update

