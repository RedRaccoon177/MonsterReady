using UnityEngine;

public class PlayerBoxHandler : MonoBehaviour
{
    [Header("연결된 Counter (카운터3)")]
    public Counter counter3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null && counter3 != null)
            {
                if (player._CurrentMeat == 0 && player._CurrentBone == 0)
                {
                    player.AddBox(counter3._currentBoxCount);
                    player.CheckPickUpObject();
                    counter3.MinusBox(player._CurrentBox);
                }
            }
        }
        else if (other.CompareTag("Npc"))
        {
            NpcAi npc = other.GetComponent<NpcAi>();
            if (counter3 != null)
            {
                if (npc._CurrentMeat == 0 && npc._CurrentBone == 0)
                {
                    npc.AddBox(counter3._currentBoxCount);
                    npc.CurrentPickUpType();
                    counter3.MinusBox(npc._CurrentBox);
                }
            }
        }
    }
}
