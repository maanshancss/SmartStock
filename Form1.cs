using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace SmartStock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;

            this.Location = new Point(Int32.Parse(ConfigurationSettings.AppSettings.GetValues("position_x")[0].ToString()),
                Int32.Parse(ConfigurationSettings.AppSettings.GetValues("position_y")[0].ToString()));
            this.Opacity = Int32.Parse(ConfigurationSettings.AppSettings.GetValues("opacity")[0].ToString());
        }

        Dictionary<string, string> m_dicCodeList = new Dictionary<string, string>();

        private void Form1_Load(object sender, EventArgs e)
        {

            foreach (string code in ConfigurationSettings.AppSettings.AllKeys)
            {
                if (string.IsNullOrWhiteSpace(code)) continue;
                if (code.StartsWith("code"))
                {
                    if (m_dicCodeList.ContainsKey(code))
                        continue;
                    m_dicCodeList.Add(code, ConfigurationSettings.AppSettings.GetValues(code)[0].ToString());
                }
            }

            //注册热键Ctrl+F12，这里的8879就是一个ID识别
            RegisterHotKey(this.Handle, 8879, 2, Keys.NumPad0);
            RegisterHotKey(this.Handle, 8880, 2, Keys.NumPad1);
            RegisterHotKey(this.Handle, 8881, 2, Keys.NumPad2);
            RegisterHotKey(this.Handle, 8882, 2, Keys.NumPad3);
            RegisterHotKey(this.Handle, 8883, 2, Keys.NumPad4);
            RegisterHotKey(this.Handle, 8884, 2, Keys.NumPad5);
            RegisterHotKey(this.Handle, 8885, 2, Keys.NumPad6);
            GetInfo(ConfigurationSettings.AppSettings.GetValues("code1")[0].ToString());
        }

        public void GetInfo(string a_strCode)
        {
            string l_strURL =
                $"http://push2.eastmoney.com/api/qt/stock/trends2/get?secid=1.{a_strCode}&fields1=f1,f2,f3,f4,f5,f6,f7,f8,f9,f10,f11,f12,f13&fields2=f51,f52,f53,f54,f55,f56,f57,f58&ut=e1e6871893c6386c5ff6967026016627&iscr=0&cb=cb_1591628037698_8038509&isqhquote=&cb_1591628037698_8038509=cb_1591628037698_8038509";
            HttpClient httpClient = new HttpClient();
            StringContent stringContent = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = httpClient.PostAsync(l_strURL, stringContent).Result;
            string l_strResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Console.WriteLine(l_strResult);
            l_strResult =
                l_strResult.Substring(l_strResult.IndexOf('{'), l_strResult.Length - l_strResult.IndexOf('{'));
            l_strResult = l_strResult.Substring(0, l_strResult.LastIndexOf('}') + 1);

            JObject jObject = JObject.Parse(l_strResult);
            if (jObject.GetValue("data") == null || string.IsNullOrWhiteSpace(jObject.GetValue("data").ToString()))
            {
                label1.Text = "err";
            }
            else
            {
                string data = jObject.GetValue("data").ToString();
                var trends = JObject.Parse(data).GetValue("trends").ToArray();
                Console.WriteLine(trends[trends.Length - 1]);

                string lastrecord = trends[trends.Length - 1].ToString().Split(',').Last();
                label1.Text = lastrecord;
                //this.Opacity = 50;
                //Thread.Sleep(2000);
                //this.Opacity = 15;
            }

            Console.WriteLine(l_strResult);
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //用来取消注册的热键
            UnregisterHotKey(this.Handle, 8879);
            UnregisterHotKey(this.Handle, 8880);
            UnregisterHotKey(this.Handle, 8881);
            UnregisterHotKey(this.Handle, 8882);
            UnregisterHotKey(this.Handle, 8883);
            UnregisterHotKey(this.Handle, 8884);
            UnregisterHotKey(this.Handle, 8885);
        }


        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hWnd">为窗口句柄</param>
        /// <param name="id">注册的热键识别ID</param>
        /// <param name="control">组合键代码  Alt的值为1，Ctrl的值为2，Shift的值为4，Shift+Alt组合键为5
        ///  Shift+Alt+Ctrl组合键为7，Windows键的值为8
        /// </param>
        /// <param name="vk">按键枚举</param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint control, Keys vk);

        /// <summary>
        /// 取消注册的热键
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="id">注册的热键id</param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // 响应热键
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0312: //这个是window消息定义的注册的热键消息     
                    if (m.WParam.ToString().Equals("8879")) //如果是注册的那个热键     
                    {
                        // 执行button按钮
                        GetInfo(ConfigurationSettings.AppSettings.GetValues("code0")[0].ToString());
                    }
                    else if (m.WParam.ToString().Equals("8880")) //如果是注册的那个热键     
                    {
                        // 执行button按钮
                        GetInfo(ConfigurationSettings.AppSettings.GetValues("code1")[0].ToString());
                    }
                    else if (m.WParam.ToString().Equals("8881")) //如果是注册的那个热键     
                    {
                        // 执行button按钮
                        GetInfo(ConfigurationSettings.AppSettings.GetValues("code2")[0].ToString());
                    }
                    else if (m.WParam.ToString().Equals("8882")) //如果是注册的那个热键     
                    {
                        // 执行button按钮
                        GetInfo(ConfigurationSettings.AppSettings.GetValues("code3")[0].ToString());
                    }
                    else if (m.WParam.ToString().Equals("8883")) //如果是注册的那个热键     
                    {
                        // 执行button按钮
                        GetInfo(ConfigurationSettings.AppSettings.GetValues("code4")[0].ToString());
                    }
                    else if (m.WParam.ToString().Equals("8884")) //如果是注册的那个热键     
                    {
                        // 执行button按钮
                        GetInfo(ConfigurationSettings.AppSettings.GetValues("code5")[0].ToString());
                    }
                    else if (m.WParam.ToString().Equals("8885")) //如果是注册的那个热键     
                    {
                        // 执行button按钮
                        this.Close();
                    }
                    break;
            }

            base.WndProc(ref m);
        }
    }
}