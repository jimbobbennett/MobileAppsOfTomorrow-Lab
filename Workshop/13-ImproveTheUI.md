# Improving the mobile app UI

Now that your app is working, it is time to add a bit of polish to the UI:

* The photos only get refreshed when the app is launched, so it would be good to support pull to refresh - a standard way to refresh lists of data. 
* The image cell you are using is functional, but not the best way to show of you amazing photos of happy Xamarin developers, so it would be good to improve the UI for these cells.

## Implementing pull to refresh

The Xamarin.Forms list view has pull to refresh built in, all you have to do is turn it on and wire up a command that gets executed whenever the user pulls the list down. When you pull, the list will show a spinner and its `IsRefreshing` property will be set to true. The spinner wil remain until this property is set to `false`, usually by binding this to a boolean value that you set to `false` in your view model.

### Adding pull to refresh support to the view model

1. Open the `MainViewModel` class and add a new `bool` property called `IsRefreshing`. Implement this using a backing field and return this in the properties `get` implementation.

    ```cs
    bool isRefreshing;
    public bool IsRefreshing
    {
        get => isRefreshing;
    }
    ```

2. For the properties `set` implementation, use the `Set` method from the `BaseViewModel` base class, as this will not only update the value, but will raise a property changed notification to tell the UI to update.

    ```cs
    public bool IsRefreshing
    {
        get => isRefreshing;
        set => Set(ref isRefreshing, value);
    }
    ```

3. This property will be bound to the `IsRefreshing` property on the list view, so make sure it is set to `false` after the `RefreshCommand` has finished executing by setting the value at the end of the `Refresh` method.

    ```cs
    async Task Refresh()
    {
        ...
        IsRefreshing = false;
    }
    ```

### Adding pull to refresh support to the UI

1. Head to the `MainPage.xaml`. Turn on pull to refresh on the list view by setting `IsPullToRefreshEnabled` to `true`.

    ```xml
    <ListView IsPullToRefreshEnabled="True"
              ...
    ```

2. Bind the `RefreshCommand` and the `IsRefreshing` properties to the list view.

    ```xml
     <ListView IsPullToRefreshEnabled="True"
              RefreshCommand="{Binding RefreshCommand}"
              IsRefreshing="{Binding IsRefreshing}"
              ...
    ```

### Test it out

Launch the app on your platform of choice. Take a photo and give it a few seconds to upload, then pull the list view to refresh. You will see a spinner whilst the refreshing is happening, then the new photo will appear at the top of the list.

## Improving the photos in the list

### Creating a new view cell for the list view

Currently the list view is using an out-the-box `ImageCell` as the item template. This is functional, but doesn't really show of the photo very well. You don't have to use the provided cell types, you can create your own.

<!-- Link needed -->

1. Create a new XAML content view in the root of the core project called `PhotoCell`.

   * For Visual Studio 2017 on Windows, right-click on the `HappyXamDevs` project and select _Add->New Item..._. Choose _Visual C#->Xamarin.Forms_ from the tree on the left, then select _Content View_ (not _Content View C#_). Set the name to be `PhotoCell` and click "Add".
   * For Visual Studio for Mac, right-click on the `HappyXamDevs` project and select _Add->New File..._. Select _Forms_ on the left, then select _Forms ContentView XAML_, set the name to be `PhotoCell` and click "New".

2. Open the `PhotoCell.xaml.cs` file and change the cells base class to `ViewCell`.

    ```cs
    public partial class PhotoCell : ViewCell
    ```

3. Open the `PhotoCell.xaml` file and change the root element to be a `ViewCell`. Delete all the content of this cell.

    ```xml
    <?xml version="1.0" encoding="UTF-8"?>
    <ViewCell xmlns="http://xamarin.com/schemas/2014/forms"
                xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                x:Class="HappyXamDevs.PhotoCell">
    </ViewCell>
    ```

4. For this cell we want 3 items laid out one on top of each other - the photo, the caption and the tags. Set the content to a `Grid` and configure 3 rows with 5 points of spacing between. To keep all the photos the same size, set the photo row height to be 200 points, and let the rows for caption and tags be auto-sized (so that if they wrap there is space).

    ```xml
    <Grid RowSpacing="5">
        <Grid.RowDefinitions>
            <RowDefinition Height="200"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
    </Grid>
    ```

