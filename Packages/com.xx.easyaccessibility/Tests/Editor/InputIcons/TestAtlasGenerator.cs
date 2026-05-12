using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace EasyAccessibility.Tests
{
    /// <summary>
    /// One-time editor utility that generates minimal (1x1 pixel) sprite atlases for each
    /// icon set family under Tests/Editor/InputIcons/TestAtlases/. Run once via the menu
    /// and commit the generated assets, tests, then load them via AssetDatabase without
    /// depending on any imported samples.
    ///
    /// To add a new icon set: append a new entry to Definitions() and declare its sprite
    /// name array below. Re-run the generator to produce the new atlas.
    /// </summary>
    public static class TestAtlasGenerator
    {
        const string k_OutputDir = "Packages/com.xx.easyaccessibility/Tests/Editor/InputIcons/TestAtlases";

        //[MenuItem("Easy Accessibility/Dev/Generate Test Atlases - 1 Create Sprites")]
        public static void Step1_CreateSprites()
        {
            Directory.CreateDirectory(ToAbsolutePath(k_OutputDir));

            foreach (var (atlasName, spriteNames) in Definitions())
            {
                var spritesDir = $"{k_OutputDir}/{atlasName}_Sprites";
                Directory.CreateDirectory(ToAbsolutePath(spritesDir));
                foreach (var spriteName in spriteNames)
                {
                    var texPath = $"{spritesDir}/{spriteName}.png";
                    if (!File.Exists(ToAbsolutePath(texPath)))
                        WriteSinglePixelPng(texPath);
                }
            }

            AssetDatabase.Refresh();

            foreach (var (atlasName, spriteNames) in Definitions())
            {
                var spritesDir = $"{k_OutputDir}/{atlasName}_Sprites";
                foreach (var spriteName in spriteNames)
                {
                    var texPath = $"{spritesDir}/{spriteName}.png";
                    var importer = AssetImporter.GetAtPath(texPath) as TextureImporter;
                    if (importer != null && importer.textureType != TextureImporterType.Sprite)
                    {
                        importer.textureType = TextureImporterType.Sprite;
                        importer.SaveAndReimport();
                    }
                }
            }

            Debug.Log("[TestAtlasGenerator] Step 1 done: sprites created. Now run Step 2 to pack atlases.");
        }

        //[MenuItem("Easy Accessibility/Dev/Generate Test Atlases - 2 Pack Atlases")]
        public static void Step2_PackAtlases()
        {
            // Force AssetDatabase to discover anything Step 1 wrote to disk.
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            // Ensure the output directory is registered as a valid folder. After the
            // refresh above it should already be valid, but if the physical directory
            // exists without a meta file (e.g. created outside of Unity), CreateFolder
            // registers it and generates the missing meta file.
            if (!AssetDatabase.IsValidFolder(k_OutputDir))
            {
                var parent = Path.GetDirectoryName(k_OutputDir)?.Replace('\\', '/');
                AssetDatabase.CreateFolder(parent, Path.GetFileName(k_OutputDir));
            }

            if (!AssetDatabase.IsValidFolder(k_OutputDir))
            {
                Debug.LogError($"[TestAtlasGenerator] '{k_OutputDir}' could not be registered. Run Step 1 first.");
                return;
            }

            foreach (var (atlasName, spriteNames) in Definitions())
            {
                var spritesDir = $"{k_OutputDir}/{atlasName}_Sprites";
                var textures = new List<Object>();
                foreach (var spriteName in spriteNames)
                {
                    var tex = AssetDatabase.LoadAssetAtPath<Texture2D>($"{spritesDir}/{spriteName}.png");
                    if (tex != null) textures.Add(tex);
                }

                var atlasPath = $"{k_OutputDir}/{atlasName}.spriteatlas";
                var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);
                if (atlas == null)
                {
                    atlas = new SpriteAtlas();
                    AssetDatabase.CreateAsset(atlas, atlasPath);
                }
                else
                {
                    var existing = SpriteAtlasExtensions.GetPackables(atlas);
                    if (existing.Length > 0)
                        SpriteAtlasExtensions.Remove(atlas, existing);
                }

                SpriteAtlasExtensions.Add(atlas, textures.ToArray());
                SpriteAtlasUtility.PackAtlases(new SpriteAtlas[] { atlas }, EditorUserBuildSettings.activeBuildTarget);
                EditorUtility.SetDirty(atlas);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[TestAtlasGenerator] Step 2 done: all test atlases packed.");
        }

        static IEnumerable<(string atlasName, string[] spriteNames)> Definitions() => new[]
        {
            ("TestAtlas_Keyboard",        k_KeyboardSprites),
            ("TestAtlas_Xbox",            k_XboxSprites),
            ("TestAtlas_PlayStation",     k_PlayStationSprites),
            ("TestAtlas_Switch",          k_SwitchSprites),
            ("TestAtlas_SteamController", k_SteamControllerSprites),
            ("TestAtlas_SteamDeck",       k_SteamDeckSprites),
        };

        static readonly string[] k_KeyboardSprites =
        {
            // Letters
            "keyboard_a", "keyboard_b", "keyboard_c", "keyboard_d", "keyboard_e",
            "keyboard_f", "keyboard_g", "keyboard_h", "keyboard_i", "keyboard_j",
            "keyboard_k", "keyboard_l", "keyboard_m", "keyboard_n", "keyboard_o",
            "keyboard_p", "keyboard_q", "keyboard_r", "keyboard_s", "keyboard_t",
            "keyboard_u", "keyboard_v", "keyboard_w", "keyboard_x", "keyboard_y",
            "keyboard_z",
            // Digits (shared with numpad 0–9)
            "keyboard_0", "keyboard_1", "keyboard_2", "keyboard_3", "keyboard_4",
            "keyboard_5", "keyboard_6", "keyboard_7", "keyboard_8", "keyboard_9",
            // Punctuation
            "keyboard_minus", "keyboard_equals",
            "keyboard_bracket_open", "keyboard_bracket_close",
            "keyboard_slash_back", "keyboard_slash_forward",
            "keyboard_semicolon", "keyboard_quote", "keyboard_tilde",
            "keyboard_comma", "keyboard_period",
            // Common
            "keyboard_space", "keyboard_enter", "keyboard_backspace",
            "keyboard_tab", "keyboard_insert", "keyboard_delete",
            // Navigation
            "keyboard_arrow_up", "keyboard_arrow_down",
            "keyboard_arrow_left", "keyboard_arrow_right",
            "keyboard_home", "keyboard_end",
            "keyboard_page_up", "keyboard_page_down",
            // Modifiers (left/right both resolve to same sprite name)
            "keyboard_shift", "keyboard_ctrl", "keyboard_alt", "keyboard_win",
            // Special
            "keyboard_escape", "keyboard_capslock",
            "keyboard_numlock", "keyboard_printscreen",
            // Function keys
            "keyboard_f1",  "keyboard_f2",  "keyboard_f3",  "keyboard_f4",
            "keyboard_f5",  "keyboard_f6",  "keyboard_f7",  "keyboard_f8",
            "keyboard_f9",  "keyboard_f10", "keyboard_f11", "keyboard_f12",
            // Numpad extras (digits/enter/minus/period/equals are covered above)
            "keyboard_plus", "keyboard_multiply", "keyboard_divide",
        };

        // All three Xbox classes (360, One, Series) use Base = "xbox", so one atlas covers all.
        // 360 needs: xbox_button_start, xbox_button_back
        // One/Series need: xbox_button_menu, xbox_button_view
        // Series also needs: xbox_button_share
        static readonly string[] k_XboxSprites =
        {
            // Face buttons
            "xbox_button_a", "xbox_button_b", "xbox_button_y", "xbox_button_x",
            // Shoulders / triggers
            "xbox_lb", "xbox_rb", "xbox_lt", "xbox_rt",
            // Stick presses
            "xbox_ls", "xbox_rs",
            // Left stick
            "xbox_stick_l",
            "xbox_stick_l_up", "xbox_stick_l_down",
            "xbox_stick_l_left", "xbox_stick_l_right",
            // Right stick
            "xbox_stick_r",
            "xbox_stick_r_up", "xbox_stick_r_down",
            "xbox_stick_r_left", "xbox_stick_r_right",
            // D-Pad
            "xbox_dpad",
            "xbox_dpad_up", "xbox_dpad_down",
            "xbox_dpad_left", "xbox_dpad_right",
            // Version-specific system buttons
            "xbox_button_start", "xbox_button_back",   // Xbox 360
            "xbox_button_menu",  "xbox_button_view",   // Xbox One / Series
            "xbox_button_share",                       // Xbox Series only
        };

        // Shared sprites use hardcoded "playstation_" prefix (no version number).
        // Version-specific start/select and device-specific paths include the Prefix (3/4/5).
        static readonly string[] k_PlayStationSprites =
        {
            // Face buttons (shared)
            "playstation_button_cross", "playstation_button_circle",
            "playstation_button_triangle", "playstation_button_square",
            // Triggers / shoulders (shared)
            "playstation_trigger_l1", "playstation_trigger_r1",
            "playstation_trigger_l2", "playstation_trigger_r2",
            // Stick presses (shared)
            "playstation_button_l3", "playstation_button_r3",
            // Left stick (shared)
            "playstation_stick_l",
            "playstation_stick_l_up", "playstation_stick_l_down",
            "playstation_stick_l_left", "playstation_stick_l_right",
            // Right stick (shared)
            "playstation_stick_r",
            "playstation_stick_r_up", "playstation_stick_r_down",
            "playstation_stick_r_left", "playstation_stick_r_right",
            // D-Pad (shared)
            "playstation_dpad",
            "playstation_dpad_up", "playstation_dpad_down",
            "playstation_dpad_left", "playstation_dpad_right",
            // PS3 version-specific  ("playstation" + Prefix + "_" + StartName/SelectName)
            "playstation3_button_start", "playstation3_button_select",
            // PS4 version-specific
            "playstation4_button_options", "playstation4_button_share",
            "playstation4_touchpad", "playstation4_touchpad_press",
            // PS5 version-specific
            "playstation5_button_options", "playstation5_button_create",
            "playstation5_touchpad", "playstation5_touchpad_press",
            "playstation5_button_mute",
        };

        // Switch and Switch 2 use identical sprite names: one atlas serves both.
        static readonly string[] k_SwitchSprites =
        {
            // Face buttons (Nintendo physical layout: B/A/X/Y)
            "switch_button_b", "switch_button_a",
            "switch_button_x", "switch_button_y",
            // Shoulders / triggers
            "switch_button_l", "switch_button_r",
            "switch_button_sl", "switch_button_sr",
            // System
            "switch_button_plus", "switch_button_minus",
            // Left stick
            "switch_stick_l",
            "switch_stick_l_up", "switch_stick_l_down",
            "switch_stick_l_left", "switch_stick_l_right",
            "switch_stick_l_press",
            // Right stick
            "switch_stick_r",
            "switch_stick_r_up", "switch_stick_r_down",
            "switch_stick_r_left", "switch_stick_r_right",
            "switch_stick_r_press",
            // D-Pad
            "switch_dpad",
            "switch_dpad_up", "switch_dpad_down",
            "switch_dpad_left", "switch_dpad_right",
        };

        // Steam Controller: right stick → right trackpad; dpad → left trackpad
        static readonly string[] k_SteamControllerSprites =
        {
            // Face buttons
            "steam_button_a", "steam_button_b",
            "steam_button_y", "steam_button_x",
            // Shoulders / triggers
            "steam_lb", "steam_rb", "steam_lt", "steam_rt",
            // System
            "steam_button_start_icon", "steam_button_back_icon",
            // Left stick (physical)
            "steam_stick",
            "steam_stick_up", "steam_stick_down",
            "steam_stick_left", "steam_stick_right",
            "steam_stick_l_press",
            // Right trackpad (mapped to rightStick paths)
            "steam_pad",
            "steam_pad_up", "steam_pad_down",
            "steam_pad_left", "steam_pad_right",
            "steam_pad_center",
            // Left trackpad (mapped to dpad paths)
            "steam_dpad",
            "steam_dpad_up", "steam_dpad_down",
            "steam_dpad_left", "steam_dpad_right",
        };

        // Steam Deck adds back grip buttons (L4/L5/R4/R5) over the standard gamepad layout.
        static readonly string[] k_SteamDeckSprites =
        {
            // Face buttons
            "steamdeck_button_a", "steamdeck_button_b",
            "steamdeck_button_y", "steamdeck_button_x",
            // Shoulders / triggers
            "steamdeck_button_l1", "steamdeck_button_r1",
            "steamdeck_button_l2", "steamdeck_button_r2",
            // System
            "steamdeck_button_options", "steamdeck_button_view",
            // Left stick
            "steamdeck_stick_l",
            "steamdeck_stick_l_up", "steamdeck_stick_l_down",
            "steamdeck_stick_l_left", "steamdeck_stick_l_right",
            "steamdeck_stick_l_press",
            // Right stick
            "steamdeck_stick_r",
            "steamdeck_stick_r_up", "steamdeck_stick_r_down",
            "steamdeck_stick_r_left", "steamdeck_stick_r_right",
            "steamdeck_stick_r_press",
            // D-Pad
            "steamdeck_dpad",
            "steamdeck_dpad_up", "steamdeck_dpad_down",
            "steamdeck_dpad_left", "steamdeck_dpad_right",
            // Back grip buttons
            "steamdeck_button_l4", "steamdeck_button_l5",
            "steamdeck_button_r4", "steamdeck_button_r5",
        };

        static void WriteSinglePixelPng(string assetPath)
        {
            var absolutePath = ToAbsolutePath(assetPath);
            var tex = new Texture2D(4, 4);
            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                    tex.SetPixel(x, y, Color.white);
            tex.Apply();
            File.WriteAllBytes(absolutePath, tex.EncodeToPNG());
            Object.DestroyImmediate(tex);
        }

        static string ToAbsolutePath(string assetPath) =>
            Path.GetFullPath(Path.Combine(Application.dataPath, "..", assetPath));
    }
}
