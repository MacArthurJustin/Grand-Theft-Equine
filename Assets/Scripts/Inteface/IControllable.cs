using UnityEngine;

public enum ButtonState
{
    Pressed,
    Held,
    Released,
    Up
}

public struct Controls
{
    public Vector2 Movement;
    public ButtonState TopLeft;
    public ButtonState TopMiddle;
    public ButtonState TopRight;
    public ButtonState BottomLeft;
    public ButtonState BottomMiddle;
    public ButtonState BottomRight;    
}

public interface IControllable {
    void HandleInput(Controls Control);	
}
