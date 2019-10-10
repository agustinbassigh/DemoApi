using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Service.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string message)
            : base(message)
        {
        }

        public ServiceException(string message, Exception e)
            : base(message, e)
        {
        }
    }

}
