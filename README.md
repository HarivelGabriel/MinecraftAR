# MinecraftAR
## AR version of Minecraft

### Presentation

Minecraft is one of the most popular game of this millenium. Player can explore (almost) infinite map composed of `Blocks`. He can destroy every block and replace them to create buildings, and everything he wants. There are particular rules to place Blocks.
  - Air isn't a Block.
  - Blocks can only be placed next to another Block. (But if you destroy the only Block next to another one, it persists.
  - Blocks are not affected by gravity.

These maps are stored in a `chunk` format. It represents a square of 16 x 16 (=256) Blocks. and each chunk are composed of several `sections`, they are cube of 16 x 16 x 16 (=4096) Blocks.

The goal of my project is to load one of these chunk, and create an augmented reality version of it in Unity with Vuforia Engine. It must also allow the player to interact with the world by placing new Blocks.

### Why Minecraft ?

I've played a lot to this game, I know it very well and it has always been a dream to see my creations appear in "real life".
Also it is easy to have a nice looking final version because every texture in the game can be found easily on the Internet.

### How does it work ?

First of all I need to parse a Minecraft save file. I decided to do it in Python because there is Library named `Anvil-Parser` (more info [here](https://github.com/matcool/anvil-parser)). I transform this file into a .txt file named `Assets/Ressources/chunkfile.txt`. This file contain in each line the coordinates of a Block and his ID (nature of the Block) in this format : `x;y;z;ID`

Then I created a prefab Object representing a Block. It is 6 oriented planes which make a Cube.

Based on chunkfile.txt, I Instantiate a Block on the target or not. Then I apply to this Block the Texture corresponding to the ID.

Now the minecraft world is set.

Another Target contains a new Block. When this target is inside the world, the block is automatically moved to a Block slot. X and Z coordinates are rounded, and a List of 256 int contains the highest value possible for a block in the Y coordinates. The user can change its texture then place it in the world.

### Difficulties

The main difficulty was time, many important features are lacking.
- I could only import 253 textures on more than 500.
- Some Blocks are not Cube and need a prefab object to Instantiate them (torch, snow, plants...).
- In Minecraft, the world has a right-handed orientation while Unity has a left-handed orientation. It confused me a lot.
- I couldn't create a Minecraft file parser inside Unity, I need to do it my self and then import it in Unity. There are not any way to change of section displayed.
- I couldn't create a scrolling menu nor a serach tool to select a particular Texture for the new block.

### Installation
 1. Clone the project
2. Add a Unity default Library and Log folder
3. Import Vuforia Engine package (download for Unity [here](https://developer.vuforia.com/downloads/SDK))
4. Open the project, check that Vuforia Objects are fine (AR Camera, WorldTarget, BlockTarget)
5. Open AR Camera configuration, place your Vuforia Licence and choose `Simultaneous Target = 2`

