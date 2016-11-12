using UnityEngine;
using Rewired;
using System.Collections;

public class PlayerController : MonoBehaviour {
    Player Player;
    Controls PlayerControls = new Controls();
    IControllable Target;

    public int PlayerID = 0;

    void Start()
    {
        Player = ReInput.players.GetPlayer(PlayerID);
    }

    public void SetTarget(GameObject Target)
    {
        this.Target = Target.GetComponent<IControllable>();
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
 //           Debug.Log(Player.GetAxis("Horizontal"));
            PlayerControls.Movement = new Vector2(Player.GetAxis("Horizontal"), Player.GetAxis("Vertical"));

            PlayerControls.TopLeft = HandleButton(Player.GetButton("Top Left"), PlayerControls.TopLeft);
            PlayerControls.TopMiddle = HandleButton(Player.GetButton("Top Middle"), PlayerControls.TopMiddle);
            PlayerControls.TopRight = HandleButton(Player.GetButton("Top Right"), PlayerControls.TopRight);

            PlayerControls.BottomLeft = HandleButton(Player.GetButton("Bottom Left"), PlayerControls.BottomLeft);
            PlayerControls.BottomMiddle = HandleButton(Player.GetButton("Bottom Left"), PlayerControls.BottomMiddle);
            PlayerControls.BottomRight = HandleButton(Player.GetButton("Bottom Left"), PlayerControls.BottomRight);

            Target.HandleInput(PlayerControls);
        }
    }
}
