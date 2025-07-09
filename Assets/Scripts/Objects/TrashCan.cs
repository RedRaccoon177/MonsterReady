using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TrashCan : MonoBehaviour
{
    PlayerController _player;
    private void Start()
    {
        _player = PlayerController._instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Npc")) return;

        if (other.CompareTag("Player"))
        {
        }
        else if (other.CompareTag("Npc"))
        {
        }
    }
}
