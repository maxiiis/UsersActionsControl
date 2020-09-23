using EFModels;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Edge = Microsoft.Msagl.Drawing.Edge;
using Controller;

namespace UI
{
    public partial class BPs : Window
    {
        private int Stage = 0;
        private long BPId;
        private bool IsStandart;
        private string BPName;
        private BPsControl BPsControl = new BPsControl();

        public BPs()
        {
            InitializeComponent();
            ChangeStage(0);
        }

        public BPs(long BPId, long @case = 0)
        {
            InitializeComponent();
            this.BPId = BPId;
            BPName = BPsControl.GetBPName(BPId);
            ChangeStage(1, BPId);
            BPLabel.Content = $"Модель бизнес-процесса: {BPName}";
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
            else
                Close();
        }

        private void ChangeStage(int stage, long BPId = 0)
        {
            switch (stage)
            {
                case 0:

                    var BPs = BPsControl.GetBP();

                    dataGrid.ItemsSource = BPs;
                    openCases.Visibility = Visibility.Visible;
                    openCasesSeparator.Visibility = Visibility.Visible;
                    statusCount.Text = $"Всего БП: {BPs.Count}";
                    break;
                case 1:
                    BPdto selectedRow = dataGrid.SelectedItem as BPdto;
                    if (selectedRow != null)
                    {
                        long BPid = BPId;
                        if (BPId == 0)
                            BPid = Convert.ToInt32(selectedRow.Номер);

                        var Cases = BPsControl.GetCases();

                        dataGrid.ItemsSource = Cases;
                        openCases.Visibility = Visibility.Collapsed;
                        openCasesSeparator.Visibility = Visibility.Collapsed;
                        statusCount.Text = $"Всего случаев: {Cases.Count}";
                    }
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
                    var selectedRow = dataGrid.SelectedItem as Controller.BPCasedto;
                    var CaseId = selectedRow.НомерСлучая;
                    if (CaseId == -1)
                    {
                        if (IsStandart)
                        {
                            var @case = new CaseBuilder().CreateStandartCase(BPId);

                            graphControl1.Graph = null;
                            graphControl1.Graph = Graph_Setup(@case);
                        }
                        else
                        {
                            newCase = new CaseBuilder().CreateGeneralCase();
                            graphControl1.Graph = null;
                            graphControl1.Graph = Graph_Setup(newCase);
                        }

                    }
                    else
                    {
                        newCase = new CaseBuilder().CreateCase(CaseId);
                        SelectCase(newCase);
                    }
                    statusCurrent.Text = $"Текущий случай: {selectedRow.НомерСлучая}";
                }
                else
                {
                    //build general case
                    var selectedBP = dataGrid.SelectedItem as BPdto;

                    if (selectedBP != null)
                    {

                        BPId = selectedBP.Номер;

                        var BPname = BPsControl.GetBPbyName(selectedBP.Название);
                        BPName = BPname;

                        BPLabel.Content = $"Модель бизнес-процесса: {BPname}";

                        newCase = new CaseBuilder().CreateGeneralCase();

                        graphControl1.Graph = null;
                        graphControl1.Graph = Graph_Setup(newCase);
                        statusCurrent.Text = $"Текущий БП: {selectedBP.Номер}";
                    }
                }
            }
        }
        private Graph Graph_Setup(Case @case)
        {
            Graph dataGraph = new Graph();

            foreach (var ev in @case.Events)
            {
                foreach (var next in ev.Next)
                {
                    Edge edge = dataGraph.AddEdge(ev.Name, next.Key.Count.ToString(), next.Key.Name);
                    edge.Attr.Separation = 1;
                }
            }

            //dataGraph.CreateGeometryGraph();

            //var geomGraph = dataGraph.GeometryGraph;

            //var geomGraphComponents = GraphConnectedComponents.CreateComponents(geomGraph.Nodes, geomGraph.Edges);
            //var settings = new SugiyamaLayoutSettings();
            ////foreach (var subgraph in geomGraphComponents)
            ////{

            ////    var layout = new LayeredLayout(subgraph, settings);
            ////    subgraph.Margins = settings.NodeSeparation / 2;
            ////    layout.Run();

            ////}

            //Microsoft.Msagl.Layout.MDS.MdsGraphLayout.PackGraphs(geomGraphComponents, settings);

            //geomGraph.UpdateBoundingBox();

            dataGraph.Attr.LayerDirection = LayerDirection.TB;
            dataGraph.LayoutAlgorithmSettings = new SugiyamaLayoutSettings();
            dataGraph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = Microsoft.Msagl.Core.Routing.EdgeRoutingMode.SugiyamaSplines;
            return dataGraph;
        }

