using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Internal;

public class JSONReader : MonoBehaviour
{
	private string _filepath = "letters.txt";

    private void Start()
    {
        

    }

    private void Read()
    {
        if (File.Exists(file))
        {
            // Store each line in array of strings 
            string[] lines = File.ReadAllLines(_filepath);

            string currentLine = lines[0];

            if(currentLine == "start")
            {
                while(currentLine != "end")
                {

                }
            }

            foreach (string ln in lines)
                Console.WriteLine(ln);
        }


    }
}

