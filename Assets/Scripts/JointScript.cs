using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointScript : MonoBehaviour
{

    [SerializeField] private PlayerController _player;
    // Start is called before the first frame update
    public void HitEnd()
    {
        _player.HitEnd();
    }

    public void StopAttack()
    {
        _player.stopAttack();
    }
}
