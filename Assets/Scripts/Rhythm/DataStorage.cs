using System;
using Utils;

namespace Rhythm
{
    public class DataStorage : Singleton<DataStorage>
    {
        public static event Action OnDataUpdated;
        
        
    }
}
