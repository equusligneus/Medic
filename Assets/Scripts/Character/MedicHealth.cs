using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicHealth : MonoBehaviour
{

    [SerializeField]
    private Ref_Bool _fainted;

    [SerializeField]
    private float _faintedTime;

    [SerializeField]
    private Ref_Int _maxHealth;

    [SerializeField]
    private Ref_Int _currentHealth;

    [SerializeField]
    private float _invulnabilityTime;
    private bool _invulnable;

    private void Awake()
    {
        _currentHealth.Set(_maxHealth.Get());
    }

    /// <summary>
    /// Hit sets the medic onto fainting condition and if still alife then in invulnarble condition
    /// </summary>
    [ContextMenu("Hit")]
    public void Hit()
    {
        Hit(_faintedTime);
    }


    /// <summary>
    /// Let the fainting condition last for given time
    /// </summary>
    /// <param name="faintedTime">The time in sec the fainting lasts</param>
    public void Hit(float faintedTime)
    {
        if (_invulnable)
        {
            return;
        }

        int currentHealth = _currentHealth.Get();
        currentHealth--;


        if(currentHealth <= 0)
        {
            currentHealth = 0;
            PlayerDied();
        }
        else 
        {
            StartCoroutine(FaintLoop(faintedTime));
        }

        _currentHealth.Set(currentHealth);
    }


    /// <summary>
    /// Gets called once the player drops to 0 hit points
    /// </summary>
    private void PlayerDied()
    {
        _fainted.Set(false);
        Debug.Log("I'm dead!");
    }

    /// <summary>
    /// The fainting and invulnability loop
    /// </summary>
    /// <param name="faintTime">time the fainting should last</param>
    /// <returns></returns>
    private IEnumerator FaintLoop(float faintTime)
    {
        _invulnable = true;

        _fainted.Set(true);

        yield return new WaitForSeconds(faintTime);

        _fainted.Set(false);


        yield return new WaitForSeconds(_invulnabilityTime);

        _invulnable = false;
    }



}
