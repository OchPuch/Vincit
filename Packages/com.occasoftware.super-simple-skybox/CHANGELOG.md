# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/), 
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

Changelog can be found online at [Super Simple Skybox Changelog](https://www.occasoftware.com/changelogs/super-simple-skybox).
## [5.0.0] - 2023-06-26
This version is compatible with Unity 2022.3.0f1

### Added
- Added option to control iterations, gain, and lacunarity of cloud texture sampling, giving greater control over the final cloud look.

## [4.2.1] - 2023-06-21
This version is compatible with Unity 2022.3.0f1

### Fixed
- Fixed an issue with the star main light matrix.

## [4.2.0] - 2023-06-21
This version is compatible with Unity 2021.3.0f1

### Fixed
- Fixed an issue with the static sun component which would cause it not to appear in the editor.

### Added
- Added a static moon component
- Added editor start menu with various helpful links

## [4.1.2] - 2023-05-22

### Fixed
- Fixed an issue where Cloud Opacity was controlling Cloud Sharpness, and Cloud Sharpness was missing from the inspector

## [4.1.1] - 2023-05-11

### Changed

-   The Samples~/ directory is now just Samples/.

## [4.1.0] - 2023-05-09

### Added

-   You can now control the ground height.

### Changed

-   You can now use both procedural stars and texture stars at the same time. Some settings are shared between the two.
-   Cleaned up the editor GUI and renamed a few options.

### Removed

-   Prefabs are no longer included in the demo project. The recommended setup path for the asset is now to simply add the components.

### Fixed

-   Fixed an issue with the Sample scene.

## [4.0.0] - 2023-05-07

### Changed

-   Migrated from Asset to Package. This asset will now appear in Packages/SuperSimpleSkybox rather than Assets/OccaSoftware/SuperSimpleSkybox. Samples are now optional additional packages.
-   Updated the OnSunriseSunsetDemo.cs demo script so that it no longer changes the sky color on time of day changes.

## [3.1.2] - April 17, 2023
-   Fixed an issue with the Cloud Height Falloff property.

## [3.1.1] - March 9, 2023

-   Corrected some language in the Readme
-   Removed an unnecessary Using directive.
-   Added Documentation and Registration links to the editor.
-   Added more cloud textures, bringing the total to 30 cloud textures.

## [3.1.0] - March 2, 2023

-   Clouds now have improved shading. You can control the shading intensity with the Shading Intensity slider.
-   Clouds now more accurately respond to changes in the directional light.
-   Clouds are now affected by an ambient lighting term.
-   Clouds are now more opaque by default. You can now control the cloud opacity more accurately using the Opacity slider in the Clouds/Look Settings section.
-   Added a prefab for the Moon object.
-   Added a "Constant Color Mode". The Constant Color Mode enables you to set fixed Skybox colors that will not change regardless of the sun and moon position. The Constant Color Mode is accompanied with a simplified editor that hides the Night color options. The Day and Night Colors mode continues to work as normal.

## [3.0.2] - February 23, 2023

-   The new VR Skybox component was not included in the upload files with release 3.0.1. This release correctly includes that component in the release.

## [3.0.1] - February 21, 2023

-   Added a VR Skybox option
-   To use it, simply drag-and-drop the VR Skybox prefab into your scene
-   Add your main camera as the camera target
-   Make sure to use the correct material for the VR Skybox material slot
-   Fixed potential null reference errors that could occur from the callback demo.

## [3.0.0] - February 3, 2023

-   SetSunPosition.cs is now replaced with DirectionalLight.cs.
-   Added two new components, Sun.cs and Moon.cs, which inherit from DirectionalLight.cs.
-   These new components enable you to position the sun and moon independently of each other.
-   Added events for sunrise, sunset, moonrise, and moonset. Subscribe to OnRise or OnSet in Sun.cs and/or Moon.cs.
-   Star rotation can now be handled independently with the SetStarRotation.cs component. Typically, you want to add this to the same game object as Sun.cs.
-   The sun and moon now automatically handle their own shadows and intensity based on the position relative to the zenith. This is an optional feature.
-   Rotation Speed has been changed to Rotations Per Hour to simplify time of day management.

## [2.1.0] - February 2, 2023

-   Added 8 additional cloud textures.

## [2.0.3] - January 10, 2023

-   Fixed an issue with environmental lighting. Now correctly uses the Skybox color when the Environmental Lighting Source is set to Skybox.

## [2.0.2] - January 1, 2023

-   Fixed an issue with orthographic rendering. Now correctly supports orthographic perspective.

## [2.0.1] - October 3, 2022

-   Fixed an issue where the sun position vector was incorrectly overwriting the sky zenith color in built projects.

## [2.0.0] - September 12, 2022

-   The sun is now a sun disc with physically-based limb darkening.
-   The sun angular diameter property replaces the sun size property.
-   You can now disable sun sky lighting (i.e., sun falloff and sunset modifiers).
-   Added procedural star rendering option.
-   Added star saturation option.
-   Added a custom UI, with separate sections for Ground Settings, Sky Settings, Sun Settings, Moon Settings, Star Settings, and Cloud Settings.

## [1.4.2] - June 17, 2022

-   The shader was incorrectly set to Depth Test Always, causing other objects to fail to appear on screen. This has been fixed.
-   Updated demo scene.

## [1.4.1]

-   The shader was incorrectly failing to identify when being used on Built-In Render Pipeline, which caused the Skybox to render pink in some cases. This has been fixed.

## [1.4.0]

-   Now works on Built-In Render Pipeline (requires Shader Graph).
-   Updated baseline Unity version to 2021.3.

## [1.3.0]

-   There is now a Moon disc rendered in the sky rendered with physically-based limb darkening. The moon disc will always appear directly opposite the sun. You can control the Moon Disc size, color, and lighting falloff from the SuperSimpleSkybox Material.

## [1.2.0]

-   You can now control the Sun Falloff using the Sun Falloff float in the Material editor. This may be useful in cases where you want a tighter emissive region in the skybox. This setting can cause the sun to completely disappear at lower sun sizes, so...
-   You can now increase the Sun Size from [0, 1] (previously clamped to [0, 0.1]).

## [1.1.0]

-   You can now hide the ground layer by unchecking the "Use Ground" checkbox in the Material editor. This may be useful in cases where the ground layer is visible and you wish it to be hidden.

## [1.0.0]

-   Initial Release.