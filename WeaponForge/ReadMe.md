# WeaponForge

**WeaponForge** is a customizable weapon creation system for V Rising, developed to enhance gameplay by allowing users to forge weapons with infusions, modifiers, and a purchase system.

## Features
- **Weapon Crafting**: Create a variety of weapons with customizable infusions and modifiers.
- **Purchase System**: Requires specific in-game resources for weapon creation.
- **Highly Configurable**: Server owners can adjust settings via a configuration file.
- **Infusion and Modifier Support**: Adds depth to weapon crafting, with infusions like Blood, Chaos, and Frost, and a variety of stat modifiers.

## Installation
1. Download the WeaponForge mod files.
2. Place the files in your `BepInEx/plugins` folder.
3. Launch your server and adjust the configuration file located at `BepInEx/config/WeaponForge.cfg`.
4. Make sure Bloodyshop, Bloodcore, Bloodstone are all installed within the folder as well.

## Usage
1. Use the `.wf [weaponType] [infusionType] [modifiers]` command in-game to craft a weapon.
   - Example: `.wf spear blood 123` creates a spear infused with Blood and modifiers for Attack Speed, Damage Reduction, and Max Health.
2. To see a list of available modifiers, use the `.modifiers` command in-game.
3. Can be reloaded with !reload

## Configuration
All settings can be adjusted in the `WeaponForge.cfg` file. You can specify:
- The type and amount of resources required for crafting.
- Enable or disable specific features.
- Customize weapon stats and infusions.

## Credits
**Developed by**: Darrean Schrom-Treffiletti   
**Vrising Username**: Inility


## License
- You are free to use and modify this code.
- Redistribution is allowed.

## Contact
For issues, feedback, or feature requests:
- **V Rising Username: Inility typically.
- ** Join my discord: https://discord.gg/9n3yN3VBVn
