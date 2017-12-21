using UnityEngine;
using System.Collections.Generic;
using System;

using Battle.Unit;

namespace Battle
{
    [System.Serializable]
    internal class SquadContainer
    {
        [SerializeField]
        [ReadOnly]
        private int depth;
        [SerializeField]
        [ReadOnly]
        private int lines;

        [SerializeField]
        [ReadOnly]
        private SquadUnitController[] units;

        internal SquadContainer(int depth, int lines)
        {
            this.depth = depth;
            this.lines = lines;
            units = new SquadUnitController[depth * lines];
        }

        internal void SetUnit(SquadUnitController unit, int d, int l)
        {
            if (d >= depth || l >= lines)
            {
                Utils.Logger.GetInstance().Error("Position[" + d + "," + l + "] is out of range");
                return;
            }
            if (units[d * lines + l] != null)
            {
                Utils.Logger.GetInstance().Error("Position[" + d + "," + l + "] is not empty");
            }
            units[d * lines + l] = unit;
        }

        internal SquadUnitController[] GetAllUnits()
        {
            return (SquadUnitController[])units.Clone();
        }

        internal SquadUnitController[] GetFrontLineUnits()
        {
            if (depth < 1)
            {
                Utils.Logger.GetInstance().Error("Is not enough depth in SquadContainer");
                return null;
            }
            List<SquadUnitController> result = new List<SquadUnitController>();
            for (int l = 0; l < lines; l++)
            {
                result.Add(units[l]);
            }
            return result.ToArray();
        }

        internal SquadUnitController[] GetDistanceLineUnits()
        {
            if (depth < 2)
            {
                Utils.Logger.GetInstance().Error("Is not enough depth in SquadContainer");
                return null;
            }
            List<SquadUnitController> result = new List<SquadUnitController>();
            for (int d = 1; d < depth; d++)
            {
                for (int l = 0; l < lines; l++)
                {
                    if (units[d * lines + l] != null)
                    {
                        result.Add(units[d * lines + l]);
                    }
                }
            }
            return result.ToArray();
        }

        internal SquadUnitController GetUnitByType(UnitType type)
        {
            for (int d = 0; d < depth; d++)
            {
                for (int l = 0; l < lines; l++)
                {
                    if (units[d * lines + l] != null && units[d * lines + l].Type == type)
                    {
                        return units[d * lines + l];
                    }
                }
            }
            return null;
        }
    }
}
