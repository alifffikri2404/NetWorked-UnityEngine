using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{
    internal class JsonSerializationObject
    {

        #region Structures

        private struct JsonTargetData
        {

            #region Fields

            public Type type;
            public string serializeName;
            public object value;

            #endregion

            #region Constructors

            public JsonTargetData(Type type, object value)
            {
                this.type = type;
                this.value = value;
                serializeName = type.Name;
            }

            public JsonTargetData(string name, Type type, object value)
            {
                serializeName = name;
                this.type = type;
                this.value = value;
            }

            #endregion

        }

        #endregion

        #region Fields

        private Type objectType;
        private StringBuilder builder;
        private List<Tuple<string, string>> values;

        #endregion

        #region Properties

        public string value => builder.ToString();

        #endregion

        #region Constructor

        public JsonSerializationObject(object obj, bool removeNulls, bool readable, int indentLevel = 0)
        {
            builder = new StringBuilder();
            values = new List<Tuple<string, string>>();

            if (indentLevel >= 50)
            {
                Debug.LogWarning("Max depth reached");
                return;
            }

            if (obj == null)
            {
                builder.Append("null");
                return;
            }
            objectType = obj.GetType();

            // Pre-Serialization
            MethodInfo[] methods = objectType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (MethodInfo info in methods)
            {
                if (info.GetCustomAttribute<JsonBeforeSerialization>() != null)
                {
                    info.Invoke(obj, null);
                }
            }

            if (IsTypeSimple(objectType) || objectType.IsEnum)
            {
                BuildObject(obj, objectType, removeNulls, readable, indentLevel);
            }
            else
            {
                if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    BuildList(obj, removeNulls, readable, indentLevel);
                }
                else if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    BuildDictionary(obj, removeNulls, readable, indentLevel);
                }
                else if (objectType.IsArray)
                {
                    BuildArray(obj, removeNulls, readable, indentLevel);
                }
                else
                {
                    BuildObject(obj, objectType, removeNulls, readable, indentLevel);
                }
            }
        }

        #endregion

        #region Private Methods

        private void BuildArray(object obj, bool removeNulls, bool readable, int level)
        {
            Array list = (Array)obj;

            if (list.Length == 0)
            {
                builder.Append("[]");
                return;
            }

            Type type = list.GetValue(0).GetType();
            bool isSimple = IsTypeSimple(type) || type.IsEnum;
            if (!isSimple && readable)
            {
                builder.Append("\r\n");
                builder.Append('\t', level);
            }
            builder.Append("[");
            level++;
            for (int i = 0; i < list.Length; i++)
            {
                if (isSimple)
                {
                    if (i > 0) builder.Append(", ");
                    builder.Append(GetSimpleSerializedValue(list.GetValue(i)));
                }
                else
                {
                    if (i > 0) builder.Append(readable ? ", " : ",");

                    builder.Append(new JsonSerializationObject(list.GetValue(i), removeNulls, readable, level).value);
                }
            }
            level--;
            if (!isSimple && readable)
            {
                builder.Append("\r\n");
                builder.Append('\t', level);
            }
            builder.Append("]");
        }

        private void BuildDictionary(object obj, bool removeNulls, bool readable, int level)
        {
            IDictionary dictionary = (IDictionary)obj;
            if (dictionary.Count == 0)
            {
                builder.Append("[]");
                return;
            }

            Type type = null;
            bool isSimple = false;
            if (readable)
            {
                builder.Append("\r\n");
                builder.Append('\t', level);
            }
            builder.Append("[");
            level++;

            bool isFirst = true;
            foreach (DictionaryEntry entry in dictionary)
            {
                if (isFirst)
                {
                    isFirst = false;
                    type = entry.Value.GetType();
                    isSimple = IsTypeSimple(type) || type.IsEnum;
                }
                else
                {
                    builder.Append(readable ? ", " : ",");
                }

                if (readable)
                {
                    builder.Append("\r\n");
                    builder.Append('\t', level);
                }
                builder.Append("{");
                level++;
                if (readable)
                {
                    builder.Append("\r\n");
                    builder.Append('\t', level);
                }

                // Key
                builder.Append("\"key\":");
                if (readable) builder.Append(' ');
                builder.Append('"');
                builder.Append(entry.Key);
                builder.Append("\",");

                // Entry
                if (readable)
                {
                    builder.Append("\r\n");
                    builder.Append('\t', level);
                }

                builder.Append("\"value\":");
                if (readable) builder.Append(' ');
                if (isSimple)
                {
                    builder.Append(GetSimpleSerializedValue(entry.Value));
                }
                else
                {
                    builder.Append(new JsonSerializationObject(entry.Value, removeNulls, readable, level).value);
                }

                level--;
                if (readable)
                {
                    builder.Append("\r\n");
                    builder.Append('\t', level);
                }
                builder.Append("}");
            }

            level--;
            if (readable)
            {
                builder.Append("\r\n");
                builder.Append('\t', level);
            }
            builder.Append("]");
        }

        private void BuildList(object obj, bool removeNulls, bool readable, int level)
        {
            IList list = (IList)obj;

            if (list.Count == 0)
            {
                builder.Append("[]");
                return;
            }

            Type type = list[0].GetType();
            bool isSimple = IsTypeSimple(type) || type.IsEnum;
            if (!isSimple && readable)
            {
                builder.Append("\r\n");
                builder.Append('\t', level);
            }
            builder.Append("[");
            level++;
            for (int i = 0; i < list.Count; i++)
            {
                if (isSimple)
                {
                    if (i > 0) builder.Append(", ");
                    builder.Append(GetSimpleSerializedValue(list[i]));
                }
                else
                {
                    if (i > 0) builder.Append(readable ? ", " : ",");

                    builder.Append(new JsonSerializationObject(list[i], removeNulls, readable, level).value);
                }
            }
            level--;
            if (!isSimple && readable)
            {
                builder.Append("\r\n");
                builder.Append('\t', level);
            }
            builder.Append("]");
        }

        private void BuildObject(object obj, Type type, bool removeNulls, bool readable, int level)
        {
            Type entryType;
            builder = new StringBuilder();
            values = new List<Tuple<string, string>>();

            // Process fields
            List<JsonTargetData> fields = GetAppropriateFields(type, obj, removeNulls);
            foreach (JsonTargetData field in fields)
            {
                entryType = field.type;
                if (IsTypeSimple(entryType) || entryType.IsEnum)
                {
                    values.Add(new Tuple<string, string>(field.serializeName, GetSimpleSerializedValue(field.value)));
                }
                else
                {
                    JsonSerializationObject jsoField = new JsonSerializationObject(field.value, removeNulls, readable, level + 1);
                    values.Add(new Tuple<string, string>(field.serializeName, jsoField.value));
                }
            }

            // Process properties
            List<JsonTargetData> properties = GetAppropriateProperties(type, obj, removeNulls);
            foreach (JsonTargetData property in properties)
            {
                entryType = property.type;
                if (IsTypeSimple(entryType) || entryType.IsEnum)
                {
                    values.Add(new Tuple<string, string>(property.serializeName, GetSimpleSerializedValue(property.value)));
                }
                else
                {
                    JsonSerializationObject jsoField = new JsonSerializationObject(property.value, removeNulls, readable, level + 1);
                    values.Add(new Tuple<string, string>(property.serializeName, jsoField.value));
                }
            }

            // Build Result
            if (readable)
            {
                if (level > 0) builder.Append("\r\n");
                builder.Append('\t', level++);
                builder.Append('{');

                for (int i = 0; i < values.Count; i++)
                {
                    if (i > 0) builder.Append(",");
                    builder.Append("\r\n");
                    builder.Append('\t', level);
                    builder.Append('"');
                    builder.Append(values[i].Item1);
                    builder.Append("\": ");
                    builder.Append(values[i].Item2);
                }

                builder.Append("\r\n");
                level--;
                builder.Append('\t', level);
                builder.Append('}');
            }
            else
            {
                builder.Append('{');
                for (int i = 0; i < values.Count; i++)
                {
                    if (i > 0) builder.Append(",");
                    builder.Append('"');
                    builder.Append(values[i].Item1);
                    builder.Append("\":");
                    builder.Append(values[i].Item2);
                }
                builder.Append('}');
            }
        }

        private string EscapeString(string s)
        {
            if (s == null || s.Length == 0)
            {
                return "";
            }

            char c;
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);
            string t;

            for (i = 0; i < len; i += 1)
            {
                c = s[i];
                switch (c)
                {
                    case '\\':
                    case '"':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '/':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    default:
                        if (c < ' ')
                        {
                            t = "000" + string.Format("X", c);
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            return sb.ToString();
        }

        private List<JsonTargetData> GetAppropriateFields(Type type, object obj, bool removeNulls)
        {
            string useName;
            List<JsonTargetData> result = new List<JsonTargetData>();
            if (obj == null || IsTypeForbidden(type)) return result;
            object fieldValue;

            FieldInfo[] fi = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            bool forceExclude, forceInclude;
            foreach (FieldInfo field in fi)
            {
                forceExclude = field.Name[0] == '<' || IsTypeForbidden(field.FieldType) || field.GetCustomAttribute<JsonDoNotSerialize>() != null;

                if (!forceExclude)
                {
                    forceInclude = field.GetCustomAttribute<JsonSerialize>() != null || field.GetCustomAttribute<JsonSerializeAs>() != null;
                    if (forceInclude || (!field.IsInitOnly && !field.IsLiteral && !field.IsPrivate))
                    {
                        useName = GetUseName(field.Name, field.GetCustomAttribute(typeof(JsonSerializeAs)));

                        fieldValue = field.GetValue(obj);
                        if (fieldValue != null || !removeNulls)
                        {
                            result.Add(new JsonTargetData { serializeName = useName, type = field.FieldType, value = fieldValue });
                        }
                    }
                }
            }

            return result;
        }

        private List<JsonTargetData> GetAppropriateProperties(Type type, object obj, bool removeNulls)
        {
            string useName;
            List<JsonTargetData> result = new List<JsonTargetData>();
            if (obj == null || IsTypeForbidden(type)) return result;
            object propertyValue;

            PropertyInfo[] pi = type.GetProperties();
            bool forceExclude, forceInclude;
            foreach (PropertyInfo property in pi)
            {
                forceExclude = property.Name[0] == '<' || IsTypeForbidden(property.PropertyType) || property.GetCustomAttribute<JsonDoNotSerialize>() != null || property.GetIndexParameters().Length > 0;
                forceInclude = property.GetCustomAttribute<JsonSerialize>() != null || property.GetCustomAttribute<JsonSerializeAs>() != null;

                if (!forceExclude)
                {
                    if (forceInclude || (property.CanRead && property.GetSetMethod() != null))
                    {
                        useName = GetUseName(property.Name, property.GetCustomAttribute(typeof(JsonSerializeAs)));

                        propertyValue = type.GetProperty(property.Name).GetValue(obj);
                        if (propertyValue != null || !removeNulls)
                        {
                            result.Add(new JsonTargetData { serializeName = useName, type = property.PropertyType, value = propertyValue });
                        }
                    }
                }
            }

            return result;
        }

        private string GetSimpleSerializedValue(object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is bool)
            {
                return ((bool)value) ? "true" : "false";
            }

            if (value is string)
            {
                return "\"" + EscapeString((string)value) + "\"";
            }

            if (value is int || value is long || value is float || value is double || value is byte ||
                value is short || value is uint || value is ulong || value is ushort || value is sbyte ||
                value is decimal)
            {
                return value.ToString();
            }

            return "\"" + EscapeString(value.ToString()) + "\"";
        }

        private string GetUseName(string defaultName, Attribute attribute)
        {
            if (attribute == null) return defaultName;
            return ((JsonSerializeAs)attribute).SerializeName;
        }

        private bool IsTypeForbidden(Type type)
        {
            return
                type == typeof(SimpleEvent) ||
                type == typeof(UnityEvent) ||
                type == typeof(Sprite)
            ;
        }

        /// <summary>
        /// Check if a field can be directly serialized
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsTypeSimple(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal) ||
                type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double) || type == typeof(byte) ||
                type == typeof(short) || type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort) || type == typeof(sbyte) ||
                type == typeof(decimal) || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64);
        }

        #endregion

    }

}