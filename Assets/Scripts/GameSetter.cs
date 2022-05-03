using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTemplateProjects
{
    public class GameSetter : MonoBehaviour
    {
        public List<Troop> FirstArmy;
        public List<Troop> SecondArmy;
        public Transform FirstArmySpawn;
        public Transform SecondArmySpawn;


        private void Start()
        {
            SetArmies();
        }

        private void SetArmies()
        {
            SetArmy(FirstArmy, FirstArmySpawn, 2.5f, 2.5f);
            SetArmy(SecondArmy, SecondArmySpawn, 2.5f, 2.5f);
        }

        private void SetArmy(List<Troop> army, Transform spawnPoint, float xSpacing, float ySpacing)
        {
            int armyIndex = 0;
            var armySqrt = Mathf.Ceil(Mathf.Sqrt(army.Count));
            int xShift = Mathf.FloorToInt((armySqrt / 2) * xSpacing);
            int yShift = Mathf.FloorToInt((armySqrt / 2) * ySpacing);
            
            for (var i = 0; i < armySqrt; i++)
            {
                for (var j = 0; j < armySqrt; j++)
                {
                    if (armyIndex == army.Count) break;
                    var troop = Instantiate(army[armyIndex], new Vector3(spawnPoint.position.x+xSpacing*i-xShift,spawnPoint.position.y,spawnPoint.position.z+ySpacing*j-yShift), Quaternion.identity);
                    //troop.FindNewTarget();
                    armyIndex++;
                }
                if (armyIndex == army.Count) break;
            }
        }
    }
}