using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
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
        private readonly HotkeyManager hotkeyManager;
        private readonly TimerManager timerManager;

        private bool updatingSaveManager;

        public static bool LoadCheatsWithSave { get; private set; }
        public static bool quickReload { get; private set; }

        private Action<float> valueEditApplyAction;

        public MainWindow()
        {
            InitializeComponent();

            bool isRunning = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly()?.Location)).Length > 1;

            if (isRunning)
            {
                MessageBox.Show("An instance of HST is already running. Aborting.");
                Environment.Exit(1);
            }

            SourceInitialized += (s, e) =>
            {
                MinWidth = ActualWidth;
                MinHeight = ActualHeight - 37;
                Height = MinHeight;
                Width = MinWidth;
            };

            // Add the version number to the titlebar
            Title += $" {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";

            quickReload = cbxQuickReload.IsChecked ?? false;
            LoadCheatsWithSave = cbxLoadCheatsWithSave.IsChecked ?? false;

            try
            {
                timerManager = new();
                timerManager.onTimerTick += (time) => Dispatcher.Invoke(() => UpdateTimer(time));
                timerManager.onNewBestTime += (time) => Dispatcher.Invoke(() => UpdateBestTime(time));
                timerManager.onUpdateAverageTime += (time) => Dispatcher.Invoke(() => UpdateAverageTime(time));

                cheatManager = new();
                cheatManager.onBilboPositionUpdate += (position) => Dispatcher.Invoke(() => UpdateBilboPosition(position));
                cheatManager.onBilboRotationUpdate += (degrees) => Dispatcher.Invoke(() => UpdateBilboRotation(degrees));
                cheatManager.onClipwarpPositionUpdate += (position) => Dispatcher.Invoke(() => UpdateClipwarpPositition(position));
                cheatManager.onHealthUpdate += (health) => Dispatcher.Invoke(() => UpdateHealth(health));
                cheatManager.onLevelUpdate += (level) => Dispatcher.Invoke(() => UpdateLevel(level));

                saveManager = new(cheatManager);
                saveManager.onSaveCollectionChanged += () => Dispatcher.Invoke(() => UpdateSavesManagerUI());
                saveManager.onSaveChanged += () => Dispatcher.Invoke(() => UpdateSavesManagerUI());

                StatusManager statusManager = new(cheatManager, saveManager, timerManager);
                cheatManager.statusManager = statusManager;

                configManager = new();
                hotkeyManager = new(saveManager, cheatManager, configManager);
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

            UpdateBilboPosition(Vector3.Zero);
            UpdateBilboRotation(0);
            UpdateClipwarpPositition(Vector3.Zero);
        }

        public void UpdateTimer(TimeSpan time)
        {
            txtTimer.Text = time.ToString("mm\\:ss\\.fff");
        }

        public void UpdateBestTime(TimeSpan time)
        {
            txtBestTime.Text = time.ToString("mm\\:ss\\.fff");
        }

        public void UpdateAverageTime(TimeSpan time)
        {
            txtAverageTime.Text = time.ToString("mm\\:ss\\.fff");
        }

        public void UpdateBilboPosition(Vector3 position)
        {
            txtBilboPosX.Text = "X: " + Math.Round(position.X, 1).ToString("0.0");
            txtBilboPosY.Text = "Y: " + Math.Round(position.Y, 1).ToString("0.0");
            txtBilboPosZ.Text = "Z: " + Math.Round(position.Z, 1).ToString("0.0");
        }

        public void UpdateBilboRotation(double degrees)
        {
            txtBilboRotation.Text = Math.Round(degrees, 1).ToString("0.0");
        }

        public void UpdateClipwarpPositition(Vector3 position)
        {
            txtClipwarpPosX.Text = "X: " + Math.Round(position.X, 1).ToString("0.0");
            txtClipwarpPosY.Text = "Y: " + Math.Round(position.Y, 1).ToString("0.0");
            txtClipwarpPosZ.Text = "Z: " + Math.Round(position.Z, 1).ToString("0.0");
        }

        public void UpdateHealth(float health)
        {
            txtBilboHealth.Text = health.ToString();
        }
        public void UpdateLevel(float level)
        {
            txtBilboLevel.Text = level.ToString();
        }

        private void UpdateSavesManagerUI()
        {
            cbxSaves.Items.Clear();

            if (saveManager.SelectedSaveCollection != null && saveManager.Saves != null)
            {
                foreach (SaveManager.Save save in saveManager.Saves)
                    cbxSaves.Items.Add(save.name);

                cbxSaves.IsEnabled = true;
                // Enable Save and Clear all current cheats.
                btnApplyCheatsCollection.IsEnabled = true;
                btnApplyCheatsSave.IsEnabled = true;
            }
            else
            {
                cbxSaves.IsEnabled = false;
                // Disable Save and Clear save specific cheats.
                btnApplyCheatsCollection.IsEnabled = false;
                btnApplyCheatsSave.IsEnabled = false;
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

        private void cbxTimerMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (timerManager == null) return;

            switch (cbxTimerMode.SelectedValue?.ToString())
            {
                case "Off":
                    timerManager.SetTimerMode(TimerManager.TIMER_MODE.OFF);
                    SetLevelTimerSettingVisibility(false);
                    SetPointTimerSettingVisibility(false);
                    break;

                case "Full Level":
                    timerManager.SetTimerMode(TimerManager.TIMER_MODE.FULL_LEVEL);
                    SetLevelTimerSettingVisibility(true);
                    SetPointTimerSettingVisibility(false);
                    break;

                case "Move To Point":
                    timerManager.SetTimerMode(TimerManager.TIMER_MODE.MOVE_TO_POINT);
                    SetLevelTimerSettingVisibility(false);
                    SetPointTimerSettingVisibility(true);
                    break;
            }
        }

        private void cbxSelectedLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (timerManager != null)
            {
                timerManager.selectedLevel = cbxSelectedLevel.SelectedIndex;
                timerManager.ResetTimerStates();
            }
        }

        private void SetLevelTimerSettingVisibility(bool visible)
        {
            lblSelectedLevel.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            cbxSelectedLevel.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetPointTimerSettingVisibility(bool visible)
        {
            lblEndPoint.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            btnSetEndPoint.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            lblPointRadius.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            txtPointRadius.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnSetEndPoint_Click(object sender, RoutedEventArgs e)
        {
            timerManager.SetEndPointPosition();
        }

        private void btnResetBestTime_Click(object sender, RoutedEventArgs e)
        {
            timerManager.ResetTimerStates();
        }

        private void btnResetAverageTime_Click(object sender, RoutedEventArgs e)
        {
            timerManager.ResetAverageTime();
        }

        private void btnApplyCheatsSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                saveManager.ApplyCheatsToSave();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot apply cheats to current save.\n{ex.Message}");
            }
        }

        private void btnApplyCheatsCollection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("This action will overwrite all of your applied cheats for this entire save collection, are you sure?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    saveManager.ApplyCheatsToCollection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot apply cheats to current collection.\n{ex.Message}");
            }
        }

        // Ensures a textbox only allows integer inputs of a given range
        public static void ClampInteger(TextBox textBox, int minValue, int maxValue)
        {
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                int value = Math.Clamp(int.Parse(textBox.Text), minValue, maxValue);
                textBox.Text = value.ToString();
            }
        }

        private void txtPointRadius_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ClampInteger(txtPointRadius, 1, 9999);

                if (timerManager != null)
                    timerManager.endPointDistance = int.Parse(txtPointRadius.Text);
            }
            catch { }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (cbxSaveCollections.SelectedIndex > 0)
            {
                saveManager.ClearSaves();

                if (saveManager.DidBackup) saveManager.RestoreOldSaves();
            }

            saveManager.TryWriteCollectionsSettingsFile();
            base.OnClosing(e);
        }

        private void cbxLoadCheatsWithSave_Click(object sender, RoutedEventArgs e)
        {
            LoadCheatsWithSave = cbxLoadCheatsWithSave.IsChecked ?? false;
        }
        private void cbxQuickReload_Click(object sender, RoutedEventArgs e)
        {
            quickReload = cbxQuickReload.IsChecked ?? false;
        }

        // A bit of a hack to prevent the plus and minus keys from flipping the load cheats checkbox
        private void CheckBox_Indeterminate(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).IsChecked = false;
        }

        private void ShowEditPrompt(string label, float value, Action<float> _applyAction)
        {
            grpEditValue.Visibility = Visibility.Visible;
            txbEditValue.Text = label;
            txtEditValue.Text = value.ToString();
            valueEditApplyAction = _applyAction;

            txtEditValue.Dispatcher.BeginInvoke(() =>
            {
                txtEditValue.Focus();
                txtEditValue.SelectAll();
            });
        }

        private void txtBilboLevel_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Action<float> applyAction = cheatManager.SetLevel;
            float value = float.Parse(txtBilboLevel.Text);
            ShowEditPrompt("Level / Max Health:", value, applyAction);
        }

        private void txtBilboHealth_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Action<float> applyAction = cheatManager.SetHealth;
            float value = float.Parse(txtBilboHealth.Text);
            ShowEditPrompt("Health:", value, applyAction);
        }

        private void btnEditValueCancel_Click(object sender, RoutedEventArgs e)
        {
            grpEditValue.Visibility = Visibility.Hidden;
        }

        private void btnEditValueApply_Click(object sender, RoutedEventArgs e)
        {
            float updateValue = float.Parse(txtEditValue.Text);
            valueEditApplyAction.Invoke(updateValue);
            grpEditValue.Visibility = Visibility.Hidden;
        }

        private void btnLockBilboHealth_Click(object sender, RoutedEventArgs e)
        {
            cheatManager.lockHealth = !cheatManager.lockHealth;
            btnLockBilboHealth.Content = cheatManager.lockHealth ? "🔓" : "🔒";
            txtBilboHealth.IsEnabled = !cheatManager.lockHealth;
        }

        private void btnLockBilboLevel_Click(object sender, RoutedEventArgs e)
        {
            cheatManager.lockLevel = !cheatManager.lockLevel;
            btnLockBilboLevel.Content = cheatManager.lockLevel ? "🔓" : "🔒";
            txtBilboLevel.IsEnabled = !cheatManager.lockLevel;
        }
    }
}
