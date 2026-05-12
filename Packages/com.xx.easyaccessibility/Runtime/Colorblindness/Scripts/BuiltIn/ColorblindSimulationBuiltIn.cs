#if UNITY_EDITOR
using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// Applies colorblind simulation using the procedural approach (Built-In Render Pipeline).
    /// NOTE: should only be used for testing purposes. Use colorblind correction for accessibility
    ///       support.
    /// </summary>
    public class ColorblindSimulationBuiltIn : MonoBehaviour
    {
        [Header("Override")]
        [SerializeField]
        bool overrideSettings;

        [SerializeField]
        AccessibilitySettings.ColorblindSimulationMode mode;

        [SerializeField]
        [Range(0f, 1f)]
        float amount = 1;

        //state
        private Material m_renderMaterial;

        void Start()
        {
            var shader = Shader.Find(Constants.ColorblindSimulationShaderBuiltInPath);
            if (shader == null)
            {
                Debug.LogError("Failed to find shader, cannot render colorblind simulation.");
                m_renderMaterial = null;
                return;
            }
            m_renderMaterial = new Material(shader);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (m_renderMaterial == null)
                return;

            m_renderMaterial.SetInt(
                "_Mode",
                (int)AccessibilitySettings.Instance.colorblindSimulationMode
            );
            m_renderMaterial.SetFloat(
                "_Amount",
                AccessibilitySettings.Instance.colorblindSimulationAmount
            );

            if (overrideSettings)
            {
                m_renderMaterial.SetInt("_Mode", (int)mode);
                m_renderMaterial.SetFloat("_Amount", amount);
            }

            Graphics.Blit(source, destination, m_renderMaterial);
        }
    }
}
#endif
