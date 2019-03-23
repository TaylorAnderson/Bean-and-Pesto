using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using InControl;
public class CharacterActions : PlayerActionSet {
  public PlayerAction Select;
  public PlayerAction Left;
  public PlayerAction Right;
  public PlayerAction Down;
  public PlayerAction Up;
  public PlayerAction Shoot;
  public PlayerAction Switch;
  public PlayerTwoAxisAction Move;

  public CharacterActions() {
    Select = CreatePlayerAction("Select");
    Left = CreatePlayerAction("Move Left");
    Right = CreatePlayerAction("Move Right");
    Up = CreatePlayerAction("Move Up");
    Down = CreatePlayerAction("Move Down");
    Shoot = CreatePlayerAction("Shoot");
    Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
    Switch = CreatePlayerAction("Switch Ships");






  }
}
