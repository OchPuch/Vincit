using UnityEngine;

public class PistolGarbageView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    //Bools
    private static readonly int IsSpinning = Animator.StringToHash("IsSpinning");
    //Triggers
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger(Shoot);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool(IsSpinning, true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool(IsSpinning, false);
        }
    }
}
