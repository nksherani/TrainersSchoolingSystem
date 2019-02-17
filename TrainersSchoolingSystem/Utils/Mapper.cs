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

            var properties = sourceObj.GetType().GetProperties();
            var modelproperties = destObj.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (modelproperties.Where(x => x.Name.ToLower() == property.Name.ToLower()).Count() > 0)
                {
                    var modelproperty = modelproperties.Where(x => x.Name.ToLower() == property.Name.ToLower()).FirstOrDefault();
                    modelproperty.SetValue(destObj, property.GetValue(sourceObj));
                }
            }

            return destObj;
        }
    }
}