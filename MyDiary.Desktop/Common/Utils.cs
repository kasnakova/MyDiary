using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDiary.Desktop.Common
{
    public class Utils
    {
        public static void AppWait() 
        {
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public static void AppResume()
        {
            Mouse.OverrideCursor = null;
        }
    }
}
