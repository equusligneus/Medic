using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicHealth : MonoBehaviour
{

    [SerializeField]
    private Ref_Bool _isAwake;

    [SerializeField]
    private Ref_Bool _isAlive;

    [SerializeField]
    private Ref_Int _maxHealth;

    [SerializeField]
    private Ref_Int _currentHealth;

    [SerializeField]
    private float _faintedTime;

    [SerializeField]
    private float _invulnerabilityTime;
    private bool _invulnerable;

    private void Awake()
    {
        _currentHealth.Set(_maxHealth.Get());
        _isAlive.Set(true);
        _isAwake.Set(true);
    }

    /// <summary>
    /// To test he demage in the scene
    /// </summary>
    [ContextMenu("Hit")]
    public void TesHit()
    {
        Hit(1);
    }

    /// <summary>
    /// Hit sets the medic onto fainting condition and if still alife then in invulnarble condition
    /// </summary>
    public void Hit(int damage)
    {
        Hit(damage, _faintedTime);
    }


    /// <summary>
    /// Let the fainting condition last for given time
    /// </summary>
    /// <param name="faintedTime">The time in sec the fainting lasts</param>
    public void Hit(int damage, float faintedTime)
    {
        if (_invulnerable)
        {
            return;
        }

        int currentHealth = _currentHealth.Get();
        currentHealth -= damage;


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
        //_notFainted.Set(false);
        _isAwake.Set(false);
        _isAlive.Set(false);
    }

    /// <summary>
    /// The fainting and invulnability loop
    /// </summary>
    /// <param name="faintTime">time the fainting should last</param>
    /// <returns></returns>
    private IEnumerator FaintLoop(float faintTime)
    {
        _invulnerable = true;

        _isAwake.Set(false);

        yield return new WaitForSeconds(faintTime);

        _isAwake.Set(true);


        yield return new WaitForSeconds(_invulnerabilityTime);

        _invulnerable = false;
    }



}
