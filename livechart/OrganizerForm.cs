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
               
                
                
                if (selectedgroup == "Adults1"||selectedgroup == "Adults2")
                {
                    
                    var data = db.Adults;
                    
                    var users = (from o in data
                                 where o.selectedgroup==selectedgroup 
                                 && SqlFunctions.DatePart("day", o.Date) == SqlFunctions.DatePart("day", now)
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
                if (selectedgroup == "Adults1"|| selectedgroup == "Adults1")
                {
                    var data = db.Adults;
                    var users = (from o in data
                                 where o.selectedgroup == selectedgroup
                                 && SqlFunctions.DatePart("day", o.Date) == SqlFunctions.DatePart("day", now)
                                 orderby o.username select o.username).Distinct();

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
                    if (selectedgroup == "Adults1"|| selectedgroup == "Adults2")
                    {
                        var data = db.Adults;
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
            ShowColumnGraph(GetTableData());

            ShowLineGraph();
            this.MinimumSize = this.Size;


            showDataGrid(dataTable);

            getDatafromMiro(dataTable);
           

        }
        DataTable dataTable = new DataTable();


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            cartesianChart1.DisableAnimations = true;
            cartesianChart2.DisableAnimations = true;

            UpdateLineGraph();


            // ShowColumnGraph();

            getDatafromMiro(dataTable);
            
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            
           ShowColumnGraph(GetTableData());
            
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

        private  void getDatafromMiro(DataTable dataTable)
        {
            
            Task getDatafromMiroTask = Task.Run(() =>
            {
                string url = "https://api.miro.com/v1/boards/o9J_kiyojvo=/widgets/?access_token=a76c74ac-256e-455d-ba1b-2d06b801f830";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                Stream s = res.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                string content = sr.ReadToEnd();
                sr.Close();

             var mirodata = JsonConvert.DeserializeObject<Root>(content);
            

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
                }
                
            }
            });
            metroGrid1.DataSource= dataTable;
            //metroGrid1.DefaultCellStyle.Font = new Font("Tahoma", 15);
            metroGrid1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            metroGrid1.GridColor = Color.White;
            metroGrid1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //metroGrid1.AutoResizeColumns();
            //metroGrid1.AutoResizeRows();
            
            //metroGrid1.Update();
            
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
    }
}
