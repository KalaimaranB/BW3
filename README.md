# BW3 README

## Scripts Overview

### AirTransport.cs
*AirTransport* manages air vehicle behavior in Unity, including movement control, interaction with passengers, and handling player orders through NavMeshAgent for flexible navigation.

### BasicUnit.cs

*BasicUnit* defines fundamental attributes and behaviors for game units in Unity, encompassing identification, health management, targeting capabilities, and status transitions between AI-controlled and player-ordered states.

### CameraInfo.cs

*CameraInfo* manages camera follow behavior in Unity, allowing customization of the offset from the target object using a Vector3 parameter.

### AutoGun.cs

*AutoGun* handles automatic targeting and firing mechanics in Unity, utilizing physics-based methods like sphere or box overlap to detect and engage designated enemy units.

### CursorManager.cs

*CursorManager* manages cursor behavior and interactions in the game, including target locking and cursor states.

### DelayedDeath.cs

*DelayedDeath* in Unity allows delayed destruction of game objects after a specified time period, ensuring objects are destroyed only after a set duration.

### Facility.cs

*Facility* in Unity manages a strategic building that spawns various units based on its type (Barracks, Factory, etc.), animates faction-specific appearances, and handles faction switching events.

### FactionManager.cs

*FactionManager* in Unity manages faction-specific units using `FactionUnits` for each faction, handling unit creation and destruction events across different factions in the game.

### Finder.cs

*Finder* provides static methods to find and manage game objects and components within the game scene.

### Flagpole.cs

*Flagpole* in Unity manages the control and appearance of a flag associated with different factions, dynamically changing its height and material based on control values, and triggering events when captured by a new faction.

### FactionTypes.cs

*FactionUnits* manages collections of infantry, vehicle, aviation, and naval units for various factions in Unity, facilitating unit creation, destruction, and search operations based on unit type and class. *Unit* represents a specific unit type with attributes like name, icon, prefab, and current instances in the game.

### MinimapCamera.cs

*MinimapCamera* adjusts the position and size of the camera and radar pulse on the minimap in Unity. It positions the radar pulse relative to the current player's position with an offset, sets the camera's position similarly with another offset, and adjusts the camera's orthographic size based on the maximum range of the radar pulse.

### Health.cs

*Health* manages the health and destruction behavior of a GameObject in Unity. It includes fields for maximum and current health, a death style enum determining the action upon death, an armor type enum for damage calculation, an explosion GameObject for visual effects, and an event for when the unit dies.

### Helipad.cs

*Helipad* manages various transport calls based on faction activation and contains positions for landing and movement.

### HelipadTower.cs

*HelipadTower* manages faction-specific interactions with a helipad, dynamically adjusting visuals and triggering transport calls based on captured status.

### ObjectiveManager.cs

*ObjectiveManager* handles primary and secondary objectives in a game scene, tracking their states and managing their completion criteria such as reaching positions, protecting targets, or destroying targets. It categorizes objectives into primary and secondary types and provides methods to query their current state.

### Identification.cs

*Identification* manages identification and categorization for game entities, distinguishing between factions, unit types, statuses, and classes. It supports dynamic initialization and destruction callbacks based on faction affiliation, facilitating game-level management and interaction.

### LevelManager.cs

*LevelManager* orchestrates game-level management and unit tracking, featuring dynamic updates for player and enemy teams based on unit creation and destruction events. It supports categorization of objectives and dialogues, with real-time updates reflecting mission completion states and team resources.

### PlayerManager.cs

*PlayerManager* manages player-related functionalities, including player units, teams, and player-controlled actions.

### ObjectPoolManager.cs

*ObjectPoolManager* facilitates object pooling in Unity, managing reusable GameObject instances via designated pools. It dynamically creates and activates objects from pools based on availability, optimizing performance by reducing instantiation overhead and enabling efficient object reuse across scenes.

### PlayerSelectedObjectUI.cs

*PlayerSelectedObjectUI* manages UI elements for displaying details of selected GameObject targets in Unity. It dynamically updates health bars and textual information based on the selected target's Identification and Health components, ensuring clear visual feedback for interactive gameplay scenarios.

### Projectile.cs

*Projectile* is a Unity script that represents a projectile fired in-game. It handles movement, collision detection, and damage application to targets based on collisions with the `Health` component. The `Shooter` GameObject is referenced to avoid self-damage and manage collision behavior, ensuring smooth gameplay mechanics in interactive scenarios.

