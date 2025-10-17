using UnityEngine;

namespace EasyAccessibility
{
    /*TODO:
     * - Clean up the way this is stored (use a localization table instead?)
     * - Add all of the supplementary information (ex: missing all of the information in colons)
     */
    public static class AuditorRequirementKeys
    {
        public const string Source_WCAG = "WCAG";

        #region 1: Perceivable

        #region 1.1: Text Alternatives

        public const string Title_WCAG_1_1_1 = "1.1.1: Non-Text Content";
        public const string Description_WCAG_1_1_1 = "All non-text content that is presented to the user has a text alternative that serves the equivalent purpose, except for the situations listed below.";
        public const string URL_WCAG_1_1_1 = "https://www.w3.org/WAI/WCAG22/quickref/#non-text-content";

        #endregion


        #region 1.2: Time-Based Media

        public const string Title_WCAG_1_2_1 = "1.2.1: Audio-only and Video-only (Prerecorded)";
        public const string Description_WCAG_1_2_1 = "For prerecorded audio-only and prerecorded video-only media, the following are true, except when the audio or video is a media alternative for text and is clearly labeled as such:";
        public const string URL_WCAG_1_2_1 = "https://www.w3.org/WAI/WCAG22/quickref/#audio-only-and-video-only-prerecorded";


        public const string Title_WCAG_1_2_2 = "1.2.2: Captions(Prerecorded)";
        public const string Description_WCAG_1_2_2 = "Captions are provided for all prerecorded audio content in synchronized media, except when the media is a media alternative for text and is clearly labeled as such.";
        public const string URL_WCAG_1_2_2 = "https://www.w3.org/WAI/WCAG22/quickref/#captions-prerecorded";


        public const string Title_WCAG_1_2_3 = "1.2.3: Audio Description or Media Alternative (Prerecorded)";
        public const string Description_WCAG_1_2_3 = "An alternative for time-based media or audio description of the prerecorded video content is provided for synchronized media, except when the media is a media alternative for text and is clearly labeled as such.";
        public const string URL_WCAG_1_2_3 = "https://www.w3.org/WAI/WCAG22/quickref/#audio-description-or-media-alternative-prerecorded";


        public const string Title_WCAG_1_2_4 = "1.2.4: Captions (Live)";
        public const string Description_WCAG_1_2_4 = "Captions are provided for all live audio content in synchronized media.";
        public const string URL_WCAG_1_2_4 = "https://www.w3.org/WAI/WCAG22/quickref/#captions-live";


        public const string Title_WCAG_1_2_5 = "1.2.5: Audio Description (Prerecorded)";
        public const string Description_WCAG_1_2_5 = "Audio description is provided for all prerecorded video content in synchronized media.";
        public const string URL_WCAG_1_2_5 = "https://www.w3.org/WAI/WCAG22/quickref/#audio-description-prerecorded";


        public const string Title_WCAG_1_2_6 = "1.2.6: Sign Language (Prerecorded)";
        public const string Description_WCAG_1_2_6 = "Sign language interpretation is provided for all prerecorded audio content in synchronized media.";
        public const string URL_WCAG_1_2_6 = "https://www.w3.org/WAI/WCAG22/quickref/#sign-language-prerecorded";


        public const string Title_WCAG_1_2_7 = "1.2.7: Extended Audio Description (Prerecorded)";
        public const string Description_WCAG_1_2_7 = "Where pauses in foreground audio are insufficient to allow audio descriptions to convey the sense of the video, extended audio description is provided for all prerecorded video content in synchronized media.";
        public const string URL_WCAG_1_2_7 = "https://www.w3.org/WAI/WCAG22/quickref/#extended-audio-description-prerecorded";


        public const string Title_WCAG_1_2_8 = "1.2.8: Media Alternative (Prerecorded)";
        public const string Description_WCAG_1_2_8 = "An alternative for time-based media is provided for all prerecorded synchronized media and for all prerecorded video-only media.";
        public const string URL_WCAG_1_2_8 = "https://www.w3.org/WAI/WCAG22/quickref/#media-alternative-prerecorded";


