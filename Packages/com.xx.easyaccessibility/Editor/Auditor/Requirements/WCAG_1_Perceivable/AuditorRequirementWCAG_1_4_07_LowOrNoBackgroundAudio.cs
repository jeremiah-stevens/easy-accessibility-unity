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
            name = AuditorRequirementKeys.Title_WCAG_1_4_7;
            description = AuditorRequirementKeys.Description_WCAG_1_4_7;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_4_7;
            status = Status.None;
        }
    }
}