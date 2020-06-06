using EFModels;
using EFModels.MainDB;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Msagl.Drawing;
using Color = Microsoft.Msagl.Drawing.Color;

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для BPs.xaml
    /// </summary>
    public partial class BPs : Window
    {
        private int Stage = 0;

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
            dataGrid.SelectedIndex = 0;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Case newCase;
                if (Stage == 1)
                {
                    //visualize case
                    BPCase selectedRow = dataGrid.SelectedItem as BPCase;
                    var CaseId = selectedRow.CaseId;
                    if (CaseId == -1)
                        newCase = new CaseBuilder().CreateGeneralCase();
                    else
                        newCase = new CaseBuilder().CreateCase(CaseId);
                }
                else
                {
                    //build general case
                    var selectedBP = dataGrid.SelectedItem as BPdto;
                    MainDBContext mainDB = new MainDBContext();
                    var BPname = mainDB.BPs.FirstOrDefault(s => s.Name == selectedBP.Название).Name;

                    newCase = new CaseBuilder().CreateGeneralCase();

                    graphControl1.Graph = null;
                    graphControl1.Graph = Graph_Setup(newCase);

                }
            }
        }
        private Graph Graph_Setup(Case @case)
        {
            Graph dataGraph = new Graph();

            foreach (var ev in @case.Events)
            {
                var dataVertex = new Node(ev.Name);
                dataGraph.AddNode(dataVertex);
            }

            foreach (var ev in @case.Events)
            {
                foreach (var next in ev.Next)
                {
                    Edge edge = dataGraph.AddEdge(ev.Name, next.Key.Count.ToString(), next.Key.Name);
                    edge.Attr.Separation = 1;
                }
            }
            return dataGraph;
        }
    }

    public class BPdto
    {
        public long Номер { get; set; }
        public string Система { get; set; }
        public string Название { get; set; }
    }
}
