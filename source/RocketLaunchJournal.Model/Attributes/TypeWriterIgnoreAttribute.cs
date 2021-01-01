using System;

namespace RocketLaunchJournal.Model.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class TypeWriterIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Tells typewriter to ignore whatever this attribute is applied to
        /// </summary>
        public TypeWriterIgnoreAttribute()
        {
        }
    }
}
