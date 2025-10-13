using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;
using System.Linq;
using System.Reflection;


namespace EasyAccessibility
{
    public class AuditorWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        AuditorRequirementsSO auditorRequirements;

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

            foreach (var type in types)
            {
                var instance = (AuditorRequirement)Activator.CreateInstance(type);
                instance.Audit();
                auditorRequirements.report.requirements.Add(instance);
            }

            AssetDatabase.SaveAssets();
        }



        public void CreateGUI()
        {
            auditorRequirements = AssetDatabase.LoadAssetByGUID<AuditorRequirementsSO>(new UnityEditor.GUID(AssetDatabase.FindAssets("t:auditorrequirementsso")[0]));

            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            var multiColumnListView = new MultiColumnListView
            {
                bindingPath = "report.requirements",
                showBoundCollectionSize = false,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight
            };
            multiColumnListView.columns.Add(new Column { bindingPath = "status", title = "Status", stretchable = true, });
            multiColumnListView.columns.Add(new Column { bindingPath = "source", title = "Source", stretchable = true });
            multiColumnListView.columns.Add(new Column { bindingPath = "name", title = "Name", stretchable = true });
            multiColumnListView.columns.Add(new Column { bindingPath = "description", title = "Description", stretchable = true });
            multiColumnListView.columns.Add(new Column { bindingPath = "referenceLink", title = "Link", stretchable = true });

            var so = new SerializedObject(auditorRequirements);
            multiColumnListView.Bind(so);
            root.Add(multiColumnListView);
        }
    }
}