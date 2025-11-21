using UnityEngine;


namespace EasyAccessibility
{
    /// <summary>
    /// Applies colorblind correction using a look-up table approach (Built-In Render Pipeline).
    /// </summary>
    public class ColorblindCorrectionLUTBuiltIn : MonoBehaviour
    {
        [Header("Override")]
        [SerializeField] bool overrideSettings;
        [SerializeField] AccessibilitySettings.ColorblindMode mode;
        [SerializeField][Range(0f, 1f)] float amount = 1;
        [SerializeField] Texture3D textureProtanopia;
        [SerializeField] Texture3D textureDeutranopia;
        [SerializeField] Texture3D textureTritanopia;

        //state
        private Material m_renderMaterial;
        



        private void SetLUT(AccessibilitySettings.ColorblindMode mode)
        {
            switch (mode)
            {
                case AccessibilitySettings.ColorblindMode.Protanopia:
                    m_renderMaterial.SetTexture("_LUT", textureProtanopia);
                    break;
                case AccessibilitySettings.ColorblindMode.Deuteranopia:
                    m_renderMaterial.SetTexture("_LUT", textureDeutranopia);
                    break;
                case AccessibilitySettings.ColorblindMode.Tritanopia:
                    m_renderMaterial.SetTexture("_LUT", textureTritanopia);
                    break;
                default:
                    m_renderMaterial.SetTexture("_LUT", null);
                    break;
            }
        }




        void Start()
        {
            var shader = Shader.Find(Constants.ColorblindCorrectionShaderLUTBuiltInPath);
            if (shader == null)
            {
                Debug.LogError("Failed to find shader, cannot render colorblind correction.");
                m_renderMaterial = null;
                return;
            }
            m_renderMaterial = new Material(shader);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (m_renderMaterial == null) return;

            m_renderMaterial.SetInt("_Mode", (int)AccessibilitySettings.Instance.colorblindCorrectionMode);
            m_renderMaterial.SetFloat("_Amount", AccessibilitySettings.Instance.colorblindCorrectionAmount);

            SetLUT(AccessibilitySettings.Instance.colorblindCorrectionMode);

            if (overrideSettings)
            {
                m_renderMaterial.SetInt("_Mode", (int)mode);
                m_renderMaterial.SetFloat("_Amount", amount);

                SetLUT(mode);
            }

            Graphics.Blit(source, destination, m_renderMaterial);
        }
    }
}