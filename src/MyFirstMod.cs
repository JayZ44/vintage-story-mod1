using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;

namespace MyFirstMod
{
    public class MyFirstMod : ModSystem
    {
        ICoreServerAPI sapi;
        static readonly string[] hostileEntities = new string[] 
        {
            "wolf-male", "wolf-female",
            "drifter-normal", "drifter-cold", "drifter-temporal",
            "bear-male", "bear-female",
            "locust"
        };

        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            
            if (api is ICoreServerAPI serverApi)
            {
                sapi = serverApi;
                sapi.Event.BlockBroken += OnBlockBroken;
            }
        }

        private void OnBlockBroken(IServerPlayer byPlayer, BlockSelection blockSel)
        {
            // Get a random entity code from our list
            string entityCode = hostileEntities[byPlayer.World.Rand.Next(hostileEntities.Length)];
            
            // Create the entity
            EntityProperties type = byPlayer.World.GetEntityType(new AssetLocation("game", entityCode));
            if (type == null) return;

            Entity entity = byPlayer.World.ClassRegistry.CreateEntity(type);
            if (entity == null) return;

            // Position the entity where the block was broken
            entity.ServerPos.SetPos(blockSel.Position.ToVec3d().Add(0.5, 1.0, 0.5));
            entity.Pos.SetFrom(entity.ServerPos);
            
            // Spawn the entity in the world
            byPlayer.World.SpawnEntity(entity);
        }
    }
} 