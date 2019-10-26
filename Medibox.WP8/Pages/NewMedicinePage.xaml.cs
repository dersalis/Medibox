using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Medibox.Resources;
using Medibox.Medibox;
using Atrx.WindowsPhone.Medibox;

namespace Medibox.Pages
{
    public partial class NewPage : PhoneApplicationPage
    {
        // Instancja pomodoro
        private MediboxManagement _medibox = MediboxManagement.Instance;

        // Przycisk Zapisz na pasku appBar
        ApplicationBarIconButton appBarBtnSave;

        // Określa czy edytować lek czy dodać nowy
        // true - edycja, false - dodać nowy
        private bool editMedicine = false;

        // Określa czy wprowadzono dane do kontrolek w przypadku gdy gdy jest edycja
        // true - wprowadzono dane, false - nie wprowadzono
        private bool isSetControls = false;

        //
        // Konstruktor
        //
        public NewPage()
        {
            InitializeComponent();

            // Ustaw DataContext
            this.DataContext = _medibox;
            // Ustaw appBar
            ApplicationBar = CreateAppBar();

            // Ukryj / dezaktywuj przycisk zapisz - gdy nie wprowadzono danych do pól to przycisk jest nie aktywny
            appBarBtnSave = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            appBarBtnSave.IsEnabled = false;

            // Data zakończenia ma być o 1 dzień wieksza
            // W polu data zakończenia zwiększ datę o 1 dzień
            boxStopDate.Value = ((DateTime)(boxStartDate.Value)).AddDays(1);
        }


        // 1.0.0.2
        #region AppBar

        //
        //tworzy pasek AppBar
        //
        private ApplicationBar CreateAppBar()
        {
            /*
             * CEL:
             * Tworzy oraz zwraca pasek AppBar na stronie NewMedicine
             * Tworzone są przyciski Save, Cancel
             */

            //instancja paska appBar
            ApplicationBar NewMedicineAppBar = new ApplicationBar();

            //ustawienia paska appBar
            NewMedicineAppBar.Mode = ApplicationBarMode.Default;
            NewMedicineAppBar.Opacity = 1.0;
            NewMedicineAppBar.IsVisible = true;
            NewMedicineAppBar.IsMenuEnabled = true;

            //utwórz przyciksk Zapisz
            ApplicationBarIconButton btnSave = new ApplicationBarIconButton();
            btnSave.IconUri = new Uri("/Assets/AppBar/save.png", UriKind.Relative);
            btnSave.Text = AppResources.ButtonSave;
            btnSave.Click += new EventHandler(btnOk_Click);
            NewMedicineAppBar.Buttons.Add(btnSave);

            //utwórz przycisk Anuluj
            ApplicationBarIconButton btnCancel = new ApplicationBarIconButton();
            btnCancel.IconUri = new Uri("/Assets/AppBar/cancel.png", UriKind.Relative);
            btnCancel.Text = AppResources.ButtonCancel;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            NewMedicineAppBar.Buttons.Add(btnCancel);

            //zwróć pasek appBar
            return NewMedicineAppBar;
        }


        //
        // Naciśnięcie przycisku Anuluj
        //
        private void btnCancel_Click(object sender, EventArgs e)
        {
            /*
             * CEL:
             * Naciśnięcie przycisku Anuluj powoduje opuszczenie strony
             */

            // Opuść stronę
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }


        //
        // Naciśnięcie przycisku Ok
        //
        private void btnOk_Click(object sender, EventArgs e)
        {
            /*
             * CEL:
             * Naciśnięcie przycisku Ok powaduje dodanie nowego lekarstwa lub zapisanie zmian w istniejącym lekarstwie
             */

            // Sprawdź czy daty są poprawne - data w dniu końca powinna być wieksza od dnia rozpoczęcia
            if (CheckDates((DateTime)boxStartDate.Value, (DateTime)boxStopDate.Value))
            {
                // Sprawdz czy edytować czy dodać nowy element
                // true - edycja, false - dodanie nowego lekarstwa
                if (editMedicine)
                {
                    // Edytuj lekarstwo

                    // Wprowadź zmiany w lekarstwie
                    _medibox.UpdateMedicine(_medibox.SelectedMedicine.Id, CreateNewMedicine());
                }
                else
                {
                    // Utwórz nowe lekarstwo

                    // Dodaj lekarstwo do bazy
                    _medibox.AddNewMedicine(CreateNewMedicine());
                    
                }

                // Opuść stronę
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            }
            else
            { 
                // Jeśli daty są identyczne
                // Wyświetl komunikat ostrzegający
                MessageBox.Show(AppResources.MessageStopDateContent, AppResources.MessageStopDateTitle,  MessageBoxButton.OK);
            }
        }


