using System.Text.RegularExpressions;
using UnityEditor;

namespace EasyAccessibility
{
    /// <summary>
    /// WCAG 2.1 Guideline 2.1.4: Character Key Shortcuts
    /// If a keyboard shortcut is implemented in content using only letter (character) keys, then at least one of the following is true:
    /// - The shortcut can be remapped by the user to use one or more non-character keys (e.g., Ctrl, Alt, etc.).
    /// - The shortcut is active only when the component it affects has focus.
    /// - The shortcut is active only when the user is performing the function that the shortcut triggers.
    /// </summary>
    public class AuditorRequirementWCAG_2_1_04_CharacterKeyShortcuts
        : AuditorRequirementWCAG_2_1_01_Keyboard
    {
        private static readonly Regex k_LegacyInputPattern = new Regex(
            @"\bInput\.(GetKey|GetKeyDown|GetKeyUp|GetButton|GetButtonDown|GetButtonUp|GetAxis|GetAxisRaw|GetMouseButton|GetMouseButtonDown|GetMouseButtonUp|mousePosition|touches|acceleration)\b",
            RegexOptions.Compiled
        );

        public override void Audit()
        {
            base.Audit();
            issues.Clear();

            var guids = AssetDatabase.FindAssets("t:MonoScript");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.StartsWith("Packages/"))
                    continue;
                var script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (script == null || string.IsNullOrEmpty(script.text))
                    continue;
                if (!k_LegacyInputPattern.IsMatch(script.text))
                    continue;

                issues.Add(
                    new Issue
                    {
                        asset = script,
                        issue =
                            "Uses legacy Input System: these inputs cannot be rebound at runtime.",
                    }
                );
            }

            status = issues.Count > 0 ? Status.Fail : Status.Unsure;
        }

        public AuditorRequirementWCAG_2_1_04_CharacterKeyShortcuts()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 1, 4);
            description = AuditorRequirementKeys.GetDescription(2, 1, 4);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 1, 4);
        }
    }
}
