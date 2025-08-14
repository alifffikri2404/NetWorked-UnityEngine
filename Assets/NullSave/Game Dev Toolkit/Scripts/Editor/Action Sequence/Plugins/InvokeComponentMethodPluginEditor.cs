using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomUniversalPluginEditor(typeof(InvokeComponentMethodPlugin))]
    public class InvokeComponentMethodPluginEditor : UniversalPluginEditor
    {

        #region Structures

        private struct MemberTypeLookup
        {
            public MemberInfo memberInfo;
            public Type valueType;
        }

        #endregion

        #region Fields

        private List<Type> components;
        private string[] componentOptions;
        private int componentIndex;

        private string[] methodOptions;
        private int methodIndex;

        private string[] signatureNames;
        private string[] signatureOptions;
        private int signatureIndex;

        private ParamData[] parameters;

        private object checkResult;
        private bool hasParamChanged;
        private string error;

        #endregion

        #region Unity Methods

        public override void OnEnable()
        {
            string searchFor;

            components = new List<Type>();
            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsSubclassOf(typeof(Component))))
            {
                components.Add(type);
            }

            components = components.OrderBy(x => x.Name).ToList();
            componentOptions = new string[components.Count];
            for (int i = 0; i < components.Count; i++)
            {
                componentOptions[i] = components[i].Name;
            }

            componentIndex = -1;
            searchFor = PropertyStringValue("componentType");
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].FullName == searchFor)
                {
                    componentIndex = i;
                    break;
                }
            }

            if (componentIndex >= 0)
            {
                UpdateMethods();

                methodIndex = -1;
                searchFor = PropertyStringValue("methodName");
                for (int i = 0; i < methodOptions.Length; i++)
                {
                    if (methodOptions[i] == searchFor)
                    {
                        methodIndex = i;
                        break;
                    }
                }

                signatureIndex = -1;
                if (methodIndex >= 0)
                {
                    UpdateSignatures(false);
                    searchFor = PropertyStringValue("methodSignature");
                    for (int i = 0; i < signatureNames.Length; i++)
                    {
                        if (signatureNames[i] == searchFor)
                        {
                            signatureIndex = i;
                            UpdateParameters();
                            LoadParamData();
                            break;
                        }
                    }
                }

            }
        }

        public override void OnInspectorGUI()
        {
            PropertyField("useRemoteTarget");

            hasParamChanged = false;
            GDTKEditor.SearchableList("Component", componentOptions, componentIndex, ChangeComponentIndex);
            if (methodOptions != null)
            {
                GDTKEditor.SearchableList("Method", methodOptions, methodIndex, ChangeMethodIndex);
                if (methodIndex >= 0)
                {
                    GDTKEditor.SearchableList("Signature", signatureOptions, signatureIndex, ChangeSignatureIndex);
                    if (parameters != null && parameters.Length > 0)
                    {
                        GUILayout.Space(8);
                        GUILayout.Label("Parameters", SubHeaderStyle);
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (parameters[i] != null)
                            {
                                checkResult = ObjectField(parameters[i].value, parameters[i].type, parameters[i].paramName);
                                if ((checkResult == null && parameters[i].value != null) || (checkResult != null && !checkResult.Equals(parameters[i].value)))
                                {
                                    parameters[i].value = checkResult;
                                    parameters[i].paramValue = checkResult == null ? null : checkResult.ToString();
                                    parameters[i].paramJson = ActionSequenceHelper.ToJson(parameters[i].value, out error);
                                    parameters[i].objectError = error;
                                    hasParamChanged = true;
                                }
                                if (!string.IsNullOrEmpty(parameters[i].objectError))
                                {
                                    GUILayout.Label(parameters[i].objectError, GDTKEditor.Styles.ErrorTextStyle);
                                }
                            }
                        }
                        if (hasParamChanged)
                        {
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                if (i > 0) sb.Append((char)1);
                                sb.Append(SimpleJson.ToJSON(parameters[i]));
                            }
                            PropertyStringValue("parameterJson", sb.ToString());
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void ChangeComponentIndex(int index)
        {
            if (componentIndex == index) return;
            PropertyStringValue("componentType", components[index].FullName);
            componentIndex = index;
            methodIndex = -1;
            UpdateMethods();
        }

        private void ChangeMethodIndex(int index)
        {
            if (methodIndex == index) return;
            PropertyStringValue("methodName", methodOptions[index]);
            methodIndex = index;
            UpdateSignatures(true);
        }

        private void ChangeSignatureIndex(int index)
        {
            if (signatureIndex == index) return;
            PropertyStringValue("methodSignature", signatureNames[index]);
            signatureIndex = index;
            UpdateParameters();
        }

        private void LoadParamData()
        {
            try
            {
                string json = PropertyStringValue("parameterJson");
                if (!string.IsNullOrEmpty(json))
                {
                    string[] parts = json.Split((char)1);
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(parts[i]))
                        {
                            ParamData pd = SimpleJson.FromJSON<ParamData>(parts[i]);
                            parameters[i].paramJson = pd.paramJson;
                            parameters[i].paramValue = pd.paramValue;
                            parameters[i].value = ActionSequenceHelper.GetValue(pd.paramValue, pd.paramJson, parameters[i].type);
                        }
                    }
                }
            }
            catch
            {
                StringExtensions.LogError("InvokeComponentMethodPlugin", "LoadParamData", "Invalid Data");
            }
        }

        private void UpdateMethods()
        {
            List<string> availableMethods = new List<string>();

            MethodInfo[] methods = components[componentIndex].GetMethods();
            foreach (MethodInfo method in methods)
            {
                if (method.IsPublic && !method.IsSpecialName)
                {
                    if (!availableMethods.Contains(method.Name))
                    {
                        availableMethods.Add(method.Name);
                    }
                }
            }

            availableMethods.Sort();
            methodOptions = availableMethods.ToArray();
        }

        private void UpdateParameters()
        {
            MethodInfo method = (MethodInfo)components[componentIndex].GetMember(methodOptions[methodIndex])[signatureIndex];
            ParameterInfo[] pi = method.GetParameters();

            parameters = new ParamData[pi.Length];
            for (int i = 0; i < pi.Length; i++)
            {
                ParamData pd = new ParamData()
                {
                    paramName = pi[i].Name,
                    type = pi[i].ParameterType,
                    paramType = pi[i].ParameterType.ToString(),
                };

                pd.value = ActionSequenceHelper.GetValue(pd.paramValue, pd.paramJson, pd.type);

                if (pd.value == null)
                {
                    pd.paramValue = null;
                    pd.paramJson = null;
                }
                else
                {
                    pd.paramValue = pd.value.ToString();
                    pd.paramJson = SimpleJson.ToJSON(pd.value);
                }
                parameters[i] = pd;
            }


        }

        private void UpdateSignatures(bool autoSetId)
        {
            MethodInfo info;
            ParameterInfo[] pi;
            string paramData;
            MemberInfo[] signatures = components[componentIndex].GetMember(methodOptions[methodIndex]);

            signatureOptions = new string[signatures.Length];
            signatureNames = new string[signatures.Length];

            for (int i = 0; i < signatures.Length; i++)
            {
                signatureNames[i] = signatures[i].ToString();
                info = (MethodInfo)signatures[i];
                pi = info.GetParameters();

                if (pi.Length == 0)
                {
                    signatureOptions[i] = "(No Parameters)";
                }
                else
                {
                    paramData = string.Empty;
                    for (int p = 0; p < pi.Length; p++)
                    {
                        if (p > 0) paramData += ", ";
                        paramData += pi[p].ParameterType.ToString() + " " + pi[p].Name;
                    }
                    signatureOptions[i] = paramData;
                }
            }

            if (autoSetId)
            {
                signatureIndex = -1;
                PropertyStringValue("parameterJson", string.Empty);
                ChangeSignatureIndex(0);
            }
        }

        #endregion

    }
}