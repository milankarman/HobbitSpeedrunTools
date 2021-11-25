﻿using System;
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

        // Toggles cheats when changing checkbox state
        private void cbxDevMode_Checked(object sender, RoutedEventArgs e)
        {
            CheatManager.devMode = CheatManager.CheatStatus.ENABLE;
        }

        private void cbxDevMode_Unchecked(object sender, RoutedEventArgs e)
        {
            CheatManager.devMode = CheatManager.CheatStatus.DISABLE;
        }

        private void cbxInfiniteJumpAttack_Checked(object sender, RoutedEventArgs e)
        {
            CheatManager.infiniteJumpAttack = true;
        }

        private void cbxInfiniteJumpAttack_Unchecked(object sender, RoutedEventArgs e)
        {
            CheatManager.infiniteJumpAttack = false;
        }

        private void cbxRenderLoadTriggers_Checked(object sender, RoutedEventArgs e)
        {
            CheatManager.loadTriggers = CheatManager.CheatStatus.ENABLE;
        }

        private void cbxRenderLoadTriggers_Unchecked(object sender, RoutedEventArgs e)
        {
            CheatManager.loadTriggers = CheatManager.CheatStatus.DISABLE;
        }

        private void cbxRenderOtherTriggers_Checked(object sender, RoutedEventArgs e)
        {
            CheatManager.otherTriggers = CheatManager.CheatStatus.ENABLE;
        }

        private void cbxRenderOtherTriggers_Unchecked(object sender, RoutedEventArgs e)
        {
            CheatManager.otherTriggers = CheatManager.CheatStatus.DISABLE;
        }

        private void cbxRenderPolycache_Checked(object sender, RoutedEventArgs e)
        {
            CheatManager.polyCache = CheatManager.CheatStatus.ENABLE;
        }

        private void cbxRenderPolycache_Unchecked(object sender, RoutedEventArgs e)
        {
            CheatManager.polyCache = CheatManager.CheatStatus.DISABLE;
        }

        private void cbxAutoResetSigns_Checked(object sender, RoutedEventArgs e)
        {
            CheatManager.autoResetSigns = true;
        }

        private void cbxAutoResetSigns_Unchecked(object sender, RoutedEventArgs e)
        {
            CheatManager.autoResetSigns = false;
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

        public void Test()
        {
            MessageBox.Show("Test");
        }
    }
}