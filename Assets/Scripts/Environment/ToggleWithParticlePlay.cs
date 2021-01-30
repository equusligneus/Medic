using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWithParticlePlay : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _psToToggleWith;

    [SerializeField]
    private GameObject[] _goToToggle;

    private bool _on = true;

    private void Update()
    {
        HandleToggle();
    }

    private void HandleToggle()
    {
        if(_psToToggleWith.isPlaying && !_on)
        {
            foreach(GameObject go in _goToToggle)
            {
                go.SetActive(true);
            }
            _on = true;
        }
        else if(_psToToggleWith.isStopped && _on)
        {
            foreach(GameObject go in _goToToggle)
            {
                go.SetActive(false);
            }
            _on = false;
        }
    }
}
