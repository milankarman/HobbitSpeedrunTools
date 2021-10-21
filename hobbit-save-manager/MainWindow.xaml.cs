﻿using System;
using System.Windows;
using System.IO;

namespace hobbit_save_manager
{
    public partial class MainWindow : Window
    {
        private string hobbitSaveDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "The Hobbit");
        private string applicationSaveDir = Path.Join(".", "save-collections");
        private string backupDir = "";

        private bool didBackup;
   
        public MainWindow()
        {
            InitializeComponent();
            InitSaveCollections();
            BackupOldSaves();
        }

        // Detects all folders within the save collections folder and lists them in its respective ComboBox
        private void InitSaveCollections()
        {
            if (!Directory.Exists(hobbitSaveDir))
            {
                MessageBox.Show($"The Hobbit saves folder not found at: {hobbitSaveDir}");
                Application.Current.Shutdown();
            }

            if (!Directory.Exists(applicationSaveDir))
            {
                MessageBox.Show($"Application saves folder not found at: {applicationSaveDir}");
                Application.Current.Shutdown();
            }

            foreach (string saveCollection in Directory.GetDirectories(applicationSaveDir, "*", SearchOption.TopDirectoryOnly))
            {
                DirectoryInfo info = new DirectoryInfo(saveCollection);
                cbxSaveCollections.Items.Add(info.Name);
            }
        }

        // Detects all saves within the selected save collection and lists them in its respective ComboBox
        private void InitSaves(string saveCollection)
        {
            cbxSaves.Items.Clear();

            foreach (string save in Directory.GetFiles(Path.Join(applicationSaveDir, saveCollection)))
            {
                FileInfo info = new FileInfo(save);
                cbxSaves.Items.Add(info.Name);
            }
        }

        // Moves all existing saves into a new folder within the saves folder for safekeeping
        private void BackupOldSaves()
        {
            string[] oldFiles = Directory.GetFiles(hobbitSaveDir);

            if (oldFiles.Length == 0)
            {
                return;
            }

            string dateTimeStamp = DateTime.Now.ToString("dd/MM/yyyy_h-mm-ss");
            string backupName = $"saves_backup_{dateTimeStamp}";
            backupDir = Path.Join(hobbitSaveDir, backupName);
            
            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }

            foreach (string save in oldFiles)
            {
                FileInfo info = new FileInfo(save);
                File.Move(save, Path.Join(backupDir, info.Name));
            }

            didBackup = true;
        }

        // Moves all backed up saves back into the saves folder
        private void RestoreOldSaves()
        {
            if (!Directory.Exists(backupDir))
            {
                return;
            }

            try
            {
                foreach (string save in Directory.GetFiles(backupDir))
                {
                    FileInfo info = new FileInfo(save);
                    File.Move(save, Path.Join(hobbitSaveDir, info.Name));
                }
            }
            catch
            {
                MessageBox.Show($"Could not automatically restore previous saves. They are located in {backupDir}");
                return;
            }

            Directory.Delete(backupDir, true);
        }

        // Removes all files in the saves folder (this does not include folders)
        private void ClearSaves()
        {
            foreach (string save in Directory.GetFiles(hobbitSaveDir))
            {
                File.Delete(save);
            }
        }

        // Copies the selected save into the saves folder
        private void SelectSave(string save)
        {
            ClearSaves();
            File.Copy(Path.Join(applicationSaveDir, cbxSaveCollections.Text, save), Path.Join(hobbitSaveDir, save));
        }

        // Updates the saves ComboBox when the save collection ComboBox is updated
        private void cbxSaveCollections_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string? selected = cbxSaveCollections.SelectedItem.ToString();

            if (selected != null && selected != string.Empty)
            {
                InitSaves(selected);
            }
        }

        // Safely gets the value of the saves ComboBox the selected save to the saves folder
        private void cbxSaves_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string? selected = cbxSaves.SelectedItem.ToString();

            if (selected != null && selected != string.Empty)
            {
                SelectSave(selected);
            }
        }

        // Ensures that proper cleanup will be done before closing the program
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            ClearSaves();

            if (didBackup)
            {
                RestoreOldSaves();
            }

            base.OnClosing(e);
        }
    }
}
