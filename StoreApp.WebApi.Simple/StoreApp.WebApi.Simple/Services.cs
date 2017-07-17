using System;
using System.Collections.Generic;

namespace StoreApp.WebApi.Simple
{
    public class Services
    {


        public static Services Instance     // I'm using a static property, but you could also create a static "getInstance" method if you prefer
         {
           get 
             {
                if (_instance == null) 
                {
                         // Note: if you are multithreading, you might need some locking here to avoid ending up with more than one instance
                          _instance = new Services();
                 }
                return _instance;
            }
        }
        public static Services _instance;
        private Services() { }

        private readonly Dictionary<Type, Func<object>> Map = new Dictionary<Type, Func<object>>();

        private readonly object Sync = new object();

        public void Add<T>(Func<T> creator)
        {
            lock (Sync)
            {
                Map[typeof(T)] = () => creator();
            }
        }

        public T Get<T>()
        {
            lock (Sync)
            {
                Func<object> creator;
                if (!Map.TryGetValue(typeof(T), out creator))
                    return default(T);

                return (T) creator();
            }
        }



         

    }
}