### RotateContinuously.cs

*RotateContinuously* is a Unity script designed to rotate a GameObject continuously along a specified axis at a customizable speed. It utilizes the `Update` method to apply rotation based on the chosen `Direction` enum (x, y, z), adjusting rotation speed dynamically with `RotationsPerMinute`. This script is ideal for creating spinning visual effects or animated elements within a Unity scene, enhancing interactive and dynamic gameplay experiences.

### Tank.cs
*Tank* is a Unity script representing a tank unit with various functionalities for movement, targeting, and firing projectiles. It inherits from `BasicUnit` and includes components such as a main turret, barrel aiming, and navigation using Unity's `NavMeshAgent`. Features include:
- **Movement**: Controls tank movement based on player input, including rotation and translation.
- **Targeting and Firing**: Uses a `TargetingSystem` to acquire and attack targets, adjusting main turret and barrel positions dynamically.
- **Projectile Firing**: Fires projectiles at targets within range, with customizable reload times and projectile types.
- **AI Behavior**: Implements AI-controlled behavior to follow players or engage enemies based on unit status and targeting.

This script integrates with Unity's physics and navigation systems, offering a robust framework for implementing tank units with interactive and dynamic gameplay mechanics.

### PlayerUnitIcon.cs

*PlayerUnitIcon* manages the UI representation of player units, displaying their icons, names, status, and unit counts dynamically. It allows switching between player units and updating their statuses based on game events.

### PlayerUnitIcons.cs

*PlayerUnitIcons* manages a collection of *PlayerUnitIcon* instances, displaying icons for active units and allowing navigation between them. It dynamically updates unit icons based on active units and handles player interactions like changing unit status and managing UI transparency.

### POWCamp.cs

*POWCamp* simulates a prisoner-of-war camp, initializing with ammo dumps and prisoners. It triggers an event to free prisoners upon ammo dump destruction, changing prisoner statuses and generating visual effects.

### ProgressBar.cs

*ProgressBar* creates a customizable progress bar UI element. It features options for title, colors, alert thresholds, and audio alerts. It updates dynamically in both edit mode and during gameplay, reflecting changes in values and triggering alerts when thresholds are met.

### RadarPing.cs

*Radar Ping* is used for creating a fading radar ping effect, dynamically adjusting its color and disappearance time based on parameters set during runtime.

### RadarPulse.cs

*Radar Pulse* generates radar pulse effects in Unity, expanding dynamically with a customizable range and fading based on faction colors. It spawns radar pings on detected units within the pulse range, displaying faction-specific colors and managing fading effects based on distance.

### ReachPosition.cs
*ReachPosition* detects when the player reaches a specific position in the game world. It sets a boolean flag `reached` to true upon collision with the player's GameObject, indicating successful arrival at the designated location.

### TargetingSystem.cs

*TargetingSystem* facilitates unit targeting based on specified conditions (unit type, faction, targeting method). It uses PhysicsOverlapSphere or PhysicsOverlapBox methods to detect valid targets within specified ranges. It dynamically updates and manages target lists for AI-controlled entities.

### TransportPassenger.cs

*TransportPassenger* manages the transportation of game objects using NavMeshAgent. It includes methods for basic and advanced transport scenarios, setting destinations, and managing interactions and movement.

### Utilities.cs

*Utilities* provides utility functions for common tasks in Unity development

### Wreckage.cs

*Wreckage* darkens the materials of specified `MeshRenderer` components to simulate visual damage or aging effects.

## Gameplay Overview

This game project involves strategic gameplay elements where players manage units, including transportation, targeting enemy factions, and managing resources like POW camps and ammo dumps. Players interact with UI elements such as progress bars to monitor and respond to in-game events effectively.

## Notes

My favourite video game is by far Battalion Wars 2 and this project was a spin-off of that. I don't have copyright for that game, so can not include all gamefiles online. 
This project shows my in-depth OOP skills. There is a lot of interaction betweent the scripts that is carefully managed. However, the project was not completed due to the fact that my local device could not support the graphics needed to work much past this point. The program needed to be optimized more, so I took a break and started learning more code to optimize my programming skills. 

## Acknowledgements

Major thanks go to Unity CodeMonkey and his dedicated youTube channel. I learned so much from his channel that helped in developing this project.
