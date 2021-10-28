using System;
using System.Windows;

namespace HobbitSpeedrunTools
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitSaveCollections();
            SaveManager.BackupOldSaves();
            
            try
            {
                CheatManager.InitCheatManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Selects first save collection by default
            if (cbxSaveCollections.Items.Count > 0)
            {
                cbxSaveCollections.SelectedIndex = 0;
            }
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

        private void cbxDevMode_Checked(object sender, RoutedEventArgs e)
        {
            CheatManager.EnableDevMode();
        }

        private void cbxDevMode_Unchecked(object sender, RoutedEventArgs e)
        {
            CheatManager.DisableDevMode();
        }

        private void cbxInfiniteJumpAttack_Checked(object sender, RoutedEventArgs e)
        {
            CheatManager.EnableInfiniteJumpAttack();
        }

        private void cbxInfiniteJumpAttack_Unchecked(object sender, RoutedEventArgs e)
        {
            CheatManager.DisableInfiniteJumpAttack();
        }

        private void cbxRenderLoadTriggers_Checked(object sender, RoutedEventArgs e)
        {
            CheatManager.EnableLoadTriggers();
        }

        private void cbxRenderLoadTriggers_Unchecked(object sender, RoutedEventArgs e)
        {
            CheatManager.DisableLoadTriggers();
        }

        private void cbxRenderOtherTriggers_Checked(object sender, RoutedEventArgs e)
        {
            CheatManager.EnableOtherTriggers();
        }

        private void cbxRenderOtherTriggers_Unchecked(object sender, RoutedEventArgs e)
        {
            CheatManager.DisableOtherTriggers();
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
    }
}
