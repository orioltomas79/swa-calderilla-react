# **Calderilla**

## **Table of Contents**

- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [How do I build and test the app locally](#build-and-test-locally)

## **Technologies Used**

Key technologies or frameworks used in the project:
- This is a static web app: [Azure App Service](https://azure.microsoft.com/en-us/products/app-service/static/)  
- **Frontend**: React  
- **Backend**: Azure Functions (C#)  

## **How to Build and Test the App Locally**

To build and test your app locally, follow these steps:

1. **Run the Backend Project:**
   - Open the project in Visual Studio and build it.
   - Start the backend server `http://localhost:7054/api/`.

2. **Start the Frontend Application:**
   - Open the frontend in your development environment.
   - Start the frontend `http://localhost:5173/`.

3. **Set Up the API Proxy with SWA:**
   - Use **SWA (Static Web Apps)** to connect the frontend to the backend.
   - Run the following command to start SWA with the development server:
     ```bash
     swa start http://localhost:5173 --api-devserver-url http://localhost:7054/api
     ```

4. **Access the Application:**
   - Once everything is set up, open your browser and navigate to `http://localhost:4280/` to view the app.



