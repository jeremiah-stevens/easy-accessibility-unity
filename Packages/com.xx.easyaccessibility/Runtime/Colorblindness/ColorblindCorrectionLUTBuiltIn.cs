using UnityEngine;


namespace EasyAccessibility
{
    public class ColorblindCorrectionLUTBuiltIn : MonoBehaviour
    {
        public Shader shader = null;
        [SerializeField] ColorblindSettings.ColorblindMode mode;
        [SerializeField][Range(0f, 1f)] float amount = 1;
        [SerializeField] Texture3D textureProtanopia;
        [SerializeField] Texture3D textureDeutranopia;
        [SerializeField] Texture3D textureTritanopia;
        private Material m_renderMaterial;




        void Start()
        {
            if (shader == null)
            {
                m_renderMaterial = null;
                return;
            }
            m_renderMaterial = new Material(shader);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            m_renderMaterial.SetInt("_Mode", (int)mode);
            m_renderMaterial.SetFloat("_Amount", amount);
            Texture3D tex = null;
            switch (mode)
            {
                case ColorblindSettings.ColorblindMode.Protanopia:
                    tex = textureProtanopia;
                    break;
                case ColorblindSettings.ColorblindMode.Deuteranopia:
                    tex = textureDeutranopia;
                    break;
                case ColorblindSettings.ColorblindMode.Tritanopia:
                    tex = textureTritanopia;
                    break;
            }
            m_renderMaterial.SetTexture("_LUT", tex);

            Graphics.Blit(source, destination, m_renderMaterial);
        }
    }
}