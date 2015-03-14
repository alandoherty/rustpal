using RustTest.Net;
using System;
using System.IO;

namespace RustTest.Entities
{
    public interface IEntity
    {
        Vector3f Position { get; set; }
        Angle2 Angle { get; set; }
        string Prefab { get; set;  }
        ushort ID { get; }
        byte LinkID { get; set; }
        string Name { get; }

        bool IsPlayer { get; }
        bool IsClient { get; }
        bool IsWeapon { get; }

        void OnDestroy();

        void Serialize(ULinkStream stream);
        void Unserialize(ULinkStream stream);
    }
}
