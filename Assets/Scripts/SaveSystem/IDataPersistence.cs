namespace SaveSystem
{
    public interface IDataPersistence
    {
        public void LoadData(SaveData data);
        public void SaveData(ref SaveData data);
    }
}