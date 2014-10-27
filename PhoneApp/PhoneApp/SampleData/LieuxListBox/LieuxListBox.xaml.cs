//      *********    NE PAS MODIFIER CE FICHIER     *********
//      Ce fichier est régénéré par un outil de création.Modifier
// .     ce fichier peut provoquer des erreurs.
namespace Expression.Blend.SampleData.LieuxListBox
{
	using System; 
	using System.ComponentModel;

// Pour réduire de façon significative le volume des données échantillons dans votre application de production, vous pouvez définir
// la constante de compilation conditionnelle DISABLE_SAMPLE_DATA et désactiver les données échantillons lors de l'exécution.
#if DISABLE_SAMPLE_DATA
	internal class LieuxListBox { }
#else

	public class LieuxListBox : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public LieuxListBox()
		{
			try
			{
				Uri resourceUri = new Uri("/PhoneApp;component/SampleData/LieuxListBox/LieuxListBox.xaml", UriKind.RelativeOrAbsolute);
				System.Windows.Application.LoadComponent(this, resourceUri);
			}
			catch
			{
			}
		}

		private ItemCollection _Collection = new ItemCollection();

		public ItemCollection Collection
		{
			get
			{
				return this._Collection;
			}
		}
	}

	public class Item : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private string _Lieu = string.Empty;

		public string Lieu
		{
			get
			{
				return this._Lieu;
			}

			set
			{
				if (this._Lieu != value)
				{
					this._Lieu = value;
					this.OnPropertyChanged("Lieu");
				}
			}
		}

		private string _Url = string.Empty;

		public string Url
		{
			get
			{
				return this._Url;
			}

			set
			{
				if (this._Url != value)
				{
					this._Url = value;
					this.OnPropertyChanged("Url");
				}
			}
		}
	}

	public class ItemCollection : System.Collections.ObjectModel.ObservableCollection<Item>
	{ 
	}
#endif
}