        private Graph Graph_Setup(StandartCase @case)
        {
            Graph dataGraph = new Graph();

            for (int i = 0; i < @case.Events.Count; i++)
            {
                for (int j = 0; j < @case.Events.Count; j++)
                {
                    if (@case.Events[i].Edges[j].Trans)
                    {
                        Edge edge = dataGraph.AddEdge(@case.Events[i].Name, @case.Events[j].Name);
                        edge.Attr.Separation = 1;
                    }
                }
            }

            dataGraph.Attr.LayerDirection = LayerDirection.TB;
            dataGraph.LayoutAlgorithmSettings = new SugiyamaLayoutSettings();
            dataGraph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = Microsoft.Msagl.Core.Routing.EdgeRoutingMode.SugiyamaSplines;
            return dataGraph;
        }

        private void SelectCase(Case @case)
        {
            ClearSelectedCase();

            for (int i = 0; i < @case.Events.Count - 1; i++)
            {
                var node = graphControl1.Graph.FindNode(@case.Events[i].Name);

                var edge = node.OutEdges.FirstOrDefault(s => s.TargetNode.LabelText == @case.Events[i + 1].Name);

                if (edge == null)
                {
                    edge = graphControl1.Graph.AddEdge(node.LabelText, @case.Events[i + 1].Name);
                    node.AddOutEdge(edge);
                    edge.Attr.Color = Color.Red;
                }

                if (!IsStandart)
                {
                    node.Attr.Color = Color.Red;
                    //node.Attr.Color = new Color(byte.MaxValue, node.Attr.Color.R, node.Attr.Color.G, node.Attr.Color.B);
                    edge.Attr.Color = node.Attr.Color;
                }
                else
                {
                    node.Attr.Color = Color.Black;
                    //node.Attr.Color = new Color(byte.MaxValue, node.Attr.Color.R, node.Attr.Color.G, node.Attr.Color.B);

                    //edge.Attr.Color = node.Attr.Color;
                    edge.LabelText = (i + 1).ToString();
                }
                node.Attr.LineWidth = 3;
                edge.TargetNode.Attr.Color = node.Attr.Color;
                edge.TargetNode.Attr.LineWidth = 3;
                edge.Attr.LineWidth = 1.8;


            }

            if (IsStandart)
            {
                Graph g = graphControl1.Graph;
                graphControl1.Graph = null;
                graphControl1.Graph = g;
            }
        }

        private void ClearSelectedCase()
        {
            if (IsStandart)
            {
                StandartCase newCase;

                newCase = new CaseBuilder().CreateStandartCase(BPId);

                graphControl1.Graph = null;
                graphControl1.Graph = Graph_Setup(newCase);
            }

            foreach (var n in graphControl1.Graph.Nodes)
            {
                n.Attr.Color = Color.Black;
                n.Attr.LineWidth = 1;
                //n.Attr.Color = new Color(byte.MaxValue / 2, n.Attr.Color.R, n.Attr.Color.G, n.Attr.Color.B);
            }
            foreach (var e in graphControl1.Graph.Edges)
            {
                e.Attr.Color = Color.Black;
                e.TargetNode.Attr.LineWidth = 1;
                e.Attr.LineWidth = 1;
                //e.Attr.Color = new Color(byte.MaxValue / 2, e.Attr.Color.R, e.Attr.Color.G, e.Attr.Color.B);
            }
        }

        private void viewStandart_Click(object sender, RoutedEventArgs e)
        {
            if (viewStandart.IsChecked == true)
            {
                IsStandart = true;
                StandartCase newCase;

                newCase = new CaseBuilder().CreateStandartCase(BPId);

                graphControl1.Graph = null;
                graphControl1.Graph = Graph_Setup(newCase);

                BPLabel.Content = $"Эталонная модель бизнес-процесса: {BPName}";
            }
            else
            {
                IsStandart = false;
                Case newCase;
                newCase = new CaseBuilder().CreateGeneralCase();
                graphControl1.Graph = null;
                graphControl1.Graph = Graph_Setup(newCase);
                BPLabel.Content = $"Модель бизнес-процесса: {BPName}";
            }
        }

        private void viewOngeneral_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenSource_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenLogs_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

