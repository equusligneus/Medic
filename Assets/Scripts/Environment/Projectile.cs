using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _FlyingSpeed;

    [SerializeField]
    private string[] _wallGroundTags;

    [SerializeField]
    private float _maxLifeTime;

    private float _startTime;

    public int Damage
    {
        get;
        set;
    }

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _startTime = Time.time;

    }
    private void Start()
    {
        HandleStart();
    }

    private void Update()
    {
        CheckForLifeEnd();
    }

    /// <summary>
    /// Sets the velocity of the projectile acording to own rotation
    /// </summary>
    private void HandleStart()
    {
        Vector3 velo = this.transform.forward * _FlyingSpeed;
        _rb.velocity = velo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MedicHealth mh = other.GetComponentInParent<MedicHealth>();
            Debug.Log("Hit Player");
            mh.Hit(Damage);
        }

        bool iswallOrGround = false;

        foreach (string tag in _wallGroundTags )
        {
            if (other.CompareTag(tag))
            {
                iswallOrGround = true;
            }
        }

        if (iswallOrGround)
        {
            DoneHitting();
        }
    }


    /// <summary>
    /// Check if the projectile is alive to long 
    /// </summary>
    private void CheckForLifeEnd()
    {
        if(Time.time < _startTime + _maxLifeTime)
        {
            return;
        }

        DoneHitting();
    }

    /// <summary>
    /// Destroys the Projectile
    /// </summary>
    private void DoneHitting()
    {
        Destroy(this.gameObject);
    }

    
}
