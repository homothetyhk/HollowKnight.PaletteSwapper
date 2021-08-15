PaletteSwapper is a toggleable mod to change the color tint of rooms. 
**PLEASE TAKE PRECAUTIONS AND EXERCISE DISCRETION BEFORE USING THIS MOD IF YOU HAVE PHOTOSENSITIVITY**
- To edit settings open the PaletteSwapper.GlobalSettings.json file in the saves folder, in AppData
- By default random colors are chosen uniformly in each RGBA component, with the alpha constrained to [0.5, 1.0].
- Settings
	- BoolValues
		- RandomByMapZone: all rooms in the same area receive the same random tint. The palette can be rerandomized by toggling the mod.
		- RandomByRoom: each room receives its own random tint. The palette can be rerandomized by toggling the mod.
		- UsePaletteFromSettings: each map zone will receive the color tint defined in the GlobalSettings Palette
		- Disco: the random tint is changed on a fixed time increment.
		- LighterColors: colors are chosen randomly with RGB in [0.5,1.0] and A = 0.5.
		- DarkerColors: colors are chosen randomly with RGB in [0.0,0.5] and A = 1.0.
	- FloatValues
		- DiscoTimer: the time increment between color changes when Disco is active. Set to 0.75 seconds, by default.
	- Palette
		- A dictionary of the form {MapZone}.{rgba}
		- For each key, set the value on the corresponding line
		- For Unity colors, each component should be a float in [0,1]. The mod will not parse colors on a [0, 255] scale.