/*
  The Device Manager returns a List of InputEvents received from
  a specific device. This abstracts the logic of collecting input
  from different devices from the actual handling of those inputs.
*/
using Godot;
using System;
using System.Collections.Generic;

public class DeviceManager {
  public enum Devices{MouseAndKeyboard, N64, Nes, Ps1, Snes};
  public Devices device;
  private int joyId;
  private bool mouseActive;
  private float sensitivityX = 0.2f;
  private float sensitivityY = 0.2f;

  
  List<bool> buttonsDown; // Store button states to check for changes.
  Vector2 mouseCur, mouseLast;

  public DeviceManager(Devices device, int joyId = -1){
    this.device = device;
    buttonsDown = new List<bool>();
    mouseActive = false;
    int buttonCount = 0;
    this.joyId = joyId;

    switch(device){
      case Devices.MouseAndKeyboard:
        Input.SetMouseMode(Input.MouseMode.Captured);
        mouseActive = true;
        buttonCount = 70;
        break;
      
      case Devices.N64:
        mouseActive = false;
        buttonCount = 14;
        break;

      case Devices.Nes:
        mouseActive = false;
        buttonCount = 4;
        break;

      case Devices.Ps1:
        mouseActive = false;
        buttonCount = 16;
        break;
      case Devices.Snes:
        mouseActive = false;
        buttonCount = 11;
        break;
    }

    for(int i = 0; i < buttonCount; i++){ 
      buttonsDown.Add(false); 
    }
  }
  
  public List<InputEvent> GetInputEvents(){
    switch(device){
      case Devices.MouseAndKeyboard: 
        return KeyboardEvents(); 
        break;
      case Devices.N64:
        return N64Events();
        break;
      case Devices.Nes:
        return NesEvents();
        break;
      case Devices.Ps1:
        return Ps1Events();
        break;
      case Devices.Snes:
        return SnesEvents();
        break;

    }
    return new List<InputEvent>();
  }

  public static string DeviceName(Devices deviceEnum){
    switch(deviceEnum){
      case Devices.MouseAndKeyboard:
        return "Mouse and Keyboard";
        break;
      case Devices.N64:
        return "Retro N64 Controller";
        break;
      case Devices.Nes:
        return "Retro NES Controller";
        break;
      case Devices.Ps1:
        return "Retro PS1 Controller";
        break;
      case Devices.Snes:
        return "Retro SNES Controller";
        break;
    }
    return "None";
  }
  
  private InputEvent Down(InputEvent.Buttons button){
      return new InputEvent(button, InputEvent.Actions.Down);
  }
  
  private InputEvent Up(InputEvent.Buttons button){
      return new InputEvent(button, InputEvent.Actions.Up);
  }
  
  private List<InputEvent> KeyEvents(int key, int buttonIndex, InputEvent.Buttons button){
    List<InputEvent> ret = new List<InputEvent>();
    
    bool press = Input.IsKeyPressed(key);
    
    if(key < 4){
      press = Input.IsMouseButtonPressed(key);
    }
    
    if(press && !buttonsDown[buttonIndex]){ 
      ret.Add(Down(button));
      buttonsDown[buttonIndex] = true;
    }
    else if(!press && buttonsDown[buttonIndex]){ 
      ret.Add(Up(button));
      buttonsDown[buttonIndex] = false;
    }
    
    return ret;
  }

  private List<InputEvent> ButtonEvents(int buttonId, int buttonIndex, InputEvent.Buttons button){

    List<InputEvent> ret = new List<InputEvent>();
    
    bool press = Input.IsJoyButtonPressed(joyId, buttonId);
    
    if(press && !buttonsDown[buttonIndex]){ 
      ret.Add(Down(button));
      buttonsDown[buttonIndex] = true;
    }
    else if(!press && buttonsDown[buttonIndex]){ 
      ret.Add(Up(button));
      buttonsDown[buttonIndex] = false;
    }
    
    return ret;

  }


