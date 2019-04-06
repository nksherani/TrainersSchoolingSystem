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
        public static FeeSetup feeSetup;
        static GlobalData()
        {
            RefreshConfiguration();
        }
        public static void RefreshConfiguration()
        {
            try
            {
                configuration = new ConfigurationViewModel();
                TrainersEntities db = new TrainersEntities();
                var config = db.Configurations.ToList();
                if (config.Count() > 0)
                {
                    var properties = configuration.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        try
                        {
                            if (config.Where(x => x.Key == property.Name).Count() > 0)
                                property.SetValue(configuration, config.Where(x => x.Key == property.Name).FirstOrDefault().Value);

                        }
                        catch (Exception ex)
                        {

                            Logger.Fatal(ex.Message);
                            Logger.Fatal(ex.Source);
                            Logger.Fatal(ex.TargetSite.Name);
                            Logger.Fatal(ex.StackTrace);
                            if (ex.InnerException != null)
                            {
                                Logger.Fatal(ex.InnerException.Message);
                                Logger.Fatal(ex.InnerException.Source);
                                Logger.Fatal(ex.InnerException.TargetSite.Name);
                                Logger.Fatal(ex.InnerException.StackTrace);
                            }
                        }
                    }
                }
                feeSetup = new FeeSetup();
                var fees = db.Fees.ToList();
                if (fees.Count() > 0)
                {
                    var properties = feeSetup.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        try
                        {
                            if (fees.Where(x => x.FeeType == property.Name).Count() > 0)
                                property.SetValue(feeSetup, (decimal)fees.Where(x => x.FeeType == property.Name).FirstOrDefault().Amount);

                        }
                        catch (Exception ex)
                        {

                            Logger.Fatal(ex.Message);
                            Logger.Fatal(ex.Source);
                            Logger.Fatal(ex.TargetSite.Name);
                            Logger.Fatal(ex.StackTrace);
                            if (ex.InnerException != null)
                            {
                                Logger.Fatal(ex.InnerException.Message);
                                Logger.Fatal(ex.InnerException.Source);
                                Logger.Fatal(ex.InnerException.TargetSite.Name);
                                Logger.Fatal(ex.InnerException.StackTrace);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                Logger.Fatal(ex.Source);
                Logger.Fatal(ex.TargetSite.Name);
                Logger.Fatal(ex.StackTrace);
                if(ex.InnerException!=null)
                {
                    Logger.Fatal(ex.InnerException.Message);
                    Logger.Fatal(ex.InnerException.Source);
                    Logger.Fatal(ex.InnerException.TargetSite.Name);
                    Logger.Fatal(ex.InnerException.StackTrace);
                }
            }
        }
    }
}