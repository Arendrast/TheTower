using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IGameFactory _gameFactory;

        private readonly IPersistentProgressService _progressService;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        public void SaveProgress()
        {
          foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
            progressWriter.UpdateProgress(_progressService.Progress);
        }

        public void LoadProgress()
        {
            using (var fileStream = new FileStream(Application.persistentDataPath + "/progress.txt", FileMode.OpenOrCreate))
            {
                var formatter = new BinaryFormatter();
                
                if (fileStream.Length == 0)
                {
                    formatter.Serialize(fileStream, new Progress(SceneNames.FirstLevel));
                }
                
                fileStream.Position = 0;
                var progress = (Progress)formatter.Deserialize(fileStream);
                _progressService.Progress = progress;
            }
        }
    }

    public static class SceneNames
    {
        public const string FirstLevel = "FirstLevel";
    }
}