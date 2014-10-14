using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp.Resources;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Tasks;

namespace PhoneApp
{

    public partial class MainPage : PhoneApplicationPage
    {
        public List<ElementAFaireBinding> Lieu;

        private Geolocator geolocator;
        // Constructeur
        public MainPage()
        {
            InitializeComponent();
            //Lieu.Add("Default");
            Lieu = new List<ElementAFaireBinding>{
                new ElementAFaireBinding {Titre = "Default", Statut = "sonnerie"},
                new ElementAFaireBinding {Titre = "Deuxième", Statut = "silencieux"},
                new ElementAFaireBinding {Titre = "Troisième", Statut = "vibreur"},
            };
            listeDesLieux.ItemsSource = Lieu;

            // Exemple de code pour la localisation d'ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Exemple de code pour la conception d'une ApplicationBar localisée
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Définit l'ApplicationBar de la page sur une nouvelle instance d'ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Crée un bouton et définit la valeur du texte sur la chaîne localisée issue d'AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Crée un nouvel élément de menu avec la chaîne localisée d'AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            nom_new.Visibility = Visibility.Visible;
            button_new.Visibility = Visibility.Visible;
        }
        private void Button_New(object sender, RoutedEventArgs e)
        {
            //SaveRingtoneTask ringtoneTask = new SaveRingtoneTask();

            ElementAFaireBinding element = new ElementAFaireBinding { Titre = nom_new.Text, Statut = "vibreur" };
            //String nom = nom_new.Text;
            Lieu.Add(element);
            listeDesLieux.ItemsSource = Lieu;

            nom_new.Text = "";
            nom_new.Visibility = Visibility.Collapsed;
            button_new.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            geolocator = new Geolocator { DesiredAccuracy = PositionAccuracy.High, MovementThreshold = 20 };
            geolocator.StatusChanged += geolocator_StatusChanged;
            geolocator.PositionChanged += geolocator_PositionChanged;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
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
                    status = "Le service de localisation est désactivé dans les paramètres";
                    break;
                case PositionStatus.Initializing:
                    status = "En cours d'initialisation";
                    break;
                case PositionStatus.Ready:
                    status = "Service de localisation prêt";
                    break;
                case PositionStatus.NotAvailable:
                case PositionStatus.NotInitialized:
                case PositionStatus.NoData:
                    break;
            }

            Dispatcher.BeginInvoke(() =>
            {
                Statut.Text = status;
            });
        }

        private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
            {
                Latitude.Text = args.Position.Coordinate.Latitude.ToString();
                Longitude.Text = args.Position.Coordinate.Longitude.ToString();
            });
        } 
       
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        { /*
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10));

                Dispatcher.BeginInvoke(() =>
                {
                    Latitude.Text = geoposition.Coordinate.Latitude.ToString();
                    Longitude.Text = geoposition.Coordinate.Longitude.ToString();
                });
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Le service de location est désactivé dans les paramètres du téléphone");
            }
            catch (Exception ex)
            {
            }*/
        } 
    }
}