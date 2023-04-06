using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField] private GameObject visualCue;
    private bool playerInRange;

    [SerializeField] private TextAsset inkJSON;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }
    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true); 
            if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerInRange=true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

}
