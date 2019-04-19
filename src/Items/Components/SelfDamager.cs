using Godot;
using System;
using System.Collections.Generic;

public class SelfDamager {
    IItem item;
    public Damage damage;
    public bool destroyOnUse;

    public SelfDamager(IItem item){
        this.item = item;
    }
    public void Config(
        Damage damage,
        bool destroyOnUse
    ){
        this.damage = damage;
        this.destroyOnUse = destroyOnUse;
    }

    public void ApplyDamage(){
        IReceiveDamage recipient = item.GetWielder() as IReceiveDamage;
        if(recipient != null && damage != null){
            recipient.ReceiveDamage(damage);
        }

        if(destroyOnUse){
            item.GetNode().QueueFree();
        }
    }
}