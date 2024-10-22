using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    private NavMeshAgent _agent;
    private Animator _animator;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.SetDestination(_target.transform.position);

    }

    private void Update()
    {
        // R�cup�rer la vitesse de l'agent
        //float speed = _agent.velocity.magnitude;

        // Mettre � jour le param�tre "Speed" dans l'Animator
        //_animator.SetFloat("Speed", speed);

        //R�cup�rer directement la vitesse de l'agent et l'envoyer au param�tre "Speed" dans l'Animator
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
}
