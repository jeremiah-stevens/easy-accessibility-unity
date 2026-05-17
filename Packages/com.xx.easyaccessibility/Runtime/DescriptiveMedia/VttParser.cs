using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace EasyAccessibility.DescriptiveMedia
{
    /// <summary>
    /// A single timed caption cue parsed from a WebVTT file.
    /// </summary>
    public readonly struct VttCue
    {
        /// <summary>Start time in seconds.</summary>
        public float Start { get; }

        /// <summary>End time in seconds.</summary>
        public float End { get; }

        /// <summary>Caption text. Multi-line cues are joined with a newline.</summary>
        public string Text { get; }

        public VttCue(float start, float end, string text)
        {
            Start = start;
            End = end;
            Text = text;
        }
    }

    /// <summary>
    /// Parses WebVTT (.vtt) files into a list of <see cref="VttCue"/> values.
    /// Handles optional cue identifiers, multi-line cue text, and NOTE/STYLE/REGION
    /// blocks. Cue positioning metadata on timestamp lines is ignored.
    /// </summary>
    public static class VttParser
    {
        public static List<VttCue> Parse(TextAsset asset)
        {
            if (asset == null)
                return new();
            return Parse(asset.text);
        }

        public static List<VttCue> Parse(string content)
        {
            var cues = new List<VttCue>();
            if (string.IsNullOrEmpty(content))
                return cues;

            var lines = content.Split('\n');
            var i = 0;

            // Advance past the WEBVTT signature line.
            while (i < lines.Length && !lines[i].TrimStart().StartsWith("WEBVTT"))
                i++;
            i++;

            while (i < lines.Length)
            {
                var line = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(line))
                {
                    i++;
                    continue;
                }

                // Skip NOTE, STYLE, and REGION blocks entirely.
                if (
                    line.StartsWith("NOTE")
                    || line.StartsWith("STYLE")
                    || line.StartsWith("REGION")
                )
                {
                    while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
                        i++;
                    continue;
                }

                // Lines without "-->" are cue identifiers, so we skip them.
                if (!line.Contains("-->"))
                {
                    i++;
                    continue;
                }

                if (!TryParseTimestampLine(line, out var start, out var end))
                {
                    i++;
                    continue;
                }
                i++;

                // Collect text lines until the next blank line.
                var textLines = new List<string>();
                while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
                {
                    textLines.Add(lines[i].Trim());
                    i++;
                }

                if (textLines.Count > 0)
                    cues.Add(new VttCue(start, end, string.Join("\n", textLines)));
            }

            return cues;
        }

        private static bool TryParseTimestampLine(string line, out float start, out float end)
        {
            start = 0;
            end = 0;

            var sep = line.IndexOf("-->", StringComparison.Ordinal);
            if (sep < 0)
                return false;

            var startStr = line[..sep].Trim();

            // End portion may have positioning metadata, so we take only the first token.
            var endToken = line[(sep + 3)..].Trim().Split(' ')[0];

            return TryParseTimestamp(startStr, out start) && TryParseTimestamp(endToken, out end);
        }

        private static bool TryParseTimestamp(string s, out float seconds)
        {
            seconds = 0;
            var parts = s.Split(':');

            try
            {
                // Supports HH:MM:SS.mmm and MM:SS.mmm
                seconds = parts.Length switch
                {
                    3
                        => float.Parse(parts[0], CultureInfo.InvariantCulture) * 3600f
                            + float.Parse(parts[1], CultureInfo.InvariantCulture) * 60f
                            + float.Parse(parts[2], CultureInfo.InvariantCulture),
                    2
                        => float.Parse(parts[0], CultureInfo.InvariantCulture) * 60f
                            + float.Parse(parts[1], CultureInfo.InvariantCulture),
                    _ => throw new FormatException($"Unexpected timestamp format: {s}"),
                };
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}