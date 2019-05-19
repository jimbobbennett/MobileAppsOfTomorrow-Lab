# Creating the solution

Let's begin by opening the MobileAppsOfTomorrow-Lab template, a cross-platform Xamarin.Forms app. Clone this repository, or download the Zip file.

## 1. Open the Workshop's Created Solution Template

### PC

1. Copy the **MobileAppsOfTomorrow-Lab** > **FinishedWorkshopSteps** > **1-CreateSolution** > **HappyXamDevs** folder somewhere with a shorter path name, such as to **C:\Projects**. This is because the Android SDK has a path length limitation, and the path created by cloning this repo will be too long for Android to work with.
1. In File Explorer, double-click **HappyXamDevs.sln** inside the copied folder.
1. Ensure the **HappyXamDevs** solution launches in Visual Studio

### Mac

1. In Finder, navigate to **MobileAppsOfTomorrow-Lab** > **FinishedWorkshopSteps** > **1-CreateSolution** > **HappyXamDevs**
1. In Finder, double-click **HappyXamDevs.sln**
1. Ensure the **HappyXamDevs** solution launches in Visual Studio

## 2. Explore the project

This solution contains 4 projects

| Project     | Description |
|-------------|-------------|
| **HappyXamDevs** | The .NET Standard shared code project. This project is shared between all the target platforms and is where most of your code and the user interface files (XAML) will go. |
| **HappyXamDevs.Android** | The Xamarin.Android project which generates the Android-specific binary package to be deployed onto Android devices. |
| **HappyXamDevs.iOS** | The Xamarin.iOS project which generates the iOS-specific binary package to be deployed onto iPhone and iPad devices. |
| **HappyXamDevs.UWP** | The Universal Windows project which can be run on Windows 10 devices. This project is only available when you create the solution with Visual Studio on Windows. If you are using Visual Studio for Mac, right-click this project and select *Delete* to remove it. |

## 3. Build & Run

Build and run each app to see them all working, using the steps in the [setup guide](../SETUP.md).

> **Note:** On Windows you won't be able to run the iOS app unless you are connected to a [Mac build server](https://docs.microsoft.com/xamarin/ios/get-started/installation/windows/connecting-to-mac/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

> **Note:** On macOS you won't be able to build and run the UWP project, so remove this project if you haven't done so already.

![The app running on Android](../Images/Step1-Android.png)

## 4. Next step

Now you have your Xamarin app solution, the next step is to [set up an Azure Functions app](./2-SetupAzureFunctions.md).
