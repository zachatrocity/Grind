using System;
using Grind.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;    
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Grind
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        ToDoListManager todoList = new ToDoListManager();
        WeatherAPI weather = new WeatherAPI();
        DispatcherTimer weatherTimer = new DispatcherTimer();
        GitHubAPI githubAPI;
        public MainPage()
        {
            this.InitializeComponent();
            //initalize boxes
            toDoBox.ItemsSource = todoList.toDoList;
           
            doneBox.ItemsSource = todoList.doneList;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings =
         Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("todolist"))
            {
                todoList.ToDoFromXml(roamingSettings.Values["todolist"].ToString());
                toDoBox.ItemsSource = todoList.toDoList;
            }

            if (roamingSettings.Values.ContainsKey("donelist"))
            {
                todoList.DoneFromXml(roamingSettings.Values["donelist"].ToString());
                doneBox.ItemsSource = todoList.doneList;
            }

            if (roamingSettings.Values.ContainsKey("githubEnabled"))
            {
                SettingsPage.Github = Convert.ToBoolean(roamingSettings.Values["githubEnabled"].ToString());
            }

            if (roamingSettings.Values.ContainsKey("githubUsername"))
            {
                SettingsPage.githubUsername = roamingSettings.Values["githubUsername"].ToString();

            }

            if (roamingSettings.Values.ContainsKey("githubPassword"))
            {
                SettingsPage.githubPassword = roamingSettings.Values["githubPassword"].ToString();
            }

            if (roamingSettings.Values.ContainsKey("weatherEnabled"))
            {
                SettingsPage.Weather = Convert.ToBoolean(roamingSettings.Values["weatherEnabled"].ToString());
            }

            if (roamingSettings.Values.ContainsKey("weatherLocation"))
            {
                SettingsPage.weatherLocation = roamingSettings.Values["weatherLocation"].ToString();
            }

            if (SettingsPage.Weather)
                setWeatherWidget();
            else
            {
                weatherWidget.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            if (SettingsPage.Github)
            {
                if (SettingsPage.githubUsername != "")
                {
                    githubAPI = new GitHubAPI(SettingsPage.githubUsername, SettingsPage.githubPassword);
                    setGithubWidget();
                }
                else
                {
                    githubUsernameText.Text = "Add username in Settings.";
                }
            }
            else
            {
                githubWidget.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // Save app data
            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["todolist"] = todoList.ToDoToXml();
            roamingSettings.Values["donelist"] = todoList.DoneToXml();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        private async void setGithubWidget()
        {
            githubWidget.Visibility = Windows.UI.Xaml.Visibility.Visible;
            try
            {
                githubUsernameText.Text = githubAPI.username;
                githubUserImage.ImageSource = await githubAPI.getUserImage();
                githubFollowersText.Text = await githubAPI.getUserFollowers();
                githubFollowingText.Text = await githubAPI.getUserFollowing();
                githubReposText.Text = await githubAPI.getUserRepoCount();
                githubRepoLabel.Visibility = Windows.UI.Xaml.Visibility.Visible;
                githubFollowersLabel.Visibility = Windows.UI.Xaml.Visibility.Visible;
                githubFollowingLabel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            catch (Octokit.RateLimitExceededException)
            {
                githubUsernameText.Text = "Unable to load.";
                githubRepoLabel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                githubFollowersLabel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                githubFollowingLabel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            
        }

        private async void setWeatherWidget()
        {
            //set weather
            string temp = await weather.getCurrentTemp();
            string summary = await weather.getCurrentSummary();
            weatherWidget.Text = temp + "° - " + summary;

            //set it to update every so many minutes
            weatherTimer = new DispatcherTimer();
            weatherTimer.Tick += weatherTimer_Tick;
            weatherTimer.Interval = new TimeSpan(0, 5, 0);
            weatherTimer.Start();
        }

        async void weatherTimer_Tick(object sender, object e)
        {
            //set weather
            string temp = await weather.getCurrentTemp();
            string summary = await weather.getCurrentSummary();
            weatherWidget.Text = temp + "° - " + summary;
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof (SettingsPage));
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }

        private void layoutButton_Click(object sender, RoutedEventArgs e)
        {
            //to do
        }


        private void donebox_OnDragOver(object sender, DragEventArgs e)
        {
            var dropTarget = sender as ListView;
            if (dropTarget == null)
            {
                return;
            }

            if (checkImage.Opacity == 0)
            {
                fadeIn.Begin();
            }
        }


        private void todobox_OnDragOver(object sender, DragEventArgs e)
        {
            var dropTarget = sender as ListView;
            if (dropTarget == null)
            {
                return;
            }

            if (undoImage.Opacity == 0)
                undoFadeIn.Begin();
        }


        private void doneBox_Drop(object sender, DragEventArgs e)
        {
            object doneItems;
            e.Data.Properties.TryGetValue("items", out doneItems);
            var doneList = doneItems as List<String>;
            foreach (var item in doneList){
                todoList.doneList.Add(item);
                todoList.toDoList.Remove(item);
            }

            fadeOut.Begin();
        }

        private void box_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            List<String> items = new List<String>();
            foreach (var str in e.Items)
            {
                items.Add(str.ToString());
            }

            e.Data.Properties.Add("items", items);
            
        }

        private void toDoBox_Drop(object sender, DragEventArgs e)
        {
            object doneItems;
            e.Data.Properties.TryGetValue("items", out doneItems);
            var doneList = doneItems as List<String>;
            foreach (var item in doneList)
            {
                todoList.toDoList.Add(item);
                todoList.doneList.Remove(item);
            }

            undoFadeOut.Begin();
        }

        private void box_DragLeave(object sender, DragEventArgs e)
        {
            var dropTarget = sender as ListView;
            if (dropTarget == null)
            {
                return;
            }

            undoFadeOut.Begin();
        }

        private void doneBox_DragLeave(object sender, DragEventArgs e)
        {
            var dropTarget = sender as ListView;
            if (dropTarget == null)
            {
                return;
            }
            
            fadeOut.Begin();
            
        }

        private void toDoBox_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            try
            {
                var item = ((ListView)sender).SelectedItem.ToString();
                if (item != null)
                {
                    todoList.doneList.Add(item);
                    todoList.toDoList.Remove(item);
                }
            }
            catch(NullReferenceException)
            {
                //do nothing
            }
        }


        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            todoList.toDoList.Add(addToDoTextBox.Text);
            addToDoTextBox.Text = "";
        }

        private void addToDoTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                todoList.toDoList.Add(addToDoTextBox.Text);
                addToDoTextBox.Text = "";
            }
        }
    }
}
