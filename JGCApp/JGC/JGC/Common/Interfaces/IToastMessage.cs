public interface IToastMessage
{
    void Show(string message);
    void ShortAlert(string message);
    void LongAlert(string message);
}