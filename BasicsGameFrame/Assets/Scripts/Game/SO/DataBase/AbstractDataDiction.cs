
using System;
using System.Collections.Generic;
using System.Diagnostics;
using QFramework;
using UnityEngine.Events;
using UnityEngine;
namespace LetterDataNamespace
{
    public interface ICouldCreateDatabase
    {
        //任何可以创造数据库的值都需要ID
        public abstract int ID { get; }
    }
    public interface ICouldCreateSpecialDatabase : ICouldCreateDatabase
    {
        //表示特殊字典的Enum的Type
        public abstract Type MyDictionTagEnum { get; }
        /// <summary>
        /// 根据某个标签找到某个特殊字典（不创建返回null）
        /// </summary>
        public abstract string GetValueByTag(string Tag);
    }
    #region  抽象数据库类
    /// <summary>
    /// 普通数据库(只通过id存储不含特殊的检索字典)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractDataModelDatabase<T> : AbstractModel where T : ICouldCreateDatabase, new()
    {
        protected override void OnInit() { }
        Dictionary<int, T> DataDiction = new Dictionary<int, T>();//通过id来拾取数据
        public T GetLetterDataWithID(int id)
        {
            if (DataDiction.ContainsKey(id)) return DataDiction[id];
            return default(T);
        }
        public void AddNewData(T Data)
        {
            DataDiction.Add(Data.ID, Data);
        }
        public void AddNewData(T[] Data)
        {
            Data.ForEach(v128 =>
            {
                DataDiction.Add(v128.ID, v128);
            });
        }
        public T this[int id]
        {
            get { return GetLetterDataWithID(id); }
        }
    }
    /// <summary>
    /// 抽象的数据储存工具 (含特殊字典)
    /// 特殊结构如下
    ///                         DataModel
    ///             Tag                        |     Tag                    (数据中可以被储存成字典的特殊枚举()(比方说信的送出天数"Day"送出人"Sent"))
    ///     key     key         key            |     key            key     (关键词(比方说信的送出的日期(1,2,3等)))
    /// List(data)  List(data)  List(data)     |     List(data)     List(data) (对应的数据(比方说某一天的信件的链表))   
    /// </summary>
    /// <typeparam name="T">可储存数据</typeparam>
    public abstract class AbstractDataModelSpecialDatabase<T> : AbstractModel where T : ICouldCreateSpecialDatabase, new()
    {
        Dictionary<int, T> DataDiction = new Dictionary<int, T>();//通过id来拾取数据
        DictionHelper<T> SpacialDiction = new DictionHelper<T>();//通过不同规则对信进行分类
        public T GetLetterDataWithID(int id)
        {
            if (DataDiction.ContainsKey(id)) return DataDiction[id];
            return default(T);
        }
        protected override void OnInit() { }
        public void AddNewData(T Data)
        {
            DataDiction.Add(Data.ID, Data);
            SpacialDiction.Add(Data);
        }
        public void AddNewData(T[] Data)
        {
            Data.ForEach(v128 =>
            {
                DataDiction.Add(v128.ID, v128);
            });
            SpacialDiction.Add(Data);
        }
        #region 索引器
        public List<T> this[string Tag, string Key]
        {
            get { return SpacialDiction?[Tag]?[Key]; }
        }
        //List<LetterData> a = LetterDataManager.Instance["Day", "1"];
        public T this[string Tag, string Key, int index]
        {
            get
            {
                List<T> dic = SpacialDiction?[Tag]?[Key];
                if (!(dic?.Count > index)) return default(T);
                return dic[index];
            }
        }
        //LetterData a = LetterDataManager.Instance["Day", "1",1];
        public T this[int id]
        {
            get { return GetLetterDataWithID(id); }
        }
        #endregion
    }
    #endregion
    #region 特殊字典
    public class SpecialDiction<T> where T : ICouldCreateSpecialDatabase, new()//用string做key 因为懒（）
    {
        string myDictionTag;
        Dictionary<string, List<T>> idDiction;//存放了所有的字典
        public SpecialDiction(string Tag)
        {
            idDiction = new Dictionary<string, List<T>>();
            myDictionTag = Tag;
        }
        public void Add(T contentData)
        {
            string key = contentData.GetValueByTag(myDictionTag);
            if (idDiction.ContainsKey(key)) idDiction[key].Add(contentData);
            else idDiction.Add(key, new List<T>() { contentData });
        }
        public List<T> GetList(string myKey)
        {
            if (idDiction.ContainsKey(myKey)) return idDiction[myKey];
            return null;
        }
        /// <param name="action">对整个列表进行处理</param>
        public List<T> GetList(string myKey, UnityEvent<List<T>> action = null)
        {
            if (idDiction.ContainsKey(myKey))
            {
                List<T> reData = idDiction[myKey];
                action?.Invoke(reData);
                return reData;
            }
            return null;
        }
        /// <param name="action">对每个信进行处理</param>
        public List<T> GetList(string myKey, UnityEvent<T> action = null)
        {
            if (idDiction.ContainsKey(myKey))
            {
                List<T> reData = idDiction[myKey];
                reData.ForEach(v =>
                {
                    action?.Invoke(v);
                });
                return reData;
            }
            return null;
        }
        public string GetDicTag()//拿到字典的tag
        {
            return myDictionTag;
        }
        public bool ContainsKey(string key)
        {
            return idDiction.ContainsKey(key);
        }
        public List<T> this[string key]
        {
            get
            {
                return GetList(key);
            }
        }
    }
    public class DictionHelper<T> where T : ICouldCreateSpecialDatabase, new()//容器
    {
        Dictionary<string, SpecialDiction<T>> Dictions;
        bool isBeCreated = false;
        public DictionHelper()
        {
            T initHelper = new T();
            if (initHelper.MyDictionTagEnum == null) return;//表示不进行特殊字典的创建(只有id字典)
            isBeCreated = true;
            Dictions = new Dictionary<string, SpecialDiction<T>>();
            foreach (object tagEnum in Enum.GetValues(initHelper.MyDictionTagEnum))//遍历枚举类的元素来创造特殊字典
            {
                SpecialDiction<T> diction = new SpecialDiction<T>(tagEnum.ToString());
                Dictions.Add(diction.GetDicTag(), diction);
            }
        }
        public DictionHelper(T[] ContentData)
        {
            Dictions = new Dictionary<string, SpecialDiction<T>>();
            isBeCreated = true;
            foreach (object tagEnum in Enum.GetValues(ContentData[0].MyDictionTagEnum))//遍历枚举类的元素来创造特殊字典
            {
                SpecialDiction<T> diction = new SpecialDiction<T>(tagEnum.ToString());
                ContentData.ForEach(v128 =>
                {
                    diction.Add(v128);
                });
                Dictions.Add(diction.GetDicTag(), diction);
            }
        }
        /// <param name="ContentData">数据</param>
        public void Add(T ContentData)
        {
            if (!isBeCreated) return;
            foreach (object tagEnum in Enum.GetValues(ContentData.MyDictionTagEnum))//遍历枚举类的元素来创造特殊字典
            {
                Dictions[tagEnum.ToString()].Add(ContentData);
            }
        }
        public void Add(T[] ContentData)
        {
            if (!isBeCreated) return;
            foreach (object tagEnum in Enum.GetValues(ContentData[0].MyDictionTagEnum))//遍历枚举类的元素来创造特殊字典
            {
                ContentData.ForEach(v128 =>
                {
                    Dictions[tagEnum.ToString()].Add(v128);
                });
            }
        }
        public SpecialDiction<T> GetDicByTag(string tag)
        {
            if (!isBeCreated) return null;
            if (Dictions.ContainsKey(tag))
            {
                return Dictions[tag];
            }
            return null;
        }
        public SpecialDiction<T> this[string tag]
        {
            get
            {
                string mytag = tag;
                return GetDicByTag(mytag);
            }
        }
    }
    #endregion


