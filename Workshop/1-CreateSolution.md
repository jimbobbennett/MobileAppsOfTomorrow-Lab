# Creating the solution

Let's begin by opening the MobileAppsOfTomorrow-Lab template, a cross-platform Xamarin.Forms app.

## 1. Open the Workshop's Created Solution Template

1. (PC) In File Explorer, navigate to **MobileAppsOfTomorrow-Lab** > **FinishedWorkshopSteps** > **1-CreateSolution** > **HappyXamDevs**
    - (Mac) In Finder, navigate to **MobileAppsOfTomorrow-Lab** > **FinishedWorkshopSteps** > **1-CreateSolution** > **HappyXamDevs**

2. (PC) In File Explorer, double-click **HappyXamDevs.sln**
    - (Mac) (Mac) In Finder, double-click **HappyXamDevs.sln**

3. Ensure the **HappyXamDevs** solution launches in Visual Studio

## 2. Explore the project

This solution contains 3 projects on Mac, and 4 on Windows.

| Project     | Description |
|-------------|-------------|
| **HappyXamDevs** | The .NET Standard shared code project. This project is shared between all the target platforms and is where most of your code and the user interface files (XAML) will go. |
| **HappyXamDevs.Android** | The Xamarin.Android project which generates the Android-specific binary package to be deployed onto Android devices. |
| **HappyXamDevs.iOS** | The Xamarin.iOS project which generates the iOS-specific binary package to be deployed onto iPhone and iPad devices. |
| **HappyXamDevs.UWP** _(PC only)_| The Universal Windows project which can be run on Windows 10 devices. This project is only available when you create the solution with Visual Studio on Windows. |

## 3. Build & Run

Build and run each app to see them all working, using the steps in the [setup guide](../SETUP.md).

> **Note:** On Windows you won't be able to run the iOS app unless you are connected to a [Mac build server](https://docs.microsoft.com/xamarin/ios/get-started/installation/windows/connecting-to-mac/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

> **Note:** On macOS you won't be able to build and run the UWP project, and a UWP project won't be present if the solution was created with Visual Studio for Mac.

![The app running on Android](../Images/Step1-Android.png)

## 4. Next step

Now you have your Xamarin app solution, the next step is to [set up an Azure Functions app](./2-SetupAzureFunctions.md).
