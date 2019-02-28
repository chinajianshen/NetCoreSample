using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApiKit
{
    class InternalHelper
    {
        public static string GetEffectiveUri(string strUri)
        {
            int iTheLastPos = strUri.Length;
            int iTheLastQuestionMarkPos = strUri.LastIndexOf('?');
            if (iTheLastQuestionMarkPos != -1)
            {
                iTheLastPos = iTheLastQuestionMarkPos;
            }
            int iTheApiPosition = strUri.IndexOf("/api/", 0, StringComparison.InvariantCultureIgnoreCase);
            if (iTheApiPosition != -1 && iTheApiPosition != 0)
            {
                return strUri.Substring(iTheApiPosition, iTheLastPos - iTheApiPosition);
            }
            return strUri.Substring(0, iTheLastPos);
        }
    }
}
