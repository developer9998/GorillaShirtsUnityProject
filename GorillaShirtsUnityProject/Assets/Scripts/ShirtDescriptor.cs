using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaShirts.Data
{
    public class ShirtDescriptor : MonoBehaviour
    {
        public string Name;
        public string Author;
        public string Info;
        public GameObject Body;
        public GameObject LeftUpperArm;
        public GameObject RightUpperArm;
        public GameObject LeftLowerArm;
        public GameObject RightLowerArm;
        public GameObject Head;
        public GameObject LeftHand;
        public GameObject RightHand;
        public bool customColors;
        public bool isCreator;
        public bool SillyNSteady;
        public GameObject Boobs;

        public bool IsBoob;
        public List<GameObject> FurTextures;  
    }

}