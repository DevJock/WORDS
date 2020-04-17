using System.Collections.Generic;

public class WordEngine
{
    public static System.Random random = new System.Random();
    private static string[] data = null;
    private static List<int> randWordIndexes = new List<int>();

    public WordEngine(string words)
    {
        data = words.Split('\n');
        if (randWordIndexes.Count > 0)
        {
            randWordIndexes.Clear();
        }
        for (int i = 0; i < data.Length; i++)
        {
            randWordIndexes.Add(i);
        }
    }

    private int randomUniquePosition
    {
        get
        {
            int randPos = random.Next(0, randWordIndexes.Count);
            int chosen = randWordIndexes[randPos];
            randWordIndexes.Remove(randPos);
            return chosen;
        }  
    }

    public Word NextWord
    {
        get
        {
            if (data.Length > 0)
            {
                Word word = new Word(data[randomUniquePosition]);
                if (random.Next(0, 2) == 1)
                {
                    Mutate(word);
                }
                return word;
            }
            else
            {
                return null;
            }
        }
    }

    public Word NextCorrectWord
    {
        get
        {
            if (data.Length > 0)
            {
                Word word = new Word(data[randomUniquePosition]);
                return word;
            }
            else
            {
                return null;
            }
        }
    }

    private static void Mutate(Word word)
    {
        word.MutationLevel = 1;
        string mutatedValue = "";
        List<int> positions = new List<int>();
        while (positions.Count < word.MutationLevel)
        {
            int rPos = random.Next(0, word.Value.Length);
            if (!positions.Contains(rPos))
            {
                positions.Add(rPos);
            }
        }
        for (int i = 0; i < word.Value.Length; i++)
        {
            if (!positions.Contains(i))
            {
                mutatedValue += word.Value[i];
                continue;
            }
            char ch = replaceWith();
            if (ch == ' ')
            {
                for (int j = i + 1; j < word.Value.Length; j++)
                {
                    i = j;
                    if (positions.Contains(j))
                    {
                        break;
                    }
                    mutatedValue += word.Value[j];
                }
            }
            else
            {
                mutatedValue += ch;
            }
        }
        word.MutatedValue = mutatedValue;
        word.Mutated = true;
    }

    private static char replaceWith()
    {
        int choice = random.Next(0, 2);
        if (choice == 0)
        {
            return (char)(random.Next(0, 26) + 'A');
        }
        else
        {
            return ' ';
        }
    }
}
