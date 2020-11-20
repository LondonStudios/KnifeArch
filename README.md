# KnifeArch - London Studios
**KnifeArch** is a **FiveM** resource coded in **C#** allowing you to setup walk-through metal detectors which can be triggered by other players, alerting you and playing an external, custom sound as it is activated.

The time has come, you are now able to run **knife arch operations** and ensure that players are not carrying anything they shouldn't be!

The included model has been taken from the DLC Casino Heist, and re textured.

This plugin is made by **LondonStudios**, we have created a variety of releases including TaserFramework, SearchHandler, ActivateAlarm, SmartTester, SmartSounds, CustodyAlarm, SmartObservations and more!

Join our Discord [here](https://discord.gg/AtPt9ND) for exclusive plugin previews.

Video demo: [here](https://streamable.com/xhopxq).


<a href="https://www.buymeacoffee.com/londonstudios" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" style="height: 51px !important;width: 217px !important;" ></a>

![KnifeArch](https://i.imgur.com/rGI87K3.png)

## Usage
**/arch [setup/remove]** - Setup or remove a knife arch at your location.
**/archdetect [true/false]** - Set whether you will activate the arch when walking through. If **DetectedWeapons** is not empty in the configuration, it will detect the specified weapons regardless if this is turned on or off.

The **/arch** command is able to use AcePermissions, this must be enabled in "config.ini".

The included **Activation** sound will play upon another player activating the metal detector.

You are only able to remove knife arches you have created and you may only setup one at a time.
## Installation
 1.  Create a new **resource folder** on your server.
 2.  Add the contents of **"resource"** inside it. This includes:
"Client.net.dll", "Server.net.dll", "fxmanifest.lua", "html", "stream", "config.ini", "SharpConfig.dll"
3. In **server.cfg**, "ensure" KnifeArch, to make it load with your server startup.
## Configuration
The "config.ini" file allows you to enable permissions for the /arch command, which means you will have to give players access in your Server.cfg or another whitelisting method.

    [KnifeArch]
    PermissionsEnabled = false
    SoundVolume = 0.6 # This must be below 1.0
    SoundRadius = 30.0 # We recommend 30.0
    DetectedWeapons = { weapon_dagger, weapon_machete, weapon_knife }

On **line 2**, you can set **PermissionsEnabled** to "true" or "false. The /archdetect command is not included in this.
On **line 3**, you can set the **SoundVolume**, this must be below 1.0.
On **line 4**, you can set the **SoundRadius**, we recommend keeping this at 30.0.
On **line 5**, you can set the **DetectedWeapons**, this is an array of weapon names. Set to { } to disable detection of weapons automatically.
  
## Source Code
Please find the source code in the **"src"** folder. Please ensure you follow the licence in **"LICENCE.md"**.

## Feedback
We appreciate feedback, bugs and suggestions related to KnifeArch and future plugins. We hope you enjoy using the resource and look forward to hearing from people!

## Screenshots
Take a look at some screenshots of the plugin in action!

![KnifeArch](https://i.imgur.com/7zAKuUL.png)
![KnifeArch](https://i.imgur.com/8vnukGE.png)
![KnifeArch](https://i.imgur.com/PkXtdea.png)
![KnifeArch](https://i.imgur.com/EnktRJ0.png)
