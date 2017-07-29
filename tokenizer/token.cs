using System;

public class Token
{
    public string value = "";
    public string type  = "";
    public int index;
    public int row;


	public Token(string value)
	{
        this.value = value;
	}

    public Token(string value, string type)
    {
        this.value = value;
        this.type  = type;
    }

    public Token(string value, string type, int index, int row)
    {
        this.value = value;
        this.type  = type;
        this.index = index;
        this.row   = row;
    }
}
