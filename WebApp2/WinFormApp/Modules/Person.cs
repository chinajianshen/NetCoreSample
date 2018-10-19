using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormApp.Modules
{
   public class Person
    {
        //建立事件信号数组
        public AutoResetEvent[] autoEvents = null;

        public Person()
        {
            autoEvents = new AutoResetEvent[]
            {
                new AutoResetEvent(false),
                new AutoResetEvent(false),
                new AutoResetEvent(false)
            };
        }

        public void GetCar()
        {
            MessageBox.Show("捡到奔驰");
            autoEvents[0].Set();
        }

        public void GetHouse()
        {
            MessageBox.Show("赚到房子");
            autoEvents[1].Set();
        }

        public void GetWife()
        {
            MessageBox.Show("骗到老婆");
            autoEvents[2].Set();
        }

        public void ShowHappy()
        {
            MessageBox.Show("人生要有的都有了，好开心");
        }
    }
}
