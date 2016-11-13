using UnityEngine;

public enum ButtonState
{
    Up,
    Pressed,
    Held,
    Released
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
    Vector2 Forward { get; }

    void SetController(IController Controller);
    void HandleInput(Controls Control);
    void LookInDirection(int Direction, int frame = 0);
}
