using System;
using System.IO;
using System.Linq;

namespace HobbitSpeedrunTools
{
    public class SaveManager
    {
        public class SaveCollection
        {
            public string name;
            public string path;
            public Save[] saves;

            public SaveCollection(string _name, string _path, Save[] _saves)
            {
                name = _name;
                path = _path;
                saves = _saves;
            }
        }

        public class Save
        {
            public string name;
            public string path;

            public Save(string _name, string _path)
            {
                name = _name;
                path = _path;
            }
        }

        public SaveCollection?[] SaveCollections { get; private set; }
        public Save[]? Saves { get => SelectedSaveCollection?.saves; }

        public SaveCollection? SelectedSaveCollection { get => SaveCollections[SaveCollectionIndex]; }
        public Save? SelectedSave { get => SelectedSaveCollection?.saves[SaveIndex]; }


        public int SaveCollectionIndex { get; private set; }
        public int SaveIndex { get; private set; }

        public bool DidBackup { get; private set; }

        public Action? onSaveCollectionChanged;
        public Action? onSaveChanged;

        private readonly string hobbitSaveDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "The Hobbit");
        private readonly string applicationSaveDir = "save-collections";
        private string backupDir = "";

        public SaveManager()
        {
            if (!Directory.Exists(hobbitSaveDir))
            {
                throw new Exception("The Hobbit saves folder not found at: {hobbitSaveDir}");
            }

            if (!Directory.Exists(applicationSaveDir))
            {
                throw new Exception("The Hobbit saves folder not found at: {applicationSaveDir}");
            }

            SaveCollections = GetSaveCollections();
        }

        private SaveCollection[] GetSaveCollections()
        {
            string[] saveCollectionPaths = Directory.GetDirectories(applicationSaveDir, "*", SearchOption.TopDirectoryOnly);
            SaveCollection[] saveCollections = new SaveCollection[saveCollectionPaths.Length + 1];

            for (int i = 0; i < saveCollectionPaths.Length; i++)
            {
                FileInfo info = new(saveCollectionPaths[i]);

                string name = info.Name;
                string path = saveCollectionPaths[i];

                Save[] saves = GetSaves(path);
                saveCollections[i + 1] = new SaveCollection(name, path, saves);
            }

            try
            {
                saveCollections = saveCollections.OrderBy(x => {
                    if (x == null) return 0;
                    return int.Parse(x.name.Split(".")[0]);
                }).ToArray();
            }
            catch
            {
                throw new Exception("The Hobbit saves folder not found at: {applicationSaveDir}");
            }

            return saveCollections;
        }

        private Save[] GetSaves(string saveCollectionPath)
        {
            string[] savePaths = Directory.GetFiles(saveCollectionPath);
            Save[] saves = new Save[savePaths.Length];

            for (int i = 0; i < savePaths.Length; i++)
            {
                FileInfo info = new(savePaths[i]);

                saves[i] = new Save(info.Name.Replace(".hobbit", ""), savePaths[i]);
            }

            try
            {
                saves = saves.OrderBy(x => int.Parse(x.name.Split(".")[0])).ToArray();
            }
            catch
            {
                throw new Exception("Failed to sort saves. Ensure the save file names are written in the right format.");
            }

            return saves;
        }

        public void SelectSaveCollection(int _saveCollectionIndex)
        {
            if (!DidBackup) BackupOldSaves();

            ClearSaves();
            SaveCollectionIndex = Math.Clamp(_saveCollectionIndex, 0, SaveCollections.Length - 1);

            if (SelectedSaveCollection != null)
            {
                SelectSave(0);
            }
            else
            {
                SaveIndex = 0;

                if (DidBackup) RestoreOldSaves();
            }

            onSaveCollectionChanged?.Invoke();
        }

        public void SelectSave(int _saveIndex)
        {
            if (SelectedSaveCollection != null)
            {
                ClearSaves();
                SaveIndex = Math.Clamp(_saveIndex, 0, SelectedSaveCollection.saves.Length - 1);
                
                if (SelectedSave != null)
                    File.Copy(Path.Join(SelectedSave.path), Path.Join(hobbitSaveDir, SelectedSave.name + ".hobbit"));
            }

            onSaveChanged?.Invoke();
        }

        public void BackupOldSaves()
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

        public void RestoreOldSaves()
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

        public void ClearSaves()
        {
            foreach (string save in Directory.GetFiles(hobbitSaveDir))
            {
                File.Delete(save);
            }
        }

        public void NextSaveCollection() => SelectSaveCollection(SaveCollectionIndex + 1);

        public void PreviousSaveCollection() => SelectSaveCollection(SaveCollectionIndex - 1);

        public void NextSave() => SelectSave(SaveIndex + 1);

        public void PreviousSave() => SelectSave(SaveIndex - 1);
    }
}
