using EFModels;
using EFModels.LogsDB;
using EFModels.MainDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для BPs.xaml
    /// </summary>
    public partial class BPs : Window
    {
        private int Stage = 1;

        public BPs()
        {
            InitializeComponent();

            ChangeStage(0);
        }

        private void openCases_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                ChangeStage(1);
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (Stage == 1)
            {
                ChangeStage(0);
            }
        }

        private void forward_Click(object sender, RoutedEventArgs e)
        {
            //openCases?
        }

        private void ChangeStage(int stage)
        {
            MainDBContext mainDB = new MainDBContext();
            switch (stage)
            {
                case 0:
                    
                    var BPs = mainDB.BPs.Select(
                        b => new BPdto
                        {
                            Номер = b.Id,
                            Система = b.System.Name,
                            Название = b.Name
                        }).ToList();
                    dataGrid.ItemsSource = BPs;
                    break;
                case 1:
                    BPdto selectedRow = dataGrid.SelectedItem as BPdto;
                    var BPId = Convert.ToInt32(selectedRow.Номер);

                    var Cases = mainDB.BPCases.Where(b => b.BPId == BPId).ToList();
                    dataGrid.ItemsSource = Cases;
                    dataGrid.Columns.Last().Visibility = Visibility.Collapsed;
                    
                    break;
            }
            Stage = stage;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Stage == 1)
            {
                if (dataGrid.SelectedItem != null)
                {
                    Case newCase;

                    BPCase selectedRow = dataGrid.SelectedItem as BPCase;
                    var CaseId = selectedRow.CaseId;
                    if (CaseId == -1)
                        newCase = new CaseBuilder().CreateGeneralCase();
                    else
                        newCase = new CaseBuilder().CreateCase(CaseId);

                    //visualize case
                }
            }
        }
    }

    public class BPdto
    {
        public long Номер { get; set; }
        public string Система { get; set; }
        public string Название { get; set; }
    }
}
