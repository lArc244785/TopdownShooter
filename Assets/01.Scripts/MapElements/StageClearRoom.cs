using System.Collections;
using System.Collections.Generic;
using TopdownShooter.GameElements;
using TopdownShooter.Interactions;
using UnityEngine;


namespace TopdownShooter.GameElements
{
    public class StageClearRoom : MonoBehaviour
    {
        [SerializeField] private SideStage[] _sideStages;

        private int _clearStageCount;

        private ISwitch[] _barricades;

        // Start is called before the first frame update
        void Start()
        {
            _sideStages = transform.root.GetComponentsInChildren<SideStage>();

            _barricades = GetComponentsInChildren<ISwitch>();

            foreach(var item in _barricades)
			{
                item.SwitchOn();
			}

            foreach (SideStage stage in _sideStages)
            {
                stage.onStageClear += () =>
                {
                    _clearStageCount++;
                    CheckSideStageAllClear();
                };

            }
        }

        private void CheckSideStageAllClear()
        {
            if (_clearStageCount >= _sideStages.Length)
            {
                foreach (var item in _barricades)
                {
                    item.SwitchOff();
                }
            }
        }
    }
}