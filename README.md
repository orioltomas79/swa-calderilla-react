# **Calderilla**

## **Technologies Used**

Key technologies or frameworks used in the project:

- **Azure Static Web Apps**: 
   - [Azure App Service](https://azure.microsoft.com/en-us/products/app-service/static/)  
   - [Azure App Service Docs](https://learn.microsoft.com/en-us/azure/static-web-apps/)

- **Frontend**: React  

- **Backend**: Azure Functions (C#)  

## **Online courses on Azure Static Web Apps**

- Authenticate users with Azure Static Web Apps
https://learn.microsoft.com/en-us/training/modules/publish-static-web-app-authentication/
- Upload images to Azure Blob Storage from a static web app
https://learn.microsoft.com/en-us/training/modules/blob-storage-image-upload-static-web-apps/

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

   Alternatively, if you're using Windows 11, you can run the run.ps1 file, which opens a single Windows Terminal instance with three tabs (one for each command).



