using FishNet.Object;
using Keys;

namespace onlinetutorial.interfaces
{
    public interface ISpawn
    {
        public void SpawnChibi(SpawnArgs args);
        
        public void DeSpawnChibi(NetworkObject connection);


    }

   
}