using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPressDetection : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Animator animator;
    [SerializeField] string AnimBoolName;
    [SerializeField] PlayerController playerController;
    private void Start()
    {
        animator = playerController.GetMyAnimator();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        animator.SetBool(AnimBoolName, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool(AnimBoolName, false);
    }
}
