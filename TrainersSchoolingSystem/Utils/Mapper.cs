using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainersSchoolingSystem.Utils
{
    public static class Mapper<TDestination>
    {
        public static TDestination GetObject(Object sourceObj)
        {
            TDestination destObj = Activator.CreateInstance<TDestination>();

            try
            {
                if (sourceObj == null)
                    return destObj;
                var properties = sourceObj.GetType().GetProperties();
                var modelproperties = destObj.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (modelproperties.Where(x => x.Name.ToLower() == property.Name.ToLower()).Count() > 0)
                    {
                        var modelproperty = modelproperties.Where(x => x.Name.ToLower() == property.Name.ToLower()).FirstOrDefault();
                        try
                        {
                            modelproperty.SetValue(destObj, property.GetValue(sourceObj));
                        }
                        catch (Exception ex)
                        {
                            Logger.Fatal($"Source Type:{sourceObj.GetType().Name} PropertyName:{property.Name}");
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
                if (ex.InnerException != null)
                {
                    Logger.Fatal(ex.InnerException.Message);
                    Logger.Fatal(ex.InnerException.Source);
                    Logger.Fatal(ex.InnerException.TargetSite.Name);
                    Logger.Fatal(ex.InnerException.StackTrace);
                }
            }

            return destObj;
        }
    }
}