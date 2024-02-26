using System.Collections.Generic;
using Currencies;
using General;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Player;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject CreatePlayer(Vector3 at);
        void Cleanup();
        GameObject GetPlayer();
        MoneyCurrency CreateMoneyCurrency();
        Upgrades CreateUpgrades(Tower tower, TowerHealth towerHealth, MoneyCurrency moneyCurrency);
        MusicPlayer CreateMusicPlayer();
    }
}