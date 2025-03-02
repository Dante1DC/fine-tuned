using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    private bool isPlayerTurn = false;
    public TextAsset[] tracks;
    private int index = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
    public void SwitchTurn()
    {
        isPlayerTurn = !isPlayerTurn;
    }
    public TextAsset CurrentTrack()
    {
        return tracks[index];
    }
    public void UpdateTrack()
    {
        index++;
    }
}
