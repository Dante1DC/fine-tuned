using Palmmedia.ReportGenerator.Core.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class NoteSpawner : MonoBehaviour
{
    const float BEAT_DURATION = 0.25f;

    public GameObject notePrefab;
    public Transform spawnPoint;
    public float noteSpeed = 120f;

    [SerializeField] private Sprite[] blackKeySprites;
    [SerializeField] private Sprite[] whiteKeySprites;
    private Dictionary<string, Sprite> blackKeys = new();
    private Dictionary<string, Sprite> whiteKeys = new();

    private List<GameObject> activeNotes = new();
    private float startTime;
    private int noteIndex = 1;

    public List<Note> noteSequence;
    private float beatCount;

    private readonly float pitch1 = 160f;
    private readonly float pitch2 = 130f;
    private readonly float pitch3 = 100f;
    private readonly float pitch4 = 70f;
    private readonly float pitch5 = 40f;

    void Start()
    {
        startTime = Time.time;

        noteSequence = new();

        foreach (Sprite keySprite in blackKeySprites)
            blackKeys.Add(keySprite.name, keySprite);
        foreach (Sprite keySprite in whiteKeySprites)
            whiteKeys.Add(keySprite.name, keySprite);
    }

    void Update()
    {
        float elapsedTime = Time.time - startTime;

        if (noteIndex < noteSequence.Count && elapsedTime >= noteSequence[noteIndex - 1].beats * BEAT_DURATION)
        {
            SpawnNote(noteSequence[noteIndex]);
            startTime = Time.time;
            noteIndex++;
        }
        MoveNotes();
        CheckForKeyPress();
    }

    public void AcceptNotes(string rawNotes)
    {
        NoteList noteList = JsonConvert.DeserializeObject<NoteList>(rawNotes);
        foreach (NoteDTO note in noteList.notes)
        {
            Debug.Log(note.key);
            noteSequence.Add(new Note { beats = note.beats, pitch = note.pitch, key = Enum.Parse<KeyCode>(note.key) });
        }
    }

    public void SpawnNote(Note note)
    {
        Debug.Log(note.key);
        GameObject newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
        newNote.transform.SetParent(spawnPoint);

        Image noteImage = newNote.GetComponent<Image>();
        noteImage.sprite = GetSpriteForKey(note.key);

        newNote.transform.position = new Vector3(spawnPoint.position.x + spawnPoint.GetComponent<RectTransform>().rect.width / 2, note.pitch, spawnPoint.position.z);

        activeNotes.Add(newNote);
    }

    private void MoveNotes()
    {
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            if (activeNotes[i] != null)
            {
                activeNotes[i].transform.position += Vector3.left * noteSpeed * Time.deltaTime;
                if (activeNotes[i].transform.position.x <= -10f)
                {
                    Destroy(activeNotes[i]);
                    activeNotes.RemoveAt(i);
                }
            }
        }
    }

    private void CheckForKeyPress()
    {
        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                for (int i = 0; i < activeNotes.Count; i++)
                {
                    if (activeNotes[i] != null && activeNotes[i].GetComponent<Note>().key == key && Mathf.Abs(activeNotes[i].transform.position.x) < 1f)
                    {
                        Destroy(activeNotes[i]);
                        activeNotes.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    private Sprite GetSpriteForKey(KeyCode key)
    {
        string keyName = key.ToString();
        return blackKeys.ContainsKey(keyName) ? blackKeys[keyName] :
               whiteKeys.ContainsKey(keyName) ? whiteKeys[keyName] : null;
    }
}