        public const string Title_WCAG_1_2_9 = "1.2.9: Audio-only (Live)";
        public const string Description_WCAG_1_2_9 = "An alternative for time-based media that presents equivalent information for live audio-only content is provided.";
        public const string URL_WCAG_1_2_9 = "https://www.w3.org/WAI/WCAG22/quickref/#audio-only-live";

        #endregion


        #region 1.3: Adaptable

        public const string Title_WCAG_1_3_1 = "1.3.1: Info and Relationships";
        public const string Description_WCAG_1_3_1 = "Information, structure, and relationships conveyed through presentation can be programmatically determined or are available in text.";
        public const string URL_WCAG_1_3_1 = "https://www.w3.org/WAI/WCAG22/quickref/#info-and-relationships";


        public const string Title_WCAG_1_3_2 = "1.3.2: Meaningful Sequence";
        public const string Description_WCAG_1_3_2 = "When the sequence in which content is presented affects its meaning, a correct reading sequence can be programmatically determined.";
        public const string URL_WCAG_1_3_2 = "https://www.w3.org/WAI/WCAG22/quickref/#meaningful-sequence";

        public const string Title_WCAG_1_3_3 = "1.3.3: Sensory Characteristics";
        public const string Description_WCAG_1_3_3 = "Instructions provided for understanding and operating content do not rely solely on sensory characteristics of components such as shape, color, size, visual location, orientation, or sound.";
        public const string URL_WCAG_1_3_3 = "https://www.w3.org/WAI/WCAG22/quickref/#sensory-characteristics";

        public const string Title_WCAG_1_3_4 = "1.3.4: Orientation";
        public const string Description_WCAG_1_3_4 = "Content does not restrict its view and operation to a single display orientation, such as portrait or landscape, unless a specific display orientation is essential.";
        public const string URL_WCAG_1_3_4 = "https://www.w3.org/WAI/WCAG22/quickref/#orientation";

        public const string Title_WCAG_1_3_5 = "1.3.5: Identify Input Purpose";
        public const string Description_WCAG_1_3_5 = "The purpose of each input field collecting information about the user can be programmatically determined when:";
        public const string URL_WCAG_1_3_5 = "https://www.w3.org/WAI/WCAG22/quickref/#identify-input-purpose";

        public const string Title_WCAG_1_3_6 = "1.3.6:  Identify Purpose";
        public const string Description_WCAG_1_3_6 = "In content implemented using markup languages, the purpose of user interface components, icons, and regions can be programmatically determined.";
        public const string URL_WCAG_1_3_6 = "https://www.w3.org/WAI/WCAG22/quickref/#identify-purpose";

        #endregion


        #region 1.4: Distinguishable

        public const string Title_WCAG_1_4_1 = "1.4.1: Use of Color";
        public const string Description_WCAG_1_4_1 = "Color is not used as the only visual means of conveying information, indicating an action, prompting a response, or distinguishing a visual element.";
        public const string URL_WCAG_1_4_1 = "https://www.w3.org/WAI/WCAG22/quickref/#use-of-color";

        public const string Title_WCAG_1_4_2 = "1.4.2: Audio Control";
        public const string Description_WCAG_1_4_2 = "If any audio on a web page plays automatically for more than 3 seconds, either a mechanism is available to pause or stop the audio, or a mechanism is available to control audio volume independently from the overall system volume level.";
        public const string URL_WCAG_1_4_2 = "https://www.w3.org/WAI/WCAG22/quickref/#audio-control";

        public const string Title_WCAG_1_4_3 = "1.4.3: Contrast (Minimum)";
        public const string Description_WCAG_1_4_3 = "The visual presentation of text and images of text has a contrast ratio of at least 4.5:1, except for the following:";
        public const string URL_WCAG_1_4_3 = "https://www.w3.org/WAI/WCAG22/quickref/#contrast-minimum";

