using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Data;
using System.Text;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp.Resources;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Tasks;
using Expression.Blend.SampleData.LieuxListBox;
using System.Runtime.CompilerServices;

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
                new ElementAFaireBinding {Titre = "Default", Url = "http://google.com"},
                new ElementAFaireBinding {Titre = "Deuxième", Url = "http://google.fr"},
            };
            listeDesLieux.ItemsSource = Lieu;

        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            if (nom_new.Visibility == (Visibility.Collapsed))
            {
                nom_new.Visibility = Visibility.Visible;
                button_new.Visibility = Visibility.Visible;
            }
            else
            {
                nom_new.Text = "";
                nom_new.Visibility = Visibility.Collapsed;
                button_new.Visibility = Visibility.Collapsed;

            }
        }
        private void Button_New(object sender, RoutedEventArgs e)
        {
            List<ElementAFaireBinding> nouvelleListe = new List<ElementAFaireBinding>(Lieu);
            nouvelleListe.Add(new ElementAFaireBinding { Titre = nom_new.Text, Url = "http://google.co" });
            Lieu = nouvelleListe;
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

        private void button_url_Click(object sender, RoutedEventArgs e)
        {
            /*
            string selection = string.Empty;
            foreach (string choix in listeDesLieux.SelectedItems)
            {
                selection += choix + ";";
            }
            Selection.Text = selection;*/
            //ElementAFaireBinding element = (string)listeDesLieux.SelectedItem;

                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri("http://google.com", UriKind.Absolute);
                webBrowserTask.Show();

        }

        private void button_modifier_Click(object sender, RoutedEventArgs e)
        {
           
            //listeDesLieux.button_add_point.Visibility = Visibility.Visible;

            //listeDesLieux.button_add_point.Visibility = Visibility.Visible;
        }
        private void button_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //MessageBox.Show(Url);
        }

        /**
         * Supprime l'item sélectionné
         */
        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            int i = listeDesLieux.SelectedIndex;
            int count = Lieu.Count;
            // Vérification si item sélectionner
            if (i != -1)
            {
                //MessageBox.Show(Convert.ToString(Selected.Content));
                List<ElementAFaireBinding> nouvelleListe = new List<ElementAFaireBinding>(Lieu);
                List<ElementAFaireBinding> temp = nouvelleListe.GetRange(0, i); //.Except(deleteElement);
                List<ElementAFaireBinding> temp2 = nouvelleListe.GetRange(i+1, count-i-1);
                nouvelleListe = temp;
                nouvelleListe.AddRange(temp2);
                Lieu = nouvelleListe;
                listeDesLieux.ItemsSource = Lieu;
            }
        }
        /*
         * private void button8_Click(object sender, System.EventArgs e)

{

string url = "http://www.google.com";

System.Diagnostics.Process.Start(url);

}
    */
    }
}