# README #

This is an extension project for the new Unity UI system which can be found at:
[Unity UI Source](https://bitbucket.org/Unity-Technologies/ui)

### What is this repository for? ###

In this repository is a collection of extension scripts to enhance your Unity UI experience. These scripts have been gathered from many sources and combined and improved over time.
(The majority of the scripts came from the Scripts thread on the [Unity UI forum here](http://bit.ly/UnityUIScriptsForumPost))

These include:

## New Controls ##
================

Control | Description | Menu Command | Component Command | Notes | Credits
--------- | -------------- | ---------------------- | ---------------------------- | ------- | ----------
**Accordion** | An Acordian style control with animated segments. Sourced from [here](http://forum.unity3d.com/threads/accordion-type-layout.271818/). For more details, [see this video demonstration](https://www.youtube.com/watch?v=YSOSVTU5klw) | N/A | Component / UI / Extensions / AccordionGroup | | ChoMPHi
 | | | Component / UI / Extensions / AccordionItem |  | ChoMPHi
**ComboBox** | A styled Combo Box control | UI / Extensions / Combobox | UI / Extensions / Combobox | Still being improved by author | perchik
**HSVPicker** | A colour picker UI | N/A | UI / Extensions / HSVPicker | Project folder includes prefab | judah4
**SelectionBox** | An RTS style selection box control | UI / Extensions / Selection Box | UI / Extensions / Selection Box | Needs documentation on use, selection area defines the area on screen that is selectable.  Uses script on selectable items on screen | Korindian, BenZed
**HorizontalScrollSnap** | A pages scroll rect that can work in steps / pages, includes button support | UI / Extensions / Horizontal Scroll Snap | UI / Extensions / Horizontal Scroll Snap | | BinaryX
**UIButton** | Improved Button control with additional events | UI / Extensions / UI Button | UI / Extensions / UI Button | | AriathTheWise
**UIWindowBase** | A draggable Window implementation | UI / Extensions / UI Window Base | UI / Extensions / UI Window Base | | GXMark, alexzzzz, CaoMengde777, TroyDavis 
**ComboBox** | A fixed combobox implementation for text | UI / Extensions / ComboBox | UI / Extensions / ComboBox | | Perchik
**AutoCompleteComboBox** | A text combobox with autocomplete selection | UI / Extensions / AutoComplete ComboBox | UI / Extensions / AutoComplete ComboBox | | Perchik
**DropDownList** | A basic drop down list with text and image support | UI / Extensions / Dropdown List | UI / Extensions / Dropdown List | | Perchik
**BoundToolTip** | An alternate Tooltip implementation with central listener | UI / Extensions / Bound Tooltip / Tooltip | UI / Extensions / Bound Tooltip / Tooltip Item | Offset and tooltip placement needs work | Martin Sharkbomb
 | | | UI / Extensions / Bound Tooltip / Tooltip Trigger |  | Martin Sharkbomb

## Effect components ##
=====================

Effect   | Description | Component Command | Notes  | Credits
--------- | -------------- | ---------------------------- | ------- | -----------
**BestFitOutline** | An improved outline effect | UI / Effects / Extensions / Best Fit Outline | | Melang
**CurvedText** | A Text vertex manipulator for those users NOT using TextMeshPro (why ever not?) | UI / Effects / Extensons / Curved Text | | Breyer
**Gradient**  | Apply vertex colours in a gradient on any UI object | UI / Effects / Extensions / Gradient | | Breyer
**LetterSpacing** | Allows finers control of text spacing |  UI / Effects / Extensions / Letter Spacing | | Deeperbeige
**NicerOutline** | Another outline control | UI / Effects / Extensions / Nicer Outline | | Melang
**RaycastMask** | An example of an enhanced mask component able to work with the image data. Enables picking on image parts and not just the Rect Transform | UI / Effects / Extensions / Raycast Mask | | senritsu
**UIFlippable** | Image component effect to flip the graphic | UI / Effects / Extensions / UI Flippable | | ChoMPHi


## Additional Components##
=======================

Component | Description | Component Command | Notes | Credits
--------- | -------------- | ---------------------------- | ------- | ------
**ReturnKeyTrigger** | Does something?? | UI / Extensions / ReturnKey Trigger | | Melang, ddreaper
**TabNavigation**  | An example Tab navigation script | UI / Extensions / Tab Navigation | | Melang, omatase
**uGUITools** | | Menu / uGUI | | Senshi
**FlowLayoutGroup** | A more rugged grid style layout group  | Layout / Extensions / Flow Layout Group | [Example Video](https://www.youtube.com/watch?v=tMe_3tJTZvc) | Simie


*More to come*

### How do I get set up? ###

Either clone / download this repository to your machine and then copy the scripts in, or use the pre-packaged .UnityPackage and import it as a custom package in to your project.

### Contribution guidelines ###

Got a script you want added, then just fork and submit a PR.  All contributions accepted (including fixes)
Just ensure 
* The header of the script matches the standard used in all scripts
* The script uses the **Unity.UI.Extensions** namespace so they do not affect any other developments
* (optional) Add Component and Editor options where possible (editor options are in the Editor\UIExtensionsMenuOptions.cs file)

### License ###
All scripts conform to the BSD license and are free to use / distribute.  See the [LICENSE](https://bitbucket.org/ddreaper/unity-ui-extensions/src/6d03f25b0150994afa97c6a55854d6ae696cad13/LICENSE?at=default) file for more information 

### Like what you see? ###

All these scripts were put together for my latest book Unity3D UI Essentials
Check out the [page on my blog](http://bit.ly/Unity3DUIEssentials) for more details and learn all about the inner workings of the new Unity UI System.

### The downloads ###
As this repo was created to support my new Unity UI Title ["Unity 3D UI Essentials"](http://bit.ly/Unity3DUIEssentials), in the downloads section you will find two custom assets (SpaceShip-DemoScene-Start.unitypackage and RollABallSample-Start.unitypackage).  These are just here as starter scenes for doing UI tasks in the book.

I will add more sample scenes for the UI examples in this repository and detail them above over time.