        public const string Title_WCAG_1_4_4 = "1.4.4: Resize Text";
        public const string Description_WCAG_1_4_4 = "Except for captions and images of text, text can be resized without assistive technology up to 200 percent without loss of content or functionality.";
        public const string URL_WCAG_1_4_4 = "https://www.w3.org/WAI/WCAG22/quickref/#resize-text";

        public const string Title_WCAG_1_4_5 = "1.4.5: Images of Text";
        public const string Description_WCAG_1_4_5 = "If the technologies being used can achieve the visual presentation, text is used to convey information rather than images of text except for the following:";
        public const string URL_WCAG_1_4_5 = "https://www.w3.org/WAI/WCAG22/quickref/#images-of-text";

        public const string Title_WCAG_1_4_6 = "1.4.6: Contrast (Enhanced)";
        public const string Description_WCAG_1_4_6 = "The visual presentation of text and images of text has a contrast ratio of at least 7:1, except for the following:";
        public const string URL_WCAG_1_4_6 = "https://www.w3.org/WAI/WCAG22/quickref/#contrast-enhanced";

        public const string Title_WCAG_1_4_7 = "1.4.7: Low or No Background Audio";
        public const string Description_WCAG_1_4_7 = "For prerecorded audio-only content that (1) contains primarily speech in the foreground, (2) is not an audio CAPTCHA or audio logo, and (3) is not vocalization intended to be primarily musical expression such as singing or rapping, at least one of the following is true:";
        public const string URL_WCAG_1_4_7 = "https://www.w3.org/WAI/WCAG22/quickref/#low-or-no-background-audio";

        public const string Title_WCAG_1_4_8 = "1.4.8: Visual Presentation";
        public const string Description_WCAG_1_4_8 = "For the visual presentation of blocks of text, a mechanism is available to achieve the following:";
        public const string URL_WCAG_1_4_8 = "https://www.w3.org/WAI/WCAG22/quickref/#visual-presentation";

        public const string Title_WCAG_1_4_9 = "1.4.9: Images of Text (No Exception)";
        public const string Description_WCAG_1_4_9 = "Images of text are only used for pure decoration or where a particular presentation of text is essential to the information being conveyed.";
        public const string URL_WCAG_1_4_9 = "https://www.w3.org/WAI/WCAG22/quickref/#images-of-text-no-exception";

        public const string Title_WCAG_1_4_10 = "1.4.10: Reflow";
        public const string Description_WCAG_1_4_10 = "Content can be presented without loss of information or functionality, and without requiring scrolling in two dimensions for:";
        public const string URL_WCAG_1_4_10 = "https://www.w3.org/WAI/WCAG22/quickref/#reflow";

        public const string Title_WCAG_1_4_11 = "1.4.11: Non-text Contrast";
        public const string Description_WCAG_1_4_11 = "The visual presentation of the following have a contrast ratio of at least 3:1 against adjacent color(s):";
        public const string URL_WCAG_1_4_11 = "https://www.w3.org/WAI/WCAG22/quickref/#non-text-contrast";

        public const string Title_WCAG_1_4_12 = "1.4.12: Text Spacing";
        public const string Description_WCAG_1_4_12 = "In content implemented using markup languages that support the following text style properties, no loss of content or functionality occurs by setting all of the following and by changing no other style property:";
        public const string URL_WCAG_1_4_12 = "https://www.w3.org/WAI/WCAG22/quickref/#text-spacing";

        public const string Title_WCAG_1_4_13 = "1.4.13: Content on Hover or Focus";
        public const string Description_WCAG_1_4_13 = "Where receiving and then removing pointer hover or keyboard focus triggers additional content to become visible and then hidden, the following are true:";
        public const string URL_WCAG_1_4_13 = "https://www.w3.org/WAI/WCAG22/quickref/#content-on-hover-or-focus";

        #endregion


        public const string Title_WCAG_x = "";
        public const string Description_WCAG_x = "";
        public const string URL_WCAG_x = "";

        #endregion
    }
}