using UnityEngine;

namespace EasyAccessibility.TestFixtures
{
    /// <summary>
    /// Uses the legacy Input System to validate WCAG 2.1.4 auditor check.
    /// </summary>
    public class LegacyInputTestFixture : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) { }
            if (Input.GetAxis("Horizontal") != 0) { }
        }
    }
}