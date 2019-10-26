using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Microsoft.Phone.Tasks;
using Medibox.Medibox;
using Medibox.Resources;
using System.Reflection;

using Atrx.WindowsPhone.Medibox;

namespace Medibox
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Instancja pomodoro
        private MediboxManagement _medibox = MediboxManagement.Instance;


        //
        // Konstruktor
        //
        public MainPage()
        {
            InitializeComponent();

            // Ustaw DataContext
            this.DataContext = _medibox;

            // Dodaj appBar
            ApplicationBar = CreateTodayAppBar();
        }


        //
        #region Start i stop

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //TODO: Dodać odświerzanie listy zadań i leków
            //_medibox.SetTasksList();
            //_medibox.SetMedicinesList();

            // Sprawdz czy dodano zadania
            if (_medibox.TasksList.Count > 0)
            {
                // Dodano zadania
                // Pokaż listę zadań
                lstMediTask.Visibility = System.Windows.Visibility.Visible;
                // Ukryj komunikat
                txtNoTasks.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                // Nie dodano zadań
                // Ukryj listę zadań
                lstMediTask.Visibility = System.Windows.Visibility.Collapsed;
                // Pokaż komunikat
                txtNoTasks.Visibility = System.Windows.Visibility.Visible;
            }

            // Sprawdz czy dodano lekarstwa
            if (_medibox.MedicinesList.Count > 0)
            {
                // Dodano leki
                // Pokaż listę leków
                lstMedicines.Visibility = System.Windows.Visibility.Visible;
                // Ukryj komunikat
                txtNoMedicines.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                // Nie dodano leków
                // Ukryj listę leków
                lstMedicines.Visibility = System.Windows.Visibility.Collapsed;
                // Pokaż komunikat
                txtNoMedicines.Visibility = System.Windows.Visibility.Visible;
            }
        }

        #endregion

        // 1.0.0.2
        #region AppBar

        //
        // Pasek appBar Dzisiaj
        //
        private ApplicationBar CreateTodayAppBar()
        {
            //nowy pasek
            ApplicationBar todayAppBar = new ApplicationBar();
            //ustawienia paska
            todayAppBar.Mode = ApplicationBarMode.Default;
            todayAppBar.Opacity = 1.0;
            todayAppBar.IsVisible = true;
            todayAppBar.IsMenuEnabled = true;

            //przycisk dodaj lek
            ApplicationBarIconButton btnAddMedicationNote = new ApplicationBarIconButton();
            btnAddMedicationNote.IconUri = new Uri("/Toolkit.Content/ApplicationBar.Add.png", UriKind.Relative);
            btnAddMedicationNote.Text = AppResources.ButtonAddMedicine;
            btnAddMedicationNote.Click += new EventHandler(btnAddMedication_click);
            todayAppBar.Buttons.Add(btnAddMedicationNote);

            //przycisk
            //ApplicationBarIconButton btnSortNotes = new ApplicationBarIconButton();
            //btnSortNotes.IconUri = new Uri("/Images/sort.png", UriKind.Relative);
            //btnSortNotes.Text = AppResources.AppBarSortNotes;
            //btnSortNotes.Click += new EventHandler(btnSortNotes_click);
            //todayAppBar.Buttons.Add(btnSortNotes);

            //pozycja menu ustawienia
            //ApplicationBarMenuItem mnuAbout = new ApplicationBarMenuItem();
            //mnuAbout.Text = "about";
            //mnuAbout.Click += new EventHandler(mnuAbout_click);
            //todayAppBar.MenuItems.Add(mnuAbout);

            //zwróć pasek
            return todayAppBar;
        }

        
        //
        // Pasek appBar Lekarstwa
        //
        private ApplicationBar CreateMedicationAppBar()
        {
            //nowy pasek
            ApplicationBar medicationAppBar = new ApplicationBar();
            //ustawienia paska
            medicationAppBar.Mode = ApplicationBarMode.Default;
            medicationAppBar.Opacity = 1.0;
            medicationAppBar.IsVisible = true;
            medicationAppBar.IsMenuEnabled = true;

            //przycisk dodaj lek
            ApplicationBarIconButton btnAddMedicationNote = new ApplicationBarIconButton();
            btnAddMedicationNote.IconUri = new Uri("/Toolkit.Content/ApplicationBar.Add.png", UriKind.Relative);
            btnAddMedicationNote.Text = AppResources.ButtonAddMedicine;
            btnAddMedicationNote.Click += new EventHandler(btnAddMedication_click);
            medicationAppBar.Buttons.Add(btnAddMedicationNote);

            //przycisk
            //ApplicationBarIconButton btnSortNotes = new ApplicationBarIconButton();
            //btnSortNotes.IconUri = new Uri("/Images/sort.png", UriKind.Relative);
            //btnSortNotes.Text = AppResources.AppBarSortNotes;
            //btnSortNotes.Click += new EventHandler(btnSortNotes_click);
            //todayAppBar.Buttons.Add(btnSortNotes);

            //pozycja menu ustawienia
            //ApplicationBarMenuItem mnuAbout = new ApplicationBarMenuItem();
            //mnuAbout.Text = AppResources.ButtonAbout;
            //mnuAbout.Click += new EventHandler(mnuAbout_click);
            //medicationAppBar.MenuItems.Add(mnuAbout);

            //zwróć pasek
            return medicationAppBar;
        }


        //
        //Pasek appBar About
        //
        private ApplicationBar CreateAboutAppBar()
        {
            //nowy pasek
            ApplicationBar aboutAppBar = new ApplicationBar();
            //ustawienia paska
            aboutAppBar.Mode = ApplicationBarMode.Default;
            aboutAppBar.Opacity = 1.0;
            aboutAppBar.IsVisible = true;
            aboutAppBar.IsMenuEnabled = true;

            //przycisk oceń
            ApplicationBarIconButton btnReviev = new ApplicationBarIconButton();
            btnReviev.IconUri = new Uri("/Images/rateMe.png", UriKind.Relative);
            btnReviev.Text = AppResources.TextRateMe;
            btnReviev.Click += new EventHandler(btnReviev_click);
            aboutAppBar.Buttons.Add(btnReviev);

            //przycisk sklep
            ApplicationBarIconButton btnStore = new ApplicationBarIconButton();
            btnStore.IconUri = new Uri("/Images/store.png", UriKind.Relative);
            btnStore.Text = AppResources.TextStore;
            btnStore.Click += new EventHandler(btnStore_click);
            aboutAppBar.Buttons.Add(btnStore);

            //przycisk kontakt
            ApplicationBarIconButton btnContact = new ApplicationBarIconButton();
            btnContact.IconUri = new Uri("/Images/email.png", UriKind.Relative);
            btnContact.Text = AppResources.TextContact;
            btnContact.Click += new EventHandler(btnContact_click);
            aboutAppBar.Buttons.Add(btnContact);

            //zwróć pasek
            return aboutAppBar;
        }


        // 
        // Naciśnięcie pozycji menu O programie
        //
        private void mnuAbout_click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }


        //
        // Naciśnięcie przycisku Kontakt
        //
        private void btnContact_click(object sender, EventArgs e)
        {
            // Wyślij email
            SendEmail();
        }


        //
        // Naciśnięcie przycisku Sklep
        //
        private void btnStore_click(object sender, EventArgs e)
        {
            // Przenieś do sklepu
            MarketplaceSearchTask marketplaceSearchTask = new MarketplaceSearchTask();
            marketplaceSearchTask.SearchTerms = "Damian Ruta";
            marketplaceSearchTask.Show();
        }


        // Naciśnięcie przycisku Oceń
        private void btnReviev_click(object sender, EventArgs e)
        {
            // Oceń program
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }


        //
        // Naciśnięcie przycisku Dodaj lekarstwo
        //
        private void btnAddMedication_click(object sender, EventArgs e)
        {
            // Sprawdz czy można przejść do strony
            // Warunek przejścia: wersja płatna aplikacji lub trial i mniej niż pięć lekarstw
            if ((_medibox.IsTrial) && (_medibox.MedicinesList.Count >= 5))
            {
                // Nie można dodać więcej leków
                MessageBoxResult msgResults = MessageBox.Show(AppResources.TextTrialContent, AppResources.TextTrialTitle, MessageBoxButton.OKCancel);
                // Jeśli wciśnięto OK
                if (msgResults == MessageBoxResult.OK)
                { 
                    // Przejdź do sklepu
                    MarketplaceDetailTask mdt = new MarketplaceDetailTask();
                    mdt.Show();
                }
            }
            else
            {
                // Przejdz do strony wprowadzania nowego lekarstwa
                NavigationService.Navigate(new Uri("/Pages/NewMedicinePage.xaml?isEdit=" + false, UriKind.Relative));
            }
        }

        #endregion


        // 1.0.0.2
        #region Zdarzenia

        //
        // Naciśnięcie adresu email
        //
        private void txtSendEmail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // Wyślij email
            SendEmail();
        }

        //
        // Zmiana strony - przesunięcie w bok
        //
        private void panPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ustaw appBar w zależności od strony
            switch (panPanel.SelectedIndex)
            {
                // Strona Dziś
                case 0:
                    ApplicationBar = CreateTodayAppBar();
                    break;
                // Strona Lekarstwa
                case 1:
                    ApplicationBar = CreateMedicationAppBar();
                    break;
                // Strona O programie
                case 2:
                    ApplicationBar = CreateAboutAppBar();
                    break;
            }
        }


        //
        // Naciśnięcie przycisku dodaj nowy lek
        //
        private void btnAddMedicine_Click(object sender, RoutedEventArgs e)
        {
            // Przełącz na nową stronę i przekaż parametr - wskazaną notatkę
            NavigationService.Navigate(new Uri("/Pages/NewMedicinePage.xaml", UriKind.Relative));
        }


        //
        // Naciśnięcie zadania na liście
        //
        private void lstMediTask_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // Jeśli nic nie wybrano to porzuć
            if (lstMediTask.SelectedItem == null)
                return;

            // Przełącz na nową stronę i przekaż parametr - wskazaną notatkę
            NavigationService.Navigate(new Uri("/Pages/ViewMedicinePage.xaml?selectedItem=" + (lstMediTask.SelectedItem as MediTask).MedicineId, UriKind.Relative));

            // Usuń zaznaczenie
            lstMediTask.SelectedItem = null;
        }


        //
        // Naciśnięcie leku na liście
        //
        private void lstMedicines_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // Jeśli nic nie wybrano to porzuć
            if (lstMedicines.SelectedItem == null)
                return;

            // Przełącz na nową stronę i przekaż parametr - wskazaną notatkę
            NavigationService.Navigate(new Uri("/Pages/ViewMedicinePage.xaml?selectedItem=" + (lstMedicines.SelectedItem as Medicine).Id, UriKind.Relative));

            // Usuń zaznaczenie
            lstMedicines.SelectedItem = null;
        }

        #endregion


        // 1.0.0.2
        #region Metody pomocnicze

        //
        // Tworzy i wysyła wiadomość email
        //
        private void SendEmail()
        {
            /*
             * CEL:
             * Tworzy i wysyła wiadomość email
             */

            // Utwórz zadanie
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            // Ustaw temat
            emailComposeTask.Subject = AppResources.TextEmailSubiect;
            //emailComposeTask.Body = "message body";
            // Dodaj adres
            emailComposeTask.To = "mobileapps@aturex.pl";
            //emailComposeTask.Cc = "cc@example.com";
            //emailComposeTask.Bcc = "bcc@example.com";

            // Utwórz okno redagowania wiadomości
            emailComposeTask.Show();
        }

        #endregion

        private void mnuEdit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //this.DataContext = _medibox;
            //lstMedicines.ItemsSource = _medibox.MedicinesList;
            // Wybierz lekarstwo
            Medicine selectedMedicine = (sender as MenuItem).DataContext as Medicine;
            
            lstMedicines.SelectedItem = null;

            //var element = (FrameworkElement)sender;
            //Medicine selectedMedicine = element.DataContext as Medicine;
            //if (selectedMedicine == null) return;

            

            MessageBox.Show(selectedMedicine.Dose, selectedMedicine.MedicinName, MessageBoxButton.OK);
           
            //lstMedicines.SelectedItem = null;
        }

        private void mnuDelete_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void btnFacebook_Click(object sender, RoutedEventArgs e)
        {
            var web = new WebBrowserTask();
            web.Uri = new Uri("https://www.facebook.com/mediboxapp");
            web.Show();
        }

        private void btnRemoveAd_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // Wyświetl komunikat
            MessageBoxResult msgResult = MessageBox.Show(AppResources.TextRemoveAdContetnt, AppResources.TextRemoveAdTitle, MessageBoxButton.OKCancel);

            // Jeśli wciśnięto OK
            if (msgResult == MessageBoxResult.OK)
            {
                MarketplaceDetailTask mdt = new MarketplaceDetailTask();
                mdt.Show();
            }
        }
    }
}