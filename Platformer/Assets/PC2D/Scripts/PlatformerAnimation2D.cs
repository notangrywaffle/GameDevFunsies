using UnityEngine;

namespace PC2D
{
    /// <summary>
    /// This is a very very very simple example of how an animation system could query information from the motor to set state.
    /// This can be done to explicitly play states, as is below, or send triggers, float, or bools to the animator. Most likely this
    /// will need to be written to suit your game's needs.
    /// </summary>

    public class PlatformerAnimation2D : MonoBehaviour
    {
        public float jumpRotationSpeed;
        public GameObject visualChild;
        public GameObject AnimChild;
        public GameObject IdleChild;

        private PlatformerMotor2D _motor;
        private Animator _animator;
        private bool _isJumping;
        private bool _isFalling;
        private bool _currentFacingLeft;

        // Use this for initialization
        void Start()
        {
            _motor = GetComponent<PlatformerMotor2D>();
            //_animator = visualChild.GetComponent<Animator>();
           
            _animator = AnimChild.GetComponent<Animator>();
            //_animator.Play("Idle");

            _motor.onJump += SetCurrentFacingLeft;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_animator.isInitialized)
            {
                Debug.LogWarning("Animator isn't initialized");
                return;
            }

            if (_motor.Dead)
            {
                _animator.SetBool("Dead", true);
            }
            else
            {
                _animator.SetBool("Dead", false);
            }


            if (_motor.motorState != PlatformerMotor2D.MotorState.Dashing)
            {
                _animator.SetBool("AttackDash", false);
            }
            if (_motor.motorState != PlatformerMotor2D.MotorState.AttackSwipe)
            {
                _animator.SetBool("AttackSwipe", false);
            }

            


            if (_motor.motorState == PlatformerMotor2D.MotorState.Jumping ||
                _isJumping &&
                    (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast))
            {
                _isJumping = true;

                if ((_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast))
                {
                    _animator.SetBool("Falling", true);
                    _isJumping = false;
                    _animator.SetBool("Jumping", false);
                }
                else
                {
                    _animator.SetBool("Jumping", true);//.Play("Jump");
                }


                if (_motor.velocity.x <= -0.1f)
                {
                    _currentFacingLeft = true;
                }
                else if (_motor.velocity.x >= 0.1f)
                {
                    _currentFacingLeft = false;
                }

                //Vector3 rotateDir = _currentFacingLeft ? Vector3.forward : Vector3.back;
                //visualChild.transform.Rotate(rotateDir, jumpRotationSpeed * Time.deltaTime);
            }
            else
            {


                 _isJumping = false;
                visualChild.transform.rotation = Quaternion.identity;

                if (_isFalling && !(_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast))
                {
                    _isFalling = false;
                    _animator.SetBool("Falling", false);
                }

                if (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast)
                {
                    _isFalling = true;
                    _animator.SetBool("Falling", true);//_animator.Play("Fall");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.WallSliding ||
                         _motor.motorState == PlatformerMotor2D.MotorState.WallSticking)
                {
                    Debug.LogWarning("Haven't created cling animation");//_animator.Play("Cling");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.OnCorner)
                {
                    Debug.LogWarning("Havne't created on corner animation");// _animator.Play("On Corner");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping)
                {
                    Debug.LogWarning("Haven't created slip animation");// _animator.Play("Slip");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.Dashing)
                {
                    _animator.SetBool("AttackDash", true);
                    //Debug.LogWarning("Haven't created dash animation");// _animator.Play("Dash");
                }
                else if(_motor.motorState == PlatformerMotor2D.MotorState.AttackSwipe)
                {
                    _animator.SetBool("AttackSwipe", true);
                }
                else
                {
                    //_animator.SetBool("Jumping", false);
                    //_animator.SetBool("Falling", false);

                    _animator.SetFloat("Speed", _motor.velocity.sqrMagnitude);
                    //if (_motor.velocity.sqrMagnitude >= 0.1f * 0.1f)
                    //{
                    //    _animator.Play("Walk");
                    //}
                    //else
                    //{
                    //    _animator.Play("Idle");
                    //}
                }
            }

            // Facing
            float valueCheck = _motor.normalizedXMovement;

            if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping ||
                _motor.motorState == PlatformerMotor2D.MotorState.Dashing ||
                _motor.motorState == PlatformerMotor2D.MotorState.Jumping)
            {
                valueCheck = _motor.velocity.x;
            }

            if (Mathf.Abs(valueCheck) >= 0.1f)
            {
                Vector3 newScale = visualChild.transform.localScale;
                newScale.x = Mathf.Abs(newScale.x) * ((valueCheck > 0) ? 1.0f : -1.0f);
                visualChild.transform.localScale = newScale;
            }



            if (Time.time > 3f && _animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Standing"))
            {
                AnimChild.GetComponent<Spriter2UnityDX.EntityRenderer>().enabled = false;
                IdleChild.SetActive(true);
            }
            else
            {
                AnimChild.GetComponent<Spriter2UnityDX.EntityRenderer>().enabled = true;
                IdleChild.SetActive(false);
            }
        }
        private void SetCurrentFacingLeft()
        {
            _currentFacingLeft = _motor.facingLeft;
        }
    }
}
