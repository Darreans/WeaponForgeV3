# WeaponForge

**WeaponForge** is an open-source mod for *V Rising* that lets you craft unique weapons using a resource-based purchase system, diverse infusions, and powerful stat modifiers—no server restarts needed.

---

## Features

- **Custom Weapon Crafting**  
  Create weapons with infusion types like Blood, Chaos, Frost, and more, plus three customizable stat modifiers.

- **Resource-Based Purchase System**  
  Uses in-game items as “currency” to balance weapon creation.

- **Infusions & Modifiers**  
  Enhance gameplay with your choice of elemental infusions and stat boosts such as Attack Speed or Damage Reduction.

- **Easy Reload**  
  Change mod settings (costs, items, etc.) and instantly apply them by typing `!reload` (admin-only).

- **Open-Source & Community Driven**  
  Built on a strong foundation provided by the modding community—thank you!

---

## Installation

1. **Download & Extract**  
   - Grab the WeaponForge files from the repository or release package.

2. **Move to BepInEx**  
   - Copy them into your server’s `BepInEx/plugins` folder.

3. **Start & Configure**  
   - Launch the server, then edit `BepInEx/config/WeaponForge.cfg` and the JSON files for modifiers and settings.

---

## Usage

### Crafting a Weapon

Use this command in-game:
```
.wf [weaponType] [infusionType] [modifiers]
```
For example:
```
.wf spear blood 123
```
- **weaponType**: See the [Weapon List](#weapon-list) below.  
- **infusionType**: Must be one of the valid infusions (e.g., blood, chaos, frost, illusion, storm, unholy).  
- **modifiers**: A 3-character code specifying stat modifiers (e.g., `123`).

> **Tip**: To view all possible stat modifiers, type:  
> ```
> .wf ?
> ```
> (Note that `.modifiers` is **not** a command.)

### Reloading

After making changes to the configuration (like adjusting costs, items, or modifiers), you can reload the mod without restarting:
```
!reload
```
(Requires admin permissions.)

---

## Weapon List

Below are the **WeaponType** options you can use in the `.wf` command:
- `spear`
- `axe`
- `crossbow`
- `greatsword`
- `mace`
- `pistols`
- `reaper`
- `slashers`
- `whip`
- `sword`
- `longbow`

*(Capitalization is optional; the command is case-insensitive.)*

---

## Configuration

**Files**:  
- `BepInEx/config/WeaponForge.cfg`  
- `BepInEx/config/WeaponForge/WeaponModifiers.json`

**What You Can Change**:
- Required resource item type and amount (`Thistle` by default).
- Infusions and modifiers available for crafting.
- Whether the command is admin-only.
- Other toggles and balance settings.

Use `!reload` to instantly apply any changes in-game.

---

## Contact & Support

For any issues, feedback, or feature requests:
- **Developer**: Darrean (Discord: `inility#4118`)
- **Join my Discord (SanguineReign)**: [SanguineReign](https://discord.gg/SanguineReign)

---

## License

**WeaponForge** is an **open-source** mod. Feel free to use, modify, and redistribute. Special thanks to the V Rising modding community for paving the way for projects like this!

Enjoy forging epic weapons and don’t forget to visit **SanguineReign** for the latest updates!