5. Add an `Image` to the first row (row index 0), bind the `Source` property to the `Photo` property (remember each cell will bind to an instance of `PhotoViewModel` from the source collection), with a height of 200 points, and the aspect set to `AspectFill`. This aspect setting will cause the photo to stretch equally in all directions to fill the available space, clipping any parts of the photo that go out of bounds.

    ```xml
    <Image Grid.Row="0"
           Source="{Binding Photo}"
           Aspect="AspectFill"
           HeightRequest="200"/>
    ```

6. Add a `Label` to the second row (row index 1), binding the `Text` to the `Caption` of the view model. Set the font size to be large, and use the `CoolPurple` color resource for the `TextColor`.

    ```xml
    <Label Grid.Row="1"
           Text="{Binding Caption}"
           FontSize="Large"
           TextColor="{StaticResource CoolPurple}"/>
    ```

7. Add another `Label` to the last row, binding the text to the `Tags` property. Use a small, dark grey, italic font.

    ```xml
    <Label Grid.Row="2"
           Text="{Binding Tags}"
           FontAttributes="Italic"
           TextColor="DarkGray"
           FontSize="Small"/>
    ```

8. To make this cell stand out more, wrap the `Grid` in a `Frame`. This is a UI element that puts a box with rounded corners and a drop-shadow around it's content. Add some padding and a margin to keep the content away from the edges, and to ensure each frame in the list view is a little way away from the ones above and below.

    ```xml
    <Frame Padding="25"
           Margin="5">
        <Grid RowSpacing="5">
            ...
        </Grid>
    </Frame>
    ```

The full code for this view cell is below.

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

### Using the new view cell

Once you have your new view cell, it's time to use it inside your list view.

1. Head to `MainPage.xaml` and add a new xml namespace for the root folder of the project.

    ```xml
    <ContentPage xmlns:local="clr-namespace:HappyXamDevs"
                 ...
    ```

2. Remove the `ImageCell` from the list view and replace it with the new `PhotoCell`. There is no need to bind anything here as this happens inside the `PhotoCell`.

    ```xml
    <ListView.ItemTemplate>
        <DataTemplate>
            <local:PhotoCell />
        </DataTemplate>
    </ListView.ItemTemplate>
    ```

3. By default list views have fixed size rows, so you will need to turn on `HasUnevenRows` to tell the list view to size its rows based off the content.

    ```xml
    <ListView HasUnevenRows="True"
              ...
    ```

4. List views also have a separator by default, leading to a faint grey line between each row. You can turn this off by setting the `SeparatorVisibility` to none.

    ```xml
    <ListView HasUnevenRows="True"
              SeparatorVisibility="None"
              ...
    ```

5. The default background color of a list view is white, so to help the rows stand out change this to an off-white color.

    ```xml
    <ListView HasUnevenRows="True"
              SeparatorVisibility="None"
              BackgroundColor="#DCDCDC"
              ...
    ```

The full code for the list view is below.

```xml
<ListView IsPullToRefreshEnabled="True"
          RefreshCommand="{Binding RefreshCommand}"
          IsRefreshing="{Binding IsRefreshing}"
          ItemsSource="{Binding Photos}"
          HasUnevenRows="True"
          BackgroundColor="#DCDCDC"
          SeparatorVisibility="None">
    <ListView.ItemTemplate>
        <DataTemplate>
            <local:PhotoCell />
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

## Try it out

Launch the app on different platforms and see how the new list looks. You will be able to run it on one platform, add a photo and see it from another platform.

__Congratulations__ - you've created a cloud-enabled, AI powered mobile app that runs on iOS, Android and UWP with 95% of code reuse across platforms. You've configured an Azure function app using Facebook authentication, added functions to upload and download photos, wired these up to Blob storage and Cosmos DB and set up triggers to listen on new photos and analyse them using the power of Azure Cognitive Services. You've also built a mobile app to use these services, as well as taking advantage of the camera present on all mobile phones, and even AI inside your mobile app.

## Next step

Your app is complete. The next step is to [cleaning up your Azure resources](./14-CleaningUp.md) to save money.