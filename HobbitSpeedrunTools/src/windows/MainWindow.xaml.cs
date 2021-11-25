using System;
using System.Windows;
using System.Windows.Threading;

namespace HobbitSpeedrunTools
{
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }

        private bool closing = false;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            // Add the version number to the titlebar
            Title += $" {About.version}";

            // Attempt to intialize the cheat manager
            try
            {
                CheatManager.InitCheatManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            HotkeyManager.InitHotkeyManager();
        }

        // Loads the save collections into their ComboBox
        private void InitSaveCollections()
        {
            try
            {
                string[] saveCollections = SaveManager.GetSaveCollections();

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

        // Resets all cheat checkboxes to be unticked
        public void ResetCheatCheckboxes()
        {
            // Ensure the window isn't closing and updating at the same time
            if (!closing)
            {
                Dispatcher.Invoke(() =>
                {
                    cbxDevMode.IsChecked = false;
                    cbxInfiniteJumpAttack.IsChecked = false;
                    cbxRenderLoadTriggers.IsChecked = false;
                    cbxRenderOtherTriggers.IsChecked = false;
                    cbxRenderPolycache.IsChecked = false;
                    cbxAutoResetSigns.IsChecked = false;
                });
            }
        }

        // Loads the saves for the selected save collection into their ComboBox
        private void InitSaves(string saveCollection)
        {
            cbxSaves.Items.Clear();

            try
            {
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

        // Back up old saves and initialize saves when the save manager is enabled
        private void cbxManageSaves_Checked(object sender, RoutedEventArgs e)
        {
            cbxSaveCollections.IsEnabled = true;
            cbxSaves.IsEnabled = true;
            SaveManager.BackupOldSaves();
            InitSaveCollections();
        }

        // Clear saves and restore old saves when the save manager is disabled
        private void cbxManageSaves_Unchecked(object sender, RoutedEventArgs e)
        {
            cbxSaveCollections.IsEnabled = false;
            cbxSaveCollections.Items.Clear();
            cbxSaves.IsEnabled = false;
            cbxSaves.Items.Clear();

            SaveManager.ClearSaves();

            if (SaveManager.DidBackup)
            {
                SaveManager.RestoreOldSaves();
            }
        }

        // Updates the saves ComboBox when the save collection ComboBox is updated
        private void cbxSaveCollections_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string? selected = cbxSaveCollections.SelectedItem?.ToString();

            if (selected != null && selected != string.Empty)
            {
                InitSaves(selected);
            }
        }

        // Safely gets the value of the saves ComboBox the selected save to the saves folder
        private void cbxSaves_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string? selectedSaveCollection = cbxSaveCollections.SelectedItem?.ToString();
            string? selectedSave = cbxSaves.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedSaveCollection) && !string.IsNullOrEmpty(selectedSave))
            {
                SaveManager.SelectSave(selectedSaveCollection, selectedSave);
            }
        }

        public void ToggleDevMode(bool changeCheckbox = false)
        {
            if (changeCheckbox) cbxDevMode.IsChecked = !cbxDevMode.IsChecked;
            CheatManager.devMode = cbxDevMode.IsChecked == true ? CheatManager.CheatStatus.ENABLE : CheatManager.CheatStatus.DISABLE;
        }

        public void ToggleInfiniteJumpAttacks(bool changeCheckbox = false)
        {
            if (changeCheckbox) cbxInfiniteJumpAttack.IsChecked = !cbxInfiniteJumpAttack.IsChecked;
            CheatManager.infiniteJumpAttack = !CheatManager.infiniteJumpAttack;
        }

        public void ToggleRenderLoadTriggers(bool changeCheckbox = false)
        {
            if (changeCheckbox) cbxRenderLoadTriggers.IsChecked = !cbxRenderLoadTriggers.IsChecked;
            CheatManager.loadTriggers = cbxRenderLoadTriggers.IsChecked == true ? CheatManager.CheatStatus.ENABLE : CheatManager.CheatStatus.DISABLE;
        }

        public void ToggleRenderOtherTriggers(bool changeCheckbox = false)
        {
            if (changeCheckbox) cbxRenderOtherTriggers.IsChecked = !cbxRenderOtherTriggers.IsChecked;
            CheatManager.otherTriggers = cbxRenderOtherTriggers.IsChecked == true ? CheatManager.CheatStatus.ENABLE : CheatManager.CheatStatus.DISABLE;
        }

        public void TogglePolycache(bool changeCheckbox = false)
        {
            if (changeCheckbox) cbxRenderPolycache.IsChecked = !cbxRenderPolycache.IsChecked;
            CheatManager.polyCache = cbxRenderPolycache.IsChecked == true ? CheatManager.CheatStatus.ENABLE : CheatManager.CheatStatus.DISABLE;
        }

        public void ToggleAutoResetSigns(bool changeCheckbox = false)
        {
            if (changeCheckbox) cbxAutoResetSigns.IsChecked = !cbxAutoResetSigns.IsChecked;
            CheatManager.autoResetSigns = !CheatManager.autoResetSigns;
        }

        // Ensures that proper cleanup will be done before closing the program
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            closing = true;

            SaveManager.ClearSaves();

            // Restore the backed up files if a backup was performed
            if (SaveManager.DidBackup)
            {
                SaveManager.RestoreOldSaves();
            }

            base.OnClosing(e);
        }

        private void cbxDevMode_Click(object sender, RoutedEventArgs e)
        {
            ToggleDevMode();
        }

        private void cbxInfiniteJumpAttack_Click(object sender, RoutedEventArgs e)
        {
            ToggleInfiniteJumpAttacks();
        }

        private void cbxRenderLoadTriggers_Click(object sender, RoutedEventArgs e)
        {
            ToggleRenderLoadTriggers();
        }

        private void cbxRenderOtherTriggers_Click(object sender, RoutedEventArgs e)
        {
            ToggleRenderOtherTriggers();
        }

        private void cbxRenderPolycache_Click(object sender, RoutedEventArgs e)
        {
            TogglePolycache();
        }

        private void cbxAutoResetSigns_Click(object sender, RoutedEventArgs e)
        {
            ToggleAutoResetSigns();
        }
    }
}