        #endregion


        // 1.0.0.2
        #region Uruchomienie i zamknięcie strony

        //
        // Polecenia wykonywane przy starcie strony
        //
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /*
             * CEL:
             * Polecenia wykonywane przy starcie strony
             */

            // Sprawdź czy dodano do kontrolek informacje
            // true - dodano, false - nie dodano
            if (!isSetControls)
            {
                // Odczytaj parametr przekazay przez poprzednią stronę
                string isEdit = string.Empty;
                if (NavigationContext.QueryString.TryGetValue("isEdit", out isEdit))
                {
                    // Przywróć parametr
                    editMedicine = bool.Parse(isEdit);
                }

                // Sprawdz czy należy edytować lekarstwo
                // true - edycja
                if (editMedicine)
                {
                    // Dodaj parametry do pól
                    boxMedicinName.Text = _medibox.SelectedMedicine.MedicinName;
                    boxDose.Text = _medibox.SelectedMedicine.Dose;
                    boxNumberOfDosePerDay.ItemsSource = _medibox.NumberOfDosesPerDayArray;
                    boxNumberOfDosePerDay.SelectedItem = _medibox.SelectedMedicine.NumberOfDosesPerDay;

                    DateTime startDate = new DateTime(_medibox.SelectedMedicine.StopDate.Year, _medibox.SelectedMedicine.StartDate.Month, _medibox.SelectedMedicine.StartDate.Day);

                    boxStartDate.Value = startDate;
                    boxStartTime.Value = _medibox.SelectedMedicine.StartDate;
                    boxStopDate.Value = _medibox.SelectedMedicine.StopDate;
                    btnReminder.IsChecked = _medibox.SelectedMedicine.IsReminder;
                    btnPriority.IsChecked = _medibox.SelectedMedicine.IsHighPriority;
                    boxNote.Text = _medibox.SelectedMedicine.Note;
                }
                // Ustaw parametr - dodano informacje do kontrolki
                isSetControls = true;
            }
        }


