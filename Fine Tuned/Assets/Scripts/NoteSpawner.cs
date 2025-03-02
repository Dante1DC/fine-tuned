using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    const float BEAT_DURATION = 0.25f;
    [SerializeField] private Sprite[] blackKeySprites;
    // [SerializeField] private Sprite[] whiteKeySprites;
    [SerializeField] private Dictionary<string, Sprite> blackKeys = new Dictionary<string, Sprite>();

    public GameObject notePrefab;
    public Transform spawnPoint;
    public float noteSpeed = 120f;
    private List<GameObject> activeNotes = new List<GameObject>();
    private float startTime;
    private int noteIndex = 1;

    public List<Note> noteSequence = new List<Note>();
    private float beatCount;

    void Start()
    {
        foreach (Sprite keySprite in blackKeySprites)
            blackKeys.Add(keySprite.name, keySprite);

        Vector3 pos = spawnPoint.position;
        pos.x = transform.parent.GetComponent<RectTransform>().rect.width / 2;
        spawnPoint.position = pos;
        startTime = Time.time;

        noteSequence.Add(new Note { display = blackKeySprites[0], beats = 1, pitch = 100f, key = KeyCode.A });
        noteSequence.Add(new Note { display = blackKeySprites[0], beats = 1, pitch = 100f, key = KeyCode.S });
        noteSequence.Add(new Note { display = blackKeySprites[0], beats = 1, pitch = 100f, key = KeyCode.D });
        noteSequence.Add(new Note { display = blackKeySprites[0], beats = 1, pitch = 100f, key = KeyCode.F });

        SpawnNote(noteSequence[0]);
    }

    void Update()
    {
        float currentTime = Time.time;
        float elapsedTime = currentTime - startTime;

        while (noteIndex < noteSequence.Count)
        {
            if (elapsedTime >= noteSequence[noteIndex - 1].beats * BEAT_DURATION)
            {
                startTime = currentTime;
                SpawnNote(noteSequence[noteIndex]);
                noteIndex++;
            }
        }
        MoveNotes();
        CheckForKeyPress();
    }

    public void SpawnNote(Note note)
    {
        GameObject newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
        Note noteComponent = newNote.GetComponent<Note>();
        if (noteComponent != null)
        {
            noteComponent.display = blackKeys[note.key.ToString()]; // Assign correct sprite
            noteComponent.beats = note.beats;
            noteComponent.pitch = note.pitch;
            noteComponent.key = note.key;
        }
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
        foreach (KeyCode key in new KeyCode[] { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F })
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
}
