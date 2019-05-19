# Create Mobile App Azure Service

Now that our Azure Function App is configured, we will implement a service in the mobile app to call the function app.

We'll be using the [Microsoft.Azure.Mobile.Client](https://www.nuget.org/packages/Microsoft.Azure.Mobile.Client/) NuGet package to call Azure Functions.

> This package can support adding an authentication flow to your mobile app using Facebook, Twitter or Microsoft Auth. You can read more about this in the [App Service docs](https://docs.microsoft.com/en-gb/azure/app-service/configure-authentication-provider-facebook).

## 1. Installing NuGet Packages

1. Open our newly created Xamarin.Forms app in Visual Studio

2. (PC) In Visual Studio, right-click the `HappyXamDevs` solution > **Manage NuGet Packages For Solution..**

    - (Mac) In Visual Studio for Mac, right-click the `HappyXamDevs` project > **Add** > **Add NuGet Packages**

3. (PC) In the **NuGet Package Manager** window, select **Browse**

    - (Mac) _Skip this step_

4. In the **NuGet Package Manager** window, in the search bar, enter **Microsoft.Azure.Mobile.Client**

5. In the **NuGet Package Manager** window, in the search results, select **Microsoft.Azure.Mobile.Client**

6. (PC) In the **NuGet Package Manager** window, select **Install**

    ![Adding the Microsoft.Azure.Mobile.Client NuGet package on PC](../Images/VS2017AddMobileClientNuget.png)

    - (Mac) In the **NuGet Package Manager** window, select **Add Package**

    ![Adding the Microsoft.Azure.Mobile.Client NuGet package on Mac](../Images/VSMacAddMobileClientNuget.png)

7. (PC) _Skip this step_

    - (Mac) In Visual Studio for Mac, right-click the **HappyXamDevs.Android** project > **Add** > **Add NuGet Packages**

8. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, in the search results, select **Microsoft.Azure.Mobile.Client**

9. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, select **Add Package**

10. (PC) _Skip this step_

    - (Mac) In Visual Studio for Mac, right-click the **HappyXamDevs.iOS** project > **Add** > **Add NuGet Packages**

11. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, in the search results, select **Microsoft.Azure.Mobile.Client**

12. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, select **Add Package**

## 2. Creating a cross-platform Azure service

1. In the Visual Studio Solution Explorer, right-click on the second-from-the-top **HappyXamDevs** option > **Add** > **New Folder**
    > **Warning:** Do not select **Add Solution Folder**. If you are given the option **Add Solution Folder**, you have right-clicked on top-most **HappyXamDevs** option.

2. In the Visual Studio Solution Explorer, name the new folder `Services`

3. (PC) In the Visual Studio Solution Explorer, right-click on the newly created **Services** folder > **Add** > **Class**

    - (Mac) On Visual Studio for Mac, right-click on the newly created `Services` folder > **Add** > **New File**

4. In the **Add New Item** window, name the file `IAzureService.cs`

5. (PC) In the **Add New Item** window, click **Add**
    - (Mac) In the **Add New Item** window, click **New**

6. In the `IAzureService.cs` editor, add the following code:

```csharp
using System.Threading.Tasks;

namespace HappyXamDevs.Services
{
    public interface IAzureService
    {
    }
}
```

1. In the Visual Studio Solution Explorer, right-click on **Services** folder > **Add** > **Class**

    - (Mac) On Visual Studio for Mac, right-click on the **Services** folder > **Add** > **New File**

2. In the **Add New Item** window, name the file `AzureServiceBase.cs`

3. (PC) In the **Add New Item** window, click **Add**
    - (Mac) In the **Add New Item** window, click **New**

4. In the `AzureServiceBase.cs` editor, add the following code
    > **Note:** Replace `[Your Function App Name]` with the name of your Azure Function App, e.g. `HappyXamDevsFunction-Minnick`

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace HappyXamDevs.Services
{
    public abstract class AzureServiceBase : IAzureService
    {
        protected const string AzureAppName = "[Your Function App Name]";
        protected readonly static string FunctionAppUrl = $"https://{AzureAppName}.azurewebsites.net";

        protected AzureServiceBase()
        {
            Client = new MobileServiceClient(FunctionAppUrl);
        }

        public MobileServiceClient Client { get; }
    }
}
```

> **About The Code**
>
>`string AzureAppName` and `string FunctionAppUrl` will be used in the platform-specific projects to connect to your Function back end
>
> `MobileServiceClient Client` provides APIs for back-end services

## Next step

Now that your Azure service has been created, the next step is to [wire up the camera using a Xamarin plugin](./5-WireUpTheCamera.md).