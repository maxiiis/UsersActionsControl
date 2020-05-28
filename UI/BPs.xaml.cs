using EFModels;
using EFModels.LogsDB;
using EFModels.MainDB;
using GraphX.Common.Enums;
using GraphX.Controls;
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
using UI.Models;

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
                    var BPname = mainDB.BPs.FirstOrDefault(s=>s.Name==selectedBP.Название).Name;

                    newCase = new CaseBuilder().CreateGeneralCase();

                    GraphArea_Setup(newCase);
                    Area.GenerateGraph();
                    Area.SetVerticesDrag(true);
                    Area.SetEdgesDashStyle(EdgeDashStyle.Solid);

                    Area.ShowAllEdgesLabels(true);

                    zoomctrl.ZoomToFill();

                }
            }
        }
        private GraphExample Graph_Setup(Case @case)
        {
            var dataGraph = new GraphExample();

            foreach (var ev in @case.Events)
            {
                var dataVertex = new DataVertex(ev.Name);

                dataGraph.AddVertex(dataVertex);
            }

            var vlist = dataGraph.Vertices.ToList();
            foreach (var ev in @case.Events)
            {
                foreach (var next in ev.Next)
                {
                    var dataEdge = new DataEdge(vlist.FirstOrDefault(s=>s.Text==ev.Name), vlist.FirstOrDefault(s=>s.Text==next.Key.Name), next.Key.Count) { Text = next.Key.Count.ToString() };
                    dataGraph.AddEdge(dataEdge);
                }
            }

            return dataGraph;
        }

        private void GraphArea_Setup(Case @case)
        {
            var logicCore = new GXLogicCoreExample() { Graph = Graph_Setup(@case) };
            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.EfficientSugiyama;
            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.EfficientSugiyama);
            

            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 100;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 100;


            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.Bundling;

            logicCore.AsyncAlgorithmCompute = true;

            Area.LogicCore = logicCore;
        }

    }

    public class BPdto
    {
        public long Номер { get; set; }
        public string Система { get; set; }
        public string Название { get; set; }
    }
}
