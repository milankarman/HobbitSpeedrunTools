using System;
using System.Windows;
using System.IO;
using System.Diagnostics;

namespace hobbit_save_manager
{
    public partial class MainWindow : Window
    {
        private string hobbitSaveDir = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "The Hobbit");
        private string applicationSaveDir = Path.Join(".", "save-collections");
   
        public MainWindow()
        {
            InitializeComponent();
            InitSaveCollections();
        }

        public void InitSaveCollections()
        {
            Trace.WriteLine("Test");

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

        public void InitSaves(string saveCollection)
        {
            cbxSaves.Items.Clear();

            foreach (string save in Directory.GetFiles(Path.Join(applicationSaveDir, saveCollection)))
            {
                cbxSaves.Items.Add(save);
            }
        }

        private void cbxSaveCollections_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            InitSaves(cbxSaveCollections.Text);
        }
    }
}
