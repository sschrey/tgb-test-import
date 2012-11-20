using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tweddle.Commons.Business
{
    /// <summary>
    /// Summary description for Entity.
    /// </summary>
    /// 
    [Serializable]
    public class Entity
    {
        private string id;

        public Entity()
        {

        }

        public Entity(string id)
        {
            this.id = id;
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj == this)
                return true;
            else
            {
                if (obj is Entity)
                {
                    Entity other = (Entity)obj;

                    if (this.id == null && other.id == null)
                        return false;
                    else if (this.id.Equals(string.Empty) && other.id.Equals(string.Empty))
                        return false;
                    else
                        return (other.id == this.id);
                }
                else
                {
                    return false;
                }
            }
        }

        public override int GetHashCode()
        {
            return this.id == null ? 29 : this.id.GetHashCode();
        }
    }
}
