using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Helper
{
    /// <summary>
    /// GuidSet用了两个HashSet，以此最大化性能表现
    /// </summary>
    public class GuidSet
    {
        private readonly HashSet<Guid> m_setGuids1 = new HashSet<Guid>();
        private readonly HashSet<Guid> m_setGuids2 = new HashSet<Guid>();

        private HashSet<Guid> m_setGuidCurr;

        public GuidSet()
        {
            m_setGuidCurr = m_setGuids1;
        }

        public bool IsExistAndAdd(Guid guidNew)
        {
            if (m_setGuids1.Contains(guidNew) || m_setGuids2.Contains(guidNew))
                return false;

            m_setGuidCurr.Add(guidNew);
            if (m_setGuidCurr.Count > 100)
            {
                if (m_setGuidCurr.Equals(m_setGuids1))
                {
                    m_setGuids2.Clear();
                    m_setGuidCurr = m_setGuids2;
                }
                else
                {
                    m_setGuids1.Clear();
                    m_setGuidCurr = m_setGuids1;
                }
            }

            return true;
        }
    }
}
