# RTSEngine

Case study of a **Real-Time Strategy**(RTS) Game.

The idea is to understand and use advanced program unity techniques and good practives as Clean Code/Archtecture and SOLID principles to build a RTS game like **Warcraft3** and **Age of Empires**.

The structure was think to be **reusable**, with greate possibility to expansion.

**Samples** are made to show single pieces of structures as well as integrated structures.

## ToDo List

- [ ] **UI Component separation** - Make the UI components separated 
- [ ] **UI Component samples** - Create Sample Scene for each UI Component 
- [ ] **Log Manager** - Make the game log system reusable 
- [ ] **UI hint Manager** - Create manager to hint panel 
- [ ] **Pre-Selection** - Show which units are going to be selected on mouse drag
- [ ] **Status** - Show model status on UI
- [ ] **Update status** - Update selecion whenever it's statuses changes
- [ ] **Show/Hide Status Bar** - Show/Hide life/mana status bar on scene  
- [ ] **Outline Model** - Outline model when mouse over
- [ ] **Component Factory** - Separate the component factory from installers
- [ ] **Panels Separation** - Separate the Panels from installers
- [ ] **Actions** - Show possible actions of selected model
- [ ] **Action Manager** - Create a system to manage the actions activation/deactivation. 
   * Active; 
   * Passive(RightClick to enable); 
   * Confirm(yes/no); 
   * Interactive(atack, place build, move, heal, etc...); 
- [ ] **Items** - Create item system
- [ ] **Items Reorder** - Manage items reordering
- [ ] **Items Use** - Manage item consumption (shoud be like actions)
- [ ] **Ghost** - Put a ghost of the selected model on mouse to show what is being instantiated(Sample scene)
- [ ] **MiniMap** - Create a minimap showing every model position
- [ ] **MiniMap Interaction** - The camera must go to de clicked position on minimap
- [ ] **Map Manager** - Create a manager to hold map data (size, style, etc...)
- [ ] **Save/Load Map** - Create a manager to Save and Load map data (size, style, model location, etc...)
- [ ] **Model Separation** - Create and manage Player/Ally/Enemy/Environment separation system
- [ ] **Model creation Editor** - Adjust Editor for model creation
- [ ] **Mouse Icon** - Change mouse icon


## Technologies
- NSubstitute
- Extenject

## Techniques
- Clean Code
- SOLID principles
- TDD