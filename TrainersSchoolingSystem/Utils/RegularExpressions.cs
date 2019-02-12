using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Utils
{
    public static class RegularExpressions
    {
        public static string Mobile = "^\\+[0-9]{12}$";
        public static string MobileError = "Enter Mobile No. in proper format like +923331234567";

        public static string Landline = "(^\\+[0-9]{11}$)|(^\\+[0-9]{12}$)";
        public static string LandlineError = "Enter Phone No. in proper format like +92211234567 or +922112345678";

    }
}