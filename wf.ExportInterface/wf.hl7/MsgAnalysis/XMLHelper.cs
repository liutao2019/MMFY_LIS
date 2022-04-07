using Lib.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.hl7
{
    public static class XMLHelper
    {
        /// <summary>
        /// 获取根节点指定路径指定属性的值
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="AttName"></param>
        /// <returns></returns>
        public static string GetAttValueForDoc(XmlDocument doc_rxml, string xpath, string AttName)
        {
            string rv = "";
            try
            {
                if (doc_rxml.DocumentElement.Attributes["xmlns"] != null)
                {
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc_rxml.NameTable);
                    nsmgr.AddNamespace("ab", doc_rxml.DocumentElement.Attributes["xmlns"].Value);
                    rv = doc_rxml.SelectSingleNode(xpath.Replace("/", "/ab:"), nsmgr).Attributes[AttName].Value;
                }
                else
                {
                    rv = doc_rxml.SelectSingleNode(xpath).Attributes[AttName].Value;
                }
            }
            catch
            {
            }
            return rv;
        }

        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="AttName"></param>
        /// <returns></returns>
        public static XmlNode GetSingleNodeForDoc(XmlDocument doc_rxml, string xpath)
        {
            XmlNode rvNode = null;
            try
            {
                if (doc_rxml.DocumentElement.Attributes["xmlns"] != null)
                {
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc_rxml.NameTable);
                    nsmgr.AddNamespace("ab", doc_rxml.DocumentElement.Attributes["xmlns"].Value);
                    rvNode = doc_rxml.SelectSingleNode(xpath.Replace("/", "/ab:"), nsmgr);
                }
                else
                {
                    rvNode = doc_rxml.SelectSingleNode(xpath);
                }
            }
            catch
            {
            }
            return rvNode;
        }

        /// <summary>
        /// 获取匹配的所有节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static XmlNodeList GetNodesForDoc(XmlDocument doc_rxml, string xpath)
        {
            XmlNodeList rvNode = null;
            try
            {
                if (doc_rxml.DocumentElement.Attributes["xmlns"] != null)
                {
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc_rxml.NameTable);
                    nsmgr.AddNamespace("ab", doc_rxml.DocumentElement.Attributes["xmlns"].Value);
                    rvNode = doc_rxml.SelectNodes(xpath.Replace("/", "/ab:"), nsmgr);
                }
                else
                {
                    rvNode = doc_rxml.SelectNodes(xpath);
                }
            }
            catch
            {
            }
            return rvNode;
        }

        /// <summary>
        /// 获取节点里面子节点名称
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static XmlNode GetChildNodeByPath(XmlDocument doc_rxml, XmlNode node, string xpath)
        {
            XmlNode rvNode = null;
            try
            {
                string[] splxpath = xpath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                if (splxpath.Length >= 1)
                {
                    if (node != null && node.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode node_ChildNode in node.ChildNodes)
                        {
                            if (node_ChildNode != null && node_ChildNode.Name == splxpath[0])
                            {
                                rvNode = node_ChildNode;
                                break;
                            }
                        }
                    }
                }



                if (splxpath.Length > 1 && rvNode != null)
                {
                    string xpathNw = "";
                    for (int i = 1; i < splxpath.Length; i++)
                    {
                        if (string.IsNullOrEmpty(xpathNw))
                        {
                            xpathNw = splxpath[i];
                        }
                        else
                        {
                            xpathNw += "/" + splxpath[i];
                        }
                    }

                    rvNode = GetChildNodeByPath(doc_rxml, rvNode, xpathNw);
                }
            }
            catch
            {
            }
            return rvNode;
        }

        /// <summary>
        /// 获取节点里面子节点名称的集合
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static List<XmlNode> GetChildNodeListByPath(XmlDocument doc_rxml, XmlNode node, string xpath)
        {
            List<XmlNode> rvNodeList = new List<XmlNode>();

            try
            {
                string[] splxpath = xpath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                if (splxpath.Length >= 1)
                {
                    if (node != null && node.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode node_ChildNode in node.ChildNodes)
                        {
                            if (splxpath.Length == 1)
                            {
                                if (node_ChildNode != null && node_ChildNode.Name == splxpath[0])
                                {
                                    rvNodeList.Add(node_ChildNode);
                                }
                            }
                            else
                            {
                                if (node_ChildNode != null && node_ChildNode.Name == splxpath[0])
                                {
                                    rvNodeList.Add(node_ChildNode);
                                    break;
                                }
                            }
                        }
                    }
                }



                if (splxpath.Length > 1 && rvNodeList.Count > 0)
                {
                    string xpathNw = "";
                    for (int i = 1; i < splxpath.Length; i++)
                    {
                        if (string.IsNullOrEmpty(xpathNw))
                        {
                            xpathNw = splxpath[i];
                        }
                        else
                        {
                            xpathNw += "/" + splxpath[i];
                        }
                    }

                    rvNodeList = XMLHelper.GetChildNodeListByPath(doc_rxml, rvNodeList[0], xpathNw);
                }
            }
            catch
            {
            }
            return rvNodeList;
        }


        public static string XMLDocumentToString(XmlDocument doc)
        {
            MemoryStream ms = null;
            XmlTextWriter XmlWt = null;
            try
            {
                ms = new MemoryStream();

                XmlWt = new XmlTextWriter(ms, Encoding.UTF8);

                XmlWt.Formatting = Formatting.Indented;

                doc.Save(XmlWt);

                StreamReader sr = new StreamReader(ms, Encoding.UTF8);

                ms.Position = 0;

                string XMLString = sr.ReadToEnd();

                sr.Close();

                ms.Close();

                return XMLString;

            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
            }
            finally
            {
                //释放资源
                if (XmlWt != null)
                {
                    XmlWt.Close();
                    ms.Close();
                    ms.Dispose();
                }
            }

            return null;
        }
    }
}
