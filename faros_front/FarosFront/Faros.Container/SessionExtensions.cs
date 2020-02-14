using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;


namespace Faros.Container
{
    public class SessionExtensions
    {
        public void SetObject(this ISession session,string key,object value)
        {
            session.SetString();
        }
    }
}
