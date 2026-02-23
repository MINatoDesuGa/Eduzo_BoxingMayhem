namespace Eduzo.Games.BoxingMayhem.Interfaces {
    public interface ISaveable {
        public Data.BoxingMayhemSaveDataHandler SaveDataHandler { get; }
        public void InitializeSaveDataHandler(Data.BoxingMayhemSaveDataHandler saveDataHandler);
        public void Save(Data.SaveType saveType, object data);
    }
}