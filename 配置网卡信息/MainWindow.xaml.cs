using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.NetworkInformation;
namespace 配置网卡信息
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void setDHCP()
        {
            string _doscmd = "netsh interface ip set address 本地连接 DHCP";
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(_doscmd.ToString());
            _doscmd = "netsh interface ip set dns 本地连接 DHCP";
            p.StandardInput.WriteLine(_doscmd.ToString());
            p.StandardInput.WriteLine("exit");
        }
        /// <summary>
        /// 设置IP地址，掩码，网关等
        /// </summary>
        private void setIPaddress(string ip,string submask,string gateway,string dns1,string dns2)
        {
            
            string str = "netsh interface ip set address name=\""+comboxbox1.SelectedValue+"\" static " + ip + " " + submask + " " + gateway + " 1";

            executeCmd(str);

            string str1 = "netsh interface ip set dns name=\"" + comboxbox1.SelectedValue + "\" static " + dns1 + " primary ";

            executeCmd(str1);

            string str2 = "netsh interface ip set dns name=\"" + comboxbox1.SelectedValue +"\"" +dns2 ;

            executeCmd(str2);
        }

        private void setIPaddress()
        {
            string str = "netsh interface ip set address name=\"" + comboxbox1.SelectedValue + "\" dhcp " ;
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();
            MessageBox.Show(output);
            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();


            Console.WriteLine(output);
        }

        private void executeCmd(string str)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序
            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str + "&exit");
            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令
            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();//等待程序执行完退出进程
            p.Close();
            Console.WriteLine(output);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)checkbox1.IsChecked)
            {
               
                setIPaddress();
            }
            else
            {
                bool flag = true;
                string[] txts = new string[] { txt1.Text, txt2.Text, txt3.Text, txt4.Text };
                for (int i = 1; i < 4; i++)
                {
                    string text = txts[i];
                    if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"^(?:(?:1[0-9][0-9]\.)|(?:2[0-4][0-9]\.)|(?:25[0-5]\.)|(?:[1-9][0-9]\.)|(?:[0-9]\.)){3}(?:(?:1[0-9][0-9])|(?:2[0-4][0-9])|(?:25[0-5])|(?:[1-9][0-9])|(?:[0-9]))$"))
                        flag = false;
                    
                }
                if(checkipgateway(txt1.Text, txt2.Text, txt3.Text))
                {
                    MessageBox.Show("不等于");
                }
                if (flag)
                {
                    setIPaddress(txt1.Text, txt2.Text, txt3.Text, txt4.Text,txt5.Text);
                }
                else
                {
                    MessageBox.Show("IP地址格式不正确");
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<MyNetworkInformation> list  = MyNetworkInformation.GetIPS();
            comboxbox1.ItemsSource = list;
            comboxbox1.DisplayMemberPath = "networkName";
            comboxbox1.SelectedValuePath = "networkName";
            comboxbox1.SelectedIndex = comboxbox1.Items.IndexOf(comboxbox1.Items[0]);
            //SelectedValue是选中的那个对象的SelectedValuePath对应的属性。
            //SelectedValuePath是指定SelectedValue映射的属性，如果不指定则SelectedValue是整个对象。
            //DisplayMemberPath是显示给用户看的映射的属性。

        }

        private void checkbox1_Checked(object sender, RoutedEventArgs e)
        {
            txt1.IsEnabled = false;
            txt2.IsEnabled = false;
            txt3.IsEnabled = false;
            txt4.IsEnabled = false;
            txt5.IsEnabled = false;
        }

        private void checkbox1_Unchecked(object sender, RoutedEventArgs e)
        {
            txt1.IsEnabled = true;
            txt2.IsEnabled = true;
            txt3.IsEnabled = true;
            txt4.IsEnabled = true;
            txt5.IsEnabled = true;
        }

        private bool checkipgateway(string ip,string mask,string gateway)
        {
            string[] p = ip.Split(new char[] { '.' });
            string[] m= mask.Split(new char[] { '.' });
            string[] g = new string[4];
            for(int i=0;i<p.Length;i++)
            {
                 g[i]=( Convert.ToInt32(p[i]) & Convert.ToInt32(m[i])).ToString();
            }
            string gy=null;
            for (int i = 0; i < g.Length; i++)
            {
                if (i!=3)
                {
                    gy += g[i] + ".";
                }
                else
                {
                    gy += g[i];
                }
            }
           
            return !gy.Equals(gateway);
        }

        private void comboxbox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<MyNetworkInformation> list = MyNetworkInformation.GetIPS();
            MyNetworkInformation my = list.Find(x=> x.networkName.Contains(comboxbox1.SelectedValue.ToString()));
            txt1.Text = my.ipaddress;
            txt2.Text = my.mask;
            txt3.Text = my.gateway;
            txt4.Text = my.dns1;
            txt5.Text = my.dns2;
            checkbox1.IsChecked = my.dhcp;
        }
    }
}
