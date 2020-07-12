using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAudioManager : MonoBehaviour
{
    
    [SerializeField] private AudioClip antigravitySound;
    [SerializeField] private AudioClip touchingGroundSound;
    
    [SerializeField] private AudioSource antigravityActivation;
    [SerializeField] private AudioSource footStepSound;
    [SerializeField] private AudioSource footTouchFloorSound;

    private PlayerMovement m_PlayerMovement;

    // Start is called before the first frame update
    private void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        ApplySoundWalkingEffect();
        ApplyAntigraviyActivationSoundEffect();
        //ApplyTouchingGroundSoundEffect();
    }

    private void ApplySoundWalkingEffect()
    {
        if (m_PlayerMovement.IsMoving)
        {
            if (!footStepSound.isPlaying)
            {
                footStepSound.Play();
            }
        }
        else
        {
            footStepSound.Stop();
        }
    }

    private void ApplyAntigraviyActivationSoundEffect()
    {
        if (!antigravityActivation.isPlaying)
        {
            if (m_PlayerMovement.IsJumpStart)
            {
                antigravityActivation.PlayOneShot(antigravitySound);
            }            
        }
    }

    


    private void ApplyTouchingGroundSoundEffect() {
        if (m_PlayerMovement.IsGrounded())
        {
            antigravityActivation.PlayOneShot(touchingGroundSound);
        }
    }
}