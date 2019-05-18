using Godot;
using System;
using System.Collections.Generic;

public class ActorFactory {
  public enum InputSources {
    None,
    Player1, // Keyboard input
    Remote,
    AI
  };

  public enum StatsHandlers {
    None,
    Icepaws
  };

  public enum InventoryHandlers {
    None,
    Simple
  };

  public enum Bodies {
    None,
    PillBody
  };


  public enum Characters {
    None,
    Debug,
    Target
  };

  public static Actor FromComponentTypes(
    InputSources inputSource,
    StatsHandlers statsHandler,
    Bodies body,
    InventoryHandlers inventoryHandler
  ){
    Actor actor = new Actor();
    InitInputHandler(inputSource, actor);
    InitStats(statsHandler, actor);
    InitBody(body, actor);
    InitInventory(inventoryHandler, actor);
    return actor;
  }

  public static void InitInputHandler(InputSources inputSource, Actor actor){
    FPSInputHandler fps;
    MappedInputSource mapped;
    switch(inputSource){
      case InputSources.Player1:
        mapped = new MappedInputSource(Session.GetDevice(0), FPSInputHandler.GetMappings());
        fps = new FPSInputHandler(actor);
        fps.RegisterInputSource(mapped as IInputSource);
        actor.inputHandler = fps as IInputHandler;
      break;
      case InputSources.Remote:
        // Set up net source
      break;
      case InputSources.AI:
        // Set up AI
      break;
    }
  }

  public static void InitStats(StatsHandlers statsHandler, Actor actor){
    switch(statsHandler){
      case StatsHandlers.Icepaws:
        actor.stats = new IcepawsStats();
      break;
    }
  }

  public static void InitBody(Bodies body, Actor actor){
    switch(body){
      case Bodies.PillBody:
        actor.body = new PillBody(actor);
      break;
    }
  }

  public static void InitInventory(InventoryHandlers inventoryHandler, Actor actor){
    switch(inventoryHandler){
      case InventoryHandlers.Simple:
        // Set up a simple inventory
      break;
    }
  }

  public static Actor FromName(string name){
      GD.Print("Making character: " + name);
      return FromCharacter(Characters.Debug);
  }

  public static Actor FromCharacter(Characters character){
    Actor ret = null;
    switch(character){
      case Characters.Debug: // Test player1
        ret = DebugCharacter();
      break;
      case Characters.Target: // Won't fight back.
        ret = TargetCharacter();
        break;
    }
    return ret;
  }

  public static Actor DebugCharacter(){
    Actor actor = FromComponentTypes(InputSources.Player1, StatsHandlers.Icepaws, Bodies.PillBody, InventoryHandlers.None);
    actor.stats.SetStat("intelligence", 5);
    actor.stats.SetStat("charisma", 5);
    actor.stats.SetStat("endurance", 5);
    actor.stats.SetStat("perception", 5);
    actor.stats.SetStat("agility", 5);
    actor.stats.SetStat("willpower", 5);
    actor.stats.SetStat("strength", 5);
    actor.stats.RestoreCondition();
    IItem item = ItemFactory.Factory(ItemFactory.Items.Knife);
    actor.hotbar.AddItem(0, item);
    actor.body.InitCam(0);
    return actor;
  }

  public static Actor TargetCharacter(){
    Actor actor = FromComponentTypes(InputSources.None, StatsHandlers.Icepaws, Bodies.PillBody, InventoryHandlers.None);
    actor.stats.SetStat("intelligence", 5);
    actor.stats.SetStat("charisma", 5);
    actor.stats.SetStat("endurance", 5);
    actor.stats.SetStat("perception", 5);
    actor.stats.SetStat("agility", 5);
    actor.stats.SetStat("willpower", 5);
    actor.stats.SetStat("strength", 5);
    actor.stats.RestoreCondition();
    IItem item = ItemFactory.Factory(ItemFactory.Items.Knife);
    actor.hotbar.AddItem(0, item);
    return actor;
  }
}