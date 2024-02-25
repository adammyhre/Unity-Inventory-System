namespace Systems.Persistence {
    public interface ISerializer {
        string Serialize<T>(T obj);
        T Deserialize<T>(string json);
    }
}