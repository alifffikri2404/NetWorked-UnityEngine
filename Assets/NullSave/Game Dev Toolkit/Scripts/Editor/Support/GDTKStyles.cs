using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    public class GDTKStyles
    {

        #region Fields

        private readonly Color proColor = new Color(0.9f, 0.9f, 0.9f, 1);
        private readonly Color freeColor = new Color(0.1f, 0.1f, 0.1f, 1);

        private Texture2D expandedIcon, collapsedIcon;
        private GUIStyle sectionHeader, toolHeader, footer, subHeader, errorText, wrappedText, titleText, slimBox;
        private GUIStyle btn, btnPressed, btnLA;
        private GUIStyle btnLeft, btnLeftPressed;
        private GUIStyle btnMid, btnMidPressed, btnMidCenter;
        private GUIStyle btnRight, btnRightPressed;
        private GUIStyle subItemBox, boxSub, redline, leftAlignedLabel, rightAlignedLabel, divider;
        private GUIStyle listItem, listItemHover, listItemText, listItemTextHover, detailsPanel, description;
        private GUIStyle listItemSmall, listItemHoverSmall, listItemTextSmall, listItemTextHoverSmall;

        #endregion

        #region Properties

        public GUIStyle BoxSub
        {
            get
            {
                return boxSub;
            }
        }

        public GUIStyle Button
        {
            get
            {
                return btn;
            }
        }

        public GUIStyle ButtonLeftAligned
        {
            get
            {
                return btnLA;
            }
        }

        public GUIStyle ButtonPressed
        {
            get
            {
                return btnPressed;
            }
        }

        public GUIStyle ButtonLeft
        {
            get
            {
                return btnLeft;
            }
        }

        public GUIStyle ButtonLeftPressed
        {
            get
            {
                return btnLeftPressed;
            }
        }

        public GUIStyle ButtonMid
        {
            get
            {
                return btnMid;
            }
        }

        public GUIStyle ButtonMidCenter
        {
            get
            {
                return btnMidCenter;
            }
        }

        public GUIStyle ButtonMidPressed
        {
            get
            {
                return btnMidPressed;
            }
        }

        public GUIStyle ButtonRight
        {
            get
            {
                return btnRight;
            }
        }

        public GUIStyle ButtonRightPressed
        {
            get
            {
                return btnRightPressed;
            }
        }

        public Texture2D CollapsedIcon
        {
            get
            {
                return collapsedIcon;
            }
        }

        public GUIStyle DescriptionStyle
        {
            get
            {
                return description;
            }
        }

        public GUIStyle DetailsPanel
        {
            get
            {
                return detailsPanel;
            }
        }

        public GUIStyle Divider
        {
            get
            {
                return divider;
            }
        }

        public Color EditorColor
        {
            get
            {
                if (EditorGUIUtility.isProSkin) return proColor;
                return freeColor;
            }
        }

        public Color EditorInvertedColor
        {
            get
            {
                if (!EditorGUIUtility.isProSkin) return proColor;
                return freeColor;
            }
        }

        public GUIStyle ErrorTextStyle
        {
            get
            {
                return errorText;
            }
        }

        public Texture2D ExpandedIcon
        {
            get
            {
                return expandedIcon;
            }
        }

        public GUIStyle FooterStyle
        {
            get
            {
                return footer;
            }
        }

        public GUIStyle LabelLeftAligned
        {
            get
            {
                return leftAlignedLabel;
            }
        }

        public GUIStyle LabelRightAligned
        {
            get
            {
                return rightAlignedLabel;
            }
        }

        public GUIStyle ListItem
        {
            get
            {
                return listItem;
            }
        }

        public GUIStyle ListItemHover
        {
            get
            {
                return listItemHover;
            }
        }

        public GUIStyle ListItemText
        {
            get
            {
                return listItemText;
            }
        }

        public GUIStyle ListItemTextHover
        {
            get
            {
                return listItemTextHover;
            }
        }

        public GUIStyle Redline
        {
            get
            {
                return redline;
            }
        }

        public GUIStyle SectionHeaderStyle
        {
            get
            {
                return sectionHeader;
            }
        }

        public bool ShowNavigation { get; set; }

        public GUIStyle SlimBox
        {
            get
            {
                return slimBox;
            }
        }

        public GUIStyle SubSectionBox
        {
            get
            {
                return subItemBox;
            }
        }

        public GUIStyle SmallListItem
        {
            get
            {
                return listItemSmall;
            }
        }

        public GUIStyle SmallListItemHover
        {
            get
            {
                return listItemHoverSmall;
            }
        }

        public GUIStyle SmallListItemText
        {
            get
            {
                return listItemTextSmall;
            }
        }

        public GUIStyle SmallListItemTextHover
        {
            get
            {
                return listItemTextHoverSmall;
            }
        }

        public GUIStyle SubHeaderStyle
        {
            get
            {
                return subHeader;
            }
        }

        public GUIStyle TitleTextStyle
        {
            get
            {
                return titleText;
            }
        }

        public GUIStyle ToolHeaderStyle
        {
            get
            {
                return toolHeader;
            }
        }

        public GUIStyle WrappedTextStyle
        {
            get
            {
                return wrappedText;
            }
        }

        #endregion

        #region Constructor

        public GDTKStyles()
        {
            boxSub = new GUIStyle(GUI.skin.box);
            boxSub.normal.textColor = Color.white;
            boxSub.padding = new RectOffset(0, 0, 6, 0);
            boxSub.normal.background = MakeTex(19, 19, new Color(0.85f, 0.85f, 0.85f));

            slimBox = new GUIStyle(GUI.skin.box);
            slimBox.padding = new RectOffset(0, 0, 0, 0);
            slimBox.margin = new RectOffset(0, 0, 0, 0);

            btn = new GUIStyle("Button");
            btn.alignment = TextAnchor.MiddleLeft;

            btnPressed = new GUIStyle("Button");
            btnPressed.normal = CopyStateWithFix(btnPressed.active);
            btnPressed.alignment = TextAnchor.MiddleLeft;

            btnLeft = new GUIStyle("ButtonLeft");
            btnLeft.alignment = TextAnchor.MiddleLeft;

            btnLeftPressed = new GUIStyle("ButtonLeft");
            btnLeftPressed.normal = CopyStateWithFix(btnLeftPressed.active);
            btnLeftPressed.alignment = TextAnchor.MiddleLeft;

            btnMid = new GUIStyle("ButtonMid");
            btnMid.alignment = TextAnchor.MiddleLeft;

            btnMidCenter = new GUIStyle("ButtonMid");
            btnMidCenter.fontSize = 10;

            btnMidPressed = new GUIStyle("ButtonMid");
            btnMidPressed.normal = CopyStateWithFix(btnMidPressed.active);
            btnMidPressed.alignment = TextAnchor.MiddleLeft;

            btnRight = new GUIStyle("ButtonRight");
            btnRight.alignment = TextAnchor.MiddleLeft;

            btnRightPressed = new GUIStyle("ButtonRight");
            btnRightPressed.normal = CopyStateWithFix(btnRightPressed.active);
            btnRightPressed.alignment = TextAnchor.MiddleLeft;

            collapsedIcon = (Texture2D)Resources.Load("Icons/collapsed");

            description = new GUIStyle(GUI.skin.label);
            description.wordWrap = true;

            detailsPanel = new GUIStyle(GUI.skin.box);
            detailsPanel.normal.textColor = Color.white;
            detailsPanel.padding = new RectOffset(0, 0, 6, 0);
            detailsPanel.margin = new RectOffset(0, 0, 0, 0);
            detailsPanel.normal.background = MakeTex(19, 19, new Color(0, 0, 0, 0.05f));
            detailsPanel.normal.scaledBackgrounds = null;

            errorText = new GUIStyle(GUI.skin.label);
            errorText.normal.textColor = new Color(1, 0.145f, 0.145f);
            errorText.wordWrap = true;

            expandedIcon = (Texture2D)Resources.Load("Icons/expanded");

            footer = new GUIStyle(GUI.skin.label);
            footer.fontSize = 10;

            leftAlignedLabel = new GUIStyle(EditorStyles.label);
            leftAlignedLabel.clipping = TextClipping.Clip;

            rightAlignedLabel = new GUIStyle(EditorStyles.label);
            rightAlignedLabel.alignment = TextAnchor.MiddleRight;
            rightAlignedLabel.clipping = TextClipping.Clip;

            listItem = new GUIStyle(GUI.skin.box);
            listItem.normal.textColor = Color.white;
            listItem.padding = new RectOffset(6, 6, 0, 0);
            listItem.margin = new RectOffset(0, 0, 0, 0);
            listItem.normal.background = MakeTex(19, 19, EditorGUIUtility.isProSkin ? new Color(.22f, .22f, .22f) : new Color(0.761f, 0.761f, 0.761f));
            listItem.normal.scaledBackgrounds = new Texture2D[] { ListItem.normal.background };
            listItem.alignment = TextAnchor.MiddleLeft;

            listItemHover = new GUIStyle(GUI.skin.box);
            listItemHover.normal.textColor = Color.white;
            listItemHover.padding = new RectOffset(6, 6, 0, 0);
            listItemHover.margin = new RectOffset(0, 0, 0, 0);
            listItemHover.normal.background = MakeTex(19, 19, new Color(0.063f, 0.604f, 0.847f));
            listItemHover.normal.scaledBackgrounds = new Texture2D[] { ListItemHover.normal.background };
            ListItemHover.alignment = TextAnchor.MiddleLeft;

            listItemText = new GUIStyle(GUI.skin.label);
            listItemText.padding = new RectOffset(6, 6, 0, 0);
            listItemText.fontSize = 14;
            listItemText.alignment = TextAnchor.MiddleLeft;
            listItemText.clipping = TextClipping.Clip;

            listItemTextHover = new GUIStyle(GUI.skin.label);
            listItemTextHover.padding = new RectOffset(6, 6, 0, 0);
            listItemTextHover.fontSize = 14;
            listItemTextHover.normal.textColor = Color.white;
            listItemTextHover.alignment = TextAnchor.MiddleLeft;
            listItemTextHover.clipping = TextClipping.Clip;

            redline = new GUIStyle(GUI.skin.label);
            redline.normal.textColor = Color.white;
            redline.normal.background = MakeTex(19, 19, new Color(0.992f, 0.427f, 0.251f, 1));
            redline.normal.scaledBackgrounds = new Texture2D[] { redline.normal.background };

            divider = new GUIStyle(GUI.skin.label);
            divider.normal.textColor = Color.white;
            divider.padding = new RectOffset(0, 0, 0, 0);
            divider.margin = new RectOffset(0, 0, 0, 0);
            divider.fixedWidth = 2;
            divider.normal.background = MakeTex(19, 19, new Color(0, 0, 0, 0.5f));

            sectionHeader = new GUIStyle(GUI.skin.label);
            sectionHeader.fontSize = 14;

            subItemBox = new GUIStyle(GUI.skin.box);
            subItemBox.normal.background = MakeTex(19, 19, !EditorGUIUtility.isProSkin ? new Color(0, 0, 0, 0.1f) : new Color(1, 1, 1, 0.1f));
            subItemBox.normal.scaledBackgrounds = new Texture2D[] { subItemBox.normal.background };
            subItemBox.margin = new RectOffset(4, 4, 4, 4);

            listItemSmall = new GUIStyle(GUI.skin.box);
            listItemSmall.normal.textColor = Color.white;
            listItemSmall.padding = new RectOffset(0, 0, 0, 0);
            listItemSmall.margin = new RectOffset(0, 0, 0, 0);
            listItemSmall.normal.background = MakeTex(19, 19, EditorGUIUtility.isProSkin ? new Color(.22f, .22f, .22f) : new Color(0.761f, 0.761f, 0.761f));
            listItemSmall.normal.scaledBackgrounds = new Texture2D[] { listItemSmall.normal.background };

            listItemHoverSmall = new GUIStyle(GUI.skin.box);
            listItemHoverSmall.normal.textColor = Color.white;
            listItemHoverSmall.padding = new RectOffset(0, 0, 0, 0);
            listItemHoverSmall.margin = new RectOffset(0, 0, 0, 0);
            listItemHoverSmall.normal.background = MakeTex(19, 19, new Color(0.063f, 0.604f, 0.847f));
            listItemHoverSmall.normal.scaledBackgrounds = new Texture2D[] { listItemHoverSmall.normal.background };

            listItemTextSmall = new GUIStyle(GUI.skin.label);
            listItemTextSmall.fontSize = 12;
            listItemTextSmall.alignment = TextAnchor.MiddleLeft;

            listItemTextHoverSmall = new GUIStyle(GUI.skin.label);
            listItemTextHoverSmall.fontSize = 12;
            listItemTextHoverSmall.normal.textColor = Color.white;
            listItemTextHoverSmall.alignment = TextAnchor.MiddleLeft;

            subHeader = new GUIStyle(GUI.skin.label);
            subHeader.fontSize = 11;
            subHeader.fontStyle = FontStyle.Bold;

            titleText = new GUIStyle(GUI.skin.label);
            titleText.clipping = TextClipping.Clip;
            titleText.wordWrap = true;
            titleText.alignment = TextAnchor.UpperLeft;

            toolHeader = new GUIStyle(GUI.skin.label);
            toolHeader.fontSize = 18;

            wrappedText = new GUIStyle(GUI.skin.label);
            wrappedText.wordWrap = true;
        }

        #endregion

        #region Private Methods

        private GUIStyleState CopyStateWithFix(GUIStyleState source)
        {
            GUIStyleState result = source;
            if (result.scaledBackgrounds != null && result.scaledBackgrounds.Length > 0)
            {
                result.background = result.scaledBackgrounds[0];
            }
            return result;
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = col;
            }

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        #endregion

    }
}