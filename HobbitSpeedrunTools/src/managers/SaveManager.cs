using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace HobbitSpeedrunTools
{
    public class SaveManager
    {
        public class SaveCollection(string _name, string _path, Save[] _saves, SaveSettings[] _saveSettings)
        {
            public string name = _name;
            public string path = _path;
            public Save[] saves = _saves;
            public SaveSettings[] saveSettings = _saveSettings;
        }

        public class Save(string _name, string _path)
        {
            public string name = _name;
            public string path = _path;
        }

        public class SaveSettings(string _name, int toggleCheatsLength)
        {
            public string name = _name;
            public bool[] toggles = new bool[toggleCheatsLength];
            public float clipwarpX;
            public float clipwarpY;
            public float clipwarpZ;
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
        private readonly CheatManager cheatManager;

        private readonly string hobbitSaveDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "The Hobbit");
        private readonly string applicationSaveDir = "save-collections";
        private readonly string saveSettingsFilename = "savesettings";
        private string backupDir = "";

        public SaveManager(CheatManager _cheatManager)
        {
            if (!Directory.Exists(hobbitSaveDir))
            {
                throw new Exception($"The Hobbit saves folder not found at {hobbitSaveDir}, make sure you've started a new game in The Hobbit and try " +
                    $"running HobbitSpeedrunTools again.");
            }

            if (!Directory.Exists(applicationSaveDir))
            {
                throw new Exception($"Save collections folder not found at {applicationSaveDir}." +
                    $"If you're installing HobbitSpeedrunTools manually make sure to extract all included files.");
            }

            cheatManager = _cheatManager;
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
                SaveSettings[] saveSettings = GetSaveSettings(path, saves);
                saveCollections[i + 1] = new SaveCollection(name, path, saves, saveSettings);
            }

            try
            {
                saveCollections = saveCollections.OrderBy(x =>
                {
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

        private static Save[] GetSaves(string saveCollectionPath)
        {
            string[] savePaths = Directory.GetFiles(saveCollectionPath).Where(name => name.EndsWith(".hobbit")).ToArray();
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

        private SaveSettings[] GetSaveSettings(string _path, Save[] saves)
        {
            string path = Path.Join(_path, saveSettingsFilename + ".json");

            SaveSettings[] collectionSettings = new SaveSettings[saves.Length];
            int cheatLength = cheatManager.toggleCheatList.Length;

            for (int i = 0; i < saves.Length; i++)
            {
                // Create Default Collection Settings.
                collectionSettings[i] = new(saves[i].name, cheatLength);
            }

            if (File.Exists(path))
            {
                // ** NOTE **
                //Check to make sure data from file was read correctly. Not sure of a way to notify the user if the data was not read successfully...
                if (JsonConvert.DeserializeObject<List<SaveSettings>>(File.ReadAllText(path)) is List<SaveSettings> collectionFileSettings)
                {
                    // Loop through default collection settings
                    for (int i = 0; i < collectionSettings.Length; i++)
                    {
                        // Check to see if setting exists. If it does, then set it to the save.
                        // Any new saves will have default settings.
                        // Any removed saves will simply not get copied over and overwritten.
                        foreach (SaveSettings fileSetting in collectionFileSettings)
                        {
                            string collectionSaveWithoutNumber = collectionSettings[i].name.Split(".", 2)[1];
                            string fileSaveWithoutNumber = fileSetting.name.Split(".", 2)[1];

                            if (collectionSaveWithoutNumber == fileSaveWithoutNumber)
                            {
                                string nameWithCorrectSaveNumber = collectionSettings[i].name;
                                collectionSettings[i] = fileSetting;
                                collectionSettings[i].name = nameWithCorrectSaveNumber;
                                break;
                            }

                        }
                    }
                }

                //Attempt to write new settings to the file and return.
                return collectionSettings;
            }

            // If File doesn't exist, create it and attempt to write to it. Then return.
            return collectionSettings;
        }

        public void ApplyCheatsToSave()
        {
            // Null checking to appease the IDE and so nothing breaks.
            if (SelectedSaveCollection is null) return;
            // Get current save specific settings of current selected collection.
            SaveSettings saveSetting = SelectedSaveCollection.saveSettings[SaveIndex];
            ToggleCheat[] toggleCheats = cheatManager.toggleCheatList;

            // Iterate through the selected saves toggles.
            for (int i = 0; i < saveSetting.toggles.Length; i++)
            {
                ToggleCheat toggleCheat = toggleCheats[i];
                saveSetting.toggles[i] = toggleCheat.Enabled;

                // If any toggle cheats with a warp is enabled, also set the current clipwarp positions.
                if (toggleCheat is WarpToggleCheat warpToggleCheat && toggleCheat.Enabled)
                    SaveWarpPosToSetting(saveSetting, warpToggleCheat);
            }
        }

        public void ApplyCheatsToCollection()
        {
            // Null checking to appease the IDE and so nothing breaks.
            if (SelectedSaveCollection is null) return;
            // Get settings of every save in the current selected collection.
            SaveSettings[] collectionSettings = SelectedSaveCollection.saveSettings;
            ToggleCheat[] toggleCheats = cheatManager.toggleCheatList;

            // Interate through every save.
            for (int i = 0; i < collectionSettings.Length; i++)
            {
                // Iterate through the selected saves toggles.
                for (int j = 0; j < collectionSettings[i].toggles.Length; j++)
                {
                    ToggleCheat toggleCheat = toggleCheats[j];
                    collectionSettings[i].toggles[j] = toggleCheat.Enabled;

                    // If any toggle cheats with a warp is enabled, also set the current clipwarp positions.
                    if (toggleCheat is WarpToggleCheat warpToggleCheat && toggleCheat.Enabled)
                        SaveWarpPosToSetting(collectionSettings[i], warpToggleCheat);
                }
            }
        }

        private static void SaveWarpPosToSetting(SaveSettings setting, WarpToggleCheat warpToggleCheat)
        {
            setting.clipwarpX = warpToggleCheat.SavedWarpPos.X;
            setting.clipwarpY = warpToggleCheat.SavedWarpPos.Y;
            setting.clipwarpZ = warpToggleCheat.SavedWarpPos.Z;
        }

        public void TryWriteCollectionsSettingsFile()
        {
            try
            {
                foreach (SaveCollection? collection in SaveCollections)
                {
                    if (collection != null)
                    {
                        string path = Path.Join(collection.path, saveSettingsFilename + ".json");
                        using StreamWriter sw = new(path);
                        sw.Write(JsonConvert.SerializeObject(collection.saveSettings));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to write save settings: \n{ex.Message}");
            }
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
            if (SelectedSaveCollection == null) return;

            ClearSaves();
            SaveIndex = Math.Clamp(_saveIndex, 0, SelectedSaveCollection.saves.Length - 1);

            if (SelectedSave == null) return;

            File.Copy(Path.Join(SelectedSave.path), Path.Join(hobbitSaveDir, SelectedSave.name + ".hobbit"));

            onSaveChanged?.Invoke();

            if (MainWindow.LoadCheatsWithSave)
            {
                SaveSettings settings = SelectedSaveCollection.saveSettings[SaveIndex];
                // Changed it all to be in one function.
                cheatManager.UpdateCheats(settings);
            }
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
