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
            Lieu = new List<ElementAFaireBinding>{
                new ElementAFaireBinding {Titre = "Polytech", Url = "http://google.com"},
                new ElementAFaireBinding {Titre = "Maison", Url = "http://facebook.com"},
            };
            listeDesLieux.ItemsSource = Lieu;

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
        /*
         * Affiche ou Cache les champs pour ajouter un nouveau lieu et url
         */
        private void Button_Add(object sender, RoutedEventArgs e)
        {
            if (nom_new_lieu.Visibility == (Visibility.Collapsed))
            {
                nom_lieu.Visibility = Visibility.Visible;
                nom_new_lieu.Visibility = Visibility.Visible;
                nom_url.Visibility = Visibility.Visible;
                nom_new_url.Visibility = Visibility.Visible;
                button_new.Visibility = Visibility.Visible;
            }
            else
            {
                nom_new_lieu.Text = "";
                nom_new_url.Text = "";
                nom_lieu.Visibility = Visibility.Collapsed;
                nom_new_lieu.Visibility = Visibility.Collapsed;
                nom_url.Visibility = Visibility.Collapsed;
                nom_new_url.Visibility = Visibility.Collapsed;
                button_new.Visibility = Visibility.Collapsed;

            }
        }
        /**
         * Enregistre un nouveau lieu et Url
         */
        private void Button_New(object sender, RoutedEventArgs e)
        {
            List<ElementAFaireBinding> nouvelleListe = new List<ElementAFaireBinding>(Lieu);
            // On suppose que l'url est valide
            nouvelleListe.Add(new ElementAFaireBinding { Titre = nom_new_lieu.Text, Url = nom_new_url.Text });
            Lieu = nouvelleListe;
            listeDesLieux.ItemsSource = Lieu;

            nom_new_lieu.Text = "";
            nom_new_url.Text = "";
            nom_lieu.Visibility = Visibility.Collapsed;
            nom_new_lieu.Visibility = Visibility.Collapsed;
            nom_url.Visibility = Visibility.Collapsed;
            nom_new_url.Visibility = Visibility.Collapsed;
            button_new.Visibility = Visibility.Collapsed;
        }
        /**
         * Lance l'url sur le navigateur
         */
        private void button_url_Click(object sender, RoutedEventArgs e)
        {
            int i = listeDesLieux.SelectedIndex;
            // Vérification si item sélectionner
            if (i != -1)
            {
                String url = Lieu[i].Url;

                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri(url, UriKind.Absolute);
                webBrowserTask.Show();
            }
        }

        private void button_modifier_Click(object sender, RoutedEventArgs e)
        {
            int i = listeDesLieux.SelectedIndex;
            // Vérification si item sélectionner
            if (i != -1)
            {
                nom_new_lieu.Text = Lieu[i].Titre;
                nom_new_url.Text = Lieu[i].Url;
                nom_lieu.Visibility = Visibility.Visible;
                nom_new_lieu.Visibility = Visibility.Visible;
                nom_url.Visibility = Visibility.Visible;
                nom_new_url.Visibility = Visibility.Visible;
                button_new.Visibility = Visibility.Visible;
                deleteElement(i, Lieu.Count);
            }
        }

        /**
         * Supprime l'item sélectionné
         */
        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            int i = listeDesLieux.SelectedIndex;
            // Vérification si item sélectionner
            if (i != -1)
            {
                deleteElement(i, Lieu.Count);
            }
        }
        /**
         * Supprime un élément de la liste
         */
        private void deleteElement(int i, int count)
        {
            List<ElementAFaireBinding> nouvelleListe = new List<ElementAFaireBinding>(Lieu);
            List<ElementAFaireBinding> temp = nouvelleListe.GetRange(0, i);
            List<ElementAFaireBinding> temp2 = nouvelleListe.GetRange(i + 1, count - i - 1);
            nouvelleListe = temp;
            nouvelleListe.AddRange(temp2);
            Lieu = nouvelleListe;
            listeDesLieux.ItemsSource = Lieu;
        }
    }
}