namespace EasyAccessibility
{
    public static class Constants
    {
        //Colorblindness
        public static string ColorblindCorrectionShaderProceduralBuiltInPath =
            "EasyAccessibility/BuiltIn/ColorblindCorrectionProcedural";
        public static string ColorblindCorrectionShaderLUTBuiltInPath =
            "EasyAccessibility/BuiltIn/ColorblindCorrectionLUT";
        public static string ColorblindCorrectionShaderProceduralURPPath =
            "Shader Graphs/ColorblindCorrectionProcedural_URP";
        public static string ColorblindCorrectionShaderLUTURPPath =
            "Shader Graphs/ColorblindCorrectionLUT_URP";
        public static string ColorblindCorrectionShaderProceduralHDRPPath =
            "Shader Graphs/ColorblindCorrectionProcedural_HDRP";
        public static string ColorblindCorrectionShaderLUTHDRPPath =
            "Shader Graphs/ColorblindCorrectionLUT_HDRP";

#if UNITY_EDITOR
        public static string ColorblindSimulationShaderBuiltInPath =
            "EasyAccessibility/BuiltIn/ColorblindSimulation";
#endif
    }
}
