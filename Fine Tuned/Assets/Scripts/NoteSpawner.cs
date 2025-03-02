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
using System.IO;
using UnityEngine.SceneManagement;

public class NoteSpawner : MonoBehaviour
{
    const float BEAT_DURATION = 0.25f;
    public Boolean isPlayer;
    public GameObject notePrefab;
    public Transform spawnPoint;
    public float noteSpeed = 120f;

    [SerializeField] private Sprite[] keySprites;
    private Dictionary<string, Sprite> keys = new();

    private List<GameObject> activeNotes = new();
    private List<Note> activeNoteData = new();
    private float startTime;
    private int numNotes = 0;
    private int noteIndex = 0;
    private int score = 0;

    public List<List<Note>> noteSequences = new();
    public List<Note> noteSequence;

    public TextAsset text;

    private int sequenceIndex = 0;

    private float pitchOffset;

    void Start()
    {
        startTime = Time.time;

        AcceptNotes(TurnManager.Instance.CurrentTrack().text);
        noteSequence = noteSequences[sequenceIndex];

        foreach (Sprite keySprite in keySprites)
            keys.Add(keySprite.name, keySprite);

        pitchOffset = !isPlayer ? 200 : 0;
    }

    void Update()
    {
        if (TurnManager.Instance.IsPlayerTurn() == isPlayer && noteSequence != null)
        {
            if (numNotes == 0)
            {
                SpawnNote(noteSequence[0]);
                startTime = Time.time;
                numNotes++;
            }
            float elapsedTime = Time.time - startTime;
            if (numNotes < noteSequence.Count && elapsedTime >= noteSequence[numNotes - 1].beats * BEAT_DURATION)
            {
                SpawnNote(noteSequence[numNotes]);
                startTime = Time.time;
                numNotes++;
            }
            MoveNotes();

            if (noteIndex < activeNotes.Count && isPlayer)
            {
                CheckForKeyPress();
            }
            if (activeNotes.Count == 0)
            {
                sequenceIndex++;
                noteSequence = (sequenceIndex < noteSequences.Count) ? noteSequences[sequenceIndex] : null;
                TurnManager.Instance.SwitchTurn();
                numNotes = 0;
                noteIndex = 0;
            }
        }
        if (!TurnManager.Instance.IsPlayerTurn() && noteSequence == null)
        {
            TurnManager.Instance.UpdateTrack();
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void AcceptNotes(string rawNotes)
    {
        ChunkList chunks = JsonConvert.DeserializeObject<ChunkList>(rawNotes);
        foreach (NoteList chunk in chunks.chunks)
        {
            List<Note> noteChunk = new();
            foreach (NoteDTO note in chunk.notes)
            {
                noteChunk.Add(new Note { beats = note.beats, pitch = note.pitch + pitchOffset, key = Enum.Parse<KeyCode>(note.key) });
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

        newNote.transform.position = new Vector3(spawnPoint.position.x + spawnPoint.GetComponent<RectTransform>().rect.width / 2, note.pitch + pitchOffset, spawnPoint.position.z);

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
            if (activeNotes[noteIndex].transform.position.x < 210 && activeNotes[noteIndex].transform.position.x > 190)
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
        return keys.ContainsKey(keyName) ? keys[keyName] : null;
    }
}
