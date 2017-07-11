using System;
using Spriter2UnityDX;
using UnityEngine;

/// <summary>
/// This class is a simple example of how to build a controller that interacts with PlatformerMotor2D.
/// </summary>
[RequireComponent(typeof(PlatformerMotor2D))]
public class PlayerController2D : MonoBehaviour
{
    public UnityEngine.UI.Text txtHp;
    public UnityEngine.UI.Text txtCoins;
    public LevelManager levelManager;
    public int PlayerDamage = 1;
    private int playerHealth;
    private Checkpoint lastCheckPoint;
    private int TotalCoins = 0;

    internal void SetLastCheckpoint(Checkpoint checkpoint)
    {
        lastCheckPoint = checkpoint;
    }
    internal Checkpoint GetLastCheckpoint()
    {
        return lastCheckPoint;
    }

    const int playerHealthDefault = 3;
    private float playerDamageCooldownRemaining;
    const float playerDamageCooldown = 2f;
    public GameObject AnimatedSprite;
    public SpriteRenderer CharIdle;
    public bool Dead = false;
    private bool SuspendPlayerInput = false;
    private float SuspendPlayerInputTimer = -1f;

    public PlatformerMotor2D Motor { get { return _motor; } }
    private PlatformerMotor2D _motor;
    private bool _restored = true;
    private bool _enableOneWayPlatforms;
    private bool _oneWayPlatformsAreWalls;

    private TouchController touchController;

    // Use this for initialization
    void Start()
    {
        _motor = GetComponent<PlatformerMotor2D>();
        touchController = GetComponent<TouchController>();

        playerHealth = playerHealthDefault;
    }

    // before enter en freedom state for ladders
    void FreedomStateSave(PlatformerMotor2D motor)
    {
        if (!_restored) // do not enter twice
            return;

        _restored = false;
        _enableOneWayPlatforms = _motor.enableOneWayPlatforms;
        _oneWayPlatformsAreWalls = _motor.oneWayPlatformsAreWalls;
    }
    // after leave freedom state for ladders
    void FreedomStateRestore(PlatformerMotor2D motor)
    {
        if (_restored) // do not enter twice
            return;

        _restored = true;
        _motor.enableOneWayPlatforms = _enableOneWayPlatforms;
        _motor.oneWayPlatformsAreWalls = _oneWayPlatformsAreWalls;
    }

