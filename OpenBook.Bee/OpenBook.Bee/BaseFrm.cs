using Autofac;
using OpenBook.Bee.Core;
using OpenBook.Bee.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBook.Bee
{
    public partial class BaseFrm : Form
    {
        //在类中定义变量
        protected static Autofac.IContainer container;

        static BaseFrm()
        {
            //构造函数中添加如下代码
            var builder = new ContainerBuilder();

            builder.RegisterType<FirstModel>().Named<IService>("First");
            builder.RegisterType<SecondModel>().Named<IService>("Second");
            builder.RegisterType<SecondModel>().Named<ISecondService>("Second");
            builder.RegisterType<ThirdModel>();
            //builder.RegisterInstance(typeof(BaseFrm)).As<BaseFrm>();

            container = builder.Build();
        }
        public BaseFrm()
        {
            InitializeComponent();            
        }
    }
}
