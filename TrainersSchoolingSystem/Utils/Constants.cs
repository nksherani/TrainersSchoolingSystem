using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Utils
{
    public static class Constants
    {
        public static List<KeyValuePair<int, string>> months = new List<KeyValuePair<int, string>>();
        public static Dictionary<string, int> months2 = new  Dictionary<string, int>();
        static Constants()
        {
            months.Add(new KeyValuePair<int, string>(1, "January"));
            months.Add(new KeyValuePair<int, string>(2, "February"));
            months.Add(new KeyValuePair<int, string>(3, "March"));
            months.Add(new KeyValuePair<int, string>(4, "April"));
            months.Add(new KeyValuePair<int, string>(5, "May"));
            months.Add(new KeyValuePair<int, string>(6, "June"));
            months.Add(new KeyValuePair<int, string>(7, "July"));
            months.Add(new KeyValuePair<int, string>(8, "August"));
            months.Add(new KeyValuePair<int, string>(9, "September"));
            months.Add(new KeyValuePair<int, string>(10, "October"));
            months.Add(new KeyValuePair<int, string>(11, "November"));
            months.Add(new KeyValuePair<int, string>(12, "December"));

            months2.Add("January",1);
            months2.Add( "February",2);
            months2.Add( "March",3);
            months2.Add( "April",4);
            months2.Add( "May",5);
            months2.Add( "June",6);
            months2.Add( "July",7);
            months2.Add( "August",8);
            months2.Add( "September",9);
            months2.Add( "October",10);
            months2.Add( "November",11);
            months2.Add( "December",12);
        }
    }
}