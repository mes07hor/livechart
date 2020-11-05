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
using System.Data.SqlClient;

namespace livechart
{
    public partial class OrganizerForm : Form
    {

        //contain firstform instance
        private FirstForm firstformInstance;
        private string selectedgroup;

        public OrganizerForm(FirstForm firstformInstance)
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
                Title = "Speaking Time[s/minute]"
            });
            cartesianChart1.AxisY[0].MinValue = 0;
            cartesianChart1.LegendLocation = LiveCharts.LegendLocation.Right;
            cartesianChart1.Series = lineseries;

            

        }
        SeriesCollection lineseries = new SeriesCollection();


        private SeriesCollection GetTableData()
        {
            SeriesCollection columnseries = new SeriesCollection();

            DateTime now = DateTime.Now;
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

            using (WorkshopEntities7 db = new WorkshopEntities7())
            {
                
                if (selectedgroup == "Adults")
                {
                    
                    var data = db.Adults;
                    
                    var users = (from o in data orderby o.username select o.username).Distinct();
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
                else
                {
                    var data = db.Students;
                    var users = (from o in data orderby o.username select o.username).Distinct();
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
                Title = "Total Speaking Time[s]"
                //LabelFormatter = value => value.ToString(),
            });
            cartesianChart2.AxisY[0].MinValue = 0;
            cartesianChart2.LegendLocation = LiveCharts.LegendLocation.Right;

            using (WorkshopEntities7 db = new WorkshopEntities7())
            {
                if (selectedgroup == "Adults")
                {
                    var data = db.Adults;
                    var users = (from o in data orderby o.username select o.username).Distinct();

                    foreach (var user in users)
                    {
                        columnlabels.Add(user.ToString());
                    }
                }
                else if (selectedgroup == "Students")
                {
                    var data = db.Students;
                    var users = (from o in data orderby o.username select o.username).Distinct();

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

            

            
        }

        private void UpdateLineGraph()
        {
            DateTime now = DateTime.Now;

            cartesianChart1.AxisX[0].Labels.Add(now.ToShortTimeString());

            SeriesCollection lineseries2 = new SeriesCollection();

            using (WorkshopEntities7 db = new WorkshopEntities7())
            {
                if (selectedgroup == "Adults")
                {
                    var data = db.Adults;
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
                else if (selectedgroup == "Students")
                {
                    var data = db.Students;
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

        static string SentenceToCompare;

        private void getTalkingLog()
        {
            DateTime now = DateTime.Now;

            string constr = @"Data Source=sfws.c7t5mjpl9wt1.ap-northeast-1.rds.amazonaws.com,1433;
                                Initial Catalog=Workshop;Connect Timeout=20;Persist Security Info=True;User ID=SSD;Password=lcopLCOP";
            // コネクションを生成します。
            using (var connection = new SqlConnection(constr))
                try
                {
                    // コマンドオブジェクトを作成します。
                    using (var command = connection.CreateCommand())
                    {
                        // コネクションをオープンします。
                        connection.Open();

                        //SQLを実行します。
                        command.CommandText = @"SELECT * FROM SentenceTest2 where Date=(select max(Date) from SentenceTest2)";  
                        
                        var reader = command.ExecuteReader();
                        // 取得結果を確認します。
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var name = reader["username"] as string;
                                var sentence = reader["sentence"] as string;

                                if (sentence != SentenceToCompare)
                                {
                                    Label speakername = new Label();
                                    speakername.Text = name;
                                    flowLayoutPanel1.Controls.Add(speakername);

                                    chat.YouBubble youBubble = new chat.YouBubble();
                                    youBubble.Body = sentence;
                                    youBubble.MsgColor = Color.LightGray;
                                    flowLayoutPanel1.Controls.Add(youBubble);
                                    flowLayoutPanel1.ScrollControlIntoView(youBubble);
                                    SentenceToCompare = sentence;
                                }

                            }
                            
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            
                        }
                    }
                }

                // 例外が発生した場合
                catch (Exception e)
                {
                    // 例外の内容を表示します。
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
        }


        private System.Windows.Threading.Dispatcher _dispatcher = null;


        private void timer_everysecond_Tick(object sender, EventArgs e)
        {
            getTalkingLog();
        }

        private void OrganizerForm_Load_1(object sender, EventArgs e)
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            this.MinimumSize = this.Size;
            ShowColumnGraph(GetTableData());

            ShowLineGraph();

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            cartesianChart1.DisableAnimations = true;
            cartesianChart2.DisableAnimations = true;

            UpdateLineGraph();
            // ShowColumnGraph();
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            ShowColumnGraph(GetTableData());

        }
    }
}
