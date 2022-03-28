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
        private const string disabledText = "Disabled";

        private readonly SaveManager saveManager;

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
                CheatManager.InitCheatManager();
                CheatManager.onBilboPositionUpdate += (x, y, z) => Dispatcher.Invoke(() => UpdateBilboPosition(x, y, z));
                CheatManager.onBilboRotationUpdate += (degrees) => Dispatcher.Invoke(() => UpdateBilboRotation(degrees));
                CheatManager.onClipwarpPositionUpdate += (x, y, z) => Dispatcher.Invoke(() => UpdateClipwarpPositition(x, y, z));

                saveManager = new SaveManager();
                ConfigManager.InitConfigManager();
                HotkeyManager.InitHotkeyManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

            itcCheats.ItemsSource = CheatManager.toggleCheatList;

            cbxSaveCollections.Items.Add("Disabled");

            foreach (SaveManager.SaveCollection saveCollection in saveManager.SaveCollections)
            {
                cbxSaveCollections.Items.Add(saveCollection.name);
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

        // Updates the saves ComboBox when the save collection ComboBox is updated
        private void cbxSaveCollections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxSaveCollections.SelectedIndex > 0)
            {
                saveManager.SelectSaveCollection(cbxSaveCollections.SelectedIndex - 1);
                cbxSaves.Items.Clear();

                foreach (SaveManager.Save save in saveManager.SelectedSaveCollection.saves)
                {
                    cbxSaves.Items.Add(save.name);
                }

                cbxSaves.IsEnabled = true;
                cbxSaves.SelectedItem = saveManager.SelectedSave.name;
            }
            else
            {
                saveManager.ClearSaves();

                if (saveManager.DidBackup) saveManager.RestoreOldSaves();

                cbxSaves.Items.Clear();
                cbxSaves.IsEnabled = false;
            }

        }

        // Safely gets the value of the saves ComboBox the selected save to the saves folder
        private void cbxSaves_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saveManager.SelectSave(cbxSaves.SelectedIndex);
        }

        // Ensures that proper cleanup will be done before closing the program
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            saveManager.ClearSaves();

            if (saveManager.DidBackup) saveManager.RestoreOldSaves();

            base.OnClosing(e);
        }

        // Opens the config file in notepad
        private void btnOpenConfig_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("notepad.exe", Path.Join(".", "config.ini"));
        }

        private void btnOpenHelp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start https://github.com/milankarman/HobbitSpeedrunTools#readme") { CreateNoWindow = true });
        }
    }
}
