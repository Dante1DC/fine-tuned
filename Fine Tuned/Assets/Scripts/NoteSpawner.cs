using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    const float BEAT_DURATION = 0.25f;

    public GameObject notePrefab;
    public Transform spawnPoint;
    public float noteSpeed = 120f;

    [SerializeField] private Sprite[] blackKeySprites;
    [SerializeField] private Sprite[] whiteKeySprites;
    private Dictionary<string, Sprite> blackKeys = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> whiteKeys = new Dictionary<string, Sprite>();

    private List<GameObject> activeNotes = new List<GameObject>();
    private float startTime;
    private int noteIndex = 1;

    public List<Note> noteSequence = new List<Note>();
    private float beatCount;

    void Start()
    {
        startTime = Time.time;

        // foreach (Sprite keySprite in blackKeySprites)
        //     blackKeys.Add(keySprite.name, keySprite);
        foreach (Sprite keySprite in whiteKeySprites)
            whiteKeys.Add(keySprite.name, keySprite);


        noteSequence.Add(new Note { display = whiteKeySprites[0], beats = 1, pitch = 100f, key = KeyCode.A });
        noteSequence.Add(new Note { display = whiteKeySprites[0], beats = 1, pitch = 100f, key = KeyCode.S });
        noteSequence.Add(new Note { display = whiteKeySprites[0], beats = 1, pitch = 100f, key = KeyCode.D });
        noteSequence.Add(new Note { display = whiteKeySprites[0], beats = 1, pitch = 100f, key = KeyCode.F });

        SpawnNote(noteSequence[0]);
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

    public void SpawnNote(Note note)
    {
        GameObject newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
        newNote.transform.SetParent(spawnPoint);
        Note noteComponent = newNote.GetComponent<Note>();

        if (noteComponent != null)
        {
            noteComponent.display = GetSpriteForKey(note.key); // Assign correct sprite
            noteComponent.beats = note.beats;
            noteComponent.pitch = note.pitch;
            noteComponent.key = note.key;
            newNote.transform.position = new Vector3(spawnPoint.position.x + spawnPoint.GetComponent<Rect>().width / 2, note.pitch, spawnPoint.position.z);
        }
        newNote.SetActive(true);
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
