using Assembly_CSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Script
{
    public class Player
    {
        private int trout = 0;
        public int Move = 0;
        public List<int> probability = new List<int>()
        {0,0,0,0};
        public int direction = 1;
        private int waitTime = 0;
        private Vector3 TargetPos;
        private int troutTier = 0;
        public bool isStart = false;
        public bool isGoal = false;

        public int id;

        public int hand;

        public int handCard_id;

        public int handCard_id_2;

        public int handCard_id_3;

        public List<List<Item>> Items = new List<List<Item>>()
        {
          new List<Item>(), new List<Item>(),new List<Item>(),new List<Item>()
        };

    }
}
