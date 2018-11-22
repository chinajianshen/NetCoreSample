using Autofac;
using OpenBook.Bee.Entity;
using OpenBook.Bee.Interface;
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
    public partial class AutoFacTestFrm : Form
    {
        //在类中定义变量
        private Autofac.IContainer container;


        public AutoFacTestFrm()
        {
            InitializeComponent();

            //构造函数中添加如下代码
            var builder = new ContainerBuilder();

            builder.RegisterType<FirstModel>().Named<IService>("First");
            builder.RegisterType<SecondModel>().Named<IService>("Second");
            builder.RegisterType<SecondModel>().Named<ISecondService>("Second");
            builder.RegisterType<ThirdModel>();
            builder.RegisterInstance(this).As<Form>();
            
            container = builder.Build();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var first = container.ResolveOptionalNamed<IService>("First");
            string str1 = first.Do();

            var second = container.ResolveOptionalNamed<IService>("Second");
            string str2 = second.Do();

            var third = container.Resolve<ThirdModel>();
            string str3 = third.Do();
        }
    }
}
