
public class Word
{
    private string _value;
    private string _mutatedValue;
    private bool _played;
    private bool _mutated;
    private int _mutationLevel;

    public Word(string value,int mutationLevel = 0)
    {
        _value = value.ToUpper();
        _played = false;
        _mutated = false;
        _mutationLevel = mutationLevel;
        _mutatedValue = _value; 
    }


    public string Value
    {
        get
        {
            return _value;
        }
    }

    public string MutatedValue
    {
        get
        {
            return _mutatedValue;
        }
        set
        {
            _mutatedValue = value;
        }
    }

    public bool Mutated
    {
        get
        {
            return _mutated;
        }
        set
        {
            _mutated = value;
        }
    }

    public bool Played
    {
        get
        {
            return _played;
        }
        set
        {
            _played = value;
        }
    }
    public int MutationLevel
    {
        get
        {
            return _mutationLevel;
        }
        set
        {
            _mutationLevel = value;
        }
    }
    public override string ToString()
    {
        if (Value == null)
            return null;

        if(!_mutatedValue.Equals(_value))
        {
            return _mutatedValue;
        }
        return _value;
    }
}