  private List<InputEvent> Ps1Events(){
    List<InputEvent> ret = new List<InputEvent>();

    ret.AddRange(ButtonEvents(2, 0, InputEvent.Buttons.Square));
    ret.AddRange(ButtonEvents(3, 1, InputEvent.Buttons.Triangle));
    ret.AddRange(ButtonEvents(1, 2, InputEvent.Buttons.O));
    ret.AddRange(ButtonEvents(0, 3, InputEvent.Buttons.X));
    ret.AddRange(ButtonEvents(5, 4, InputEvent.Buttons.R1));
    ret.AddRange(ButtonEvents(5, 5, InputEvent.Buttons.L1));
    ret.AddRange(ButtonEvents(11, 6, InputEvent.Buttons.Start));
    ret.AddRange(ButtonEvents(10, 7, InputEvent.Buttons.Select));
    ret.AddRange(ButtonEvents(9, 8, InputEvent.Buttons.RClick));
    ret.AddRange(ButtonEvents(8, 9, InputEvent.Buttons.LClick));
    ret.AddRange(ButtonEvents(7, 10, InputEvent.Buttons.R2));
    ret.AddRange(ButtonEvents(6, 11, InputEvent.Buttons.L2));
    ret.AddRange(ButtonEvents(12, 12, InputEvent.Buttons.DUp));
    ret.AddRange(ButtonEvents(15, 13, InputEvent.Buttons.DRight));
    ret.AddRange(ButtonEvents(13, 14, InputEvent.Buttons.DDown));
    ret.AddRange(ButtonEvents(14, 15, InputEvent.Buttons.DLeft));
    ret.AddRange(AxisEvents(0, 1, 0.005f, InputEvent.Axes.Left, false, true));
    ret.AddRange(AxisEvents(2, 3, 0.005f, InputEvent.Axes.Right, false, true));

    return ret;    
  }

  private List<InputEvent> NesEvents(){
    List<InputEvent> ret = new List<InputEvent>();

    ret.AddRange(ButtonEvents(1, 0, InputEvent.Buttons.A));
    ret.AddRange(ButtonEvents(16, 1, InputEvent.Buttons.B));
    ret.AddRange(ButtonEvents(3, 2, InputEvent.Buttons.Start));
    ret.AddRange(ButtonEvents(11, 3, InputEvent.Buttons.Select));
    ret.AddRange(AxisEvents(0, 1, 0.005f, InputEvent.Axes.Left, false, true));

    return ret;    
  }

  private List<InputEvent> SnesEvents(){
    List<InputEvent> ret = new List<InputEvent>();

    ret.AddRange(ButtonEvents(1, 0, InputEvent.Buttons.A));
    ret.AddRange(ButtonEvents(16, 1, InputEvent.Buttons.B));
    ret.AddRange(ButtonEvents(0, 2, InputEvent.Buttons.X));
    ret.AddRange(ButtonEvents(2, 3, InputEvent.Buttons.Y));
    ret.AddRange(ButtonEvents(11, 4, InputEvent.Buttons.Select));
    ret.AddRange(ButtonEvents(3, 5, InputEvent.Buttons.R));
    ret.AddRange(ButtonEvents(10, 6, InputEvent.Buttons.L));
    ret.AddRange(AxisEvents(0, 1, 0.005f, InputEvent.Axes.Left, false, true));

    return ret;    
  }

