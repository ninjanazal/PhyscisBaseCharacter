# Physics Base Character

Physics base character for mobile test

## ** CHANGE LOG **

- (2-10) - Since is a mobile game, may drop the physics part of the game.
  screen Coords to canvas position is working well.

# Base Model

Base character model, with bones and painted influence

- [x] Start base model
- [x] Better base model
- [x] Bones
- [x] Base weight painting

# Mechanic implementation

- [x] Base mesh on basic ragdoll
  - - [x] Configurable joint set
- [x] Movement to active ragdoll
- [x] Active ragdoll response to player input
  - - [x] Horizontal movement
      - - [x] Correct player rotate to direction
  - - [x] Slope Movement
  - - [x] Gravity implementation
  - - [x] Jumping
  - - [x] Player Stunt response
      - - [x] Correc stunt resonse
  - - [ ] Player action response
    * - [ ] Grab/Drag Objects

# Information system (Mobile adaptation)

- [x] Mobile Implementation
  - - [x] JoyStick Implementation
  - - [x] Jump Screen Action
- [ ] Inprove performace issue

## NOTE::

- (DONE) Implementing correct translation from pixel positions to correct canvas position
- (TODO) Improving performance for the game. ps:: reduce render reduce the

### Debug Info

- [x] Fps information
- [x] Debug information

# Dev Notes

- Current notes
  - Grabing objects
- Setting Game to Mobile
  - Switch input implementation to this platform
