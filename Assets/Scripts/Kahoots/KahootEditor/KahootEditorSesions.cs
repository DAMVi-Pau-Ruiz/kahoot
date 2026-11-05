
public static class KahootEditorSession
{
    public static bool IsEditing = false;
    public static string FilePath = null;

    public static void StartNewKahoot()
    {
        IsEditing = false;
        FilePath = null;
    }

    public static void StartEditing(string filePath)
    {
        IsEditing = true;
        FilePath = filePath;
    }
}
