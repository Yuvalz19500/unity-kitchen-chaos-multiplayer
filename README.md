# Kitchen Chaos

Welcome to Kitchen Chaos, a multiplayer cooking game where players collaborate to prepare and serve dishes under time constraints. This project showcases the integration of Unity's powerful game development features with multiplayer networking, providing an engaging and challenging experience for players.

## Table of Contents

- Overview
- Features
- Technologies Used
- Game Mechanics
- Multiplayer Functionality
- Code Structure
- Setup and Installation
- Showcase
- Contributing
- License

## Overview

Kitchen Chaos is a fast-paced multiplayer game where players must work together to complete orders in a chaotic kitchen environment. The game emphasizes teamwork, time management, and quick decision-making. Players take on different roles, such as chopping ingredients, cooking dishes, and serving customers, to ensure the kitchen runs smoothly.

## Features

Multiplayer Gameplay: Play with friends or other players online, coordinating tasks to complete orders efficiently.
Variety of Recipes: A wide range of recipes with different preparation steps, adding complexity and variety to the gameplay.
User-Friendly Interface: Intuitive controls and a clean user interface make the game accessible to players of all skill levels.

## Technologies Used

Unity: The game engine used for developing the game, providing robust tools for 2D and 3D game development.
Unity Netcode for GameObjects: Used for implementing multiplayer functionality, allowing players to connect and play together online.
C#: The programming language used for scripting game logic and mechanics.
TextMesh Pro: For rendering high-quality text and UI elements.
ScriptableObjects: Used for managing game data such as recipes, kitchen objects, and player settings.
Game Mechanics
Cooking and Preparation
Players must follow specific steps to prepare dishes, including chopping ingredients, cooking them on the stove, and assembling the final dish. Each step requires different actions and coordination among players.

Orders and Time Management
Orders come in with a time limit, and players must prioritize tasks to complete them before the timer runs out. Successfully completing orders earns points, while failing to do so results in penalties.

Kitchen Layout
The kitchen layout changes periodically, introducing new obstacles and requiring players to adapt their strategies. This keeps the gameplay fresh and challenging.

Multiplayer Functionality
Networked Gameplay
The game uses Unity Netcode for GameObjects to handle multiplayer interactions. Players can host or join games, and the network code ensures smooth synchronization of game state across all clients.

Server and Client Logic
The game logic is divided into server and client components, with server-side code handling critical game state updates and client-side code managing player inputs and local interactions.

Code Structure
The project is organized into several key components:

Scripts: Contains all the C# scripts for game logic, including player controls, kitchen object interactions, and network management.
Assets: Includes all game assets such as sprites, animations, and UI elements.
Prefabs: Prefabricated game objects that can be reused throughout the game, such as kitchen counters, stoves, and player characters.
Scenes: Different game scenes, including the main menu, lobby, and various kitchen layouts.
Key Scripts
GameManager.cs: Manages the overall game state, including starting and ending games, tracking player progress, and handling game events.
KitchenGameMultiplayer.cs: Handles network-related functionality, including spawning and destroying kitchen objects, and managing player connections.
Player.cs: Manages player-specific actions and interactions within the game.
CuttingCounter.cs: Implements the logic for chopping ingredients on the cutting counter.
StoveCounter.cs: Manages cooking processes on the stove, including frying and burning mechanics.
Setup and Installation
To set up the project locally, follow these steps:

Clone the Repository:
Open in Unity: Open the project in Unity Hub and ensure you have the correct version of Unity installed.
Install Dependencies: Install any required packages via the Unity Package Manager, including Unity Netcode for GameObjects.
Build and Run: Build the project for your target platform and run the game.
Showcase
Gameplay Screenshot Multiplayer Lobby Cooking Action

Contributing
Contributions are welcome! If you have ideas for new features or improvements, feel free to fork the repository and submit a pull request. Please ensure your code follows the project's coding standards and includes appropriate documentation.

License
This project is licensed under the MIT License. See the LICENSE file for more details.
