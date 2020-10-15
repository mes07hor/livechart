using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CartesianChart = LiveCharts.WinForms.CartesianChart;
using LiveCharts.Configurations;
using Brushes = System.Windows.Media.Brushes;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using LiveCharts.Helpers;
using System.Threading;
using System.Windows.Threading;

namespace livechart
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            var ListOfTimeforCulumn1 = new List<List<double>>() { };
            var ListOfTimeforCulumn2 = new List<List<double>>() { };


        }

        private static int chartRangeMinute = 60;
        bool firstflag = true;

        private void ShowLineGraph()
        {
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisY.Clear();
            DateTime now = DateTime.Now;
            var labels = new List<string>();
            for (DateTime itime = now.AddMinutes(-chartRangeMinute); itime.TimeOfDay <= now.TimeOfDay; itime = itime.AddMinutes(1))
            {
                labels.Add(itime.ToShortTimeString());
            }
            cartesianChart1.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Time",
                Labels = labels
            });

            cartesianChart1.Zoom = ZoomingOptions.X;
            

            cartesianChart1.AxisY.Add(new Axis
            {
                Title="Speaking Time[s/minute]"
            });
            cartesianChart1.AxisY[0].MinValue = 0;  
            cartesianChart1.LegendLocation = LiveCharts.LegendLocation.Right;
            cartesianChart1.Series = lineseries;

            
            using(WorkshopEntities7 db =new WorkshopEntities7())
            {/*
                var data = db.voicerecords;

                var users = (from o in data orderby o.username select o.username).Distinct();

                SeriesCollection series = new SeriesCollection();

                foreach (var user in users)
                {
                  
                    LineSeries line = new LineSeries()
                    {
                        Title = user.ToString(),
                        DataLabels = false,
                        Values = new ChartValues<double>(),
                        //Values=userdatas.AsChartValues()
                    };

                    //line.Values.Add(userdatas);
                    //line.Values = data.Where(a => a.username == user).Where(a=>a.Date>itime).OrderBy(a => a.Date).Select(a=>a.time);


                    
                    for (DateTime itime = now.AddMinutes(-chartRangeMinute); itime < now; itime = itime.AddMinutes(1))
                    {
                        double vv = 0;
                        //var ruleDate = Convert.ToDateTime(itime).TimeOfDay;

                        
                        var userdatas = from oo in data
                                        where SqlFunctions.DatePart("minute", oo.Date)== SqlFunctions.DatePart("minute", itime)
                                        && oo.username.Equals(user)
                                        && SqlFunctions.DatePart("hour", oo.Date) == SqlFunctions.DatePart("hour", itime)
                                        && oo.time<60
                                        //orderby oo.Date ascending
                                        select  oo.time ;
                                        
                        Console.WriteLine(itime.ToShortTimeString());

                        if (userdatas.FirstOrDefault() != null)
                        {
                            vv = (double)userdatas.FirstOrDefault();
                        }
                        line.Values.Add(vv);
                    }
                    
                    series.Add(line);
                }
                
                cartesianChart1.Series = series;*/
            }

        }
        SeriesCollection lineseries = new SeriesCollection();

        

        private SeriesCollection GetTableData()
        {
            SeriesCollection columnseries = new SeriesCollection();

            DateTime now = DateTime.Now;
            using (WorkshopEntities7 db = new WorkshopEntities7())
            {
                var data = db.voicerecords;

                var users = (from o in data orderby o.username select o.username).Distinct();

                //SeriesCollection lineseries = new SeriesCollection();

                ColumnSeries column = new ColumnSeries()
                {
                    Title = now.AddMinutes(-chartRangeMinute).ToShortTimeString() + "～" + now.AddMinutes(-chartRangeMinute / 2).ToShortTimeString(),

                    DataLabels = false,
                    Values = new ChartValues<double>(),
                    //LabelPoint = point => point.Y.ToString()

                };
                ColumnSeries column2 = new ColumnSeries()
                {
                    Title = now.AddMinutes(-chartRangeMinute / 2).ToShortTimeString() + "～" + now.ToShortTimeString(),

                    DataLabels = false,
                    Values = new ChartValues<double>(),
                    //LabelPoint = point => point.Y.ToString()

                };
                

                foreach (var user in users)
                {
                    LineSeries line = new LineSeries()
                    {
                        Title = user.ToString(),
                        DataLabels = false,
                        Values = new ChartValues<double>(),
                        //Values=userdatas.AsChartValues()
                    };


                    double totalseconds1 = 5;
                    double totalseconds2 = 5;
                    

                    var timesForCulumn1 = new List<double>();
                    var timesForCulumn2 = new List<double>();


                    for (DateTime itime = now.AddMinutes(-chartRangeMinute); itime < now; itime = itime.AddMinutes(1))
                    {
                        double vv = 0;
                        //var ruleDate = Convert.ToDateTime(itime).TimeOfDay;


                        var userdatas = from oo in data
                                        where SqlFunctions.DatePart("minute", oo.Date) == SqlFunctions.DatePart("minute", itime)
                                        && oo.username.Equals(user)
                                        && SqlFunctions.DatePart("hour", oo.Date) == SqlFunctions.DatePart("hour", itime)
                                        && oo.time < 60
                                        select oo.time;

                        Console.WriteLine(itime.ToShortTimeString());


                        if (userdatas.FirstOrDefault() != null)
                        {
                            vv = (double)userdatas.FirstOrDefault();

                        }
                        line.Values.Add(vv);

                        if (itime.TimeOfDay < now.AddMinutes(-chartRangeMinute / 2).TimeOfDay)
                        {
                            //totalseconds1 = totalseconds1 + vv;
                            timesForCulumn1.Add(vv);

                        }
                        else
                        {
                            //totalseconds2 = totalseconds2 + vv;
                            timesForCulumn2.Add(vv);

                        }
                    }
                    if (firstflag)
                    {
                        lineseries.Add(line);
                        
                    }
                    //column.Values.Add(totalseconds1);
                    //column2.Values.Add(totalseconds2);
                    column.Values.Add(timesForCulumn1.Sum()+5);
                    column2.Values.Add(timesForCulumn2.Sum()+5);
                    

                }
                firstflag = false;
                //cartesianChart1.Series = series;
                columnseries.Add(column);
                columnseries.Add(column2);

            }
            return columnseries;
        }
        
        

        private void ShowColumnGraph(SeriesCollection columnseries)
        {
            cartesianChart2.AxisX.Clear();
            cartesianChart2.AxisY.Clear();
            DateTime now = DateTime.Now;
            var columnlabels = new List<string>();
            
            cartesianChart2.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "",
                Labels = columnlabels
            });
           
            cartesianChart2.AxisY.Add(new Axis
            {
                Title="Total Speaking Time[s]"
                //LabelFormatter = value => value.ToString(),
            });
            cartesianChart2.AxisY[0].MinValue = 0;
            cartesianChart2.LegendLocation = LiveCharts.LegendLocation.Right;

            using (WorkshopEntities7 db = new WorkshopEntities7())
            {
                var data = db.voicerecords;
                var users = (from o in data orderby o.username select o.username).Distinct();

                foreach (var user in users)
                {
                    columnlabels.Add(user.ToString());
                }
            }

            cartesianChart2.Series = columnseries;


            //using (TestEntities1 db = new TestEntities1())

            

            using (WorkshopEntities7 db =new WorkshopEntities7())
            {/*
                var data = db.voicerecords;
                var users = (from o in data orderby o.username select o.username).Distinct();
                

                ColumnSeries column = new ColumnSeries()
                {
                    Title = now.AddMinutes(-chartRangeMinute).ToShortTimeString() + "～" + now.AddMinutes(-chartRangeMinute / 2).ToShortTimeString(),

                    DataLabels = false,
                    Values = new ChartValues<double>(),                 

                };
                ColumnSeries column2 = new ColumnSeries()
                {
                    Title = now.AddMinutes(-chartRangeMinute / 2).ToShortTimeString() + "～" + now.ToShortTimeString(),

                    DataLabels = false,
                    Values = new ChartValues<double>(),

                };


                foreach (var user in users)
                {
                    
                    columnlabels.Add(user.ToString());
                    
                    double totalseconds1 = 5;
                    double totalseconds2 = 5;

                    for (DateTime itime = now.AddMinutes(-chartRangeMinute); itime < now; itime = itime.AddMinutes(1))
                    {
                        double v1 = 0;
                        var ruleDate = Convert.ToDateTime(itime).TimeOfDay;

                        var userdatas = from oo in data
                                        where SqlFunctions.DatePart("minute", oo.Date) == SqlFunctions.DatePart("minute", itime)
                                        && oo.username.Equals(user)
                                        && SqlFunctions.DatePart("hour", oo.Date) == SqlFunctions.DatePart("hour", itime)
                                        && oo.time < 60
                                        select oo.time;

                        if (userdatas.FirstOrDefault() != null) //changed from SingleOrDefault
                        {
                            v1 = (double)userdatas.FirstOrDefault();

                        }

                        if (itime.TimeOfDay < now.AddMinutes(-chartRangeMinute/2).TimeOfDay)
                        {                           
                            totalseconds1 = totalseconds1 + v1;
                        }
                        else
                        {
                            totalseconds2 = totalseconds2 + v1;
                        }
                        Console.WriteLine(itime);
                        
                    }

                    column.Values.Add(totalseconds1);
                    column2.Values.Add(totalseconds2);

                }
                columnseries.Add(column);
                columnseries.Add(column2);
                
                */
            }
            
        }

        private void UpdateLineGraph()
        {
            
            DateTime now = DateTime.Now;
           
            cartesianChart1.AxisX[0].Labels.Add(now.ToShortTimeString());
            
            SeriesCollection lineseries2 = new SeriesCollection(); 

            using (WorkshopEntities7 db=new WorkshopEntities7())
            {

                var data = db.voicerecords;
                var users = (from o in data orderby o.username select o.username).Distinct();  //fuking retard code
                int lineCounter = 0;

                if (users.Count() != cartesianChart1.Series.Count()) //in case new one's data added after running ShowLingraph
                {                                                   //conflict between number of users and number of Series occur
                    lineseries2.Clear();
                    GetTableData();
                    ShowLineGraph();
                    return;
                      
                }
                
                foreach (var user in users)
                {
                    var iValues = cartesianChart1.Series[lineCounter].Values;
                    double latestvalue = 0;

                    var userdatas = from oo in data
                                    where oo.username.Equals(user) &&
                                    (60 * SqlFunctions.DatePart("hour", oo.Date)) + (SqlFunctions.DatePart("minute", oo.Date))
                                        == (60 * SqlFunctions.DatePart("hour", now)) + (SqlFunctions.DatePart("minute",now))
                                    && oo.time < 60
                                    select  new { oo.time,oo.Date };

                    if (userdatas.FirstOrDefault() != null)
                    {
                        latestvalue = (double)userdatas.FirstOrDefault().time;
                    }
                    iValues.Add(latestvalue);
                    //cartesianChart1.Series[lineCounter].Values.Add(latestvalue);
                    lineCounter++;
                    
                    

                    LineSeries line = new LineSeries()
                    {
                        Title = user.ToString(),
                        DataLabels = false,
                        Values = iValues,
                       
                    };
                    lineseries2.Add(line);

                }

                cartesianChart1.Series = lineseries2;
            }
            cartesianChart1.AxisX[0].MaxValue = double.NaN;

        }


        private System.Windows.Threading.Dispatcher _dispatcher = null;


        private void Form1_Load(object sender, EventArgs e)
        {

            //this.pictureBox1.Visible = true;

            this.backgroundWorker1.RunWorkerAsync();

            _dispatcher = Dispatcher.CurrentDispatcher;
            ShowColumnGraph(GetTableData());

            ShowLineGraph();

        }
        
        private void Form1_Shown(object sender, EventArgs e)
        {
            //Task<SeriesCollection> task = Task.Run<SeriesCollection>(new Func<SeriesCollection>(GetTableData));
            //var clumndata = await task;
            //ShowColumnGraph(clumndata);

        }


        private void timer1_Tick(object sender, EventArgs e)    //every 1 minute
        {
            cartesianChart1.DisableAnimations = true;
            cartesianChart2.DisableAnimations = true;

            UpdateLineGraph();
           // ShowColumnGraph();
        }

        private async void timer2_Tick(object sender, EventArgs e)    //every
        {
            //columnseries.Clear();
            //cartesianChart2.Series.Clear();
            //lineseries.Clear();

            
            //GetTableData();
            ShowColumnGraph(GetTableData());
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }
    }

}
