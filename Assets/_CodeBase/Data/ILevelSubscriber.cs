namespace _CodeBase.Data
{
  public interface ILevelSubscriber
  {
    void OnLevelLoad();
    void OnLevelExit();
  }
}