using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using LearningMachine.Resources;

namespace LearningMachine
{
    public partial class MainPage : PhoneApplicationPage
    {

        private Geolocator geolocator;

        alglib.clusterizerstate s;
        alglib.kmeansreport rep;



        // Data context for the local database
        private RecordedCoordinatesDataContext recordedCoordinatesDB;

        // Define an observable collection property that controls can bind to.
        private ObservableCollection<RecordedCoordinatesItem> _recordedCoordinatesItems;

        public ObservableCollection<RecordedCoordinatesItem> RecordedCoordinatesItems
        {
            get
            {
                return _recordedCoordinatesItems;
            }
            set
            {
                if (_recordedCoordinatesItems != value)
                {
                    _recordedCoordinatesItems = value;
                }
            }
        }

        
        // Constructeur
        public MainPage()
        {
            InitializeComponent();

            // Connect to the database and instantiate data context.
            recordedCoordinatesDB = new RecordedCoordinatesDataContext(RecordedCoordinatesDataContext.DBConnectionString);

            // Data context and observable collection are children of the main page.
            this.DataContext = this;

        }

            
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Define the query to gather all of the to-do items.
            var recordedCoordinatesItemsInDB = from RecordedCoordinatesItem coords in recordedCoordinatesDB.RecordedCoordinatesItems
                                               select coords;

            // Execute the query and place the results into a collection.
            RecordedCoordinatesItems = new ObservableCollection<RecordedCoordinatesItem>(recordedCoordinatesItemsInDB);
            locationCount.Text = _recordedCoordinatesItems.Count + "";
            
            geolocator = new Geolocator { DesiredAccuracy = PositionAccuracy.High, MovementThreshold = 20 };
            geolocator.StatusChanged += geolocator_StatusChanged;
            geolocator.PositionChanged += geolocator_PositionChanged;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            recordedCoordinatesDB.SubmitChanges();

            geolocator.PositionChanged -= geolocator_PositionChanged;
            geolocator.StatusChanged -= geolocator_StatusChanged;
            geolocator = null;

            base.OnNavigatedFrom(e);
        }

        private void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            string status = "";

            switch (args.Status)
            {
                case PositionStatus.Disabled:
                    status = "Disabled";
                    break;
                case PositionStatus.Initializing:
                    status = "Initializing";
                    break;
                case PositionStatus.Ready:
                    status = "Ready";
                    break;
                case PositionStatus.NotAvailable:
                    status = "Not Available";
                    break;
                case PositionStatus.NotInitialized:
                    status = "Not Initialized";
                    break;
                case PositionStatus.NoData:
                    status = "No Data";
                    break;
            }

            Dispatcher.BeginInvoke(() =>
            {
                System.Diagnostics.Debug.WriteLine("Status = " + status);
            });
        }

        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
            {
            });
        }

        private async void Add_Location_Button_Clicked(object sender, RoutedEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 5;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );


                // Create a new to-do item based on the text box.
                RecordedCoordinatesItem newRecordedCoordinates = new RecordedCoordinatesItem { Latitude = geoposition.Coordinate.Latitude, Longitude = geoposition.Coordinate.Longitude };

                // Add a to-do item to the observable collection.
                RecordedCoordinatesItems.Add(newRecordedCoordinates);

                // Add a to-do item to the local database.
                recordedCoordinatesDB.RecordedCoordinatesItems.InsertOnSubmit(newRecordedCoordinates);
                latestMeasure.Text = geoposition.Coordinate.Latitude.ToString() + ", " + geoposition.Coordinate.Longitude.ToString();
            }
            catch (Exception ex)
            { }
            locationCount.Text = RecordedCoordinatesItems.Count + "";
        }

        private void Run_Clustering_Clicked(object sender, RoutedEventArgs e)
        {
            int clusterNumber = int.Parse(ClusterNumber.Text);

            double[,] coords = new double[RecordedCoordinatesItems.Count, 2];

            int j = 0;
            foreach (RecordedCoordinatesItem item in RecordedCoordinatesItems)
            {
                coords[j, 0] = item.Latitude;
                coords[j, 1] = item.Longitude;
                ++j;
            }

            alglib.clusterizercreate(out s);
            alglib.clusterizersetpoints(s, coords, 2);
            alglib.clusterizersetkmeanslimits(s, 5, 0);
            alglib.clusterizerrunkmeans(s, clusterNumber, out rep);

            ResultsDisplay.Text = "";
            System.Diagnostics.Debug.WriteLine(rep.c.Length);
            for (int i = 0; i < rep.c.Length / 2; ++i)
            {
                ResultsDisplay.Text += "Cluster #" + i + ": lat" + Math.Round(rep.c[i, 0], 3) + ", lon" + Math.Round(rep.c[i, 1], 3) + "\n";
            }
        }


        private async void WhereAmI_Click(object sender, RoutedEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 5;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );
                int myClusterIndex = 0;
                double minDistance = distance(geoposition.Coordinate.Latitude, rep.c[0, 0], geoposition.Coordinate.Longitude, rep.c[0, 1]);
                for (int i = 1; i < rep.c.Length / 2; ++i)
                {
                    double d = distance(geoposition.Coordinate.Latitude, rep.c[i, 0], geoposition.Coordinate.Longitude, rep.c[i, 1]);
                    if (d < minDistance)
                    {
                        minDistance = d;
                        myClusterIndex = i;
                    }
                }
                MessageBox.Show("You are in cluster #" + myClusterIndex);
            }
            catch (Exception ex)
            { }
        }

        private double distance(double lat1, double lat2, double lon1, double lon2)
        {
            return Math.Sqrt(Math.Pow(lon2 - lon1, 2) + Math.Pow(lat2 - lat1, 2));
        }

    }

    public class RecordedCoordinatesDataContext : DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:/RecordedCoordinatesItem.sdf";

        public RecordedCoordinatesDataContext(string connectionString) : base(connectionString)
        { }

        public Table<RecordedCoordinatesItem> RecordedCoordinatesItems;
    }

    [Table]
    public class RecordedCoordinatesItem
    {
        private int _recordedCoordinatesItemId;
        private double _latitude, _longitude;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int RecordedCoordinatesItemId
        {
            get
            {
                return _recordedCoordinatesItemId;
            }
            set
            {
                if (_recordedCoordinatesItemId != value)
                {
                    _recordedCoordinatesItemId = value;
                }
            }
        }

        [Column]
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                if (_latitude != value)
                {
                    _latitude = value;
                }
            }
        }


        [Column]
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                if (_longitude != value)
                {
                    _longitude = value;
                }
            }
        }
    }
}