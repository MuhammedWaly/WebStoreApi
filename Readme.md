# Web Store

## Technologies Used
  - C#
  - .NET 7
  - Entity Framework Core
  - Identity and JWT for Authentication and Authorization
  - MailKit and MimeKit for sending emails

- **Development Environment:**
  - Visual Studio 2022

 ## Getting Started

1. Open the project in Visual Studio 2022.

2. Set up your database connection in the `appsettings.json` file.

2. Set up your Outlook Email and Password in the `appsettings.json` file.

4. Run the project.


## Application Scenarios and Tasks
### E-commerce Management System 

### Products :
1. **Adding a New Product or Category[Admin]:**
   - Implement the ability to add new products or categories to the system.

2. **Get all Products with relevant details or Get a Product By Id:**
   - Retrieve a paginated list of all products with options for sorting, filtering, and searching
 
3. **Updating Product or Category Information[Admin]:**
   - Allow users to update information for existing products or categories.

4. **Deleting a Product or Category[Admin]:**
   - Provide functionality to delete products or categories from the system.
5. **Get a Product By Id:** 
   - fetch a specific product by its ID.  

### User Management :

2. **GetAllUsers[Admin]**
   - Retrieve a paginated list of all users or filter users by name
3. **GetUserById[Admin]**   
4. **GetUserbyUsername**


### Orders Management :

1. **GetAllOrders**
   - Retrieve a paginated list of all orders.

  
3. **GetOrderById**
   - Retrieve details of a specific order by its ID.

3. **DeleteOrder**
   - Delete a specific order by its ID.

4. **UpdateOrder**
   - Update details of a specific order by its ID.

### Shopping Cart :

1. **GetCart**
   - Retrieve the contents of the shopping cart for the currently authenticated user.

2. **GetPaymentMethods**
   - Retrieve a list of available payment methods.

### Contacts :

1. **Add a New Contact**
   - Create a new contact.

2. **Get All Contacts[Admin]**
   - Retrieve a list of all contacts.

3. **Get Contact by ID[Admin]**
   - Retrieve details of a specific contact by its ID.

4. **Update Contact**
   - Update details of a specific contact by its ID.

5. **Delete Contact**
   - Delete a specific contact by its ID.

6. **Get Subjects Associated with Contacts**
   - Retrieve a list of subjects associated with contacts.

### Account Management :

1. **Register a New Account :** 
   - Create a new user account.
  
2. **Login to an Account :**
   - Authenticate and log in a user.
   
3. **Get User Profile :**
   - Retrieve the profile details of the currently authenticated user.

4. **Update User Profile :**
   - Update the profile details of the currently authenticated user.   

5. **Update Password :**
    - Update the password of the currently authenticated user.

6. **Forgot Password :**
   - Sending an Email with user token to reset password
7. **Reset Password :**
   - Complete the password reset process. the Request Body Include the user's new password and reset token.
   
  
### Services

1. **JWT Reader:**
   - Read the user's ID and role from JWT tokens.
   - Get User Claims from JWT

2. **Order Helper Service:**
   - Retrieve product identifiers from the frontend.
   - Initialize payment methods, payment statuses and order statuses.
