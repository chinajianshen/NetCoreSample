using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;
using CommLib;

namespace BLL
{
    public class UserManager
    {
        private readonly string m_strDatabaseFile = AppDomain.CurrentDomain.BaseDirectory + "Database.xml";
        private readonly XmlDocument m_xmldocDatabase = new XmlDocument();

        public void Load()
        {
            m_xmldocDatabase.Load(m_strDatabaseFile);
        }

        public IEnumerable<UserInfo_BLL> GetUsers()
        {
            XmlNodeList userNodes = m_xmldocDatabase.SelectNodes("/Persons/Person");
            return from XmlNode node in userNodes select GetUserInfoFromXmlNode(node);
        }

        public UserInfo_BLL GetUser(string strUserName)
        {
            XmlNode node = m_xmldocDatabase.SelectSingleNode(string.Format("/Persons/Person[UserName='{0}']", strUserName));
            return node != null ? GetUserInfoFromXmlNode(node) : null;
        }

        public string GetEncryptedPwdOfUser(string strUserName)
        {
            return GetNodePassword(strUserName).InnerText;
        }

        public string GetUserRole(string strUserName)
        {
            XmlNode node = m_xmldocDatabase.SelectSingleNode(string.Format("/Persons/Person/Role[../UserName='{0}']", strUserName));
            return node != null ? node.InnerText : string.Empty;
        }

        /// <summary>
        /// 只能设置RealName,Height和Birthday
        /// </summary>
        public void SetUser(UserInfo_BLL userinfo)
        {
            XmlNode node = GetNodePerson(userinfo.UserName);

            //检查时间戳
            if(userinfo.UpdateTicks != long.Parse(node.SelectSingleNode("UpdateTicks").InnerText))
                throw new WebApiException(WebApiExceptionCode.ConcurrencyConflict);

            node.SelectSingleNode("RealName").InnerText = userinfo.RealName;
            node.SelectSingleNode("Height").InnerText = userinfo.Height.ToString(CultureInfo.InvariantCulture);
            node.SelectSingleNode("Birthday").InnerText = userinfo.Birthday.SimpleDate();
            if (!string.IsNullOrEmpty(userinfo.Role))
            {
                node.SelectSingleNode("Role").InnerText = userinfo.Role;
            }
            node.SelectSingleNode("UpdateTicks").InnerText = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            Save();
        }

        public void SetPassword(string strUserName, string strPasswordNew)
        {
            XmlNode node = GetNodePassword(strUserName);
            node.InnerText = Md5.MD5TwiceEncode(strPasswordNew);
            Save();
        }

        private void Save()
        {
            m_xmldocDatabase.Save(m_strDatabaseFile);
        }

        private XmlNode GetNodePerson(string strUserName)
        {
            XmlNode node = m_xmldocDatabase.SelectSingleNode(string.Format("/Persons/Person[UserName='{0}']", strUserName));
            if (node == null)
            {
                throw new WebApiException(WebApiExceptionCode.ItemDoesNotExist) { ParameterObject = new { UserName = strUserName } };
            }
            return node;
        }

        private XmlNode GetNodePassword(string strUserName)
        {
            XmlNode node = m_xmldocDatabase.SelectSingleNode(string.Format("/Persons/Person/Password[../UserName='{0}']", strUserName));
            if (node == null)
            {
                throw new WebApiException(WebApiExceptionCode.ItemDoesNotExist) { ParameterObject = new { UserName = strUserName } };
            }
            return node;
        }

        private UserInfo_BLL GetUserInfoFromXmlNode(XmlNode node)
        {
            Debug.WriteLine(node.SelectSingleNode("UpdateTicks").InnerText);

            return new UserInfo_BLL
            {
                UserName = node.SelectSingleNode("UserName").InnerText,
                RealName = node.SelectSingleNode("RealName").InnerText,
                Height = float.Parse(node.SelectSingleNode("Height").InnerText),
                Birthday = DateTime.Parse(node.SelectSingleNode("Birthday").InnerText),
                Role = node.SelectSingleNode("Role").InnerText,
                UpdateTicks = long.Parse(node.SelectSingleNode("UpdateTicks").InnerText)
            };
        }
    }
}