        //
        // Polecenia wykonywane przy opuszczaniu strony
        //
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //

        }

        #endregion


        // 1.0.0.2
        #region Zdarzenia

        //
        // Zaznaczenie przełącznika Przypomnienie
        //
        private void btnReminder_Checked(object sender, RoutedEventArgs e)
        {
            // Zmien opis na On
            btnReminder.Content = AppResources.TextReminderOn;

        }


        //
        // Odznaczenie przełącznika Przypomnienie
        //
        private void btnReminder_Unchecked(object sender, RoutedEventArgs e)
        {
            // Zmien opis na Off
            btnReminder.Content = AppResources.TextReminderOff;
        }


        //
        // Zaznaczenie przełącznika Priorytet
        //
        private void btnPriority_Checked(object sender, RoutedEventArgs e)
        {
            // Zmień opis na High
            btnPriority.Content = AppResources.TextPriorityHigh;

        }


        //
        // Odznaczenie przełącznika Priorytet
        //
        private void btnPriority_Unchecked(object sender, RoutedEventArgs e)
        {
            // Zmień opis
            btnPriority.Content = AppResources.TextPriorityNormal;

        }


        //
        // Zdarzenie zachodzi podczas zmiany zawartości pola
        //
        private void boxMedicinName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Gdy pola tekstowe są nie puste to pokaż przycisk zapisz
            appBarBtnSave.IsEnabled = CheckTextBox(boxDose, boxMedicinName);
        }

        #endregion


        // 1.0.0.2
        #region Metody pomocnicze

        //
        // Tworzy nowe lekarstwo na podstawie informacji z komponentów
        //
        private Medicine CreateNewMedicine()
        {
            /*
             * CEL:
             * Tworzy nowe lekarstwo na podstawie informacji z komponentów
             * 
             * WARTOŚĆ ZWRACANA:
             * Medicine - obiekt lekarstwo
             */

            // Przygotuj obiekt lekarstwa
            // Godzina rozpoczęcia
            TimeSpan startTime = new TimeSpan(0, ((DateTime)boxStartTime.Value).Hour, ((DateTime)boxStartTime.Value).Minute, 0);
            // Data rozpoczęcia
            DateTime startDate = ((DateTime)boxStartDate.Value) + startTime;
            // Data zakończenia
            DateTime stopDate = (DateTime)boxStopDate.Value + startTime;
            // Obiekt tymczasowy lekarstwa
            Medicine newMedicine = new Medicine()
            {
                MedicinName = boxMedicinName.Text,
                NumberOfDosesPerDay = (int)boxNumberOfDosePerDay.SelectedItem,
                Dose = boxDose.Text,
                IsReminder = (bool)btnReminder.IsChecked,
                StartDate = startDate,
                StopDate = stopDate,
                IsActive = true,
                IsHighPriority = (bool)btnPriority.IsChecked,
                Note = boxNote.Text
            };

            // Zwróć lekarstwo
            return newMedicine;
        }


        //
        // Sprawdza czy podane pola textowe są nie puste - w tym przypadku zwraca true.
        //
        private bool CheckTextBox(PhoneTextBox textBox1, PhoneTextBox textBox2)
        {
            /*
             * CEL:
             * Sprawdza czy podane pola textowe są nie puste.
             * Jeśli pola są wypełnione to zwraca true.
             * Jeśli pola są puste to zwracane jest false.
             * 
             * PARAMETRY:
             * textBox1:PhoneTextBox - sprawdzane pole tekstowe
             * textBox2:PhoneTextBox - sprawdzane pole tekstowe
             * 
             * WARTOŚĆ ZWRACANE:
             * bool - zależy od tego czy pola są wypełnione czy puste
             */

            bool checkedPositive = false;
            // Sprawdz czy pola są nie puste
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
                // Jeśli pola są wypełnione to ustaw true
                checkedPositive = true;
            // Zwróć wartość
            return checkedPositive;
        }


        //
        // Sprawdza podane daty - jeśli różnica jest większa od 1 dnia zwraca true
        //
        private bool CheckDates(DateTime date1, DateTime date2)
        {
            /*
             * CEL:
             * Sprawdza podane daty - jeśli różnica jest większa od 1 dnia zwraca true
             * 
             * PARAMETRY:
             * date1:DateTime - podana data
             * date2:DateTime - podana data
             * 
             * WARTOŚĆ ZRWACANE:
             * bool - zależy od różnicy dat.
             */

            bool checkedPositive = false;
            // Oblicz różnice dat
            TimeSpan datesDifference = date2 - date1;
            // Sprawdź różnice
            if (datesDifference >= new TimeSpan(1, 0, 0, 0))
                checkedPositive = true;
            // Zwróć różnice 
            return checkedPositive;

        }

        #endregion

       
        // 1.0.0.2
        #region Pomoc - wiadomości

        //
        // Wyświetla komunikat pomocy
        //
        private void ShowHelpMessage(string title, string caption)
        { 
            /*
             * CEL: 
             * Wyświetla komunikat pomocy
             * 
             * PARAMETRY:
             * title:string - tytuł wiadomości
             * caption:string - treść wiadomości
             */

            // Wyświetl komunikat
            MessageBox.Show(caption, title, MessageBoxButton.OK);
        }


        //
        // Wyświetla komunikat pomocy - Nazwa leku
        //
        private void txtHelpMedicinName_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowHelpMessage(AppResources.TextMedicineName, AppResources.HelpMedicineName);
        }

        //
        //
        //
        private void txtHelpDose_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowHelpMessage(AppResources.TextDose, AppResources.HelpDose);
        }


        //
        //
        //
        private void txtHelpNumberOfDosePerDay_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowHelpMessage(AppResources.TextNumberOfDosePerDay, AppResources.HelpNumberOfDosePerDay);
        }


        //
        //
        //
        private void txtHelpStartDate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowHelpMessage(AppResources.TextStartDate, AppResources.HelpStartDate);
        }


        //
        //
        //
        private void txtHelpStartTime_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowHelpMessage(AppResources.TextStartTime, AppResources.HelpStartTime);
        }


        //
        //
        //
        private void txtStopDate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowHelpMessage(AppResources.TextStopDate, AppResources.HelpStopDate);
        }


        //
        //
        //
        private void txtHelpReminder_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowHelpMessage(AppResources.TextReminder, AppResources.HelpReminder);
        }


        //
        //
        //
        private void txtHelpPriority_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowHelpMessage(AppResources.TextPriority, AppResources.HelpPriority);
        }


        //
        //
        //
        private void txtHelpNote_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowHelpMessage(AppResources.TextNote, AppResources.HelpNote);
        }

        #endregion
    }
}