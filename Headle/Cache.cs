using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UartCollect.Headle
{
    public static class Cache
    {
        private static string CachePath = @"Data\";
        #region 缓存读取写入
        /// <summary>
        /// 读取缓存 调用
        /// </summary>
        /// <returns></returns>
        public static object Read_CacheBin(string filename)
        {
            object obj = null;
            string fullpath = CachePath + filename + ".bin";
            try
            {
                if (FileHelper.IsExistFile(fullpath))
                {
                    //对象二进制序列化
                    BinaryFormatter bf = new BinaryFormatter();
                    using (FileStream fsRead = new FileStream(fullpath, FileMode.Open))
                    {
                        obj = bf.Deserialize(fsRead);
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                UIAction.AppendLog("读取缓存异常：" + ex.Message);
                return obj;
            }
        }
        /// <summary>
        /// 缓存数据 调用
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool Save_Cache(object obj, string filename)
        {
            try
            {
                //对象二进制序列化
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fsWrite = new FileStream(CachePath + filename + ".bin", FileMode.Create))
                {
                    bf.Serialize(fsWrite, obj);
                    return true;
                }
            }
            catch (Exception ex)
            {
                UIAction.AppendLog("写入缓存异常：" + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void Clear()
        {
            string[] files = FileHelper.GetFileNames(CachePath);
            foreach (string item in files)
            {
                FileHelper.DeleteFile(item);
            }
            UIAction.AppendLog("缓存已清除");
        }
        #endregion
    }
}
