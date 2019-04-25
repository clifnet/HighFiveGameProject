﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;
using Game.Data;
using System.IO;

namespace Game.Model.ItemSystem
{
    public abstract class AbstractItem:ITxtSerializable
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public int Capacity { get; set; }

        public string SpritePath { get; set; }


        //基类属性数量
        protected virtual int BasePropertyCount
        {
            get
            {
                return 5;
            }
        }


        public override string ToString()
        {
            return this.Name + "\n" + this.Description;
        }

        internal virtual void Init(string args)
        {
            var strs = args.Split('|');
            Assert.IsTrue(strs.Length == 5);
            this.ID = Convert.ToInt32(strs[0].Trim());
            this.Name = strs[1].Trim();
            this.Capacity =Convert.ToInt32( strs[2].Trim());
            this.Description = strs[3].Trim();
            this.SpritePath = strs[4].Trim();

        }

        #region ITxtSerializable
        public abstract string Sign{get;} 
        public  void LoadTxt(StreamReader sr){
            var line=TxtManager.ReadUntilValue(sr);
            Assert.IsFalse(string.IsNullOrEmpty(line));
            Init(line);
        }
        public  void SignInit(string initLine){}


        #endregion
    }
}
