using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PhoneApp
{
    public class ElementAFaireBinding : INotifyPropertyChanged
    {
        private String titre;
        public String Titre
        {
            get { return titre; }
            set { NotifyPropertyChanged(ref titre, value); }
        }
        private String url;
        public String Url
        {
            get { return url; }
            set { NotifyPropertyChanged(ref url, value); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nomPropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nomPropriete));
        }

        private bool NotifyPropertyChanged<T>(ref T variable, T valeur, [CallerMemberName] string nomPropriete = null)
        {
            if (object.Equals(variable, valeur)) return false;

            variable = valeur;
            NotifyPropertyChanged(nomPropriete);
            return true;
        }
    }
}
