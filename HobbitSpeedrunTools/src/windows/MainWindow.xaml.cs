using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace HobbitSpeedrunTools
{
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }
        private const string disabledText = "Disabled";

        public MainWindow()
        {
            InitializeComponent();

            SourceInitialized += (s, e) =>
            {
                SizeToContent = SizeToContent.Manual;
                MinWidth = ActualWidth;
                MinHeight = ActualHeight - 37;
                Height = MinHeight;
                Width = MinWidth;
            };

            // Add the version number to the titlebar
            Title += $" {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion}";

            // Attempt to intialize the cheat manager
            try
            {
                CheatManager.InitCheatManager();
                CheatManager.onBilboPositionUpdate += (x, y, z) => Dispatcher.Invoke(() => UpdateBilboPosition(x, y, z));
                CheatManager.onBilboRotationUpdate += (degrees) => Dispatcher.Invoke(() => UpdateBilboRotation(degrees));
                CheatManager.onClipwarpPositionUpdate += (x, y, z) => Dispatcher.Invoke(() => UpdateClipwarpPositition(x, y, z));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

            // Attempt to intialize the config manager
            try
            {
                ConfigManager.InitConfigManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

            itcCheats.ItemsSource = CheatManager.toggleCheatList;
            HotkeyManager.InitHotkeyManager();

            UpdateBilboPosition(0, 0, 0);
            UpdateBilboRotation(0);
            UpdateClipwarpPositition(0, 0, 0);

            InitSaveCollections();
        }

        public void UpdateBilboPosition(float x, float y, float z)
        {
            txtBilboPosX.Text = Math.Round(x, 1).ToString("n1");
            txtBilboPosY.Text = Math.Round(y, 1).ToString("n1");
            txtBilboPosZ.Text = Math.Round(z, 1).ToString("n1");
        }

        public void UpdateBilboRotation(double degrees)
        {
            txtBilboRotation.Text = Math.Round(degrees, 1).ToString("n1");
        }

        public void UpdateClipwarpPositition(float x, float y, float z)
        {
            txtClipwarpPosX.Text = Math.Round(x, 1).ToString("n1");
            txtClipwarpPosY.Text = Math.Round(y, 1).ToString("n1");
            txtClipwarpPosZ.Text = Math.Round(z, 1).ToString("n1");
        }

        private void cbxCheat_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            if (cbx.DataContext is ToggleCheat cheat)
            {
                cheat.onEnable += () => Dispatcher.Invoke(() => cbx.IsChecked = true);
                cheat.onDisable += () => Dispatcher.Invoke(() => cbx.IsChecked = false);
            }
        }

        private void cbxCheat_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cmd = (CheckBox)sender;
            if (cmd.DataContext is ToggleCheat cheat) cheat.Enable();
        }

        private void cbxCheat_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cmd = (CheckBox)sender;
            if (cmd.DataContext is ToggleCheat cheat) cheat.Disable();
        }

        // Loads the save collections into their ComboBox
        private void InitSaveCollections()
        {
            try
            {
                string[] saveCollections = SaveManager.GetSaveCollections();

                cbxSaveCollections.Items.Add(disabledText);

                // Adds save collections to their ComboBox
                foreach (string collection in saveCollections)
                {
                    cbxSaveCollections.Items.Add(collection);
                }

                // Selects first save collection by default
                if (cbxSaveCollections.Items.Count > 0)
                {
                    cbxSaveCollections.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Loads the saves for the selected save collection into their ComboBox
        private void InitSaves(string saveCollection)
        {
            cbxSaves.Items.Clear();

            try
            {
                if (saveCollection == disabledText) return;

                string[] sortedSaves = SaveManager.GetSaves(saveCollection);

                // Adds saves to their ComboBox
                foreach (string save in sortedSaves)
                {
                    cbxSaves.Items.Add(save);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Updates the saves ComboBox when the save collection ComboBox is updated
        private void cbxSaveCollections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selected = cbxSaveCollections.SelectedItem?.ToString();

            if (selected == disabledText)
            {
                cbxSaves.IsEnabled = false;
                cbxSaves.Items.Clear();

                // Restore the backed up files if a backup was performed
                if (SaveManager.DidBackup)
                {
                    SaveManager.RestoreOldSaves();
                }

                return;
            }

            if (selected != null && selected != string.Empty)
            {
                // Restore the backed up files if a backup was performed
                if (!SaveManager.DidBackup)
                {
                    SaveManager.BackupOldSaves();
                }

                cbxSaves.IsEnabled = true;
                InitSaves(selected);
                SaveManager.SelectedSaveCollectionIndex = cbxSaveCollections.SelectedIndex;
                cbxSaves.SelectedIndex = 0;
            }
        }

        // Safely gets the value of the saves ComboBox the selected save to the saves folder
        private void cbxSaves_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedSaveCollection = cbxSaveCollections.SelectedItem?.ToString();
            string? selectedSave = cbxSaves.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedSaveCollection) && !string.IsNullOrEmpty(selectedSave))
            {
                SaveManager.SelectSave(selectedSaveCollection, selectedSave);
                SaveManager.SelectedSaveIndex = cbxSaves.SelectedIndex;
            }
        }

        // Save manager keyboard navigation
        public void NextSaveCollection()
        {
            if (cbxSaveCollections.SelectedIndex < cbxSaveCollections.Items.Count) cbxSaveCollections.SelectedIndex += 1;
        }

        public void PreviousSaveCollection()
        {
            if (cbxSaveCollections.SelectedIndex > 0) cbxSaveCollections.SelectedIndex -= 1;
        }

        public void NextSave()
        {
            if (cbxSaves.SelectedIndex < cbxSaves.Items.Count) cbxSaves.SelectedIndex += 1;
        }

        public void PreviousSave()
        {
            if (cbxSaves.SelectedIndex > 0) cbxSaves.SelectedIndex -= 1;
        }

        // Ensures that proper cleanup will be done before closing the program
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            SaveManager.ClearSaves();

            // Restore the backed up files if a backup was performed
            if (SaveManager.DidBackup)
            {
                SaveManager.RestoreOldSaves();
            }

            base.OnClosing(e);
        }

        // Opens the config file in notepad
        private void btnOpenConfig_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("notepad.exe", Path.Join(".", "config.ini"));
        }
    }
}
