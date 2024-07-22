using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using AssemblyCSharp.Assets.Logic.Inventory.Entities;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AssemblyCSharp.Assets.Logic.SaveSystem.Entities;

namespace AssemblyCSharp.Assets.Logic.SaveSystem.Singletone
{
    public static class SaveSystem
    {
        private static string Datapath = Application.dataPath + "/Saves/";
        private static string FileExtension = ".save";

        #region GENERIC
        private static T LoadData<T>(string filepath) where T: class, new()
        {
            FileStream dataStream;
            BinaryFormatter converter = new BinaryFormatter();
            string savefile = Datapath + filepath + FileExtension;

            if (!Directory.Exists(Datapath))
            {
                Directory.CreateDirectory(Datapath);
            }

            if (!File.Exists(savefile))
            {
                dataStream = new FileStream(savefile, FileMode.Create);
                converter.Serialize(dataStream, new T());
                dataStream.Dispose();
                dataStream.Close();

                return new T();
            }
            else
            {
                dataStream = new FileStream(savefile, FileMode.Open);
                T ret = converter.Deserialize(dataStream) as T;
                dataStream.Dispose();
                dataStream.Close();

                return ret;
            }
        }
        private static void SaveData<T>(string filepath, T data) where T : class, new()
        {
            FileStream dataStream;
            BinaryFormatter converter = new BinaryFormatter();
            string savefile = Datapath + filepath + FileExtension;

            dataStream = new FileStream(savefile, FileMode.Create);
            converter.Serialize(dataStream, data);
            dataStream.Dispose();
            dataStream.Close();
        }
        #endregion

        #region SLAVE
        public static SlaveContainer SlaveContainer;
        private static string SlaveDataPath = "SlaveContainer";

        public static SlaveContainer Load_Slave()
        {
            if (SlaveContainer != null)
                return SlaveContainer;

            SlaveContainer = LoadData<SlaveContainer>(SlaveDataPath);
            return SlaveContainer;
        }
        public static void Save_Slave()
        {
            SaveData(SlaveDataPath, SlaveContainer);
        }
        #endregion

        #region LOCAL INVENTORY
        public static Inventory.Entities.Inventory LocalInventory;
        private static string LocalInventoryDataPath = "LocalInventory";

        public static Inventory.Entities.Inventory Load_LocalInventory()
        {
            if (LocalInventory != null)
                return LocalInventory;

            LocalInventory = LoadData<Inventory.Entities.Inventory>(LocalInventoryDataPath);
            return LocalInventory;
        }
        public static void Save_LocalInventory()
        {
            SaveData(LocalInventoryDataPath, LocalInventory);
        }
        #endregion

        #region GLOBAL INVENTORY
        public static Inventory.Entities.Inventory GlobalInventory;
        private static string GlobalInventoryDataPath = "GlobalInventory";

        public static Inventory.Entities.Inventory Load_GlobalInventory()
        {
            if (GlobalInventory != null)
                return GlobalInventory;

            GlobalInventory = LoadData<Inventory.Entities.Inventory>(GlobalInventoryDataPath);
            return GlobalInventory;
        }
        public static void Save_GlobalInventory()
        {
            SaveData(GlobalInventoryDataPath, GlobalInventory);
        }
        #endregion

        #region PREPARATION DATA
        public static PreparationData Preparation;
        private static string PreparationDataPath = "Preparation";

        public static PreparationData Load_Preparation()
        {
            if (Preparation != null)
                return Preparation;

            Preparation = LoadData<PreparationData>(PreparationDataPath);
            return Preparation;
        }
        public static void Save_Preparation(bool isInDungeon)
        {
            Preparation.IsInDungeon = isInDungeon;
            SaveData(PreparationDataPath, Preparation);
        }
        #endregion
    }
}
