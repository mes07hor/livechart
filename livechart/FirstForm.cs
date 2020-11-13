using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace livechart
{
    public partial class FirstForm : Form
    {
        public FirstForm()
        {
            InitializeComponent();
        }

        private void FirstForm_Load(object sender, EventArgs e)
        {
            //GetTableCatalog();
            //metroComboBox1.DataSource = TableCatalog;
            metroComboBox1.Items.Add("Adults1");
            metroComboBox1.Items.Add("Adults2");
            metroComboBox1.Items.Add("Students1");
            metroComboBox1.Items.Add("Students2");
            metroComboBox1.Items.Add("voicerecord");


        }


        //SQL to get table catalog
        private static readonly string SelectTableListSql = "SELECT * FROM sys.objects where type='U'";
        //list of table catalog
        static string[] TableCatalog = new string[10];
        static List<string> intermediateTableCatalog = new List<string>();

        static void GetTableCatalog()
        {
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

                        // テーブル一覧取得のSQLを実行します。
                        command.CommandText = SelectTableListSql;
                        var reader = command.ExecuteReader();
                        // 取得結果を確認します。
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var name = reader["name"] as string;
                                Console.WriteLine(name);
                                intermediateTableCatalog.Add(name);
                            }
                            TableCatalog = intermediateTableCatalog.ToArray();
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            Console.WriteLine(TableCatalog[i]);
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

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (radioButton_facilitator.Checked == true)
            {
                var chartform = new Form1(this);
                SplashForm.ShowSplash(chartform);
                chartform.Show();
            }
            else if (radioButton_organizer.Checked == true)
            {
                var chartform = new OrganizerForm(this);
                SplashForm.ShowSplash(chartform);
                chartform.Show();
            }
            else
            {

            }
            
        }
        public static string selectedTable0;
        public string selectedTable { get => selectedTable0; set => selectedTable0 = value; }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = metroComboBox1.SelectedIndex;
            selectedTable0 = metroComboBox1.Items[index].ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
