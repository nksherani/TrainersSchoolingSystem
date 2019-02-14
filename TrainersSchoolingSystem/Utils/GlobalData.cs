using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrainersSchoolingSystem.Models;
using TrainersSchoolingSystem.Models.DTOs;

namespace TrainersSchoolingSystem.Utils
{
    public static class GlobalData
    {
        public static ConfigurationViewModel configuration;
        static GlobalData()
        {
            RefreshConfiguration();
        }
        public static void RefreshConfiguration()
        {
            configuration = new ConfigurationViewModel();
            TrainersEntities db = new TrainersEntities();
            var config = db.Configurations.ToList();
            if (config.Count() > 0)
            {
                var properties = configuration.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (config.Where(x => x.Key == property.Name).Count() > 0)
                        property.SetValue(configuration, config.Where(x => x.Key == property.Name).FirstOrDefault().Value);
                }
            }
        }
    }
}