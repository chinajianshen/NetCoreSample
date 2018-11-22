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
    public partial class AutoFacTestFrm : BaseFrm
    {
      


        public AutoFacTestFrm()
        {
            InitializeComponent();           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var first = container.ResolveOptionalNamed<IService>("First");
            string str1 = first.Do();

            var second = container.ResolveOptionalNamed<IService>("Second");
            string str2 = second.Do();

            var second2 = container.ResolveOptionalNamed<ISecondService>("Second");
            string str22 = second2.Do();

            var third = container.Resolve<ThirdModel>();
            string str3 = third.Do();
        }
    }
}
