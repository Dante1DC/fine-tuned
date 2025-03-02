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
    private List<Note> activeNoteData = new();
    private float startTime;
    private int numNotes = 1;
    private int noteIndex = 0;
    private int score = 0;

    public List<List<Note>> noteSequences;
    public List<Note> noteSequence;

    public TextAsset text;

    private int sequenceIndex = 0;

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

        if (text != null)
        {
            AcceptNotes(text.text);
        }
    }

    void Update()
    {
        float elapsedTime = Time.time - startTime;

        if (numNotes < noteSequence.Count && elapsedTime >= noteSequence[numNotes - 1].beats * BEAT_DURATION)
        {
            SpawnNote(noteSequence[numNotes]);
            startTime = Time.time;
            numNotes++;
        }
        MoveNotes();
        if (noteIndex < activeNotes.Count)
        {
            CheckForKeyPress();
        }
    }

    public void AcceptNotes(string rawNotes)
    {
        List<NoteList> notes = JsonConvert.DeserializeObject<List<NoteList>>(rawNotes);
        foreach (NoteList chunk in notes)
        {
            List<Note> noteChunk = new();
            foreach (NoteDTO note in chunk.notes)
            {
                noteChunk.Add(new Note { beats = note.beats, pitch = note.pitch, key = Enum.Parse<KeyCode>(note.key) });
            }
            noteSequences.Add(noteChunk);
        }
    }

    public void SpawnNote(Note note)
    {
        GameObject newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
        newNote.transform.SetParent(spawnPoint);

        Image noteImage = newNote.GetComponent<Image>();
        noteImage.sprite = GetSpriteForKey(note.key);

        newNote.transform.position = new Vector3(spawnPoint.position.x + spawnPoint.GetComponent<RectTransform>().rect.width / 2, note.pitch, spawnPoint.position.z);

        activeNotes.Add(newNote);
        activeNoteData.Add(note);
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
                    activeNoteData.RemoveAt(i);
                    if (noteIndex > 0)
                    {
                        noteIndex--;
                    }
                }
            }
        }
    }

    private void CheckForKeyPress()
    {
        if (activeNoteData.Count > 0
            && Input.GetKeyDown(activeNoteData[noteIndex].key))
        {
            if (activeNotes[noteIndex].transform.position.x < 100f && activeNotes[noteIndex].transform.position.x > 10f)
            {
                activeNotes[noteIndex].GetComponent<Image>().color = Color.green;
                score += 100;
            }
            else
            {
                activeNotes[noteIndex].GetComponent<Image>().color = Color.red;
            }
            noteIndex++;
        }
    }

    private Sprite GetSpriteForKey(KeyCode key)
    {
        string keyName = key.ToString();
        return blackKeys.ContainsKey(keyName) ? blackKeys[keyName] :
               whiteKeys.ContainsKey(keyName) ? whiteKeys[keyName] : null;
    }
}
