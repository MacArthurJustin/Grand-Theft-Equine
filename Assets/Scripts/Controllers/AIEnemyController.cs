using UnityEngine;
using System.Linq;

public class AIEnemyController : MonoBehaviour, IController {
    public enum State
    {
        Idle,
        Walking,
        Searching,
        Attacking
    }

    IControllable _target;
    public Weapon _weapon;
    public State _state;
    PlayableCharacter Enemy;
    public int WalkDirection = 0;
    float ChangeTime = 0;

    void Start()
    {
        SetTarget(GetComponent<IControllable>());
    }

    public void SetTarget(IControllable Target)
    {
        _target = Target;
        _weapon = GetComponentInChildren<Weapon>();
        ((PlayableCharacter)_target).SetWeapon(_weapon);
    }

    private PlayableCharacter FindEnemy()
    {
        Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.position, 20);

        PlayableCharacter[] Characters = Colliders.Select(character => character.GetComponent<PlayableCharacter>())
                                                  .Where(Character => Character != null)
                                                  .ToArray();

        if(Characters.Length > 0)
        {
            foreach(PlayableCharacter Char in Characters)
            {
                if(Char != this)
                {
                    if(Char.CharacterConfiguration.Alignment > 0)
                    {
                        return Char;
                    }
                }
            }
        }

        return null;
    }

    // Update is called once per frame
    void Update ()
    {
        Controls C = new Controls();

        switch (_state)
        {
            case State.Attacking:
                if(Enemy == null || !Enemy.isAlive)
                {
                    _state = State.Idle;
                    break;
                }

                // Look at the Enemy
                Vector2 Direction = (Enemy.transform.position - transform.position).normalized;

                float Angle = Mathf.Atan2(Direction.y, -Direction.x);
                int Octant = Mathf.RoundToInt(8 * Angle / (2 * Mathf.PI) + 8) % 8;
                //_target.LookInDirection(Octant);

                RaycastHit2D[] Hits = Physics2D.RaycastAll(transform.position, _target.Forward, 9);
                Collider2D _collider = GetComponent<Collider2D>();
                _target.LookInDirection(Octant);
                foreach (RaycastHit2D Hit in Hits) {
                    if(Hit.collider != null && Hit.collider != _collider)
                    {
                        if(Hit.collider.GetComponent<PlayableCharacter>() == Enemy)
                        {
                            C.TopLeft = ButtonState.Pressed;
                            _target.HandleInput(C);
                            return;
                        }
                    }
                }

                // Move to get in angle
                C.Movement = Direction.normalized;
                _target.HandleInput(C);
                // Fire
                break;

            case State.Searching:
                break;

            case State.Walking:
                Enemy = FindEnemy();

                if (Enemy != null)
                {
                    _state = State.Attacking;
                    break;
                }

                ChangeTime -= Time.deltaTime;

                if (ChangeTime <= 0)
                {
                    WalkDirection = Random.Range(0, 8) * 45;
                    ChangeTime = Random.Range(2, 3);
                }

                C.Movement = new Vector2(Mathf.Cos(WalkDirection), Mathf.Sin(WalkDirection));
                
                _target.HandleInput(C);
                break;

            case State.Idle:
            default:
                Enemy = FindEnemy();

                if(Enemy == null)
                {
                    _state = State.Walking;
                    break;
                }

                _state = State.Attacking;
                break;
        }
	}
}
