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
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace livechart
{

    public partial class Form1 : Form
    {
        //contain firstform instance
        private FirstForm firstformInstance;
        private string selectedgroup;

        public Form1(FirstForm firstformInstance)
        {
            InitializeComponent();

            this.firstformInstance = firstformInstance;

            var ListOfTimeforCulumn1 = new List<List<double>>() { };
            var ListOfTimeforCulumn2 = new List<List<double>>() { };
            selectedgroup = this.firstformInstance.selectedTable;
            Console.WriteLine(selectedgroup);

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
            ColumnSeries column = new ColumnSeries()
            {
                Title = now.AddMinutes(-chartRangeMinute).ToShortTimeString() + "～\n" + now.AddMinutes(-chartRangeMinute / 2).ToShortTimeString(),

                DataLabels = false,
                Values = new ChartValues<double>(),
                //LabelPoint = point => point.Y.ToString()

            };
            ColumnSeries column2 = new ColumnSeries()
            {
                Title = now.AddMinutes(-chartRangeMinute / 2).ToShortTimeString() + "～\n" + now.ToShortTimeString(),

                DataLabels = false,
                Values = new ChartValues<double>(),
                //LabelPoint = point => point.Y.ToString()

            };

            using (WorkshopEntities7 db = new WorkshopEntities7())
            {
                if (selectedgroup == "Adults1"||selectedgroup=="Adults2")
                {
                    var data = db.Adults;
                    var users = (from o in data
                                 where o.selectedgroup == selectedgroup
                                 && SqlFunctions.DatePart("day", o.Date) == SqlFunctions.DatePart("day", now)
                                 && o.username != "ファシリテータ" && o.username != "ファシリテーター"
                                 orderby o.username select o.username).Distinct();
                    //SeriesCollection lineseries = new SeriesCollection();

                    foreach (var user in users)
                    {
                        LineSeries line = new LineSeries()
                        {
                            Title = user.ToString(),
                            DataLabels = false,
                            Values = new ChartValues<double>(),
                            //Values=userdatas.AsChartValues()
                        };
                        //double totalseconds1 = 5;
                        //double totalseconds2 = 5;
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
                        column.Values.Add(timesForCulumn1.Sum() + 5);
                        column2.Values.Add(timesForCulumn2.Sum() + 5);
                    }
                    firstflag = false;
                    //cartesianChart1.Series = series;
                    columnseries.Add(column);
                    columnseries.Add(column2);
                }
                else if(selectedgroup=="Students1"||selectedgroup=="Students2")
                {
                    var data = db.Students;
                    var users = (from o in data
                                 where o.selectedgroup == selectedgroup
                                 && SqlFunctions.DatePart("day", o.Date) == SqlFunctions.DatePart("day", now)
                                 && o.username != "ファシリテータ" && o.username != "ファシリテーター"
                                 orderby o.username select o.username).Distinct();
                    //SeriesCollection lineseries = new SeriesCollection();

                    foreach (var user in users)
                    {
                        LineSeries line = new LineSeries()
                        {
                            Title = user.ToString(),
                            DataLabels = false,
                            Values = new ChartValues<double>(),
                            //Values=userdatas.AsChartValues()
                        };
                        //double totalseconds1 = 5;
                        //double totalseconds2 = 5;
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
                        column.Values.Add(timesForCulumn1.Sum() + 5);
                        column2.Values.Add(timesForCulumn2.Sum() + 5);
                    }
                    firstflag = false;
                    //cartesianChart1.Series = series;
                    columnseries.Add(column);
                    columnseries.Add(column2);
                }
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
                if (selectedgroup == "Adults1"||selectedgroup=="Adults2")
                {
                    var data = db.Adults;
                    var users = (from o in data
                                 where o.selectedgroup == selectedgroup
                                 && SqlFunctions.DatePart("day", o.Date) == SqlFunctions.DatePart("day", now)
                                 && o.username != "ファシリテータ" && o.username != "ファシリテーター"
                                 orderby o.username select o.username).Distinct();

                    foreach (var user in users)
                    {
                        columnlabels.Add(user.ToString());
                    }
                }else if (selectedgroup == "Students1"||selectedgroup=="Students2")
                {
                    var data = db.Students;
                    var users = (from o in data
                                 where o.selectedgroup == selectedgroup
                                 && SqlFunctions.DatePart("day", o.Date) == SqlFunctions.DatePart("day", now)
                                 && o.username != "ファシリテータ" && o.username != "ファシリテーター"
                                 orderby o.username select o.username).Distinct();

                    foreach (var user in users)
                    {
                        columnlabels.Add(user.ToString());
                    }
                }
                else
                {
                    var data = db.voicerecords;
                    var users = (from o in data orderby o.username select o.username).Distinct();

                    foreach (var user in users)
                    {
                        columnlabels.Add(user.ToString());
                    }
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
                if (selectedgroup == "Adults1"||selectedgroup=="Adults2")
                {
                    var data = db.Adults;
                    var users = (from o in data
                                 where o.selectedgroup == selectedgroup
                                 && SqlFunctions.DatePart("day", o.Date) == SqlFunctions.DatePart("day", now)
                                 && o.username != "ファシリテータ" && o.username != "ファシリテーター"
                                 orderby o.username select o.username).Distinct();  //fuking retard code
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
                                            == (60 * SqlFunctions.DatePart("hour", now)) + (SqlFunctions.DatePart("minute", now))
                                        && oo.time < 60
                                        select new { oo.time, oo.Date };

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

                }
                else if (selectedgroup == "Students1"||selectedgroup=="Students2")
                {
                    var data = db.Students;
                    var users = (from o in data
                                 where o.selectedgroup == selectedgroup
                                 && SqlFunctions.DatePart("day", o.Date) == SqlFunctions.DatePart("day", now)
                                 && o.username != "ファシリテータ" && o.username != "ファシリテーター"
                                 orderby o.username select o.username).Distinct();  //fuking retard code
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
                                            == (60 * SqlFunctions.DatePart("hour", now)) + (SqlFunctions.DatePart("minute", now))
                                        && oo.time < 60
                                        select new { oo.time, oo.Date };

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

                }
                else
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
                                            == (60 * SqlFunctions.DatePart("hour", now)) + (SqlFunctions.DatePart("minute", now))
                                        && oo.time < 60
                                        select new { oo.time, oo.Date };

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
            ShowColumnGraphforSticker(getDatafromMiro());

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
            ShowColumnGraphforSticker(getDatafromMiro());
            UpdateLineGraph();
            
           // ShowColumnGraph();
        }

        private void timer2_Tick(object sender, EventArgs e)    //every
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

        private SeriesCollection getDatafromMiro()
        {
            var now = DateTime.Now;
            SeriesCollection stickerColumnseries = new SeriesCollection();

            cartesianChart3.DisableAnimations = true;


            ColumnSeries stickercolumn2 = new ColumnSeries()
            {
                Title = now.AddMinutes(-chartRangeMinute).ToShortTimeString() + "～\n" + now.AddMinutes(-chartRangeMinute / 2).ToShortTimeString(),

                DataLabels = false,
                Values = new ChartValues<int>(),
                //LabelPoint = point => point.Y.ToString()

            };
            ColumnSeries stickercolumn = new ColumnSeries()
            {
                Title = now.AddMinutes(-chartRangeMinute / 2).ToShortTimeString() + "～\n" + now.ToShortTimeString(),

                DataLabels = false,
                Values = new ChartValues<int>(),
                //LabelPoint = point => point.Y.ToString()

            };

            Task getDatafromMiroTask = Task.Run(() =>
            {
                string boardID = "o9J_kiyojvo=";
                if (selectedgroup == "Adults1")
                {
                    boardID = "o9J_lcv3i5Y=";   //I have to make this part editable without coding by user
                }
                else if (selectedgroup == "Students1")
                {
                    boardID = "o9J_lcvhh7c=";
                }

                string url = "https://api.miro.com/v1/boards/" + boardID + "/widgets/?access_token=a76c74ac-256e-455d-ba1b-2d06b801f830";


                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                Stream s = res.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                string content = sr.ReadToEnd();
                sr.Close();

                var mirodata = JsonConvert.DeserializeObject<Root>(content);

                int whiteWidgetCount = 0;
                int yellowWidgetCount = 0;
                int redWidgetCount = 0;
                int greenWidgetCount = 0;
                int malibuWidgetCount = 0;

                int whiteWidgetCount2 = 0;
                int yellowWidgetCount2 = 0;
                int redWidgetCount2 = 0;
                int greenWidgetCount2 = 0;
                int malibuWidgetCount2 = 0;

                foreach (var item in mirodata.data)
                {
                    
                    if (item.type == "sticker" && item.modifiedAt.ToLocalTime() > now.AddMinutes(-30))
                    {
                        switch (item.style.backgroundColor)
                        {
                            case "#f5f6f8":
                                whiteWidgetCount++;
                                break;
                            case "#fff9b1":
                                yellowWidgetCount++;
                                break;
                            case "#f16c7f":
                                redWidgetCount++;
                                break;
                            case "#d5f692":
                                greenWidgetCount++;
                                break;
                            case "#6cd8fa":
                                malibuWidgetCount++;
                                break;
                        }
                    }
                    else if (item.type == "sticker" && item.modifiedAt.ToLocalTime() > now.AddMinutes(-60) && item.modifiedAt.ToLocalTime() < now.AddMinutes(-30))
                    {
                        switch (item.style.backgroundColor)
                        {
                            case "#f5f6f8":
                                whiteWidgetCount2++;
                                break;
                            case "#fff9b1":
                                yellowWidgetCount2++;
                                break;
                            case "#f16c7f":
                                redWidgetCount2++;
                                break;
                            case "#d5f692":
                                greenWidgetCount2++;
                                break;
                            case "#6cd8fa":
                                malibuWidgetCount2++;
                                break;
                        }
                    }


                }
                stickercolumn.Values.Add(whiteWidgetCount);
                stickercolumn.Values.Add(yellowWidgetCount);
                stickercolumn.Values.Add(redWidgetCount);
                stickercolumn.Values.Add(greenWidgetCount);
                stickercolumn.Values.Add(malibuWidgetCount);

                stickercolumn2.Values.Add(whiteWidgetCount2);
                stickercolumn2.Values.Add(yellowWidgetCount2);
                stickercolumn2.Values.Add(redWidgetCount2);
                stickercolumn2.Values.Add(greenWidgetCount2);
                stickercolumn2.Values.Add(malibuWidgetCount2);


                stickerColumnseries.Add(stickercolumn2);
                stickerColumnseries.Add(stickercolumn);
            });

            
            //metroGrid1.AutoResizeColumns();
            //metroGrid1.AutoResizeRows();            
            //metroGrid1.Update();

            return stickerColumnseries;

        }

        private void ShowColumnGraphforSticker(SeriesCollection columnseries)
        {
            cartesianChart3.AxisX.Clear();
            cartesianChart3.AxisY.Clear();
            DateTime now = DateTime.Now;
            var columnlabels = new List<string>();

            cartesianChart3.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "",
                Labels = columnlabels,
                Separator = new Separator // force the separator step to 1, so it always display all labels
                {
                    Step = 1,
                    IsEnabled = false //disable it to make it invisible.
                }

            });

            cartesianChart3.AxisY.Add(new Axis
            {
                Title = "Total Stickers[s]"
                //LabelFormatter = value => value.ToString(),
            });
            cartesianChart3.AxisY[0].MinValue = 0;
            cartesianChart3.LegendLocation = LiveCharts.LegendLocation.Right;

            string whiteguy = metroTextBox1.Text.ToString();
            string yellowguy = metroTextBox2.Text.ToString();
            string redguy = metroTextBox3.Text.ToString();
            string greenguy = metroTextBox4.Text.ToString();
            string malibuguy = metroTextBox5.Text.ToString();
            columnlabels.Add(whiteguy);
            columnlabels.Add(yellowguy);
            columnlabels.Add(redguy);
            columnlabels.Add(greenguy);
            columnlabels.Add(malibuguy);

            cartesianChart3.Series = columnseries;

        }



        // following classes define the object data style to deal JSON sent from miro API in C#
        public class Assignee
        {
            public object userId { get; set; }
        }

        public class Style
        {
            public string backgroundColor { get; set; }
            public int? fontSize { get; set; }
            public string fontFamily { get; set; }
            public string textAlign { get; set; }
            public object padding { get; set; }
            public object backgroundOpacity { get; set; }
            public object borderColor { get; set; }
            public object borderStyle { get; set; }
            public object borderOpacity { get; set; }
            public object borderWidth { get; set; }
            public object textColor { get; set; }
            public string textAlignVertical { get; set; }
            public object shapeType { get; set; }
        }

        public class CreatedBy
        {
            public string type { get; set; }
            public string name { get; set; }
            public string id { get; set; }
        }

        public class ModifiedBy
        {
            public string type { get; set; }
            public string name { get; set; }
            public string id { get; set; }
        }

        public class Datum
        {
            public string id { get; set; }
            public string type { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public object date { get; set; }
            public object card { get; set; }
            public double x { get; set; }
            //public int rotation { get; set; }
            public Assignee assignee { get; set; }
            public double y { get; set; }
            public double scale { get; set; }
            public Style style { get; set; }
            public DateTime createdAt { get; set; }
            public CreatedBy createdBy { get; set; }
            public DateTime modifiedAt { get; set; }
            public ModifiedBy modifiedBy { get; set; }
            public List<string> children { get; set; }
            public double? width { get; set; }
            public double? height { get; set; }
            public string frameType { get; set; }
            public string frameFormat { get; set; }
            public string text { get; set; }
        }

        public class Root
        {
            public List<Datum> data { get; set; }
            public int size { get; set; }
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            ShowColumnGraphforSticker(getDatafromMiro());

        }
    }

}
