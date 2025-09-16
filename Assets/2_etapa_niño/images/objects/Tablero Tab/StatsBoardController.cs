using UnityEngine;

public class StatsBoardController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource chainSound;
    [SerializeField] private bool pauseGameWhenOpen = true;

    private bool isOpen;

    void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!chainSound) chainSound = GetComponent<AudioSource>();
        SetOpen(false, instant: true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Toggle();

        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            Toggle();
    }

    public void Toggle() => SetOpen(!isOpen);

    public void SetOpen(bool open, bool instant = false)
    {
        isOpen = open;
        animator.SetBool("IsOpen", open);

        if (pauseGameWhenOpen)
        {
            if (open)
            {
                Time.timeScale = 0f;
                animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            }
            else
            {
                Time.timeScale = 1f;
                animator.updateMode = AnimatorUpdateMode.Normal;
            }
        }

        if (open && chainSound != null)
        {
            chainSound.Play();
        }

        if (instant)
        {
            animator.Update(0f);
        }
    }
}
