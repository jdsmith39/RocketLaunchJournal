using System;
using System.Collections.Generic;
using System.Text;

namespace RocketLaunchJournal.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class AuditLogAttribute : Attribute
    {
        private Enums.LogTypeEnum _logType;
        private string _auditFieldsType;

        /// <summary>
        /// Tells the data context to automatically log all changes (audits) of a record
        /// </summary>
        /// <param name="logType">Log Type that will be added to the database</param>
        /// <param name="auditFieldsType">Location of the auditfields to use for the table.  Defaults to LogType.GetDisplayName if null</param>
        public AuditLogAttribute(Enums.LogTypeEnum logType, string auditFieldsType = null)
        {
            _logType = logType;
            _auditFieldsType = auditFieldsType ?? logType.GetDisplayName();
        }

        public string AuditFieldsType { get { return _auditFieldsType; } }
        public Enums.LogTypeEnum LogType { get { return _logType; } }
    }
}
