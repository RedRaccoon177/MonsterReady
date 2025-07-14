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
    }
}
