using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;
    public float speed;
    [SerializeField] float _moveLimit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public Animator GetMyAnimator()
    {
        return animator;
    }
    private void FixedUpdate()
    {
        Vector3 direction = transform.forward * variableJoystick.Vertical + transform.right * variableJoystick.Horizontal;
        direction.Normalize();
        transform.position += direction*speed;
    }
    private void Update()
    {
        if(variableJoystick.Vertical > _moveLimit)
        {
            animator.SetBool("ForwardWalking", true);
        }
        else if(variableJoystick.Vertical < _moveLimit)
        {
            animator.SetBool("ForwardWalking", false);
        }
        if (variableJoystick.Vertical < -_moveLimit)
        {
            animator.SetBool("RightWalking", true);
        }
        else if (variableJoystick.Vertical > -_moveLimit)
        {
            animator.SetBool("RightWalking", false);
        }
        if (variableJoystick.Horizontal < -_moveLimit)
        {
            animator.SetBool("BackwardWalking", true);
        }
        else if (variableJoystick.Horizontal > -_moveLimit)
        {
            animator.SetBool("BackwardWalking", false);
        }
        if (variableJoystick.Horizontal > _moveLimit)
        {
            animator.SetBool("LeftWalking", true);
        }
        else if (variableJoystick.Horizontal < _moveLimit)
        {
            animator.SetBool("LeftWalking", false);
        }

    }
}