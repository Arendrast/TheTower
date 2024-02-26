using Enemies;
using Player;
using TMPro;
using UnityEngine;

namespace UI.Popups
{
    public class LosingPopup : BasePopup
    {
        [SerializeField] private int _difficultyLevel;
        [SerializeField] private TMP_Text _difficultyLevelText;
        [SerializeField] private TMP_Text _highestWaveText;
        [SerializeField] private TMP_Text _killedByText;
        [SerializeField] private SpawnEnemy _spawnEnemy;
        [SerializeField] private TowerHealth _towerHealth;

        protected override void OnOpenPopup()
        {
            base.OnOpenPopup();
            _killedByText.text += " " + _towerHealth.NameLastDamager;
            _highestWaveText.text += " " + _spawnEnemy.CurrentWave;
            _difficultyLevelText.text += " " + _difficultyLevel;
        }
    }
}
