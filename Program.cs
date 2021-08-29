using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;

namespace ConsoleAppTest
{
    class Cat
    {
        private string m_name; // 猫的名字
        private ActionCollection m_actions; // 具备的动作集合

        public Cat()
        {

        }

        public Cat(string name, ActionCollection ac)
        {
            this.m_name = name;
            this.m_actions = ac;
        }

        public string Name { get => m_name; set => m_name = value; }
        public ActionCollection Actions { get => m_actions; set => m_actions = value; }
    }

    class Action
    {
        public Action()
        {

        }

        public Action(string name, int step)
        {
            m_name = name;
            m_step = step;
        }

        public override string ToString()
        {
            return $"Action Name:{m_name}, Step: {m_step.ToString()}";
        }

        private string m_name; // 动作名称
        private int m_step; // 动作步骤

        public string Name { get => m_name; set => m_name = value; }
        public int Step { get => m_step; set => m_step = value; }
    }

    class ActionCollection : CollectionBase
    {
        public void Add(Action action)
        {
            this.InnerList.Add(action);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // 准备一只猫
            Action run = new Action("Running", 2);
            Action jump = new Action("Jump", 2);
            ActionCollection actions = new ActionCollection();
            actions.Add(run);
            actions.Add(jump);
            Cat helloKitty = new Cat("HelloKitty", actions);

            // 将猫放入json
            string jsonPath = "cat.json";
            Save(helloKitty, jsonPath);

            //// 错误方法取出猫
            //Cat helloKittyClone = Load(jsonPath);

            //Console.WriteLine(helloKittyClone.Name);
            //foreach (Action x in helloKittyClone.Actions)
            //{
            //    Console.WriteLine(x.ToString());
            //}

            // 正确方法取出猫
            Cat helloKittyCloneX = LoadJson(jsonPath);

            Console.WriteLine(helloKittyCloneX.Name);
            foreach (Action x in helloKittyCloneX.Actions)
            {
                Console.WriteLine(x.ToString());
            }
        }

        /// <summary>
        /// 反序列化(错误方法)
        /// </summary>
        /// <param name="jsonPath"></param>
        public static Cat Load(string jsonPath)
        {
            string jsonString = File.ReadAllText(jsonPath);
            Cat cat = JsonConvert.DeserializeObject<Cat>(jsonString);
            return cat;
        }

        /// <summary>
        /// 反序列化(正确方法)
        /// </summary>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        public static Cat LoadJson(string jsonPath)
        {
            string jsonString = File.ReadAllText(jsonPath);
            Cat cat = JsonConvert.DeserializeObject<Cat>(jsonString);

            ActionCollection actions = new ActionCollection();
            foreach(var item in cat.Actions)
            {
                string itemString = item.ToString();
                Action ac = JsonConvert.DeserializeObject<Action>(itemString);
                actions.Add(ac);
            }
            cat.Actions = actions;

            return cat;
        }


        /// <summary>
        /// 序列化一个Cat对象到Json
        /// </summary>
        /// <param name="cat"></param>
        /// <param name="jsonPath"></param>
        public static void Save(Cat cat, string jsonPath)
        {
            string jsonString = JsonConvert.SerializeObject(cat);
            File.WriteAllText(jsonPath, jsonString);
        }
    }
}
