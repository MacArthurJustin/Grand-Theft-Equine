using UnityEngine;
using Rewired;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IController {
    Player Player;
    Controls PlayerControls = new Controls();
    IControllable Target;

    public int PlayerID = 0;

    void Start()
    {
        Player = ReInput.players.GetPlayer(PlayerID);
    }

    public void SetTarget(IControllable Target)
    {
        if(this.Target != null)
        {
            this.Target.SetController(null);
        }
        this.Target = Target;
        Target.SetController(this);
    }

    private ButtonState HandleButton(bool NewState, ButtonState Button)
    {
        if (NewState)
        {
            if (Button == ButtonState.Up || Button == ButtonState.Released)
            {
                return ButtonState.Pressed;
            }

            return ButtonState.Held;
        }
        
        if (Button == ButtonState.Held || Button == ButtonState.Pressed)
        {
            return ButtonState.Released;
        }

        return ButtonState.Up;
    }

    void FixedUpdate()
    {
        if (Target != null)
        {
            PlayerControls.Movement = new Vector2(Player.GetAxis("Horizontal"), Player.GetAxis("Vertical"));

            PlayerControls.TopLeft = HandleButton(Player.GetButton("Top Left"), PlayerControls.TopLeft);
            PlayerControls.TopMiddle = HandleButton(Player.GetButton("Top Middle"), PlayerControls.TopMiddle);
            PlayerControls.TopRight = HandleButton(Player.GetButton("Top Right"), PlayerControls.TopRight);

            PlayerControls.BottomLeft = HandleButton(Player.GetButton("Bottom Left"), PlayerControls.BottomLeft);
            PlayerControls.BottomMiddle = HandleButton(Player.GetButton("Bottom Middle"), PlayerControls.BottomMiddle);
            PlayerControls.BottomRight = HandleButton(Player.GetButton("Bottom Right"), PlayerControls.BottomRight);

            Target.HandleInput(PlayerControls);
        }
    }
}
