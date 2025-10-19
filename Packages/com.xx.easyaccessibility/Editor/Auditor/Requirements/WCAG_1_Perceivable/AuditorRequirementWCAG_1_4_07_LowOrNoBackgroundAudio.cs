namespace EasyAccessibility
{
    /*TODO:
     * - Set up some static analysis where the audio in the music track of audio mixer is at the right dB
     */
    public class AuditorRequirementWCAG_1_4_07_LowOrNoBackgroundAudio : AuditorRequirement
    {
        public AuditorRequirementWCAG_1_4_07_LowOrNoBackgroundAudio()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,4,7);
            description = AuditorRequirementKeys.GetDescription(1,4,7);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,7);
            
        }
    }
}