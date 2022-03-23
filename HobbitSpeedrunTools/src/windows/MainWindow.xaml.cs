using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace HobbitSpeedrunTools
{
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }

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

            CheatManager.GetCheat(CHEAT_ID.DEV_MODE).onEnable += () => cbxDevMode.IsChecked = true;

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
            SaveManager.IsEnabled = true;
            cbxSaveCollections.SelectedIndex = 0;
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

            SaveManager.IsEnabled = false;
        }

        // Updates the saves ComboBox when the save collection ComboBox is updated
        private void cbxSaveCollections_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string? selected = cbxSaveCollections.SelectedItem?.ToString();

            if (selected != null && selected != string.Empty)
            {
                InitSaves(selected);
                SaveManager.SelectedSaveCollectionIndex = cbxSaveCollections.SelectedIndex;
                cbxSaves.SelectedIndex = 0;
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
                SaveManager.SelectedSaveIndex = cbxSaves.SelectedIndex;
            }
        }

        // Save manager keyboard navigation
        public void NextSaveCollection()
        {
            if (cbxSaveCollections.SelectedIndex < cbxSaves.Items.Count) cbxSaveCollections.SelectedIndex += 1;
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

        public void ToggleSaveManager()
        {
            cbxManageSaves.IsChecked = !cbxManageSaves.IsChecked;
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

        // Binds the checkbox click events to the right toggles
        private void cbxDevMode_Click(object sender, RoutedEventArgs e)
        {
            ToggleCheat? cheat = CheatManager.GetCheat(CHEAT_ID.DEV_MODE);

            if (cheat != null)
            {
                cheat.Enabled = !cheat.Enabled;
                cbxDevMode.IsChecked = cheat.Enabled;
            }
        }

        private void cbxInfiniteJumpAttack_Click(object sender, RoutedEventArgs e)
        {
            ToggleCheat? cheat = CheatManager.GetCheat(CHEAT_ID.INFINITE_JUMP_ATTACKS);

            if (cheat != null)
            {
                cheat.Enabled = !cheat.Enabled;
                cbxInfiniteJumpAttack.IsChecked = cheat.Enabled;
            }
        }

        private void cbxRenderLoadTriggers_Click(object sender, RoutedEventArgs e)
        {
            ToggleCheat? cheat = CheatManager.GetCheat(CHEAT_ID.RENDER_LOAD_TRIGGERS);

            if (cheat != null)
            {
                cheat.Enabled = !cheat.Enabled;
                cbxRenderLoadTriggers.IsChecked = cheat.Enabled;
            }
        }

        private void cbxRenderOtherTriggers_Click(object sender, RoutedEventArgs e)
        {
            ToggleCheat? cheat = CheatManager.GetCheat(CHEAT_ID.RENDER_OTHER_TRIGGERS);

            if (cheat != null)
            {
                cheat.Enabled = !cheat.Enabled;
                cbxRenderOtherTriggers.IsChecked = cheat.Enabled;
            }
        }

        private void cbxRenderPolycache_Click(object sender, RoutedEventArgs e)
        {
            ToggleCheat? cheat = CheatManager.GetCheat(CHEAT_ID.RENDER_POLY_CACHE);

            if (cheat != null)
            {
                cheat.Enabled = !cheat.Enabled;
                cbxRenderPolycache.IsChecked = cheat.Enabled;
            }
        }

        private void cbxAutoResetSigns_Click(object sender, RoutedEventArgs e)
        {
            ToggleCheat? cheat = CheatManager.GetCheat(CHEAT_ID.AUTO_RESET_SIGNS);

            if (cheat != null)
            {
                cheat.Enabled = !cheat.Enabled;
                cbxAutoResetSigns.IsChecked = cheat.Enabled;
            }
        }

        private void cbxInvincibility_Click(object sender, RoutedEventArgs e)
        {
            ToggleCheat? cheat = CheatManager.GetCheat(CHEAT_ID.INVINCIBILITY);

            if (cheat != null)
            {
                cheat.Enabled = !cheat.Enabled;
                cbxInvincibility.IsChecked = cheat.Enabled;
            }
        }

        private void cbxLockWarp_Click(object sender, RoutedEventArgs e)
        {
            ToggleCheat? cheat = CheatManager.GetCheat(CHEAT_ID.LOCK_CLIPWARP);

            if (cheat != null)
            {
                cheat.Enabled = !cheat.Enabled;
                cbxLockWarp.IsChecked = cheat.Enabled;
            }
        }

        // Opens the config file in notepad
        private void btnOpenConfig_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("notepad.exe", Path.Join(".", "config.ini"));
        }
    }
}
