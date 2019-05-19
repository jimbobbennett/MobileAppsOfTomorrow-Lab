# Improving the Mobile App UI

Now that our app is working, it is time to add a bit of polish to the UI.

The `ImageCell` in our `ListView` is functional, but not the prettiest way to show off the amazing photos of happy Xamarin developers; let's make it better

## 1. Creating a new `ViewCell` for the list view

Currently the `ListView` is using an out-the-box `ImageCell` as the item template. This is functional, but doesn't display the photo very well.

1. (PC) In the Visual Studio Solution Explorer, right-click **HappyXamDevs** > **Add** > **New Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click **HappyXamDevs** > **Add** > **New File**

2. (PC) In the **Add New Item** window, select **Installed** > **Visual C# Items** > **Xamarin.Forms** > **Content View**

    > **Warning:** Select **Content View**, _not_ **Content View (C#)**
    - (Mac) In the **New File** window, select **Forms** > **Forms ContentView XAML**
    > **Wanring:** Select **Forms ContentView XAML**, _not_ **Forms ContentView**

3. (PC) In the **Add New Item** window, set the name to be `PhotoCell.xaml`

    - (Mac) In the **New File** set the name to be `PhotoCell.xaml`

4. (PC) In the **Add New Item** window, click **Add**

    - (Mac) In the **New File** window, click **Add**

5. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **PhotoCell.xaml**

6. In the `PhotoCell.xaml` editor, replace the provided template with the following code:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<ViewCell xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="HappyXamDevs.PhotoCell">
    <Frame Padding="25"
           Margin="5">
        <Grid RowSpacing="5">
            <Grid.RowDefinitions>
                <RowDefinition Height="200"/>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="Auto"/>
            </Grid.RowDefinitions>
            <Image Grid.Row="0"
                   Source="{Binding Photo}"
                   Aspect="AspectFill"
                   HeightRequest="200"/>
            <Label Grid.Row="1"
                   Text="{Binding Caption}"
                   FontSize="Large"
                   TextColor="{StaticResource CoolPurple}"/>
            <Label Grid.Row="2"
                   Text="{Binding Tags}"
                   FontAttributes="Italic"
                   TextColor="DarkGray"
                   FontSize="Small"/>
        </Grid>
    </Frame>
</ViewCell>
```

> **About the Code**
>
> `Frame` is used to make this cell stand out. It contains the `Grid`. `Frame` is a UI element that puts a box with rounded corners and a drop-shadow around its content. We add some padding and a margin to keep the content away from the edges, and to ensure each frame has adequate spacing in the `ListView`.
>
> `Grid` creates 3 items laid out one on top of each other - the photo, the caption and the tags. This `Grid` contains 3 rows with 5 points of spacing between each. To keep all the photos the same size, we set the photo row height to be 200 points, and allow the rows for caption and tags be auto-size.
>
> An `Image` is in the first row (row index 0), bound to the `Source` property to the `Photo` property (remember each cell will bind to an instance of `PhotoModel` from the source collection), with a height of 200 points, and the aspect set to `AspectFill`. This aspect setting will cause the photo to stretch equally in all directions to fill the available space, clipping any parts of the photo that go out of bounds.
>
> A `Label` is in the second row (row index 1), binding its `Text` to the `Caption` of the `PhotoModel`. We set the font size to be large, and use the `CoolPurple` color resource for the `TextColor`.
>
> A second `Label` is in the last row, binding its `Text` to the `Tags` property of the `PhotoModel`. We use a small, dark grey, italic font.

2. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **PhotoCell.xaml.cs**

3. In the **PhotoCell.xaml.cs** editor, enter the following code

```csharp
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HappyXamDevs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoCell : ViewCell
    {
        public PhotoCell()
        {
            InitializeComponent();
        }
    }
}
```

## 2. Using the new `ViewCell` in the `ListView`

Once you have your new view cell, it's time to use it inside your list view.

1. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **MainPage.xaml**

2. In the **MainPage.xaml** editor, enter the following code:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             Title="Happy Developers"
             xmlns:viewModels="clr-namespace:HappyXamDevs.ViewModels"
             xmlns:local="clr-namespace:HappyXamDevs"
             x:Class="HappyXamDevs.MainPage">

    <ContentPage.BindingContext>
        <viewModels:MainViewModel />
    </ContentPage.BindingContext>

    <ContentPage.ToolbarItems>
        <ToolbarItem Order="Primary"
                     Icon="TakePhoto.png"
                     Priority="0"
                     Command="{Binding TakePhotoCommand}" />
        <ToolbarItem Order="Primary"
                     Icon="SelectFromLibrary.png"
                     Priority="1"
                     Command="{Binding SelectFromLibraryCommand}" />
    </ContentPage.ToolbarItems>

    <ListView x:Name="PhotosListView"
              ItemsSource="{Binding Photos}"
              IsPullToRefreshEnabled="True"
              RefreshCommand="{Binding RefreshCommand}"
              IsRefreshing="{Binding IsRefreshing}"
              HasUnevenRows="True"
              SeparatorVisibility="None"
              BackgroundColor="#DCDCDC"
              SelectionMode="None">
        
        <ListView.RefreshControlColor>
            <OnPlatform x:TypeArguments="Color">
                <On Platform="iOS" Value="White"/>
                <On Platform="Android, UWP" Value="{StaticResource CoolPurple}" />
            </OnPlatform>
        </ListView.RefreshControlColor>
        
        <ListView.ItemTemplate>
            <DataTemplate>
                <local:PhotoCell />
            </DataTemplate>
        </ListView.ItemTemplate>
    </ListView>

</ContentPage>
```

> **About the Code**
>
> `<ListView HasUnevenRows="True"` By default the `ListView` rows are a fixed size. Since each photo will have a different length for its caption and its tags, this ensures that the row will adjust accordingly.
>
> `SeparatorVisibility="None"` By default the `ListView` displays a black line between each row. This removes that black line.
>
> `BackgroundColor="#DCDCDC"` sets the `ListView` backgroun color to the hex value of `#DCDCDC`
>
> `<DataTemplate> <local:PhotoCell /> </DataTemplate>` tells the `ListView` to use the `PhotoCell` for the template of each row

## Congratulations

We've created a cloud-enabled, AI powered mobile app that runs on iOS, Android and UWP with 95% of code reuse across platforms. You've configured an Azure function app, added functions to upload and download photos, wired these up to Blob storage and Cosmos DB and set up triggers to listen on new photos and analyse them using the power of Azure Cognitive Services. You've also built a mobile app to use these services, as well as taking advantage of the camera present on all mobile phones, and even AI inside your mobile app.

## Next step

Your app is complete. The next step is to [cleaning up your Azure resources](./13-CleaningUp.md) to save money.
