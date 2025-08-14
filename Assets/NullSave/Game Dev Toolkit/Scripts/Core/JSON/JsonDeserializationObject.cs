using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NullSave.GDTK
{

    internal class JsonDeserializationObject
    {

        #region Fields

        private string fullJson;

        #endregion

        #region Constructor

        public JsonDeserializationObject(string json)
        {
            fullJson = json;
        }

        #endregion

        #region Public Methods

        public T Deserialize<T>()
        {
            return (T)Deserialize(typeof(T));
        }

        public object Deserialize(Type objectType)
        {
            object instance = Activator.CreateInstance(objectType);
            if (instance is null || instance.ToString() == "null")
            {
                throw new Exception("Cannot deserialize JSON to new instances of '" + objectType.Name + "'");
            }

            if (IsTypeSimple(objectType) || objectType.IsEnum)
            {
                BuildObject(fullJson, ref instance, objectType, 0);
            }
            else
            {
                if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    instance = BuildList(fullJson, objectType);
                }
                else if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    instance = BuildDictionary(fullJson, objectType);
                }
                else if (objectType.IsArray)
                {
                    instance = BuildArray(fullJson, objectType);
                }
                else
                {
                    BuildObject(fullJson, ref instance, objectType, 0);
                }
            }

            // Post deserialize
            MethodInfo[] methods = objectType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (MethodInfo info in methods)
            {
                if (info.GetCustomAttribute<JsonAfterDeserialization>() != null)
                {
                    info.Invoke(instance, null);
                }
            }

            return instance;
        }

        #endregion

        #region Private Methods

        private object BuildArray(string json, Type type)
        {
            List<object> list = new List<object>();

            int startIndex = 1;
            Type objType = type.GetElementType();

            ReadWhitespace(json, ref startIndex);
            while (startIndex < json.Length - 1)
            {
                list.Add(GetValueNoList(objType, json, ref startIndex));
                ReadWhitespace(json, ref startIndex);
                if (json[startIndex] == ',') startIndex++;
            }

            Array result = Array.CreateInstance(objType, new[] { list.Count });
            for (int i = 0; i < list.Count; i++)
            {
                result.SetValue(Convert.ChangeType(list[i], objType), i);
            }

            return result;
        }

        private object BuildDictionary(string json, Type type)
        {
            IDictionary instance = (IDictionary)Activator.CreateInstance(type);
            if (instance is null || instance.ToString() == "null")
            {
                throw new Exception("Cannot deserialize JSON to new instances of '" + type.Name + "'");
            }

            Type itemType = type.GenericTypeArguments[1];
            int startIndex = 1;
            Tuple<string, object> keyValue;

            while (startIndex < json.Length - 2)
            {
                keyValue = ReadKeyAndValue(json, itemType, ref startIndex);
                instance.Add(keyValue.Item1, keyValue.Item2);
                ReadWhitespace(json, ref startIndex);
                if (json[startIndex] == ',') startIndex++;
            }

            return instance;
        }

        private object BuildList(string json, Type type)
        {
            IList instance = (IList)Activator.CreateInstance(type);
            if (instance is null || instance.ToString() == "null")
            {
                throw new Exception("Cannot deserialize JSON to new instances of '" + type.Name + "'");
            }

            Type itemType = type.GenericTypeArguments[0];
            int startIndex = 1;

            ReadWhitespace(json, ref startIndex);
            while (startIndex < json.Length - 2)
            {
                instance.Add(GetValueNoList(itemType, json, ref startIndex));
                ReadWhitespace(json, ref startIndex);
                if (json[startIndex] == ',') startIndex++;
            }

            return instance;
        }

        private void BuildObject(string json, ref object obj, Type type, int startIndex)
        {
            if (json == "null" || json.Length == 0) return;

            List<FieldInfo> fields = GetAppropriateFields(type, obj);
            List<PropertyInfo> properties = GetAppropriateProperties(type, obj);

            ReadWhitespace(json, ref startIndex);
            if (json[startIndex] != '{' && json[startIndex] != '[')
            {
                throw new Exception("Invalid object");
            }

            // Assign object values
            bool objectClosed = false;
            FieldInfo field;
            PropertyInfo property;
            while (!objectClosed && startIndex < json.Length)
            {
                switch (json[startIndex])
                {
                    case ' ':   // Whitespace
                    case '\t':
                    case '\r':
                    case '\n':
                        startIndex++;
                        break;
                    case '{':   // New object start
                        startIndex++;
                        break;
                    case '[':   // New array start
                        startIndex++;
                        break;
                    case '"':   // Literal start
                        // Get Key
                        string literal = ReadKeyName(json, ref startIndex);

                        // Read separator
                        ReadSeparatorAndWhitespace(json, ref startIndex);

                        // Read value
                        field = GetMatchingField(fields, literal);
                        if (field != null)
                        {
                            field.SetValue(obj, GetValue(field.FieldType, json, ref startIndex));
                        }
                        else
                        {
                            property = GetMatchingProperty(properties, literal);
                            if (property != null)
                            {
                                property.SetValue(obj, GetValue(property.PropertyType, json, ref startIndex));
                            }
                            else
                            {
                                throw new Exception("Object does not have a field or property named '" + literal + "'");
                            }
                        }

                        break;
                    case ']':
                        startIndex++;
                        break;
                    case '}':
                        objectClosed = true;
                        startIndex++;
                        break;
                    default:
                        throw new Exception("Unexpected character '" + json[startIndex] + "' and " + startIndex);
                }
            }

            if (!objectClosed)
            {
                throw new Exception("Unexpected end of file");
            }
        }

        private int FindFieldEnd(string json, int startIndex)
        {
            bool inQuotes = false;
            while (startIndex < json.Length)
            {
                switch (json[startIndex])
                {
                    case '"':
                        if (startIndex == 0 || json[startIndex - 1] != '\\')
                        {
                            inQuotes = !inQuotes;
                        }
                        break;
                    case ',':
                    case ']':
                    case '}':
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        if (!inQuotes)
                        {
                            return startIndex;
                        }
                        break;
                }
                startIndex++;
            }

            throw new Exception("Could not find end of field");
        }

        private List<FieldInfo> GetAppropriateFields(Type type, object obj)
        {
            List<FieldInfo> result = new List<FieldInfo>();
            if (obj == null) return result;

            FieldInfo[] fi = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            bool forceExclude, forceInclude;
            foreach (FieldInfo field in fi)
            {
                forceExclude = field.Name[0] == '<' || field.GetCustomAttribute<JsonDoNotSerialize>() != null;

                if (!forceExclude)
                {
                    forceInclude = field.GetCustomAttribute<JsonSerialize>() != null || field.GetCustomAttribute<JsonSerializeAs>() != null;

                    if (forceInclude || (!field.IsInitOnly && !field.IsLiteral && !field.IsPrivate))
                    {
                        result.Add(field);
                    }
                }
            }

            return result;
        }

        private List<PropertyInfo> GetAppropriateProperties(Type type, object obj)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            if (obj == null)
            {
                return result;
            }

            PropertyInfo[] pi = type.GetProperties();
            bool forceExclude;
            foreach (PropertyInfo property in pi)
            {
                forceExclude = property.Name[0] == '<';
                if (!forceExclude)
                {
                    foreach (var attrib in property.CustomAttributes)
                    {
                        if (attrib.AttributeType == typeof(JsonDoNotSerialize))
                        {
                            forceExclude = true;
                            break;
                        }
                    }
                    if (!forceExclude)
                    {
                        try
                        {
                            if (property.CanRead && property.CanWrite)
                            {
                                result.Add(property);
                            }
                        }
                        catch { }
                    }
                }
            }

            return result;
        }

        private FieldInfo GetMatchingField(List<FieldInfo> fields, string name)
        {
            foreach (FieldInfo info in fields)
            {
                JsonSerializeAs serializeAs = (JsonSerializeAs)info.GetCustomAttribute(typeof(JsonSerializeAs));
                if (serializeAs != null && serializeAs.SerializeName == name)
                {
                    return info;
                }
                else if (info.Name == name)
                {
                    return info;
                }
            }
            return null;
        }

        private PropertyInfo GetMatchingProperty(List<PropertyInfo> properties, string name)
        {
            foreach (PropertyInfo info in properties)
            {
                JsonSerializeAs serializeAs = (JsonSerializeAs)info.GetCustomAttribute(typeof(JsonSerializeAs));
                if (serializeAs != null && serializeAs.SerializeName == name)
                {
                    return info;
                }
                else if (info.Name == name)
                {
                    return info;
                }
            }
            return null;
        }

        private object GetValue(Type type, string json, ref int startIndex)
        {
            if (type.IsEnum)
            {
                return Enum.Parse(type, ReadString(json, ref startIndex));
            }
            else if (type == typeof(string))
            {
                return ReadString(json, ref startIndex);
            }
            else if (type == typeof(bool))
            {
                return ReadBoolean(json, ref startIndex);
            }
            else if (IsTypeNumeric(type))
            {
                return ReadNumber(json, type, ref startIndex);
            }
            else
            {
                string localObj = ReadObject(json, ref startIndex);
                if (json[startIndex] == ',') startIndex++;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return BuildList(localObj, type);
                }
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    return BuildDictionary(localObj, type);
                }
                else if (type.IsArray)
                {
                    return BuildArray(localObj, type);
                }
                else
                {
                    JsonDeserializationObject jso = new JsonDeserializationObject(localObj);
                    return jso.Deserialize(type);
                }
            }
        }

        private object GetValueNoList(Type type, string json, ref int startIndex)
        {
            int orgStart = startIndex;
            if (type.IsEnum)
            {
                return Enum.Parse(type, ReadString(json, ref startIndex));
            }
            else if (type == typeof(string))
            {
                return ReadString(json, ref startIndex);
            }
            else if (type == typeof(bool))
            {
                return ReadBoolean(json, ref startIndex);
            }
            else if (IsTypeNumeric(type))
            {
                return ReadNumber(json, type, ref startIndex);
            }
            else
            {
                string localObj = ReadObject(json, ref startIndex);
                if(string.IsNullOrEmpty(localObj))
                {
                    return null;
                }
                JsonDeserializationObject jso = new JsonDeserializationObject(localObj);
                return jso.Deserialize(type);
            }
        }

        private bool IsTypeNumeric(Type type)
        {
            return type == typeof(decimal) || type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double) ||
                type == typeof(byte) || type == typeof(short) || type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort) ||
                type == typeof(sbyte) || type == typeof(decimal) || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64);
        }

        /// <summary>
        /// Check if a field can be directly serialized
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsTypeSimple(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || IsTypeNumeric(type);
        }

        private bool ReadBoolean(string json, ref int startIndex)
        {
            ReadWhitespace(json, ref startIndex);
            int e = FindFieldEnd(json, startIndex);

            string result = json.Substring(startIndex, e - startIndex);

            startIndex = e++;
            if (json[startIndex] == ',') startIndex++;

            switch (result)
            {
                case "false":
                case "FALSE":
                case "0":
                    return false;
                case "true":
                case "TRUE":
                case "1":
                case "-1":
                    return true;
                default:
                    throw new Exception("Invalid value for boolean object: " + result);
            }
        }

        private object ReadDictionaryObject(string json, Type type, ref int startIndex)
        {
            if (type.IsEnum)
            {
                return Enum.Parse(type, ReadString(json, ref startIndex));
            }
            else if (type == typeof(string))
            {
                return ReadString(json, ref startIndex);
            }
            else if (type == typeof(bool))
            {
                return ReadBoolean(json, ref startIndex);
            }
            else if (IsTypeNumeric(type))
            {
                return ReadNumber(json, type, ref startIndex);
            }
            else
            {
                string localObj = ReadObject(json, ref startIndex);
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return BuildList(localObj, type);
                }
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    return BuildDictionary(localObj, type);
                }
                else if (type.IsArray)
                {
                    return BuildArray(localObj, type);
                }
                else
                {
                    JsonDeserializationObject jso = new JsonDeserializationObject(localObj);
                    return jso.Deserialize(type);
                }
            }
        }

        private string ReadKeyName(string json, ref int startIndex)
        {
            int i = startIndex + 1;
            while (true)
            {
                startIndex++;
                switch (json[startIndex])
                {
                    case '"':
                        if (json[startIndex - 1] == '\\') continue;
                        startIndex++;
                        return json.Substring(i, startIndex - 1 - i);
                }
            }
        }

        private Tuple<string, object> ReadKeyAndValue(string json, Type type, ref int startIndex)
        {
            bool keyAssigned = false;
            bool valueAssigned = false;
            string key = null;
            object value = null;

            ReadWhitespace(json, ref startIndex);

            if (json[startIndex++] != '{')
            {
                throw new Exception("Missing '{'");
            }

            ReadWhitespace(json, ref startIndex);

            string v1 = ReadKeyName(json, ref startIndex);
            switch (v1)
            {
                case "key":
                    ReadSeparatorAndWhitespace(json, ref startIndex);
                    key = ReadString(json, ref startIndex);
                    keyAssigned = true;
                    break;
                case "value":
                    ReadSeparatorAndWhitespace(json, ref startIndex);
                    value = ReadDictionaryObject(json, type, ref startIndex);
                    valueAssigned = true;
                    break;
                default:
                    throw new Exception("Invalid key name '" + v1 + "'");
            }

            ReadWhitespace(json, ref startIndex);

            v1 = ReadKeyName(json, ref startIndex);
            switch (v1)
            {
                case "key":
                    if (keyAssigned)
                    {
                        throw new Exception("Duplicate key found");
                    }
                    ReadSeparatorAndWhitespace(json, ref startIndex);
                    key = ReadString(json, ref startIndex);
                    keyAssigned = true;
                    break;
                case "value":
                    if (valueAssigned)
                    {
                        throw new Exception("Duplicate value found");
                    }
                    ReadSeparatorAndWhitespace(json, ref startIndex);
                    value = ReadDictionaryObject(json, type, ref startIndex);
                    valueAssigned = true;
                    break;
                default:
                    throw new Exception("Invalid key name '" + v1 + "'");
            }

            ReadWhitespace(json, ref startIndex);

            if (json[startIndex++] != '}')
            {
                throw new Exception("Missing '}'");
            }

            if (json[startIndex] == ',') startIndex++;

            if (!keyAssigned) throw new Exception("No key assigned");
            if (!valueAssigned) throw new Exception("No value assigned");

            return new Tuple<string, object>(key, value);
        }

        private object ReadNumber(string json, Type type, ref int startIndex)
        {
            ReadWhitespace(json, ref startIndex);
            int e = FindFieldEnd(json, startIndex);
            string value = json.Substring(startIndex, e - startIndex);

            startIndex = e++;
            if (json[startIndex] == ',') startIndex++;

            return Convert.ChangeType(value, type);
        }

        private string ReadObject(string json, ref int startIndex)
        {
            ReadWhitespace(json, ref startIndex);

            int i = startIndex;
            int arrayCount = 0;
            int objectCount = 0;
            bool isObject = json[startIndex] == '{';
            bool isArray = json[startIndex] == '[';
            bool inQuotes = false;

            if (!isObject && !isArray)
            {
                return ReadString(json, ref startIndex);
            }

            while (startIndex < json.Length)
            {
                switch (json[startIndex])
                {
                    case '{':
                        if (!inQuotes)
                        {
                            objectCount += 1;
                        }
                        break;
                    case '[':
                        if (!inQuotes)
                        {
                            arrayCount += 1;
                        }
                        break;
                    case '}':
                        if (!inQuotes)
                        {
                            objectCount -= 1;
                            if (isObject && objectCount == 0)
                            {
                                startIndex++;
                                return json.Substring(i, startIndex - i);
                            }
                        }
                        break;
                    case ']':
                        if (!inQuotes)
                        {
                            arrayCount -= 1;
                            if (isArray && arrayCount == 0)
                            {
                                startIndex++;
                                return json.Substring(i, startIndex - i);
                            }
                        }
                        break;
                    case '\"':
                        if (json[startIndex - 1] != '\\')
                        {
                            inQuotes = !inQuotes;
                        }
                        break;
                    case ',':
                    case '\r':
                    case '\n':
                    case '\t':
                    case ' ':
                        if (!inQuotes)
                        {
                            if (isObject && objectCount == 0)
                            {
                                startIndex++;
                                return json.Substring(i, startIndex - 1 - i);
                            }
                            else if (isArray && arrayCount == 0)
                            {
                                startIndex++;
                                return json.Substring(i, startIndex - 1 - i);
                            }
                            else if (!isObject && !isArray)
                            {
                                // This is the end
                                if (arrayCount != 0)
                                {
                                    throw new Exception("Missing ']'");
                                }
                                if (objectCount != 0)
                                {
                                    throw new Exception("Missing '}'");
                                }
                                startIndex++;
                                return json.Substring(i, startIndex - 1 - i);
                            }
                        }
                        break;
                }

                startIndex++;
            }

            return json.Substring(i);
        }

        private void ReadSeparator(string json, ref int startIndex)
        {
            while (true)
            {
                switch (json[startIndex])
                {
                    case ':':
                        startIndex++;
                        return;
                    case ' ':
                    case '\r':
                    case '\n':
                    case '\t':
                        break;
                    default:
                        throw new Exception("Unexpected character at " + startIndex);
                }
                startIndex++;
            }
        }

        private void ReadSeparatorAndWhitespace(string json, ref int startIndex)
        {
            ReadSeparator(json, ref startIndex);
            ReadWhitespace(json, ref startIndex);
        }

        private string ReadString(string json, ref int startIndex)
        {
            ReadWhitespace(json, ref startIndex);
            int e = FindFieldEnd(json, startIndex);
            string value = json.Substring(startIndex, e - startIndex);

            startIndex = e++;
            if (json[startIndex] == ',') startIndex++;

            if (value.Length > 0 && value[0] == '"')
            {
                if (value[value.Length - 1] != '"')
                {
                    throw new Exception("String with unterminated quotes");
                }
                else
                {
                    value = value.Substring(1, value.Length - 2);
                }
            }

            return UnescapeString(value);
        }

        private void ReadWhitespace(string json, ref int startIndex)
        {
            if (startIndex >= json.Length) return;

            while (true)
            {
                switch (json[startIndex])
                {
                    case ' ':
                    case '\r':
                    case '\n':
                    case '\t':
                        break;
                    default:
                        return;
                }
                startIndex++;
            }
        }

        private string UnescapeString(string input)
        {
            StringBuilder unescaped = new StringBuilder(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];

                // Check for escape character \
                if (currentChar == '\\')
                {
                    i++; // Move to the next character after \
                    if (i >= input.Length)
                        break;

                    char escapeChar = input[i];
                    switch (escapeChar)
                    {
                        case '"':
                            unescaped.Append('"');
                            break;
                        case '\\':
                            unescaped.Append('\\');
                            break;
                        case '/':
                            unescaped.Append('/');
                            break;
                        case 'b':
                            unescaped.Append('\b');
                            break;
                        case 'f':
                            unescaped.Append('\f');
                            break;
                        case 'n':
                            unescaped.Append('\n');
                            break;
                        case 'r':
                            unescaped.Append('\r');
                            break;
                        case 't':
                            unescaped.Append('\t');
                            break;
                        case 'u':
                            // Handle Unicode escape sequence
                            if (i + 4 < input.Length && uint.TryParse(input.Substring(i + 1, 4), System.Globalization.NumberStyles.HexNumber, null, out uint unicodeValue))
                            {
                                unescaped.Append((char)unicodeValue);
                                i += 4;
                            }
                            else
                            {
                                // Invalid Unicode escape sequence, treat it as a regular 'u' character
                                unescaped.Append('u');
                            }
                            break;
                        default:
                            // Unrecognized escape sequence, ignore the escape character
                            unescaped.Append(escapeChar);
                            break;
                    }
                }
                else
                {
                    unescaped.Append(currentChar);
                }
            }

            return unescaped.ToString();
        }

        #endregion

    }

}