  private List<InputEvent> N64Events(){
    List<InputEvent> ret = new List<InputEvent>();

    ret.AddRange(ButtonEvents(6, 0, InputEvent.Buttons.A));
    ret.AddRange(ButtonEvents(10, 1, InputEvent.Buttons.B));
    ret.AddRange(ButtonEvents(3, 2, InputEvent.Buttons.CUp));
    ret.AddRange(ButtonEvents(1, 3, InputEvent.Buttons.CRight));
    ret.AddRange(ButtonEvents(0, 4, InputEvent.Buttons.CDown));
    ret.AddRange(ButtonEvents(2, 5, InputEvent.Buttons.CLeft));
    ret.AddRange(ButtonEvents(11, 6, InputEvent.Buttons.Start));
    ret.AddRange(ButtonEvents(5, 7, InputEvent.Buttons.R));
    ret.AddRange(ButtonEvents(4, 8, InputEvent.Buttons.L));
    ret.AddRange(ButtonEvents(7, 9, InputEvent.Buttons.Z));
    ret.AddRange(ButtonEvents(12, 10, InputEvent.Buttons.DUp));
    ret.AddRange(ButtonEvents(13, 11, InputEvent.Buttons.DDown));
    ret.AddRange(ButtonEvents(15, 12, InputEvent.Buttons.DRight));
    ret.AddRange(ButtonEvents(14, 13, InputEvent.Buttons.DLeft));
    ret.AddRange(AxisEvents(0, 1, 0.005f, InputEvent.Axes.Left, false, true));

    return ret;    
  }


  
  private List<InputEvent> KeyboardEvents(){
    List<InputEvent> ret = new List<InputEvent>();

    ret.AddRange(KeyEvents(87, 0, InputEvent.Buttons.W));
    ret.AddRange(KeyEvents(65, 1, InputEvent.Buttons.A));
    ret.AddRange(KeyEvents(83, 2, InputEvent.Buttons.S));
    ret.AddRange(KeyEvents(68, 3, InputEvent.Buttons.D));
    ret.AddRange(KeyEvents(16777221, 4, InputEvent.Buttons.Enter)); // GD.KEY_RETURN is broken
    ret.AddRange(KeyEvents(16777232, 5, InputEvent.Buttons.Up));
    ret.AddRange(KeyEvents(16777234, 6, InputEvent.Buttons.Down));
    ret.AddRange(KeyEvents(16777231, 7, InputEvent.Buttons.Left));
    ret.AddRange(KeyEvents(16777233, 8, InputEvent.Buttons.Right));
    ret.AddRange(KeyEvents(1, 9, InputEvent.Buttons.M1));
    ret.AddRange(KeyEvents(2, 10, InputEvent.Buttons.M2));
    ret.AddRange(KeyEvents(3, 11, InputEvent.Buttons.M3));
    ret.AddRange(KeyEvents(16777217, 12, InputEvent.Buttons.Esc));
    ret.AddRange(KeyEvents(16777218, 13, InputEvent.Buttons.Tab));
    ret.AddRange(KeyEvents(32, 14, InputEvent.Buttons.Space));
    ret.AddRange(KeyEvents(16777237, 15, InputEvent.Buttons.Shift));
    ret.AddRange(KeyEvents(82, 16, InputEvent.Buttons.R));
    ret.AddRange(KeyEvents(69, 17, InputEvent.Buttons.E));
    ret.AddRange(KeyEvents(49, 18, InputEvent.Buttons.One));
    ret.AddRange(KeyEvents(50, 19, InputEvent.Buttons.Two));
    ret.AddRange(KeyEvents(51, 20, InputEvent.Buttons.Three));
    ret.AddRange(KeyEvents(52, 21, InputEvent.Buttons.Four));
    ret.AddRange(KeyEvents(53, 22, InputEvent.Buttons.Five));
    ret.AddRange(KeyEvents(54, 23, InputEvent.Buttons.Six));
    ret.AddRange(KeyEvents(55, 24, InputEvent.Buttons.Seven));
    ret.AddRange(KeyEvents(56, 25, InputEvent.Buttons.Eight));
    ret.AddRange(KeyEvents(57, 26, InputEvent.Buttons.Nine));
    ret.AddRange(KeyEvents(48, 27, InputEvent.Buttons.Zero));

    ret.AddRange(MouseEvents());

    return ret;
  }

  private List<InputEvent> MouseEvents(){
    List<InputEvent> ret = new List<InputEvent>();
    
    Vector2 mm = Util.GetMouseMovement();
    float dx = mm.x * sensitivityX;
    float dy = mm.y * sensitivityY;
    ret.Add(new InputEvent(InputEvent.Axes.Mouse, -dx, -dy));
    return ret;
  }

  private List<InputEvent> AxisEvents(int axisX, int axisY, float deadZone, InputEvent.Axes axisEnum, bool invertX, bool invertY){

    List<InputEvent> ret = new List<InputEvent>();

    float x = Input.GetJoyAxis(joyId, axisX);
    if(x < deadZone && x > -deadZone){
      x = 0f;
    }
    if(invertX){
      x *= -1;
    }
    
    float y = Input.GetJoyAxis(joyId, axisY);
    if(y < deadZone && y > -deadZone){
      y = 0f;
    }
    if(invertY){
      y *= -1;
    }

    if(x != 0f || y != 0f){
      ret.Add(new InputEvent(axisEnum, x, y));
    }

    return ret;
  }

  public static List<int> ConnectedJoypads(){
    List<System.Object> joypads = Util.ArrayToList(Input.GetConnectedJoypads());
    List<int> ret = new List<int>();
    foreach(System.Object obj in joypads){
      ret.Add((int)obj);
    }
    return ret;
  }

  public static bool JoypadConnected(int joyId){
    return ConnectedJoypads().Contains(joyId);
  }


  public static void SpamJoyPadInput(int joyPad){
    for(int i = 0; i < 200; i++){
      if(Input.IsJoyButtonPressed(joyPad, i)){
        GD.Print("Button " + i + " pressed.");
      }
    }
    for(int i = 0; i < 10; i++){
      float axis = Input.GetJoyAxis(joyPad, i);
      if(axis != 0f){
        GD.Print("Joy axis " + i + " set to " + axis);
      }
    }
  }

}
