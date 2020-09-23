﻿using Controller;
using System;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для Parametres.xaml
    /// </summary>
    public partial class Parametres : Window
    {
        private BPsControl BPsControl = new BPsControl();
        public Parametres()
        {
            InitializeComponent();

            BPsDTOs _tasks = (BPsDTOs)this.Resources["tasks"];
            _tasks = BPsControl.GetBps(_tasks);

        }

        private void dataGrid1_AutoGeneratedColumns(object sender, EventArgs e)
        {
            //dataGrid1.Columns[0].Visibility = Visibility.Collapsed;
            //dataGrid1.Columns[1].Visibility = Visibility.Collapsed;
            dataGrid1.ColumnWidth = DataGridLength.SizeToCells;

        }

        private void dataGrid1_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
        }

        private void dataGrid1_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (*.json)|*.json";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.SafeFileName;
                (sender as TextBox).Text = filename;
            }

        }

        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.SafeFileName;
                (sender as TextBox).Text = filename;
            }
        }
    }
}