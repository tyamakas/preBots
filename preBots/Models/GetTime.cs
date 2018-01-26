using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace preBots.Models
{
    public class GetTime
    {
        private string nowTimes;
        
        private string getNowTimes()
        {
            DateTime dtNow = DateTime.Now;
            nowTimes = dtNow.Hour.ToString() + ":" + dtNow.Minute.ToString();

            return nowTimes;
        }
        private void setNowTimes(string times)
        {
            nowTimes = times;
        }
    }
}