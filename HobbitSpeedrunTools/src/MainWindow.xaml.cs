using System;
using System.Windows;
using System.IO;
using System.Linq;

namespace HobbitSpeedrunTools
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

            // Selects first save collection by default
            if (cbxSaveCollections.Items.Count > 0)
            {
                cbxSaveCollections.SelectedIndex = 0;
            }
        }

        // Detects all folders within the save collections folder and lists them in its respective ComboBox
        private void InitSaveCollections()
        {
            // Check if the required directories exist
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

            // Sorts the save collections based on the number before the period
            string[] unsortedSaveCollections = Directory.GetDirectories(applicationSaveDir, "*", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < unsortedSaveCollections.Length; i++)
            {
                FileInfo info = new FileInfo(unsortedSaveCollections[i]);
                unsortedSaveCollections[i] = info.Name;
            }

            string[] sortedSaveCollections = new string[unsortedSaveCollections.Length];

            // Handle sorting errors
            try
            {
                sortedSaveCollections = SortStringArrayByLeadingNumber(unsortedSaveCollections);
            }
            catch
            {
                MessageBox.Show("Failed to sort save collections. Ensure the folder names are written in the right format.");
                sortedSaveCollections = unsortedSaveCollections;
            }
            finally
            {
                // Adds save collections to their ComboBox
                foreach (string saveCollection in sortedSaveCollections)
                {
                    cbxSaveCollections.Items.Add(saveCollection);
                }
            }
        }

        // Detects all saves within the selected save collection and lists them in its respective ComboBox
        private void InitSaves(string saveCollection)
        {
            cbxSaves.Items.Clear();

            // Sorts the saves collections based on the number before the period
            string[] unsortedSaves = Directory.GetFiles(Path.Join(applicationSaveDir, saveCollection));

            for (int i = 0; i < unsortedSaves.Length; i++)
            {
                FileInfo info = new FileInfo(unsortedSaves[i]);
                unsortedSaves[i] = Path.GetFileNameWithoutExtension(info.Name);
            }

            string[] sortedSaves = new string[unsortedSaves.Length];

            // Handle sorting errors
            try
            {
                sortedSaves = SortStringArrayByLeadingNumber(unsortedSaves);
            }
            catch
            {
                MessageBox.Show("Failed to sort saves. Ensure the save file names are written in the right format.");
                sortedSaves = unsortedSaves;
            }
            finally
            {
                // Adds saves to their ComboBox
                foreach (string save in sortedSaves)
                {
                    cbxSaves.Items.Add(save);
                }
            }
        }

        // Moves all existing saves into a new folder within the saves folder for safekeeping
        private void BackupOldSaves()
        {
            // Get old files and cancel if there are none
            string[] oldFiles = Directory.GetFiles(hobbitSaveDir);

            if (oldFiles.Length == 0)
            {
                return;
            }

            // Generate a name for the backup folder
            string dateTimeStamp = DateTime.Now.ToString("dd/MM/yyyy_h-mm-ss");
            string backupName = $"saves_backup_{dateTimeStamp}";

            backupDir = Path.Join(hobbitSaveDir, backupName);

            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }

            // Moves all old files into backup folder
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
            // Check if the backup still exists
            if (!Directory.Exists(backupDir))
            {
                return;
            }

            // Attempt to move the files back
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

            // Remove the backup directory
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

            try
            {
                File.Copy(Path.Join(applicationSaveDir, cbxSaveCollections.Text, $"{save}.hobbit"), Path.Join(hobbitSaveDir, $"{save}.hobbit"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy save into {hobbitSaveDir}. \n Error: {ex}");
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
            string? selected = cbxSaves.SelectedItem?.ToString();

            if (selected != null && selected != string.Empty)
            {
                SelectSave(selected);
            }
        }

        // Sorts an array of strings based on leading numbers (Example: "7. My String")
        private string[] SortStringArrayByLeadingNumber(string[] array)
        {
            return array.OrderBy(x => int.Parse(x.Split(".")[0])).ToArray();
        }

        // Ensures that proper cleanup will be done before closing the program
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            ClearSaves();

            // Restore the backed up files if a backup was performed
            if (didBackup)
            {
                RestoreOldSaves();
            }

            base.OnClosing(e);
        }
    }
}
