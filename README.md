# LudoLite

**Author:** Madasu Rakesh

**Contributors:**
- Madasu Rakesh
- Cursor

A simple UI with a Roll button to roll a virtual die and a Reset button to
return the chip to its initial position.


# Features
- A Ludo board game layout with a single game piece (chip) on one of the
starting positions.
- When the Roll button is clicked, the game displays a simple animation of the die
rolling, fetch a random number from an online service and show the final
number obtained from the die roll.
- When the chip is tapped, the chip is moved to the appropriate position based
on the last die roll.
- Unity's Addressables system has been integrated into the scene. Nevertheless, due to constraints related to time, only the chip and dice images have undergone the implementation of Addressables.


# Screen layout and supported resolutions
The whole game is implemented with Unity UI system. Dynamic calculations have been implemented to make it flexible enough to support various screen resolutions and orientations. 
However, portrait orientation is preferred if you want a better looking layout.


# User interface grid implementation
For path calculation simplicity: each of the four colors has its own 3x6 grid.
Yellow, blue, and red grids are rotated to meet game board layout but all path calculations are based on non-rotated grids to reduce complexity.


# Implementation entry points
- UI layout calculations start from `GamePanelUiComponent` class
- Gameplay logic and button functionality implemented in `GameplayManager` class.
- Chip move path calculations are implemented in `ChipMover` class
- Random number fetching is implemented in `RandomUtility` class
- Dice roll animation is implemented in `DiceAnimate` class.
