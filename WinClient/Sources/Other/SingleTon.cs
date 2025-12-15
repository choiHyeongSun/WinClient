using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinClient.Sources.Interfaces;

namespace WinClient.Sources.Other
{
    internal class SingleTon<T> where T : class , new()
    {
        private static T? instance;

        public static void Create()
        {
            instance = new T();
            if (instance is ISingleTon_Init init)
                init.Initialization();
        }

        public static void Destroy()
        {
            if (instance is ISingleTon_Remove destroy)
                destroy.Remove();

            instance = null;
        }
    }
}
