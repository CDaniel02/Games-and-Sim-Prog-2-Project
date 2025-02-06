using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class TextReader : MonoBehaviour
{
	private string _filepath = "Assets/Scripts/Letters/letters.txt";

    private List<Letter> _letters;
    private Dictionary<string, NPC> _npcs; 

    private void Start()
    {
        // initialize variables 
        _letters = new List<Letter>();
        _npcs = new Dictionary<string, NPC>();

        // run the read function to get our letters list full of all our letters
        Read();

        // get all NPCs from the NPC gameobject in scene
        List<NPC> npcList = new(GameObject.Find("NPCs").GetComponentsInChildren<NPC>());
        // add NPCs to dictionary by NPC name
        foreach(NPC npc in npcList)
        {
            _npcs[npc.Name] = npc; 
        }
        // add all letters to their corresponding NPCs
        foreach(Letter letter in _letters)
        {
            if (_npcs.ContainsKey(letter.From))
            {
                _npcs[letter.From].mailbox.AddOutgoingMail(letter);
                Debug.Log("Added letter to " + letter.From + " that goes to " + letter.To + "."); 
            }

        }
    }

    private void Read()
    {
        // check file exists
        if (File.Exists(_filepath))
        {
            
            // Store each line in array of strings 
            string[] lines = File.ReadAllLines(_filepath);

            int index = 0; 
            string currentLine = lines[index];

            // repeat this loop until our currentline index is greater than the lines lengths
            while(index < lines.Length)
            {
                // "start" begins a new object
                if (currentLine == "start")
                {
                    Letter currentLetter = new Letter();
                    currentLine = lines[++index];
                    
                    // loop through all fields and fill them up in our object
                    while (currentLine != "end")
                    {
                        int colonpos = currentLine.IndexOf(":");
                        string property = currentLine.Substring(0, colonpos); // property is everything before the colon
                        string value = currentLine.Substring(colonpos + 2); // value is everything after the colon

                        // if the property is the prereqletters, we have to do some special stuff
                        // this is prolly poorly coded but i couldnt think of anything better
                        if (property == "PrereqLetters")
                        {
                            string[] indexesAsStrings = value.Split(", ");
                            List<int> letterIndexes = new List<int>();
                            foreach(string letterIndex in indexesAsStrings)
                            {
                                letterIndexes.Add(int.Parse(letterIndex)); 
                            }

                            currentLetter.PrereqLetters = letterIndexes; 
                            
                        }
                        else
                        {
                            currentLetter[property] = value;
                        }

                        currentLine = lines[++index];
                    }
                    // once we reach end, add our completed letter to our list
                    _letters.Add(currentLetter);
                }
                else
                {
                    try
                    {
                        currentLine = lines[++index];
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }
    }
}

