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
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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
            var ListOfSticker = new List<List<int>>() { };
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
                            
                if (selectedgroup == "Adults1"||selectedgroup == "Adults2")
                {
                    
                    
                    var data = db.Adults;
                    
                    var users = (from o in data
                                 where o.selectedgroup==selectedgroup 
                                 && SqlFunctions.DatePart("day", o.Date) == SqlFunctions.DatePart("day", now)
                                 && o.username != "ファシリテータ" && o.username != "ファシリテーター"&&o.username!="test3"
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
                        /*
                        var userdatas = from oo in data
                                        where oo.Date < now
                                        && oo.username.Equals(user)
                                        && oo.selectedgroup == selectedgroup
                                        && SqlFunctions.DatePart("day", oo.Date) == SqlFunctions.DatePart("day", now)
                                        orderby oo.Date
                                        select new { oo.time ,oo.Date};
                        */                       

                        for (DateTime itime = now.AddMinutes(-chartRangeMinute); itime < now; itime = itime.AddMinutes(1))
                        {
                            double vv = 0;
                            //var ruleDate = Convert.ToDateTime(itime).TimeOfDay;

                            
                            var userdatas = from oo in data
                                            where SqlFunctions.DatePart("minute", oo.Date) == SqlFunctions.DatePart("minute", itime)
                                            && oo.username.Equals(user)
                                            && SqlFunctions.DatePart("hour", oo.Date) == SqlFunctions.DatePart("hour", itime)
                                            && oo.selectedgroup == selectedgroup
                                            && SqlFunctions.DatePart("day", oo.Date) == SqlFunctions.DatePart("day", now)
                                            && oo.username != "ファシリテータ" && oo.username != "ファシリテーター" 

                                            //&& oo.time < 60
                                            select oo.time;
                                            
                            //bool datafoudflag = false;
                            /*
                            foreach (var a in userdatas)
                            {
                                if (a.Date.Minute == itime.Minute && a.Date.Hour == itime.Hour)
                                {
                                    line.Values.Add(a.time);
                                    datafoudflag = true;
                                }
                            }
                            if (datafoudflag == false)
                            {
                                line.Values.Add(vv);
                            }*/

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
                else if(selectedgroup == "Students1" || selectedgroup == "Students2")
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
                                            && oo.selectedgroup == selectedgroup
                                            //&& SqlFunctions.DatePart("day", oo.Date) == SqlFunctions.DatePart("day", now)
                                            //&& oo.time < 60
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
            firstLoadflag = false;
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
                Labels = columnlabels,
                Separator = new Separator // force the separator step to 1, so it always display all labels
                {
                    Step = 1,
                    IsEnabled = false //disable it to make it invisible.
                }
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
                if (selectedgroup == "Adults1"|| selectedgroup == "Adults2")
                {
                    var data = db.Adults;
                    var users = (from o in data
                                 where o.selectedgroup == selectedgroup
                                 && SqlFunctions.DatePart("day", o.Date) == SqlFunctions.DatePart("day", now)
                                 && o.username!="ファシリテータ"&&o.username!="ファシリテーター" 
                                 orderby o.username select o.username).Distinct();

                    foreach (var user in users)
                    {
                        columnlabels.Add(user.ToString());
                    }
                }
                else if (selectedgroup == "Students1" || selectedgroup == "Students2")
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
            string greenguy = metroTextBox4.Text.ToString() ;
            string malibuguy = metroTextBox5.Text.ToString();
            columnlabels.Add(whiteguy);
            columnlabels.Add(yellowguy);
            columnlabels.Add(redguy);
            columnlabels.Add(greenguy);
            columnlabels.Add(malibuguy);

            cartesianChart3.Series = columnseries;

        }

        private void UpdateLineGraph()
        {
            DateTime now = DateTime.Now;

            cartesianChart1.AxisX[0].Labels.Add(now.ToShortTimeString());

            SeriesCollection lineseries2 = new SeriesCollection();
            
            using (WorkshopEntities7 db = new WorkshopEntities7())
                {
                    if (selectedgroup == "Adults1"|| selectedgroup == "Adults2")
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
                                            && oo.selectedgroup == selectedgroup
                                            && SqlFunctions.DatePart("day", oo.Date) == SqlFunctions.DatePart("day", now)
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
                    else if (selectedgroup == "Students1" || selectedgroup == "Students2")
                    {
                        var data = db.Students;
                        var users = (from o in data
                                     where o.selectedgroup == selectedgroup
                                     && SqlFunctions.DatePart("day", o.Date) == SqlFunctions.DatePart("day", now)
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
                                            && oo.selectedgroup == selectedgroup
                                            && SqlFunctions.DatePart("day", oo.Date) == SqlFunctions.DatePart("day", now)
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
                                var selectedgrouptocompare = reader["selectedgroup"] as string;


                                if (sentence != SentenceToCompare&&selectedgroup==selectedgrouptocompare)
                                {
                                    Label speakername = new Label();
                                    speakername.Text = name;

                                    if (speakername.Text == "ファシリテータ" || speakername.Text == "ファシリテーター")
                                    {
                                        var meBubble = new chat.MeBubble();
                                        meBubble.Body = sentence;
                                        meBubble.AutoSize =false;
                                        meBubble.Width = (int)(flowLayoutPanel1.Width);
                                        meBubble.MsgColor = Color.Blue;
                                        flowLayoutPanel1.Controls.Add(meBubble);
                                        flowLayoutPanel1.ScrollControlIntoView(meBubble);
                                        SentenceToCompare = sentence;
                                    } else {
                                        flowLayoutPanel1.Controls.Add(speakername);

                                        chat.YouBubble youBubble = new chat.YouBubble();
                                        youBubble.Body = sentence;
                                        //youBubble.AutoSize = true;
                                        youBubble.Width = (int)(flowLayoutPanel1.Width);
                                        youBubble.MsgColor = Color.LightGray;
                                        flowLayoutPanel1.Controls.Add(youBubble);
                                        flowLayoutPanel1.ScrollControlIntoView(youBubble);
                                        SentenceToCompare = sentence;
                                    }
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
            ShowColumnGraph(GetTableData());

            ShowLineGraph();
            this.MinimumSize = this.Size;


            showDataGrid(dataTable);

            ShowColumnGraphforSticker(getDatafromMiro(dataTable));
           

        }
        DataTable dataTable = new DataTable();

        bool firstLoadflag = true;
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            var now = DateTime.Now;

            cartesianChart1.DisableAnimations = true;
            cartesianChart2.DisableAnimations = true;


            if (now.Second == 0&&firstLoadflag==false)
            {
                UpdateLineGraph();
                ShowColumnGraphforSticker(getDatafromMiro(dataTable));

                if (now.Minute % 2 == 0)
                {
                    ShowColumnGraph(GetTableData());

                }

            }

            // ShowColumnGraph();


        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            
           //ShowColumnGraph(GetTableData());
            
        }

        private void showDataGrid(DataTable dataTable)
        {
            dataTable.Columns.Add("column0");
            dataTable.Columns.Add("column1");
            dataTable.Columns.Add("column2");
            dataTable.Columns.Add("column3");
            

            var rowCount = 9;
            for (int i=0;i<rowCount;i++) {
                var row = dataTable.NewRow();
                dataTable.Rows.Add(row);
            }
            this.metroGrid1.DataSource = dataTable;

            for(int j = 0; j < rowCount; j++)
            {
                metroGrid1.Rows[j].Height =50;
            }
            metroGrid1.Columns[0].Width = 50;
            metroGrid1.Columns[1].Width = 50;
            metroGrid1.Columns[2].Width = 50;
            metroGrid1.Columns[3].Width = 50;

        }

        private  SeriesCollection getDatafromMiro(DataTable dataTable)
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
                }else if (selectedgroup == "Students1")
                {
                    boardID = "o9J_lcvhh7c=";
                }

                string url = "https://api.miro.com/v1/boards/"+boardID+"/widgets/?access_token=a76c74ac-256e-455d-ba1b-2d06b801f830";


                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                Stream s = res.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                string content = sr.ReadToEnd();
                sr.Close();

                var mirodata = JsonConvert.DeserializeObject<Root>(content);

                int whiteWidgetCount = 0;
                int yellowWidgetCount =0;
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
                    if (item.type == "frame") {
                        int widgetCount = 0;

                        if (item.title.Length>=5)
                        {
                            char c1 = item.title[0];
                            char c2 = item.title[2];
                            char c3 = item.title[4];
                            if (c1 == '[' && c2==',' && c3==']')
                            {
                                Regex re = new Regex(@"[^0-9]");    //extract numbers from string like [1,2], that result in 12
                                int tmp = int.Parse(re.Replace(item.title, ""));
                                int rowDigit = tmp / 10;    //2nd digit
                                int columnDigit = tmp % 10; //1st digit
                            

                                foreach (var id in item.children)   //item.children.count sometimes contain unexpected widgets like "line"
                                {
                                    foreach(var item2 in mirodata.data)
                                    {
                                        if (item2.id==id && item2.type == "sticker")
                                        {
                                            widgetCount++;
                                        }
                                    }
                                }
                                dataTable.Rows[rowDigit][columnDigit] = widgetCount;
                            }

                        }
                    }else if (item.type == "sticker"&&item.modifiedAt.ToLocalTime()>now.AddMinutes(-30))
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
                    else if (item.type == "sticker" && item.modifiedAt.ToLocalTime() > now.AddMinutes(-60)&&item.modifiedAt.ToLocalTime()<now.AddMinutes(-30))
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

            
            metroGrid1.DataSource= dataTable;
            //metroGrid1.DefaultCellStyle.Font = new Font("Tahoma", 15);
            metroGrid1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            metroGrid1.GridColor = Color.White;
            metroGrid1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //metroGrid1.AutoResizeColumns();
            //metroGrid1.AutoResizeRows();            
            //metroGrid1.Update();

            return stickerColumnseries;
         
        }

        

        private void metroGrid1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.Value != null)
            {
                int result = 0;
                string cellValueString = e.Value.ToString();
                var ret = int.TryParse(cellValueString, out result);
                int cellValue = result;

                if (cellValue < 7) { e.CellStyle.BackColor = Color.Silver; }
                else if (cellValue >= 7 && cellValue < 9) { e.CellStyle.BackColor = ColorTranslator.FromHtml("#0086AB"); e.CellStyle.ForeColor = Color.White; }
                else if (cellValue >= 9 && cellValue < 11) { e.CellStyle.BackColor = ColorTranslator.FromHtml("#F6CA06"); e.CellStyle.ForeColor = Color.White; }
                else if (cellValue >= 11) { e.CellStyle.BackColor = ColorTranslator.FromHtml("#DA5019"); e.CellStyle.ForeColor = Color.White; }

            }
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            ShowColumnGraphforSticker(getDatafromMiro(dataTable));
        }
    }
}
