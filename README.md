# Crop's Kingdom

Crop's Kingdom is my entry for GameLoop's RTS class ([GL-RTS](https://forum.gameloop.it/d/566-gl-rts-build-a-multiplayer-rts-with-gameloop)).

## Introduction

The game is a multiplayer real-time strategy battle among atleast 2 players (up to 6). Every player starts as a vegetable-faction with a king (a controllable unit), the headquarter and a builder. The objective is to kill others' kings while defending your own. If your king dies, you lose the game.

To do so, you can build your vegetable-kingdom by recruiting your army, collecting resources, creating new buildings, advancing in the technology tree faster than your opponents, etc.

## Goals

* Facing the challenge of building a RTS game
* Improving my network programming skills
* Creating a multiplayer, wannabe-competitive game experience (in the end)

## Factions

I start with few factions (I think Potatoes and Tomatoes, just for testing), but of course any vegetable can be a faction. When I'll complete this project, I'll take care of adding more factions (eggplants, carrots, etc).
Factions have different graphics, matching with the different vegetable they represent. They also have unique king's abilities and some differences in their units/buildings.

## Units

Units are controllable entities that can perform specific tasks for you. You can't create more units than the current maximum units limit.

* __*King*__: the most important unit. It has special faction abilities and protecting its life is your interest in order to win the game. When there are no allied buildings in its range, it starts to lose HP.
* __*Builder*__: the unit who can create buildings by spending resources, loot resources from the environment and work in buildings if needed. Low HP, usually no special abilities.
* __*Fighter*__: the unit who can deal damage to opponent units and buildings. There are multiple types of fighters: ranged dps, melee dps, melee tank. They can have different statistics, abilities and spells based on their faction.

## Buildings

Buildings are units with fixed position and specific behaviors. They allow you to unlock some features, they can be destroyed by yourself or by the enemy units. They cannot be moved.

* __*Headquarter*__: it produces builders and has some powerups/abilities based on the faction. From this building the water irrigation starts. _If you lose it and you have no builders, you basically can't build anything anymore!_
* __*Garden*__: it produces fighters, by seeding the terrain.
* __*Botanic Lab*__: it allows you to unlock new abilities and technologies by investing resources and time.
* __*House*__: it allows you to increase the maximum units limit in order to create more units.
* __*Resources processing*__: these kind of buildings allow you to process resources in a specific way. Example: they can transform wood in compost, etc.
* __*Water well*__: it increases the water production and extends the water irrigation range.

## Resources

Resources can be produced and extracted from the environment or with specific buildings/units. They allow you to build everything in your kingdom.

* __*Wood*__: it can be extracted with a builder from surrounding trees.
* __*Compost*__: the main currency in this world.
* __*Water*__: the main source of energy in this world.

## Constraints

GL-RTS has some constraints we have to implement for each entry:

* __*camera management system*__: the camera should be moved with arrow keys or by moving the mouse pointer to the screen's borders. It also has to focus the selected unit or (if no units selected) your king.
* __*grid management system*__: for building, pathfinding, etc.
* __*building management system*__: for buildings creation and management. Buildings creation is contrained to the grid system. Building something requires resources and time (both requirements can be "0" for no-cost, instantly building).
* __*resources management system*__: for resources management.
* __*units management system*__: for units management.

## Milestones

> TODO