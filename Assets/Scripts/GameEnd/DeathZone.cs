using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Ref<int> m_maxHealth = default;
    
    private void OnTriggerEnter(Collider _other)
    {
        Debug.Log(_other.gameObject, _other.gameObject);
        if (_other.GetComponent<MedicHealth>() != null)
        {
            _other.GetComponent<MedicHealth>().Hit(m_maxHealth.Get());
        }
    }
}
