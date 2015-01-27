using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace CopyFileDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread timeThread;
        Thread copyThread;
        public MainWindow()
        {
            InitializeComponent();
            this.displayTimeByThread.Text = DateTime.Now.ToLocalTime().ToString("yyyy年MM月dd日 hh:mm:ss"); ;
            timeThread = new Thread(new ThreadStart(DispatcherThread));
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            timeThread.Start();
        }
        public void DispatcherThread()
        {
            //可以通过循环条件来控制UI的更新
            while (true)
            {
                ///线程优先级，最长超时时间，方法委托（无参方法）
                displayTimeByThread.Dispatcher.BeginInvoke(
                   DispatcherPriority.Normal, new Action(UpdateTime));
                Thread.Sleep(1000);
            }
        }


        private void UpdateTime()
        {
            this.displayTimeByThread.Text = DateTime.Now.ToLocalTime().ToString("yyyy年MM月dd日 hh:mm:ss");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ///关闭所有启动的线程
            timeThread.Abort();
            copyThread.Abort();
            Application.Current.Shutdown();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ///设定要复制的文件全路径
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.AddExtension = true;
            openFile.CheckPathExists = true;
            openFile.Filter = "*.rar|*.rar|all files|*.*";
            openFile.FilterIndex = 0;
            openFile.Multiselect = false;
            bool? f = openFile.ShowDialog();
            if (f != null && f.Value)
            {
                this.srcFile.Text = openFile.FileName;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ///设定目标文件全路径
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.AddExtension = true;
            saveFile.Filter = "*.rar|*.rar|all files|*.*";
            saveFile.FilterIndex = 0;

            bool? f = saveFile.ShowDialog();
            if (f != null && f.Value)
            {
                this.saveFilePath.Text = saveFile.FileName;
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string fileName = this.srcFile.Text.Trim();
            string destPath = this.saveFilePath.Text.Trim();
            if (!File.Exists(fileName))
            {
                MessageBox.Show("源文件不存在");
                return;
            }

            ///copy file and nodify ui that rate of progress of file copy          
            this.copyFlag.Text = "开始复制。。。";

            //设置进度条最大值，这句代码写的有点郁闷
            this.copyProgress.Maximum = (new FileInfo(fileName)).Length;

            //保存复制文件信息，以进行传递
            CopyFileInfo c = new CopyFileInfo() { SourcePath = fileName, DestPath = destPath };
            //线程异步调用复制文件
            copyThread = new Thread(new ParameterizedThreadStart(CopyFile));
            copyThread.Start(c);

            this.copyFlag.Text = "复制完成。。。";


        }
        /// <summary>
        /// 复制文件的委托方法
        /// </summary>
        /// <param name="obj">复制文件的信息</param>
        private void CopyFile(object obj)
        {
            CopyFileInfo c = obj as CopyFileInfo;
            CopyFile(c.SourcePath, c.DestPath);
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        private void CopyFile(string sourcePath, string destPath)
        {
            FileInfo f = new FileInfo(sourcePath);
            FileStream fsR = f.OpenRead();
            FileStream fsW = File.Create(destPath);
            long fileLength = f.Length;
            byte[] buffer = new byte[102400000000];
            int n = 0;

            while (true)
            {
                ///设定线程优先级
                ///异步调用UpdateCopyProgress方法
                ///并传递2个long类型参数fileLength 与 fsR.Position
                this.displayCopyInfo.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle,
                    new Action<long, long>(UpdateCopyProgress), fileLength, fsR.Position);

                //读写文件
                n = fsR.Read(buffer, 0, 1024);
                if (n == 0)
                {
                    break;
                }
                fsW.Write(buffer, 0, n);
                fsW.Flush();
                Thread.Sleep(1);
            }
            fsR.Close();
            fsW.Close();
        }

        private void UpdateCopyProgress(long fileLength, long currentLength)
        {
            this.displayCopyInfo.Text = string.Format("总大小：{0},已复制:{1}", fileLength, currentLength);
            //刷新进度条            
            this.copyProgress.Value = currentLength;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
    public class CopyFileInfo
    {
        public string SourcePath { get; set; }
        public string DestPath { get; set; }
    }
}
