
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string npcName;
    
    [TextArea(3, 10)]
    public string[] initSentences;
    
    private LinkedList<string> sentences = new LinkedList<string>();

    public LinkedList<string> GetSentences()
    {
        if (sentences.Count == 0)
        {
            foreach (var initSentence in initSentences)
            {
                sentences.AddLast(initSentence);
            }
        }
        
        return sentences;
    }

    public void ClearAndAdd(string newSentence)
    {
        sentences.Clear();
        sentences.AddFirst(newSentence);
    }
}