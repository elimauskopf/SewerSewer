using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : StationController
{
    private void Start()
    {
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        _animator.SetBool(Tags.InRange, true);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        _animator.SetBool(Tags.InRange, false);
    }
}
