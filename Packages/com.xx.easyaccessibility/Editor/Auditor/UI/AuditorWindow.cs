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
        [SerializeField] private int m_selectedIndex = -1;

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        VisualElement m_rightPane;
        ListView m_listView;

        AuditorRequirementsSO auditorRequirements;
        SerializedObject m_auditorRequirements;

        [MenuItem("Window/Easy Accessibility/Auditor")]
        public static void ShowExample()
        {
            AuditorWindow wnd = GetWindow<AuditorWindow>();
            wnd.titleContent = new GUIContent("Auditor");
        }


        [MenuItem("Window/Easy Accessibility/Audit")]
        public static void Audit()
        {
            var baseType = typeof(AuditorRequirement);
            var assembly = typeof(AuditorRequirement).Assembly;
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
            
            var auditorRequirements = AssetDatabase.LoadAssetByGUID<AuditorRequirementsSO>(new UnityEditor.GUID(AssetDatabase.FindAssets("t:auditorrequirementsso")[0]));
            auditorRequirements.report = new();

            int issueCount = 0;

            foreach (var type in types)
            {
                var instance = (AuditorRequirement)Activator.CreateInstance(type);
                instance.Audit();
                auditorRequirements.report.requirements.Add(instance);
                issueCount += instance.issues.Count;
            }

            Debug.Log($"Finished! Found '{issueCount}' issues");

            EditorUtility.SetDirty(auditorRequirements);
            AssetDatabase.SaveAssets();

        }



        public void CreateGUI()
        {
            auditorRequirements = AssetDatabase.LoadAssetByGUID<AuditorRequirementsSO>(new UnityEditor.GUID(AssetDatabase.FindAssets("t:auditorrequirementsso")[0]));
            m_auditorRequirements = new SerializedObject(auditorRequirements);

            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            var splitView = new UnityEngine.UIElements.TwoPaneSplitView
            {
                fixedPaneIndex = 0,
                fixedPaneInitialDimension = 250f,
                orientation = TwoPaneSplitViewOrientation.Horizontal,
            };
            root.Add(splitView);



            var leftPane = new VisualElement();
            //var requirementSearchField = new ToolbarSearchField();
            //requirementSearchField.RegisterValueChangedCallback(OnSearchChanged);
            //TODO: add in buttons for switching off/on pass/fail/unsure items

            //leftPane.Add(requirementSearchField);
            leftPane.Add(CreateRequirementSearchOptions());
            m_listView = CreateLayoutListView();
            leftPane.Add(m_listView);
            m_rightPane = new VisualElement();
            splitView.Add(leftPane);
            splitView.Add(m_rightPane);

            m_listView.selectedIndex = m_selectedIndex;
        }

        private VisualElement CreateRequirementSearchOptions()
        {
            var output = new VisualElement();
            output.style.flexDirection = FlexDirection.Row;
            output.style.height = 40;

            var requirementSearchField = new ToolbarSearchField();
            requirementSearchField.style.width = 100;
            requirementSearchField.RegisterValueChangedCallback(OnSearchChanged);
            output.Add(requirementSearchField);

            var toggleButtons = new ToggleButtonGroup();
            toggleButtons.isMultipleSelection = true;
            toggleButtons.allowEmptySelection = true;

            toggleButtons.Add(new Button { iconImage = EditorGUIUtility.FindTexture("TestPassed") });
            toggleButtons.Add(new Button { iconImage = EditorGUIUtility.FindTexture("TestInconclusive") });
            toggleButtons.Add(new Button { iconImage = EditorGUIUtility.FindTexture("TestFailed") });

            output.Add(toggleButtons);

            return output;
        }

        private ListView CreateLayoutListView()
        {
            var listView = new ListView
            {
                bindingPath = "report.requirements",
                //itemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.xx.easyaccessibility/Editor/Auditor/UI/AuditorRequirementEntry.uxml"),
            };

            listView.makeItem = () => new Label();
            listView.bindItem = (item, index) =>
            {
                var label = item as Label;
                string icon = "";
                switch(auditorRequirements.report.requirements[index].status)
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

                label.text = $"[{icon}] {auditorRequirements.report.requirements[index].name}";
            };
            listView.itemsSource = auditorRequirements.report.requirements;
            listView.selectionChanged += OnRequirementSelected;

            listView.selectionChanged += (items) => { m_selectedIndex = listView.selectedIndex; };

            return listView;
        }

        private VisualElement CreateLayoutMultiColumnListView(List<AuditorRequirement.Issue> issues = null)
        {
            var multiColumnListView = new MultiColumnListView
            {
                bindingPath = "issues",
                showBoundCollectionSize = false,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                dataSourceType = typeof(AuditorRequirement.Issue)
            };

            var assetCol = new Column
            {
                title = "Asset",
                width = 200,
                sortable = true
            };
            assetCol.makeCell += MakeObjectCell;
            assetCol.bindCell += BindObjectCell;
            multiColumnListView.columns.Add(assetCol);

            var issueCol = new Column()
            {
                title = "Issue",
                width = 500,
                sortable = true
            };
            issueCol.makeCell += MakeLabelCell;
            issueCol.bindCell += BindLabelCell;
            multiColumnListView.columns.Add(issueCol);

            multiColumnListView.itemsSource = issues;
            return multiColumnListView;
        }

        private void OnSearchChanged(ChangeEvent<string> evt)
        {
            if(string.IsNullOrEmpty(evt.newValue))
            {
                m_listView.itemsSource = auditorRequirements.report.requirements;
            }
            else
            {
                m_listView.itemsSource = auditorRequirements.report.requirements.Where(item => item.name.ToLower().Contains(evt.newValue.ToLower(), StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            m_listView.Rebuild();
        }

        private VisualElement MakeObjectCell()
        {
            var objectField = new ObjectField();
            //TODO: make this un-editable
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
            objectField.bindingPath = $"report.requirements.Array.data[{m_selectedIndex}].issues.Array.data[{index}].asset";
            objectField.Bind(m_auditorRequirements);
        }

        private void BindLabelCell(VisualElement visualElement, int index)
        {
            var labelCell = (Label)visualElement;
            labelCell.bindingPath = $"report.requirements.Array.data[{m_selectedIndex}].issues.Array.data[{index}].issue";
            labelCell.Bind(m_auditorRequirements);
        }



        private void OnRequirementSelected(IEnumerable<object> selected)
        {
            m_rightPane.Clear();

            var enumerator = selected.GetEnumerator();
            if(enumerator.MoveNext())
            {
                var req = enumerator.Current as AuditorRequirement;
                if(req != null)
                {
                    //TODO: add descriptive information (name of requirement, links, etc.)
                    var view = CreateLayoutMultiColumnListView(req.issues);
                    m_rightPane.Add(view);
                }
            }
        }
    }
}