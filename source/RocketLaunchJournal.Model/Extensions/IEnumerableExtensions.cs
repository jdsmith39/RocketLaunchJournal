using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Generates a CSV file based on the list of objects passed in.  File name is the class name.
        /// Defaults to use Properties and does not fill in nulls with "Null"
        /// </summary>
        /// <param name="listToWriteOut">data to be written out</param>
        /// <param name="stream">Stream to be written to</param>
        public static void GenerateCSV(this IEnumerable listToWriteOut, Stream stream)
        {
            listToWriteOut.GenerateCSV(stream, new IEnumerableExtensionOptions() { UsePropertiesForColumns = true });
        }

        /// <summary>
        /// Generates a CSV file based on the list of objects passed in.  File name is the class name.
        /// </summary>
        /// <param name="listToWriteOut">data to be written out</param>
        /// <param name="stream">Stream to be written to</param>
        /// <param name="configOptions">configuration options</param>
        public static void GenerateCSV(this IEnumerable listToWriteOut, Stream stream, IEnumerableExtensionOptions configOptions)
        {
            var sw = new StreamWriter(stream);
            var firstLoop = true;
            var header = new StringBuilder();
            var data = new StringBuilder();
            
            // columns
            PropertyInfo[]? properties = null;
            FieldInfo[]? fields = null;

            // loop through all objects
            foreach (var obj in listToWriteOut)
            {
                var dataRow = GetRowData(configOptions, firstLoop, header, ref properties, ref fields, obj);

                if (firstLoop && configOptions.ShowHeaderRow)
                {
                    firstLoop = false;
                    sw.WriteLine(header.ToString().Trim(','));
                }
                // remove first ","
                sw.WriteLine(dataRow.ToString().Remove(0, 1));
            }

            sw.Flush();
        }

        /// <summary>
        /// Generates a CSV file based on the list of objects passed in.  File name is the class name.
        /// Defaults to use Properties and does not fill in nulls with "Null"
        /// </summary>
        /// <param name="listToWriteOut">data to be written out</param>
        /// <param name="stream">Stream to be written to</param>
        public async static Task GenerateCSVAsync(this IEnumerable listToWriteOut, Stream stream)
        {
            await listToWriteOut.GenerateCSVAsync(stream, new IEnumerableExtensionOptions() { UsePropertiesForColumns = true });
        }

        /// <summary>
        /// Generates a CSV file based on the list of objects passed in.  File name is the class name.
        /// </summary>
        /// <param name="listToWriteOut">data to be written out</param>
        /// <param name="stream">Stream to be written to</param>
		/// <param name="usePropertiesForColumns">True:  uses Properties for column definitions.  False:  uses Fields for column definitions.</param>
        /// <param name="fillInNULLs">if True, null values will be filled with NULL, false will be empty</param>
        /// <param name="IgnoreList">properties/fields that are to be ignored</param>
        public async static Task GenerateCSVAsync(this IEnumerable listToWriteOut, Stream stream, IEnumerableExtensionOptions configOptions)
        {
            var sw = new StreamWriter(stream);
            var firstLoop = true;
            var header = new StringBuilder();

            // columns
            PropertyInfo[]? properties = null;
            FieldInfo[]? fields = null;

            // loop through all objects
            foreach (var obj in listToWriteOut)
            {
                var dataRow = GetRowData(configOptions, firstLoop, header, ref properties, ref fields, obj);

                if (firstLoop && configOptions.ShowHeaderRow)
                {
                    firstLoop = false;
                    await sw.WriteLineAsync(header.ToString().Trim(','));
                }
                // remove first ","
                await sw.WriteLineAsync(dataRow.ToString().Remove(0, 1));
            }

            await sw.FlushAsync();
        }

        public static async Task GenerateCSVAsync(this IEnumerable<Dictionary<string, object>> listToWriteOut, Stream stream, IEnumerableExtensionOptions options)
        {
            var sw = new StreamWriter(stream);
            var firstLoop = true;
            var header = new StringBuilder();

            // loop through all objects
            foreach (var obj in listToWriteOut)
            {
                var dataRow = new StringBuilder();
                foreach (var item in obj)
                {
                    if (firstLoop && options.ShowHeaderRow)
                    {
                        header.Append($"{item.Key},");
                    }

                    var oValue = item.Value;
                    if (oValue == null && options.FillInNulls)
                        oValue = "Null";

                    if (oValue == null)
                        dataRow.Append(",");
                    else
                    {
                        Boolean typeIsString = item.Value.GetType() == typeof(String);
                        if (typeIsString)
                            dataRow.Append(String.Format(",\"{0}\"", oValue!.ToString().Replace("\"", "\"\"")));
                        else
                            dataRow.Append(String.Format(",{0}", oValue));
                    }
                }

                if (firstLoop && options.ShowHeaderRow)
                {
                    firstLoop = false;
                    await sw.WriteLineAsync(header.ToString().Trim(','));
                }
                // remove first ","
                await sw.WriteLineAsync(dataRow.ToString().Remove(0, 1));
            }

            await sw.FlushAsync();
        }

        /// <summary>
        /// Generates a CSV string based on the list of objects passed in.
        /// Defaults to use Properties and does not fill in nulls with "Null"
        /// </summary>
        /// <param name="listToWriteOut">data to be written out</param>
        /// <returns>String representing the csv</returns>
		public static String GenerateCSVString(this IEnumerable listToWriteOut)
        {
			return listToWriteOut.GenerateCSVString(new IEnumerableExtensionOptions() { UsePropertiesForColumns = true });
        }

        /// <summary>
        /// Generates a CSV string based on the list of objects passed in.
        /// </summary>
        /// <param name="listToWriteOut">data to be written out</param>
		/// <param name="usePropertiesForColumns">True:  uses Properties for column definitions.  False:  uses Fields for column definitions.</param>
        /// <param name="fillInNULLs">if True, null values will be filled with NULL, false will be empty</param>
        /// <param name="IgnorePropertyList">Array of properties that are to be ignored</param>
        /// <returns>String representing the csv</returns>
		public static String GenerateCSVString(this IEnumerable listToWriteOut, IEnumerableExtensionOptions configOptions)
        {
            var firstLoop = true;
            var header = new StringBuilder();
            var data = new StringBuilder();

            // columns
            PropertyInfo[]? properties = null;
            FieldInfo[]? fields = null;

            // loop through all objects
            foreach (var obj in listToWriteOut)
            {
                var dataRow = GetRowData(configOptions, firstLoop, header, ref properties, ref fields, obj);

                if (firstLoop && configOptions.ShowHeaderRow)
                {
                    firstLoop = false;
                    data.AppendLine(header.ToString().Trim(','));
                }
                // remove first ","
                data.AppendLine(dataRow.ToString().Remove(0, 1));
            }

            return data.ToString();
        }

        /// <summary>
        /// Gets the data for the row.
        /// If first loop is true then also returns header information.
        /// </summary>
        /// <param name="configOptions"></param>
        /// <param name="firstLoop"></param>
        /// <param name="header"></param>
        /// <param name="properties"></param>
        /// <param name="fields"></param>
        /// <param name="obj"></param>
        /// <returns>string builder with row information.  If first row, then header stringbuilder is also filled in</returns>
        private static StringBuilder GetRowData(IEnumerableExtensionOptions configOptions, bool firstLoop, StringBuilder header, ref PropertyInfo[]? properties, ref FieldInfo[]? fields, object obj)
        {
            if (configOptions.UsePropertiesForColumns)
                properties = obj.GetType().GetProperties().RemoveUnneededColumns(configOptions);
            else
                fields = obj.GetType().GetFields().RemoveUnneededColumns(configOptions);

            var dataRow = new StringBuilder();

            // loop through all properties
            var fieldCountGreaterThan0 = (configOptions.UsePropertiesForColumns ? properties!.Length : fields!.Length) > 0;
            if (fieldCountGreaterThan0 && obj.GetType() != typeof(String))
            {
                for (int blah = 0; blah < (configOptions.UsePropertiesForColumns ? properties!.Length : fields!.Length); blah++)
                {
                    // on first loop generate the header
                    if (firstLoop)
                    {
                        if (configOptions.UsePropertiesForColumns)
                            header.Append(String.Format(",{0}", GetFieldName(properties![blah].Name, configOptions)));
                        else
                            header.Append(String.Format(",{0}", GetFieldName(fields![blah].Name, configOptions)));
                    }

                    Object oValue;
                    if (configOptions.UsePropertiesForColumns)
                        oValue = properties![blah].GetValue(obj, null);
                    else
                        oValue = fields![blah].GetValue(obj);

                    if (oValue == null && configOptions.FillInNulls)
                        oValue = "Null";

                    if (oValue == null)
                        dataRow.Append(",");
                    else
                    {
                        Boolean typeIsString = (configOptions.UsePropertiesForColumns ? properties![blah].PropertyType : fields![blah].FieldType) == typeof(String);
                        if (typeIsString)
                            dataRow.Append(String.Format(",\"{0}\"", oValue.ToString().Replace("\"", "\"\"")));
                        else
                            dataRow.Append(String.Format(",{0}", oValue));
                    }
                }
            }
            else
            {
                if (obj.GetType() == typeof(String))
                    dataRow.Append(String.Format(",\"{0}\"", obj.ToString().Replace("\"", "\"\"")));
                else
                    dataRow.Append(String.Format(",{0}", obj));
            }

            return dataRow;
        }

        /// <summary>
        /// Will return in dictionary format, all string properties and their longest length
        /// </summary>
        /// <param name="list">data to be used</param>
		/// <param name="usePropertiesForColumns">True:  uses Properties for column definitions.  False:  uses Fields for column definitions.</param>
        /// <returns>Dictionary<String, Int> String is property and int is the longest string length.  Null otherwise.</returns>
        public static Dictionary<String, int> GetLengthOfAllStringFields(this IEnumerable list, Boolean usePropertiesForColumns)
        {
            var data = new Dictionary<String, int>();
            PropertyInfo[]? properties = null;
			FieldInfo[]? fields = null;
            // loop through all objects
            foreach (var obj in list)
            {
				if (usePropertiesForColumns)
				{
					if (properties == null)
						properties = obj.GetType().GetProperties();
				}
				else
				{
					if (fields == null)
						fields = obj.GetType().GetFields();
				}

                // loop through all properties/fields
                for (int blah = 0; blah < (usePropertiesForColumns ? properties!.Length : fields!.Length); blah++)
                {
					if (usePropertiesForColumns)
					{
						if (properties![blah].PropertyType != typeof(String))
							continue;
					}
					else
					{
						if (fields![blah].FieldType != typeof(String))
							continue;
					}

					String name;
					if (usePropertiesForColumns)
						name = properties![blah].Name;
					else
						name = fields![blah].Name;

                    int stringLength;
					if (usePropertiesForColumns)
						stringLength = (properties![blah].GetValue(obj, null) ?? "").ToString().Length;
					else
						stringLength = (fields![blah].GetValue(obj) ?? "").ToString().Length;

                    if (data.ContainsKey(name))
                    {
                        if (data[name] < stringLength)
                            data[name] = stringLength;
                    }
                    else
                        data.Add(name, stringLength);
                }
            }

            return data;
        }

        /// <summary>
        /// Will return in dictionary format, all string properties and their longest length
        /// </summary>
        /// <param name="list">data to be used</param>
        /// <returns>Dictionary<String, Int> String is property and int is the longest string length.  Null otherwise.</returns>
        public static Dictionary<String, int> GetLengthOfAllStringFields(this DataTable list)
        {
            var data = new Dictionary<String, int>();
            var columnIndexToName = new Dictionary<int, String>();
            for (int i = 0; i < list.Columns.Count; i++)
            {
                columnIndexToName[i] = list.Columns[i].ColumnName;
                data[list.Columns[i].ColumnName] = 0;
            }
            // loop through all objects
            foreach (DataRow obj in list.Rows)
            {
                // loop through all properties
                for (int blah = 0; blah < obj.ItemArray.Length; blah++)
                {
                    if (obj.ItemArray[blah].GetType() != typeof(String))
                        continue;

                    var stringLength = obj.ItemArray[blah].ToString().Length;

                    if (data[columnIndexToName[blah]] < stringLength)
                        data[columnIndexToName[blah]] = stringLength;
                }
            }

            return data;
        }

        public enum AggregateFunction
        {
            Max,
            Min,
            Sum,
            Avg
        }

        /// <summary>
        /// Returns Object T with all numeric properties having had the Aggregate Function applied to them.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">data to be used</param>
        /// <param name="aggregateFunction">Enum telling what aggregate to use</param>
        /// <returns>object of Type T.  Null otherwise.</returns>
        /// <remarks>
        /// If doing an average, make sure the properties are doubles, otherwise do a SUM and do your own division after.
        /// </remarks>
        public static T GetAggregateForAllNumericProperties<T>(this IEnumerable<T> list, AggregateFunction aggregateFunction, String[]? IgnorePropertyList = null)
        {
            int listCount = list.Count();
            if (listCount == 0)
                return default(T)!;

            T data = (T)Activator.CreateInstance(typeof(T), true);
            PropertyInfo[] properties = typeof(T).GetProperties();
            if (IgnorePropertyList != null)
                properties = properties.Where(p => IgnorePropertyList != null && !IgnorePropertyList.Contains(p.Name)).ToArray();

            Boolean firstLoop = true;

            // loop through all objects
            foreach (T obj in list)
            {
                // loop through all properties
                for (int blah = 0; blah <= properties.Count() - 1; blah++)
                {
                    // if not a numeric type then continue for
                    Type propType = properties[blah].PropertyType;
                    if (!IsNumericType(propType))
                        continue;

                    var value = properties[blah].GetValue(obj, null);
                    if (value == null)
                        continue;
                    var aggregateValue = properties[blah].GetValue(data, null);

                    Type underlyingType = Nullable.GetUnderlyingType(propType);

                    switch (aggregateFunction)
                    {
                        case AggregateFunction.Max:
                            if (Convert.ToDouble(value) > Convert.ToDouble(aggregateValue) || firstLoop)
                                properties[blah].SetValue(data, value, null);
                            break;
                        case AggregateFunction.Min:
                            if (Convert.ToDouble(value) < Convert.ToDouble(aggregateValue) || firstLoop)
                                properties[blah].SetValue(data, value, null);
                            break;
                        case AggregateFunction.Sum:
                        case AggregateFunction.Avg:
                            var newValue = Convert.ToDouble(value) + (aggregateValue == null ? 0 : Convert.ToDouble(aggregateValue));
                            if (underlyingType != null)
                                properties[blah].SetValue(data, Convert.ChangeType(newValue, underlyingType, null), null);
                            else
                                properties[blah].SetValue(data, Convert.ChangeType(newValue, propType, null), null);
                            break;
                    }
                }

                if (firstLoop)
                    firstLoop = false;
            }

            // if average then go through all properties again
            if (aggregateFunction == AggregateFunction.Avg & listCount > 0)
            {
                // loop through all properties
                for (int blah = 0; blah <= properties.Count() - 1; blah++)
                {
                    // if not a numeric type then continue for
                    if (!IsNumericType(properties[blah].PropertyType))
                        continue;

                    Type propType = properties[blah].PropertyType;
                    Type underlyingType = Nullable.GetUnderlyingType(properties[blah].PropertyType);

                    var aggregateValue = properties[blah].GetValue(data, null);
                    if (underlyingType != null)
                        properties[blah].SetValue(data, Convert.ChangeType((aggregateValue == null ? 0 : Convert.ToDouble(aggregateValue)) / listCount, underlyingType, null), null);
                    else
                        properties[blah].SetValue(data, Convert.ChangeType((aggregateValue == null ? 0 : Convert.ToDouble(aggregateValue)) / listCount, propType, null), null);
                }
            }

            return data;
        }

        /// <summary>
        /// Determines if a type is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <remarks>
        /// Boolean is not considered numeric.
        /// </remarks>
        public static bool IsNumericType(Type type)
        {
            if (type == null)
                return false;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        switch (Type.GetTypeCode(Nullable.GetUnderlyingType(type)))
                        {
                            case TypeCode.Byte:
                            case TypeCode.Decimal:
                            case TypeCode.Double:
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.SByte:
                            case TypeCode.Single:
                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                                return true;
                        }
                        return false;
                    }
                    return false;
            }
            return false;
        }

        /// <summary>
        /// DataTable to IEnumerable<Dictionary<string,object>>
        /// </summary>
        /// <param name="table">Data Table</param>
        /// <returns>IEnumerable<Dictionary<string,object>></returns>
        public static IEnumerable<Dictionary<string,object>> ToList(this DataTable table)
        {
            var list = new List<Dictionary<string, object>>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                var item = table.Rows[i];
                var newRow = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns)
                {
                    newRow[column.ColumnName] = item[column];
                }
                list.Add(newRow);
            }
            return list;
        }

        /// <summary>
        /// Convert IEnumberable to Data Table
        /// </summary>
        /// <param name="queryResult">data to be converted</param>
        /// <param name="configOptions">configuration options</param>
        /// <returns>Datatable representing the data</returns>
        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> queryResult, IEnumerableExtensionOptions configOptions)
        {
            DataTable dtReturn = new DataTable("Result");

            if (queryResult == null)
                return dtReturn;

            // columns 
			PropertyInfo[]? properties = null; 
			FieldInfo[]? fields = null;
            if (configOptions.UsePropertiesForColumns)
                properties = typeof(T).GetProperties().RemoveUnneededColumns(configOptions);
            else
                fields = typeof(T).GetFields().RemoveUnneededColumns(configOptions);

            // Use reflection to get property names, to create table
			for (int i = 0; i < (configOptions.UsePropertiesForColumns ? properties!.Length : fields!.Length); i++)
			{
				Type colType;
				if (configOptions.UsePropertiesForColumns)
					colType = properties![i].PropertyType;
				else
					colType = fields![i].FieldType;

				if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
				{
					colType = colType.GetGenericArguments()[0];
				}

				String fieldName;
				if (configOptions.UsePropertiesForColumns)
					fieldName = properties![i].Name;
				else
					fieldName = fields![i].Name;
				
				fieldName = GetFieldName(fieldName, configOptions);

				dtReturn.Columns.Add(new DataColumn(fieldName, colType));
			}

            foreach (T rec in queryResult)
            {
                DataRow dr = dtReturn.NewRow();

				for (int i = 0; i < (configOptions.UsePropertiesForColumns ? properties!.Length : fields!.Length); i++)
				{
					String fieldName;
					if (configOptions.UsePropertiesForColumns)
						fieldName = properties![i].Name;
					else
						fieldName = fields![i].Name;

                    fieldName = GetFieldName(fieldName, configOptions);

                    if (configOptions.UsePropertiesForColumns)
						dr[fieldName] = properties![i].GetValue(rec, null) == null ? DBNull.Value : properties[i].GetValue(rec, null);
					else
						dr[fieldName] = fields![i].GetValue(rec) == null ? DBNull.Value : fields[i].GetValue(rec);
                }

                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }

        /// <summary>
        /// Used to modify properties of an object returned from a LINQ query
        /// </summary>
        public static TSource Set<TSource>(this TSource input, Action<TSource> updater)
        {
            updater(input);
            return input;
        }

        #region Helpers

        private static T[] RemoveUnneededColumns<T>(this T[] columns, IEnumerableExtensionOptions configOptions) where T: MemberInfo
        {
            if (configOptions.ExcludeList != null)
                columns = columns.Where(p => !configOptions.ExcludeList.Contains(p.Name)).ToArray();

            if (configOptions.IncludeList != null)
                columns = columns.Where(p => configOptions.IncludeList.Contains(p.Name)).ToArray();

            return columns;
        }

        private static string GetFieldName(string fieldName, IEnumerableExtensionOptions configOptions)
        {
            if (configOptions.FieldNameChange != null && configOptions.FieldNameChange.ContainsKey(fieldName))
                fieldName = configOptions.FieldNameChange[fieldName];
            return fieldName;
        }

        #endregion
    }

    public class IEnumerableExtensionOptions
    {
        /// <summary>
        /// True:  uses Properties for column definitions.  False:  uses Fields for column definitions.
        /// </summary>
        public bool UsePropertiesForColumns { get; set; }

        /// <summary>
        /// if True, null values will be filled with "Null", false will be empty
        /// </summary>
        public bool FillInNulls { get; set; }

        /// <summary>
        /// Properties/Fields to exclude.  Looks for exact name match.  ExcludeList process before IncludeList
        /// </summary>
        public List<string>? ExcludeList { get; set; }

        /// <summary>
        /// Properties/Fields to include.  Looks for exact name match.  ExcludeList process before IncludeList
        /// </summary>
        public List<string>? IncludeList { get; set; }

        /// <summary>
        /// Takes the key property/field name and changes it to the value
        /// </summary>
        public Dictionary<string, string>? FieldNameChange { get; set; }

        /// <summary>
        /// Show header row as first column.  Defaults to true
        /// </summary>
        public bool ShowHeaderRow { get; set; } = true;
    }
}
