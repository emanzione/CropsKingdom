using System;

namespace CropsKingdom.Gameplay.Entities
{
    public struct Entity : IEquatable<Entity>
    {
        public int Id;

        public static bool operator ==(Entity entity1, Entity entity2)
        {
            return entity1.Id == entity2.Id;
        }

        public static bool operator !=(Entity entity1, Entity entity2)
        {
            return !(entity1 == entity2);
        }

        public bool Equals(Entity other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Entity other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return "Entity#" + Id.ToString();
        }
    }
}