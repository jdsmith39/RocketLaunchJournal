using RocketLaunchJournal.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Client.Helpers
{
    public static class PropertyReflections
    {
        private static List<PropertyInfo>? _rocketDtoProperties;
        public static List<PropertyInfo> RocketDtoProperties
        {
            get
            {
                if (_rocketDtoProperties == null)
                    _rocketDtoProperties = typeof(RocketDto).GetProperties().ToList();

                return _rocketDtoProperties;
            }
        }

        private static List<PropertyInfo>? _launchDtoProperties;
        public static List<PropertyInfo> LaunchDtoProperties
        {
            get
            {
                if (_launchDtoProperties == null)
                    _launchDtoProperties = typeof(LaunchDto).GetProperties().ToList();

                return _launchDtoProperties;
            }
        }

        public static PropertyInfo GetRocketDtoProperty(string name)
        {
            return RocketDtoProperties.Single(w => w.Name == name);
        }

        public static PropertyInfo GetLaunchDtoProperty(string name)
        {
            return LaunchDtoProperties.Single(w => w.Name == name);
        }

        public static PropertyInfo GetPropertyByName(List<PropertyInfo> properties, string name)
        {
            return properties.Single(w => w.Name == name);
        }
        

        /// <summary>
        /// Returns proper label name for a property based on attributes
        /// </summary>
        /// <param name="propertyInfo">property's reflection info</param>
        /// <returns>string</returns>
        public static string GetLabelName(PropertyInfo propertyInfo)
        {
            var result = propertyInfo.Name;
            {
                var attribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (attribute != null && !string.IsNullOrWhiteSpace(attribute.Name))
                    result = attribute.Name;
            }

            {
                var attribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null && !string.IsNullOrWhiteSpace(attribute.Description))
                    result = attribute.Description;
            }
            return result;
        }

        /// <summary>
        /// Returns label name for a property based on a list
        /// </summary>
        /// <param name="properties">list of property infos</param>
        /// <param name="propertyToGet">property string name. hint use: nameof(property)</param>
        /// <returns>string</returns>
        public static string GetLabelName(List<PropertyInfo> properties, string propertyToGet, bool isShortName = false)
        {
            var propInfo = properties.Find((p) => p.Name == propertyToGet);
            if (propInfo == null)
                throw new ArgumentException(nameof(propertyToGet));

            if (isShortName)
                return GetLabelShortName(propInfo);
         
            return GetLabelName(propInfo);
        }

        /// <summary>
        /// Gets the display short name and returns it.
        /// </summary>
        /// <param name="propertyInfo">property's reflection info</param>
        /// <returns>string</returns>
        public static string GetLabelShortName(PropertyInfo propertyInfo)
        {
            var result = propertyInfo.Name;
            {
                var attribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
                if (attribute != null && !string.IsNullOrWhiteSpace(attribute.ShortName))
                    result = attribute.ShortName;
                else
                    result = GetLabelName(propertyInfo);
            }
            return result;
        }

        /// <summary>
        /// returns true if the field is required
        /// </summary>
        /// <param name="propertyInfo">property info of the property</param>
        /// <returns>bool</returns>
        public static bool IsRequired(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                return false;

            if (propertyInfo.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true).Length > 0)
                return true;

            return propertyInfo.PropertyType.IsValueType && Nullable.GetUnderlyingType(propertyInfo.PropertyType) == null;
        }
    }
}
