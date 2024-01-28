using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace PanicBuying
{
    public class ItemObject_Holding : MonoBehaviour
    {
        virtual public bool Use()
        {
            return true;
        }

        NetworkInventory parentInventory;
    }
}
