using Godot;
using System;

/*
  A brain is a base class for scripts that control an Actor's behavior.
*/

public class Brain {
  public Actor actor;
  
  public Brain(Actor actor){
      this.actor = actor;
  }
  
  public virtual string ToString(){
    return "Brain: Base brain";
  }

  /* Called from update loop. Override this to control the actor.*/
  public virtual void Update(float delta){
    GD.Print("Please override this method:Update");
  }
  
}