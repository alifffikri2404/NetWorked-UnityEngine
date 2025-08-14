using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomUniversalPluginEditor(typeof(SetComponentValuePlugin))]
    public class SetComponentValuePluginEditor : UniversalPluginEditor
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

        private string[] memberOptions;
        private Type[] memberTypes;
        private int memberIndex;

        private object memberValue;
        private Type memberType;

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
                UpdateMembers(components[componentIndex]);

                memberIndex = -1;
                memberType = null;
                memberValue = null;
                searchFor = PropertyStringValue("memberName");
                for (int i = 0; i < memberOptions.Length; i++)
                {
                    if (memberOptions[i] == searchFor)
                    {
                        memberIndex = i;
                        memberType = memberTypes[i];
                        memberValue = ActionSequenceHelper.GetValue(PropertyStringValue("memberValue"), PropertyStringValue("memberJson"), memberType);
                        break;
                    }
                }
            }
        }

        public override void OnInspectorGUI()
        {
            error = string.Empty;

            PropertyField("useRemoteTarget");

            GDTKEditor.SearchableList("Component", componentOptions, componentIndex, ChangeComponentIndex);
            if (memberOptions != null)
            {
                GDTKEditor.SearchableList("Field", memberOptions, memberIndex, ChangeMemberIndex);
                if (memberType != null)
                {
                    object resultValue = ObjectField(memberValue, memberType, "Value");
                    if ((resultValue == null && memberValue != null) || (resultValue != null && !resultValue.Equals(memberValue)))
                    {
                        memberValue = resultValue;
                        PropertyStringValue("memberValue", memberValue.ToString());
                        PropertyStringValue("memberJson", ActionSequenceHelper.ToJson(memberValue, out error));
                        if (!string.IsNullOrEmpty(error))
                        {
                            GUILayout.Label(error, GDTKEditor.Styles.ErrorTextStyle);
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
            memberIndex = -1;
            UpdateMembers(components[componentIndex]);
        }

        private void ChangeMemberIndex(int index)
        {
            if (memberIndex == index) return;
            PropertyStringValue("memberName", memberOptions[index]);
            memberIndex = index;

            if (memberIndex == -1)
            {
                memberType = null;
                memberValue = null;
            }
            else
            {
                memberType = memberTypes[memberIndex];
                memberValue = null;
                memberValue = ActionSequenceHelper.GetValue(null, null, memberType);
            }
        }

        private void UpdateMembers(Type type)
        {
            List<MemberTypeLookup> accessableMembers = new List<MemberTypeLookup>();

            // build list
            MemberInfo[] members = type.GetMembers();
            foreach (MemberInfo member in members)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        FieldInfo fi = (FieldInfo)member;
                        if (fi.IsPublic)
                        {
                            accessableMembers.Add(new MemberTypeLookup { memberInfo = member, valueType = fi.FieldType });
                        }
                        break;
                    case MemberTypes.Property:
                        PropertyInfo pi = (PropertyInfo)member;
                        MethodInfo mi = pi.GetSetMethod();
                        if (mi != null && mi.IsPublic)
                        {
                            accessableMembers.Add(new MemberTypeLookup { memberInfo = member, valueType = pi.PropertyType });
                        }
                        break;
                }
            }

            // Set data
            accessableMembers = accessableMembers.OrderBy(x => x.memberInfo.Name).ToList();
            memberOptions = new string[accessableMembers.Count];
            memberTypes = new Type[accessableMembers.Count];
            for (int i = 0; i < accessableMembers.Count; i++)
            {
                memberOptions[i] = accessableMembers[i].memberInfo.Name;
                memberTypes[i] = accessableMembers[i].valueType;
            }
        }

        #endregion

    }
}