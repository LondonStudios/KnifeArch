![London Studios](https://i.ibb.co/1mwSS1q/Untitled-design.png)

# This resource is archived

**KnifeArch** is a free FiveM resource we released in 2020. It is now **archived** and **no longer supported**.

If you want a modern, maintained knife arch (and a full set of police equipment alongside it), get **Police Tools** from our store:

### **[Get Police Tools](https://store.londonstudios.net/package/police-tools)**

---

# Upgrade to Police Tools

Since forming London Studios in April 2020 we've created a number of **high quality** and **premium** resources for the FiveM project, focusing on the emergency services and aiming to bring your server to the next level.

Although we made a number of free resources such as this one in the first year, we've now switched to creating paid content, keeping them constantly updated and working along with providing the best possible support to our customers.

With **thousands** of **happy customers** we are confident you'll love our resources and our active support team are on hand to help if you have any questions!

**Police Tools** is the successor for knife arch operations. It includes an improved **Knife Arch** with temporary or permanent placement, weapon and item scanning, and a cleaner placement flow, plus a full toolkit for everyday policing.

### Knife Arch in Police Tools

Place temporary or permanent metal detection arches, position and rotate them in-world, then scan players walking through for configured weapons and items. Ideal for knife arch operations, checkpoints and station security.

![Police Tools](https://img.londonstudios.net/cdn-cgi/image/anim=false,format=jpeg,quality=70,width=720/policetools-gifs/arch.gif)
![Police Tools](https://img.londonstudios.net/cdn-cgi/image/anim=false,format=jpeg,quality=70,width=720/policetools-gifs/wand.gif)

### Also included in Police Tools

* **Cuffing, dragging and tackling** - Restrain players, escort them, and take them to the ground
* **Seat in vehicle** - Place a cuffed person into a nearby vehicle, or take them back out
* **Hands up and knees** - Raise hands or kneel with hands up
* **Radial menu** - Quick access to player actions and tools (default **F6**)
* **Megaphone** - Handheld and vehicle megaphone support with popular voice systems
* **Police notepad** - Multi-page notes you can share, copy and save between sessions
* **Search wand** - Scan the nearest person for configured weapons and items
* **Speed gun** - Read speed and plate, with optional fining through Smart Speed Cameras
* **Warrant card / police ID** - UK or USA style ID shown to nearby players
* **Flashlight** - Police flashlight with a beam visible to nearby players
* **PAVA spray** - Handheld irritant spray for nearby players (and optional pedestrians)
* **Stingers** - Place, deploy and pick up spike strips with a handheld remote
* **Permissions, notifications and item support** - Ace, QBCore, ESX, vRP, QBX and common inventories

![Police Tools](https://img.londonstudios.net/policetools/img3.png)
![Police Tools](https://img.londonstudios.net/policetools/img4.png)
![Police Tools](https://img.londonstudios.net/policetools/img5.png)
![Police Tools](https://img.londonstudios.net/policetools/img6.png)

# Our store: https://store.londonstudios.net/package/police-tools
# Our discord: https://discord.gg/nC2krpN

Documentation: [Police Tools docs](https://docs.londonstudios.net/london-studios-documentation/resources/police-tools)

---

# KnifeArch (legacy) - London Studios

> **Archived / unsupported.** This free resource is likely **out of date** and is **no longer supported**. The full source code remains available if you wish to maintain it yourself. For an updated knife arch with ongoing updates and support, use **[Police Tools](https://store.londonstudios.net/package/police-tools)**.

**KnifeArch** is a **FiveM** resource coded in **C#** allowing you to setup walk-through metal detectors which can be triggered by other players, alerting you and playing an external, custom sound as it is activated.

The time has come, you are now able to run **knife arch operations** and ensure that players are not carrying anything they shouldn't be!

The included model has been taken from the DLC Casino Heist, and re textured.

This plugin is made by **LondonStudios**. Join our Discord [here](https://discord.gg/nC2krpN) for support on our paid resources and exclusive plugin previews.

Video demo (legacy): [here](https://streamable.com/xhopxq).

<a href="https://www.buymeacoffee.com/londonstudios" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" style="height: 51px !important;width: 217px !important;" ></a>

![KnifeArch](https://i.imgur.com/rGI87K3.png)

## Usage (legacy)

**/arch [setup/remove]** - Setup or remove a knife arch at your location.
**/archdetect [true/false]** - Set whether you will activate the arch when walking through. If **DetectedWeapons** is not empty in the configuration, it will detect the specified weapons regardless if this is turned on or off.

The **/arch** command is able to use AcePermissions, this must be enabled in "config.ini".

The included **Activation** sound will play upon another player activating the metal detector.

You are only able to remove knife arches you have created and you may only setup one at a time.

## Installation (legacy)

 1.  Create a new **resource folder** on your server.
 2.  Add the contents of **"resource"** inside it. This includes:
"Client.net.dll", "Server.net.dll", "fxmanifest.lua", "html", "stream", "config.ini", "SharpConfig.dll"
3. In **server.cfg**, "ensure" KnifeArch, to make it load with your server startup.

## Configuration (legacy)

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

This legacy resource is no longer supported. For bugs, suggestions and support on our current products (including Police Tools), join our [Discord](https://discord.gg/nC2krpN) or visit the [store](https://store.londonstudios.net/package/police-tools).

## Screenshots (legacy)

Take a look at some screenshots of the original plugin in action!

![KnifeArch](https://i.imgur.com/7zAKuUL.png)
![KnifeArch](https://i.imgur.com/8vnukGE.png)
![KnifeArch](https://i.imgur.com/PkXtdea.png)
![KnifeArch](https://i.imgur.com/EnktRJ0.png)
