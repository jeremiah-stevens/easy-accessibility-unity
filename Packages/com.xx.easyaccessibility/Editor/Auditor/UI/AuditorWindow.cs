using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyAccessibility
{
    public class AuditorWindow : EditorWindow
    {
        [SerializeField]
        private int m_selectedIndex = -1;

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        VisualElement m_rightPane;
        ListView m_listView;

        AuditorRequirementsSO auditorRequirements;
        SerializedObject m_auditorRequirements;

        List<AuditorRequirement> m_selectedReqs = new();

        [MenuItem("Window/Easy Accessibility/Auditor")]
        public static void ShowWindow()
        {
            AuditorWindow wnd = GetWindow<AuditorWindow>();
            wnd.titleContent = new GUIContent("Accessibility Auditor");
        }

        public void CreateGUI()
        {
            if (!AccessibilityAuditor.HasExistingReport()) //TODO: consider setting up a view with no data and a button to perform audit instead of auto-performing
            {
                AccessibilityAuditor.Audit();
            }

            auditorRequirements = AssetDatabase.LoadAssetByGUID<AuditorRequirementsSO>(
                new UnityEditor.GUID(AssetDatabase.FindAssets("t:auditorrequirementsso")[0])
            );
            m_auditorRequirements = new SerializedObject(auditorRequirements);

            VisualElement root = rootVisualElement;
            root.Add(CreateToolbar());
            root.Add(CreateSplitView());

            m_listView.selectedIndex = m_selectedIndex;
        }

        private VisualElement CreateToolbar()
        {
            var toolbar = new Toolbar() { style = { justifyContent = Justify.FlexEnd } };

            toolbar.Add(
                new ToolbarButton(() => AccessibilityAuditor.Audit()) { text = "Audit Project" }
            );
            toolbar.Add(
                new ToolbarButton(() =>
                    Application.OpenURL(
                        "https://gitlab.com/a-la-code-group/easy-accessibility-unity"
                    )
                )
                {
                    iconImage = EditorGUIUtility.FindTexture("d__Help@2x"),
                }
            ); //TODO: change out icon for Gitlab repo
            //TODO: link to docs

            return toolbar;
        }

        private VisualElement CreateSplitView()
        {
            var splitView = new UnityEngine.UIElements.TwoPaneSplitView
            {
                fixedPaneIndex = 0,
                fixedPaneInitialDimension = 250f,
                orientation = TwoPaneSplitViewOrientation.Horizontal,
            };
            var leftPane = new VisualElement();
            leftPane.Add(CreateRequirementSearchOptions());
            m_listView = CreateLayoutListView();
            leftPane.Add(m_listView);
            m_rightPane = new VisualElement();
            splitView.Add(leftPane);
            splitView.Add(m_rightPane);

            return splitView;
        }

        private VisualElement CreateRequirementSearchOptions()
        {
            var output = new VisualElement();
            output.style.flexDirection = FlexDirection.Row;
            output.style.height = 24;
            output.style.minHeight = 24;

            var requirementSearchField = new ToolbarSearchField();
            requirementSearchField.style.width = 100;
            requirementSearchField.RegisterValueChangedCallback(OnSearchChanged);
            output.Add(requirementSearchField);

            //TODO: re-enable when we support toggle group for filtering options
            //var toggleButtons = new ToggleButtonGroup();
            //toggleButtons.isMultipleSelection = true;
            //toggleButtons.allowEmptySelection = true;

            //toggleButtons.Add(new Button { iconImage = EditorGUIUtility.FindTexture("TestPassed") });
            //toggleButtons.Add(new Button { iconImage = EditorGUIUtility.FindTexture("TestInconclusive") });
            //toggleButtons.Add(new Button { iconImage = EditorGUIUtility.FindTexture("TestFailed") });

            //output.Add(toggleButtons);

            return output;
        }

        private ListView CreateLayoutListView()
        {
            var listView = new ListView { bindingPath = "report.requirements" };

            listView.makeItem = () => new Label();
            listView.bindItem = (item, index) =>
            {
                var label = item as Label;
                string icon = "";
                switch (m_selectedReqs[index].status) //TODO: these should be icons, not text characters
                {
                    case AuditorRequirement.Status.Pass:
                        icon = "✓";
                        break;
                    case AuditorRequirement.Status.Fail:
                        icon = "x";
                        break;
                    default:
                        icon = "?";
                        break;
                }

                label.text = $"[{icon}] {m_selectedReqs[index].name}";
            };
            m_selectedReqs = auditorRequirements.report.requirements;
            listView.itemsSource = m_selectedReqs;
            listView.selectionChanged += OnRequirementSelected;

            listView.selectionChanged += (items) =>
            {
                m_selectedIndex = listView.selectedIndex;
            };

            return listView;
        }

        private VisualElement CreateLayoutMultiColumnListView(
            List<AuditorRequirement.Issue> issues = null
        )
        {
            var multiColumnListView = new MultiColumnListView
            {
                bindingPath = "issues",
                showBoundCollectionSize = false,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                dataSourceType = typeof(AuditorRequirement.Issue),
            };

            var assetCol = new Column
            {
                title = "Asset",
                width = 200,
                sortable = true,
            };
            assetCol.makeCell += MakeObjectCell;
            assetCol.bindCell += BindObjectCell;
            multiColumnListView.columns.Add(assetCol);

            var issueCol = new Column()
            {
                title = "Issue",
                width = 500,
                sortable = true,
            };
            issueCol.makeCell += MakeLabelCell;
            issueCol.bindCell += BindLabelCell;
            multiColumnListView.columns.Add(issueCol);

            multiColumnListView.itemsSource = issues;
            return multiColumnListView;
        }

        private void OnSearchChanged(ChangeEvent<string> evt)
        {
            if (string.IsNullOrEmpty(evt.newValue))
            {
                m_selectedReqs = auditorRequirements.report.requirements;
            }
            else
            {
                var list = auditorRequirements
                    .report.requirements.Where(item =>
                        item.name.ToLower()
                            .Contains(
                                evt.newValue.ToLower(),
                                StringComparison.InvariantCultureIgnoreCase
                            )
                    )
                    .ToList();
                m_selectedReqs = list;
            }
            m_listView.itemsSource = m_selectedReqs;
            m_listView.Rebuild();
        }

        private VisualElement MakeObjectCell()
        {
            var objectField = new ObjectField() { enabledSelf = false };
            return objectField;
        }

        private VisualElement MakeLabelCell()
        {
            var labelCell = new Label();
            return labelCell;
        }

        private void BindObjectCell(VisualElement visualElement, int index)
        {
            var objectField = (ObjectField)visualElement;
            objectField.bindingPath =
                $"report.requirements.Array.data[{m_selectedIndex}].issues.Array.data[{index}].asset";
            objectField.Bind(m_auditorRequirements);
        }

        private void BindLabelCell(VisualElement visualElement, int index)
        {
            var labelCell = (Label)visualElement;
            labelCell.bindingPath =
                $"report.requirements.Array.data[{m_selectedIndex}].issues.Array.data[{index}].issue";
            labelCell.Bind(m_auditorRequirements);
        }

        private void OnRequirementSelected(IEnumerable<object> selected)
        {
            m_rightPane.Clear();

            var enumerator = selected.GetEnumerator();
            if (enumerator.MoveNext())
            {
                var req = enumerator.Current as AuditorRequirement;
                if (req != null)
                {
                    var headerGroup = new VisualElement()
                    {
                        style =
                        {
                            flexDirection = FlexDirection.Row,
                            minHeight = 30, //TODO: code this to match with the header size and margin
                        },
                    };
                    headerGroup.Add(new Label() { text = req.name, style = { fontSize = 24 } });
                    headerGroup.Add(
                        new Button(() => Application.OpenURL(req.referenceLink))
                        {
                            text = "",
                            iconImage = EditorGUIUtility.FindTexture("d_Linked"),
                            style =
                            {
                                backgroundColor = new Color(0, 0, 0, 0),
                                borderTopWidth = 0,
                                borderBottomWidth = 0,
                                borderLeftWidth = 0,
                                borderRightWidth = 0,
                            },
                        }
                    );
                    m_rightPane.Add(headerGroup);

                    m_rightPane.Add(
                        new Label()
                        {
                            text = req.description,
                            style = { whiteSpace = WhiteSpace.PreWrap, marginBottom = 20 },
                        }
                    );
                    var view = CreateLayoutMultiColumnListView(req.issues);
                    m_rightPane.Add(view);
                }
            }
        }
    }
}
