# Changelog

All notable changes to this project will be documented in this file.
Format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [Unreleased]

## [0.0.1] - 2026-05-12

### Added

#### Accessibility Auditor
- Auditor: audits your project for common accessibility issues and offers recommendations for correcting them
- Added an Editor window for auditing a project for WCAG compliance
- Added ScriptableObject report file for storing the results of an audit
- Added requirement classes for each WCAG standard; requirement coverage will expand with future releases
#### Colorblindness Correction
- Colorblindness: provides both correction for and simulation of the most common forms of colorblindness
- Added colorblindness correction for URP, HDRP, and Built-In render pipelines
- Added two correction modes for each pipeline:
  - Lookup Table (LUT): faster using pre-baked correction, but has memory footprint (64mb)
  - Procedural: runtime-generated correction via daltonization; slower, but lower memory footprint
- Added colorblindness simulation mode (visualise how content appears to colorblind users)

#### Rebindable Keys
- Rebindable Keys: orchestrates rebind operations in Unity's Input System, including component-based rebind options, preference saving, and dynamic icon swapping based on input device
- Added RebindableInputManager for coordinating rebind operations 
- Added RebindableAction for handling single-button rebind interactions
- Added input icon set classes for device-specific button icons
- Added sample icon set using [Kenney's CC0 Input Prompts](https://kenney.nl/assets/input-prompts) asset pack
- Added AccessibilitySettings integration for persisting rebind overrides across sessions

[Unreleased]: https://github.com/jeremiah-stevens/easy-accessibility-unity/compare/v0.0.1...HEAD
[0.0.1]: https://github.com/jeremiah-stevens/easy-accessibility-unity/releases/tag/v0.0.1
