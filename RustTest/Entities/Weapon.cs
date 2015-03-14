using RustTest.Net;
using System;

namespace RustTest.Entities
{
    public class Weapon : Entity
    {
        #region Fields
        public static string[] WeaponPrefabs = new string[] {
        };
        #endregion

        #region Properties
        #endregion

        #region Methods
        public void Action3B(ULinkStream stream) {
            // begin swing
        }

        public void Action1B(ULinkStream stream) {
            // end swing
        }

        public void Action2(ULinkStream stream) {

        }

        /// <summary>
        /// Determines whether [is prefab weapon] [the specified prefab].
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <returns></returns>
        public static bool IsPrefabWeapon(string prefab) {
            foreach (string weapon in WeaponPrefabs) {
                if (weapon == prefab)
                    return true;
            }

            return false;
        }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Weapon"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="id">The identifier.</param>
        public Weapon(World world, ushort id)
            : base(world, id) { }
        #endregion
    }
}
