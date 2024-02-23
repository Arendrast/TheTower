using Enemies;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LoosePanel : Switchable
    {
        [Space]
        
        [SerializeField] private int _difficultyLevel;
        [SerializeField] private TMP_Text _difficultyLevelText;
        [SerializeField] private TMP_Text _highestWaveText;
        [SerializeField] private TMP_Text _killedByText;
        [SerializeField] private SpawnEnemy _spawnEnemy;
        [SerializeField] private TowerHealth _towerHealth;

        public override void SetState()
        {
            _killedByText.text += " " + _towerHealth.NameLastDamager;
            _highestWaveText.text += " " + _spawnEnemy.CurrentWave;
            _difficultyLevelText.text += " " + _difficultyLevel;
            base.SetState();
        }
        
        public override void SetStateAfterWhile(float time) => Invoke(nameof(SetState), time);
    }
}
