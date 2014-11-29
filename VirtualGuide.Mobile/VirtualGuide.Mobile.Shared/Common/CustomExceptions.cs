using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualGuide.Mobile.Common
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class TravelAlreadyOwnedException : Exception
    {
        public TravelAlreadyOwnedException()
        {
        }

        public TravelAlreadyOwnedException(string message)
            : base(message)
        {
        }

        public TravelAlreadyOwnedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
