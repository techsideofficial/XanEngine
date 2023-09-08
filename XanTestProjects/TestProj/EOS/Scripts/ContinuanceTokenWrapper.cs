using Godot;
using Epic.OnlineServices;

public partial class ContinuanceTokenWrapper : RefCounted
{
    public ContinuanceTokenWrapper(ContinuanceToken token)
    {
        _internalToken = token;
    }

    public ContinuanceTokenWrapper()
    {
    }

    public new string GetClass()
    {
        return "ContinuanceTokenWrapper";
    }

    public override string ToString()
    {
        string tokenString = _internalToken.ToString();
        return "ContinuanceTokenWrapper(" + tokenString + ")";
    }

    public ContinuanceToken _internalToken;
}