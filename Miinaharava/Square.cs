using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miinaharawa
{
    class Square : Button
    {
        
        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }
        public Square()
        {
            this.FlatStyle = FlatStyle.Flat;

        }

    }
}
