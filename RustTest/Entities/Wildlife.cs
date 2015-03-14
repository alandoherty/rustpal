using RustTest.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RustTest.Entities
{
    public class Wildlife : Entity
    {
        #region Fields
        private WildlifeState state;

        public static string[] AnimalPrefabs = new string[] {
            ":chicken_prefab",
            ":rabbit_prefab_a",
            ":bear_prefab",
            ":mutant_wolf",
            ":boar_prefab",
            ":mutant_bear",
            ":stag_prefab",
            ":wolf_prefab"
        };
        #endregion

        #region Properties
        public WildlifeState State {
            get {
                return state;
            }
        }
        #endregion

        #region Methods   
        public void Snd(ULinkStream stream) {
            // unkown
            stream.ReadBytes(2);

            // state
            this.state = (WildlifeState)stream.ReadUByte();
        }

        public void GetNetworkUpdate(ULinkStream stream) {
            // unkown
            stream.ReadBytes(5);

            // read pos
            this.Position = new Vector3f(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
            
            // read angle
            this.Angle = new Angle2() {
                encoded = stream.ReadInt()
            };
        }

        /// <summary>
        /// Determines whether [is prefab wildlife] [the specified prefab].
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns></returns>
        public static bool IsPrefabWildlife(string prefab) {
            foreach (string animal in AnimalPrefabs) {
                if (animal == prefab)
                    return true;
            }

            return false;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Wildlife"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Wildlife(World world, ushort id)
            : base(world, id) { }
        #endregion
    }

    public enum WildlifeState {
        Idle,
        Warn,
        Attack,
        Afraid,
        Death,
        Chase,
        ChaseClose
    }
}
