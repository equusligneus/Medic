using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable_Launcher : ATriggerable
{
    [SerializeField]
    private Projectile _projectile;

    [SerializeField]
    private int _damage;

    public override void GotTriggered(TriggerPlate by)
    {
        HandleProjectile();
    }

    private void HandleProjectile()
    {
        Projectile pro = Instantiate(_projectile, this.transform);
        pro.Damage = _damage;
        pro.transform.position = transform.position;
        pro.transform.rotation = transform.rotation;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }

}
