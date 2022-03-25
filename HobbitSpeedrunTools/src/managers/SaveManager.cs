using System;
using System.IO;
using System.Linq;

namespace HobbitSpeedrunTools
{
    public static class SaveManager
    {
        public static bool DidBackup { get; private set; }

        public static int SelectedSaveCollectionIndex { get; set; }
        public static int SelectedSaveIndex { get; set; }

        private static readonly string hobbitSaveDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "The Hobbit");
        private static readonly string applicationSaveDir = Path.Join(".", "save-collections");
        private static string backupDir = "";

        // Detects all folders within the save collections folder and lists them in its respective ComboBox
        public static string[] GetSaveCollections()
        {
            // Check if the required directories exist
            if (!Directory.Exists(hobbitSaveDir))
            {
                throw new Exception("The Hobbit saves folder not found at: {hobbitSaveDir}");
            }

            if (!Directory.Exists(applicationSaveDir))
            {
                throw new Exception("The Hobbit saves folder not found at: {applicationSaveDir}");
            }

            // Sorts the save collections based on the number before the period
            string[] unsortedSaveCollections = Directory.GetDirectories(applicationSaveDir, "*", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < unsortedSaveCollections.Length; i++)
            {
                FileInfo info = new(unsortedSaveCollections[i]);
                unsortedSaveCollections[i] = info.Name;
            }

            string[] sortedSaveCollections;

            // Handle sorting errors
            try
            {
                sortedSaveCollections = SortStringArrayByLeadingNumber(unsortedSaveCollections);
            }
            catch
            {
                return unsortedSaveCollections;
                throw new Exception("The Hobbit saves folder not found at: {applicationSaveDir}");
            }

            return sortedSaveCollections;
        }

        // Detects all saves within the selected save collection and lists them in its respective ComboBox
        public static string[] GetSaves(string saveCollection)
        {
            // Sorts the saves collections based on the number before the period
            string[] unsortedSaves = Directory.GetFiles(Path.Join(applicationSaveDir, saveCollection));

            for (int i = 0; i < unsortedSaves.Length; i++)
            {
                FileInfo info = new(unsortedSaves[i]);
                unsortedSaves[i] = Path.GetFileNameWithoutExtension(info.Name);
            }

            string[] sortedSaves;

            // Handle sorting errors
            try
            {
                sortedSaves = SortStringArrayByLeadingNumber(unsortedSaves);
            }
            catch
            {
                return unsortedSaves;
                throw new Exception("Failed to sort saves. Ensure the save file names are written in the right format.");
            }

            return sortedSaves;
        }

        // Copies the selected save into the saves folder
        public static void SelectSave(string saveCollection, string save)
        {
            ClearSaves();

            File.Copy(Path.Join(applicationSaveDir, saveCollection, $"{save}.hobbit"), Path.Join(hobbitSaveDir, $"{save}.hobbit"));
        }

        // Moves all existing saves into a new folder within the saves folder for safekeeping
        public static void BackupOldSaves()
        {
            // Get old files and cancel if there are none
            string[] oldFiles = Directory.GetFiles(hobbitSaveDir);

            if (oldFiles.Length == 0)
            {
                return;
            }

            // Generate a name for the backup folder
            string dateTimeStamp = DateTime.Now.ToString("dd-MM-yyyy_h-mm-ss");
            string backupName = $"saves_backup_{dateTimeStamp}";

            backupDir = Path.Join(hobbitSaveDir, backupName);

            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }

            // Moves all old files into backup folder
            foreach (string save in oldFiles)
            {
                FileInfo info = new(save);
                File.Move(save, Path.Join(backupDir, info.Name));
            }

            DidBackup = true;
        }

        // Moves all backed up saves back into the saves folder
        public static void RestoreOldSaves()
        {
            // Check if the backup still exists
            if (!Directory.Exists(backupDir))
            {
                return;
            }

            // Attempt to move the files back
            try
            {
                // Delete copied save manager saves
                foreach (string directoryFile in Directory.GetFiles(hobbitSaveDir))
                {
                    File.Delete(directoryFile);
                }

                // Move the saves out of the backup
                foreach (string save in Directory.GetFiles(backupDir))
                {
                    FileInfo info = new(save);
                    File.Move(save, Path.Join(hobbitSaveDir, info.Name));
                }

                DidBackup = false;
            }
            catch
            {
                throw new Exception($"Could not automatically restore previous saves. They are located in {backupDir}");
            }

            // Remove the backup directory
            Directory.Delete(backupDir, true);
            DidBackup = false;
        }


        // Removes all files in the saves folder (this does not include folders)
        public static void ClearSaves()
        {
            foreach (string save in Directory.GetFiles(hobbitSaveDir))
            {
                File.Delete(save);
            }
        }

        // Sorts an array of strings based on leading numbers (Example: "7. My String")
        private static string[] SortStringArrayByLeadingNumber(string[] array)
        {
            return array.OrderBy(x => int.Parse(x.Split(".")[0])).ToArray();
        }
    }
}
