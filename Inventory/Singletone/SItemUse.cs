using AssemblyCSharp.Assets.Logic.WeaponSystem.ScriptableObjects;
using AssemblyCSharp.Assets.Logic.Inventory.Enums;
using AssemblyCSharp.Assets.Logic.Unit.Components;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharp.Assets.Logic.Inventory.Singletone
{
    public static class SItemUse
    {
        public static void UseItemByType(
            GameObject user,
            EItemType type,
            Object referedObject
            )
        {
            if (user == null)
                return;
            if (referedObject == null)
                return;

            switch (type)
            {
                case EItemType.Default:
                    break;

                case EItemType.Weapon:
                    UseWeapon(user, referedObject);
                    break;
                case EItemType.Arrow:
                    UseArrow(user, referedObject);
                    break;
                case EItemType.Consumable:
                    UseConsumable(user, referedObject);
                    break;
            }
        }

        private static void UseWeapon(
            GameObject user,
            Object referedObject
            )
        {
            if (!user.GetComponent<UCombatController>())
                return;
            if (referedObject.GetType() != typeof(WeaponSO))
                return;

            user.GetComponent<UCombatController>().ChangeWeapon(referedObject as WeaponSO);
        }
        private static void UseArrow(
           GameObject user,
           Object referedObject
           )
        {
            if (!user.GetComponent<UCombatController>())
                return;

            user.GetComponent<UCombatController>().ChangeArrow(referedObject);
        }

        private static void UseConsumable(
           GameObject user,
           Object referedObject
           )
        {
            if (!user.GetComponent<UCombatController>())
                return;

            //do
        }
    }
}