    // Update is called once per frame
    void Update()
    {

        if (playerDamageCooldownRemaining > 0f)
        {
            playerDamageCooldownRemaining -= Time.deltaTime;
            float dec = playerDamageCooldownRemaining % 1.0f;
            if (Mathf.Round(dec*100f) % 2 == 0f)
            {
                float col = (float)playerHealth / (float)playerHealthDefault;
                AnimatedSprite.GetComponent<EntityRenderer>().Color = new Color(col, col, col, 1.0f);
                CharIdle.color = new Color(col, col, col, 1.0f);
            }
            else
            {
                float col = (float)(playerHealth + 1.0f) / (float)playerHealthDefault;
                AnimatedSprite.GetComponent<EntityRenderer>().Color = new Color(col, col, col, 1.0f);
                CharIdle.color = new Color(col, col, col, 1.0f);
                //AnimatedSprite.GetComponent<EntityRenderer>().Color = Color.white;
                //CharIdle.color = Color.white;
            }
            
        }
        else
        {
            float col = (float)playerHealth / (float)playerHealthDefault;
            AnimatedSprite.GetComponent<EntityRenderer>().Color = new Color(col, col, col, 1.0f);
            CharIdle.color = new Color(col, col, col, 1.0f);
            //AnimatedSprite.GetComponent<EntityRenderer>().Color = Color.white;
        }


        float Vertical = 0f;//Input.GetAxis(PC2D.Input.VERTICAL);
        float Horizontal = 0f;//Input.GetAxis(PC2D.Input.HORIZONTAL);
        bool Jump = Input.GetButton(PC2D.Input.JUMP);
        bool AttackDash = Input.GetButtonDown(PC2D.Input.DASH);
        bool AttackSwipe = Input.GetKey(KeyCode.LeftAlt);

        if (levelManager.GetGameState() == LevelManager.GameStates.Running)
        {
            Horizontal = 1f;
        }

        if (touchController.UseTouch)
        {
            //Horizontal = touchController.x;
            //Vertical = touchController.y;
            Jump = touchController.jump;
            AttackDash = touchController.attack;
        }
         
        if (Dead && Jump)
        {
            Respawn();
            return;
        }
        if (SuspendPlayerInput)
        {
            Vertical = 0f;
            Horizontal = 0f;
            Jump = false;
            AttackDash = false;
            AttackSwipe = false;

            if (SuspendPlayerInputTimer > 0f && SuspendPlayerInputTimer < Time.time)
            {
                SuspendPlayerInput = false;
                SuspendPlayerInputTimer = -1f;
            }
        }

        // use last state to restore some ladder specific values
        if (_motor.motorState != PlatformerMotor2D.MotorState.FreedomState)
        {
            // try to restore, sometimes states are a bit messy because change too much in one frame
            FreedomStateRestore(_motor);
        }

        // Jump?
        // If you want to jump in ladders, leave it here, otherwise move it down
        if (Jump)
        {
            _motor.Jump();
            _motor.DisableRestrictedArea();
        }
        //Debug.Log(Jump);
        _motor.jumpingHeld = Jump;

        // XY freedom movement
        if (_motor.motorState == PlatformerMotor2D.MotorState.FreedomState)
        {
            _motor.normalizedXMovement = Horizontal;
            _motor.normalizedYMovement = Vertical;

            return; // do nothing more
        }

        // X axis movement
        if (Mathf.Abs(Horizontal) > PC2D.Globals.INPUT_THRESHOLD)
        {
            _motor.normalizedXMovement = Horizontal;
        }
        else
        {
            _motor.normalizedXMovement = 0;
        }

        if (Vertical != 0)
        {
            bool up_pressed = Vertical > 0;
            if (_motor.IsOnLadder())
            {
                if (
                    (up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Top)
                    ||
                    (!up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Bottom)
                 )
                {
                    // do nothing!
                }
                // if player hit up, while on the top do not enter in freeMode or a nasty short jump occurs
                else
                {
                    // example ladder behaviour

                    _motor.FreedomStateEnter(); // enter freedomState to disable gravity
                    _motor.EnableRestrictedArea();  // movements is retricted to a specific sprite bounds

                    // now disable OWP completely in a "trasactional way"
                    FreedomStateSave(_motor);
                    _motor.enableOneWayPlatforms = false;
                    _motor.oneWayPlatformsAreWalls = false;

                    // start XY movement
                    _motor.normalizedXMovement = Horizontal;
                    _motor.normalizedYMovement = Vertical;
                }
            }
        }
        else if (Vertical < -PC2D.Globals.FAST_FALL_THRESHOLD)
        {
            _motor.fallFast = false;
        }

        if (AttackDash)
        {
            _motor.Dash();
        }

        if (AttackSwipe)
        {
            _motor.Swipe();
        }
    }

    public void RecieveDamage(int damage)
    {
        if (playerDamageCooldownRemaining <= 0f)
        {
            if (!Dead)
            {
                playerHealth -= damage;
                txtHp.text = "HP: " + playerHealth;

                SuspendPlayerInput = true;
                SuspendPlayerInputTimer = Time.time + 0.5f;

                _motor.HitMove(new Vector2(-5f, 5f));
            }
            else
            {
                //txtHp.text = "Jump to continue";
                GameObject.Find("GameManager").GetComponent<LevelManager>().PlayerDied();
            }

            if (playerHealth < 1)
            {
                Dead = true;
                _motor.Dead = true;
                SuspendPlayerInput = true;
            }
            else
            {
                playerDamageCooldownRemaining = playerDamageCooldown;
            }
            
            


        }
        

    }

    public void RespawnForContinue()
    {
        playerHealth = playerHealthDefault;
        txtHp.text = "HP: " + playerHealth;
        playerDamageCooldownRemaining = playerDamageCooldown;

        _motor.Dead = false;
        this.Dead = false;
        SuspendPlayerInput = false;
    }

    private void Respawn()
    {
        GameObject deadPlayer = GameObject.Instantiate(AnimatedSprite.transform.parent.gameObject);
        deadPlayer.transform.SetParent(this.gameObject.transform);
        deadPlayer.transform.localPosition = Vector3.zero;
        //deadPlayer.transform.localRotation = Quaternion.Euler(Vector3.zero);
        //deadPlayer.transform.localScale = Vector3.one;

        
        
        deadPlayer.transform.SetParent(null);

        //Reset everything
        this.transform.position = lastCheckPoint.transform.position;
        playerHealth = playerHealthDefault;
        txtHp.text = "HP: " + playerHealth;
        playerDamageCooldownRemaining = playerDamageCooldown;

        _motor.Dead = false;
        this.Dead = false;
        SuspendPlayerInput = false;
    }

    public void PickedUpCoin(int value)
    {
        TotalCoins += value;
        txtCoins.text = TotalCoins.ToString();
    }
}
