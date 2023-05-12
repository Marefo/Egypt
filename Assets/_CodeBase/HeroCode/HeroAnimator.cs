using UnityEngine;

namespace _CodeBase.HeroCode
{
  public class HeroAnimator : MonoBehaviour
  {
    [SerializeField] private Animator _animator;
    
    private readonly int _aimingHash = Animator.StringToHash("Aiming");
    private readonly int _dieHash = Animator.StringToHash("Die");
    private readonly int _winHash = Animator.StringToHash("Win");

    public void ChangeAimingState(bool aiming) => _animator.SetBool(_aimingHash, aiming);
    
    public void PlayDeathAnimation()
    {
      _animator.applyRootMotion = true;
      _animator.SetTrigger(_dieHash);
    }

    public void PlayWinAnimation() => _animator.SetTrigger(_winHash);
  }
}