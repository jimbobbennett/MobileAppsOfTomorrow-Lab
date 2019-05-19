# Set up an Azure Functions app

For this app, we will create our back-end using Azure Functions.

## 1. Create the Functions app in the Azure portal

1. In your browser, navigate to the [Azure portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn)

2. In the Azure Portal, on the left-hand toolbar, click **+ Create a resource**  

    > **Note:** If the toolbar is collapsed, it will be shown as a green **+**

3. In the **Search the marketplace** box, type **Function app**  

4. On your keyboard, tap the **Return** key

5. In the search results, select **Function App**

   ![Searching for function app in the portal](../Images/PortalSearchFuncApp.png)

6. At the bottom of the **Function App** window, click **Create**

7. In the **Function App** window, enter the following:

    - **App Name**: HappyXamDevsFunction-[Your Last Name]
      - E.g. `HappyXamDevsFunction-Minnick`

    > **Note:** App Name needs to be unique because this will be used as the domain for your Functions App. If the App Name entered is unique, you will see a green ✔**️** on the right; if not, you will see a red **X**

    - **Subscription**: [Select your Azure Subscription]

    > Note: As a reminder, if you don't have an Azure account, [you can create a free one](https://azure.microsoft.com/free?WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

    - **Resource Group**
        - [x] Create new
        - **Name:** HappyXamDevs
    - **OS:** Windows
    - **Hosting Plan:** Consumption
    - **Location:** Pick the location closest to you, for example West Europe
    - **Runtime Stack**: .NET
    - **Storage**
        - [x] Create new
        - Leave the default value in here
    - **Application Insights**
        - **Disabled**

8. Click **Create**

9. Standby while the Functions App is provisioned in Azure

## Next step

Now you have your Azure Function App created, the next step is to [create an Azure Service inside the mobile app](./3-CreateAnAzureServiceInTheMobileApp.md).
