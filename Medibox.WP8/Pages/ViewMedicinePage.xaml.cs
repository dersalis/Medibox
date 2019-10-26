using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Medibox.Medibox;
using Medibox.Resources;

namespace Medibox.Pages
{
    public partial class ViewMedicinePage : PhoneApplicationPage
    {
        // Instancja Medibox
        MediboxManagement _medibox = MediboxManagement.Instance;


        //
        // Konstruktor
        //
        public ViewMedicinePage()
        {
            InitializeComponent();

            // Ustaw DataContext
            this.DataContext = _medibox;

            // Utwórz AppBar
            ApplicationBar = CreateViewMedicationAppBar();
        }


        // 1.0.0.2
        #region AppBar

        //
        // Pasek appBar 
        //
        private ApplicationBar CreateViewMedicationAppBar()
        {
            //nowy pasek
            ApplicationBar viewMedicationAppBar = new ApplicationBar();
            //ustawienia paska
            viewMedicationAppBar.Mode = ApplicationBarMode.Default;
            viewMedicationAppBar.Opacity = 1.0;
            viewMedicationAppBar.IsVisible = true;
            viewMedicationAppBar.IsMenuEnabled = true;

            //przycisk edytuj
            ApplicationBarIconButton btnEditMedication = new ApplicationBarIconButton();
            btnEditMedication.IconUri = new Uri("/Assets/AppBar/edit.png", UriKind.Relative);
            btnEditMedication.Text = AppResources.ButtonEditMedicine;
            btnEditMedication.Click += new EventHandler(btnEditMedication_click);
            viewMedicationAppBar.Buttons.Add(btnEditMedication);

            //przycisk usuń
            ApplicationBarIconButton btnDeleteMedication = new ApplicationBarIconButton();
            btnDeleteMedication.IconUri = new Uri("/Assets/AppBar/delete.png", UriKind.Relative);
            btnDeleteMedication.Text = AppResources.ButtonDeleteMedicine;
            btnDeleteMedication.Click += new EventHandler(btnDeleteMedication_click);
            viewMedicationAppBar.Buttons.Add(btnDeleteMedication);

            //zwróć pasek
            return viewMedicationAppBar;
        }


        //
        // Naciśnięcie przycisku Usuń lekarstwo
        //
        private void btnDeleteMedication_click(object sender, EventArgs e)
        {
            // Usuń lekarstwo z bazy
            _medibox.DeleteMedicine(_medibox.SelectedMedicine.Id);

            // Opuść stronę
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }


        //
        // Naciśnięcie przycisku Edytuj lekarstwo
        //
        private void btnEditMedication_click(object sender, EventArgs e)
        {
            // Przejdz do strony wprowadzania nowego lekarstwa - przekaż parametr określający edycję
            NavigationService.Navigate(new Uri("/Pages/NewMedicinePage.xaml?isEdit=" + true, UriKind.Relative));
        }

        #endregion


        // 1.0.0.2
        #region Uruchamianie i zamykanie strony

        //
        // Polecenia wykonywane przy starcie strony
        //
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           // Odczytaj parametry przekazywane przez poprzednią stronę
            string selectedIndex = string.Empty;
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                int id = int.Parse(selectedIndex);

                _medibox.SetSelecteMedicine(id);
            }

            // Sprawdz czy zaplanowano dawki na dziś
            if (_medibox.SelectedTodaysDoses.Count > 0)
            {
                // Zaplanowano dawki
                // Podaż listę
                lstTodysDoses.Visibility = System.Windows.Visibility.Visible;
                // Ukryj komunikat
                txtNoTasks.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            { 
                // Nie zaplanowano dawek
                // Ukryj listę
                lstTodysDoses.Visibility = System.Windows.Visibility.Collapsed;
                // Pokaż komunikat
                txtNoTasks.Visibility = System.Windows.Visibility.Visible;
            }
            
        }


        //
        // Polecenia wykonywane przy opuszczaniu strony
        //
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Wyczyść zawartość
            //_medibox.SelectedMedicine = null;
            //_medibox.SelectedTodaysDoses = null;
        }

        #endregion
    }
}