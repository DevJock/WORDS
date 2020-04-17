using UnityEngine;

public class WordManager
{
    private static WordEngine wordEngine;

    public static void Initialize()
    {
        wordEngine = new WordEngine(OnlineManager.Word_Server.Words);
    }

    public static GameObject NewWord
    {
        get
        {
            float moveValue = 0.025f;
            float charPosition = 0;
            Word word = wordEngine.NextCorrectWord;
            int mid = word.Value.Length / 2;
            charPosition = -mid * moveValue;
            GameObject wordGameObject = new GameObject();
            WordObject wordObject = wordGameObject.AddComponent<WordObject>();
            wordObject.value = word.Value;
            wordObject.mutatedValue = word.MutatedValue;
            wordObject.mutated = word.Mutated;
            wordObject.mutationLevel = word.MutationLevel;
            wordObject.played = word.Played;
            wordObject.name = "Word";
            if(word.Mutated)
            {
                wordGameObject.tag = "MutatedWord";
            }
            else
            {
                wordGameObject.tag = "Word";
            }
            for (int i = 0; i < word.Value.Length; i++)
            {
                char ch = word.Value[i];
                if((ch < 'A' || ch > 'Z'))
                {
                    continue;
                }
                GameObject character = GameObject.Instantiate(Resources.Load("Prefabs/Letters/letter" + ch) as GameObject); 
                character.name = ""+ch;
                character.transform.parent = wordObject.transform;
                character.transform.localPosition = new Vector3(charPosition,0,0);
                character.transform.localRotation = new Quaternion(0, 180, 0, 0);
                character.GetComponent<Renderer>().material.color = Color.gray;
                charPosition += moveValue;
            }
            wordObject.length = word.Value.Length;
            BoxCollider collider = wordGameObject.AddComponent<BoxCollider>();
            collider.isTrigger = false;
            collider.center = new Vector3(-0.015f,0.0325f,0);
            collider.size = new Vector3(wordObject.length * moveValue, 0.03f,0.01f);
            return wordGameObject;
        }
    }
}