    #region 使用例
    // public enum DataEnum
    // {
    //     特征1, 特征2
    // }
    // class Data : ICouldCreateSpecialDatabase
    // {
    //     public Data() { }
    //     public Data(string te1, string te2, string name)
    //     {
    //         特征1 = te1;
    //         特征2 = te2;
    //         Name = name;
    //     }
    //     public int id;
    //     public string 特征1;
    //     public string 特征2;
    //     public string Name;
    //     public Type MyDictionTagEnum => typeof(DataEnum);
    //     public int ID => id;
    //     public string GetValueByTag(string Tag)
    //     {
    //         switch (Tag)
    //         {
    //             case "特征1":
    //                 return 特征1;
    //             case "特征2":
    //                 return 特征2;
    //         }
    //         return null;
    //     }
    // }
    // class dataBase : AbstractDataModelSpecialDatabase<Data> { }
    // class useData
    // {
    //     public void Main(string[] args)
    //     {
    //         dataBase myDataBase = new dataBase();
    //         Data data1 = new Data("高", "大", "1");
    //         Data data2 = new Data("瘦", "小", "0");
    //         Data data3 = new Data("高", "小", "00");
    //         Data data4 = new Data("瘦", "大", "01");
    //         myDataBase.AddNewData(new Data[] { data1, data2, data3, data4 });
    //         List<Data> a = myDataBase["特征1", "高"];//a.name={"1","00"}
    //         List<Data> b = myDataBase["特征2", "大"];//a.name={"1","01"}
    //         Data c = myDataBase["特征1", "高", 0];//c.name="1"//按添加进去的顺序进行索引
    //     }
    // }
    #endregion
}
