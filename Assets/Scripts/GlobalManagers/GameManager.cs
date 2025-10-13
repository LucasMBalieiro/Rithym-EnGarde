using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-5)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}
    
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Transform spawnCrosshair;
    [SerializeField] private int poolSize = 20;
    
    [Header("Camera Sensitivity")] 
    [SerializeField] private float cameraSensitivityX;
    [SerializeField] private float cameraSensitivityY;

    private Queue<GameObject> noteQueue = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        for (int i = 0; i < poolSize; i++)
        {
            GameObject note = Instantiate(notePrefab, spawnCrosshair);
            note.SetActive(false);
            noteQueue.Enqueue(note);
        }
    }
    
    public GameObject GetNote()
    {
        GameObject note = noteQueue.Dequeue();
        note.SetActive(true);
        return note;
    }
    
    public void ReturnNote(GameObject note)
    {
        note.SetActive(false);
        noteQueue.Enqueue(note);
    }

    public void GetCameraSensitivity(out float sensX, out float sensY)
    {
        sensX = cameraSensitivityX;
        sensY = cameraSensitivityY;
    }
}
