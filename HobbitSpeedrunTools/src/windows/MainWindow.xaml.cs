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
        private readonly SaveManager saveManager;
        private readonly CheatManager cheatManager;
        private readonly ConfigManager configManager;

        private bool updatingSaveManager;

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

            try
            {
                cheatManager = new CheatManager();
                cheatManager.onBilboPositionUpdate += (x, y, z) => Dispatcher.Invoke(() => UpdateBilboPosition(x, y, z));
                cheatManager.onBilboRotationUpdate += (degrees) => Dispatcher.Invoke(() => UpdateBilboRotation(degrees));
                cheatManager.onClipwarpPositionUpdate += (x, y, z) => Dispatcher.Invoke(() => UpdateClipwarpPositition(x, y, z));

                saveManager = new SaveManager();
                saveManager.onSaveCollectionChanged += () => Dispatcher.Invoke(() => UpdateSavesManagerUI());
                saveManager.onSaveChanged += () => Dispatcher.Invoke(() => UpdateSavesManagerUI());

                StatusManager statusManager = new(cheatManager, saveManager);
                cheatManager.statusManager = statusManager;

                configManager = new ConfigManager();
                HotkeyManager hotkeyManager = new(saveManager, cheatManager, configManager);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

            itcCheats.ItemsSource = cheatManager.toggleCheatList;

            foreach (SaveManager.SaveCollection? saveCollection in saveManager.SaveCollections)
            {
                if (saveCollection != null) cbxSaveCollections.Items.Add(saveCollection.name);
                else cbxSaveCollections.Items.Add("Disabled");
            }

            cbxSaveCollections.SelectedIndex = 0;

            UpdateBilboPosition(0, 0, 0);
            UpdateBilboRotation(0);
            UpdateClipwarpPositition(0, 0, 0);
        }

        public void UpdateBilboPosition(float x, float y, float z)
        {
            txtBilboPosX.Text = Math.Round(x, 1).ToString("0.0");
            txtBilboPosY.Text = Math.Round(y, 1).ToString("0.0");
            txtBilboPosZ.Text = Math.Round(z, 1).ToString("0.0");
        }

        public void UpdateBilboRotation(double degrees)
        {
            txtBilboRotation.Text = Math.Round(degrees, 1).ToString("0.0");
        }

        public void UpdateClipwarpPositition(float x, float y, float z)
        {
            txtClipwarpPosX.Text = Math.Round(x, 1).ToString("0.0");
            txtClipwarpPosY.Text = Math.Round(y, 1).ToString("0.0");
            txtClipwarpPosZ.Text = Math.Round(z, 1).ToString("0.0");
        }

        private void UpdateSavesManagerUI()
        {
            cbxSaves.Items.Clear();

            if (saveManager.SelectedSaveCollection != null && saveManager.Saves != null)
            {
                foreach (SaveManager.Save save in saveManager.Saves)
                    cbxSaves.Items.Add(save.name);

                cbxSaves.IsEnabled = true;
            }
            else
            {
                cbxSaves.IsEnabled = false;
            }

            cbxSaveCollections.SelectedIndex = saveManager.SaveCollectionIndex;
            cbxSaves.SelectedIndex = saveManager.SaveIndex;
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

        private void btnOpenHelp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start https://github.com/milankarman/HobbitSpeedrunTools#readme") { CreateNoWindow = true });
        }

        private void cbxSaveCollections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!updatingSaveManager && cbxSaveCollections.SelectedIndex >= 0)
            {
                updatingSaveManager = true;
                saveManager.SelectSaveCollection(cbxSaveCollections.SelectedIndex);
                updatingSaveManager = false;
            }
        }

        private void cbxSaves_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!updatingSaveManager && cbxSaves.SelectedIndex >= 0)
            {
                updatingSaveManager = true;
                saveManager.SelectSave(cbxSaves.SelectedIndex);
                updatingSaveManager = false;
            }
        }

        private void btnOpenConfig_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("notepad.exe", Path.Join(".", "config.ini"));
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            saveManager.ClearSaves();

            if (saveManager.DidBackup) saveManager.RestoreOldSaves();

            base.OnClosing(e);
        }
    }
}
