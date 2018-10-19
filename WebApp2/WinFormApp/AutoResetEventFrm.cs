using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormApp.Modules;

namespace WinFormApp
{
    public partial class AutoResetEventFrm : Form
    {
        public AutoResetEventFrm()
        {
            InitializeComponent();
        }

        private void AutoResetEventFrm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //定义一个人对象
            Person person = new Person();

            Thread GetCarThread = new Thread(new ThreadStart(person.GetCar));
            GetCarThread.Start();

            Thread GetHouseThead = new Thread(new ThreadStart(person.GetHouse));
            GetHouseThead.Start();

            Thread GetWillThead = new Thread(new ThreadStart(person.GetWife));
            GetWillThead.Start();           

            //异常点 不支持一个 STA 线程上针对多个句柄的 WaitAll
            //在启动 Program中 将[STAThread] 改成 [MTAThread]
            AutoResetEvent.WaitAll(person.autoEvents); // AutoResetEvent继承WaitHandle 等同于：WaitHandle.WaitAll();

            person.ShowHappy();
        }
    }
}
