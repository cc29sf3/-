using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{

    public static class Util
    {
        /// <summary>
        /// 计算目录中所有嵌套的文件的个数
        /// </summary>
        /// <param name="sPathName">文件夹路径</param>
        /// <returns>文件个数</returns>
        public int Traverse(string sPathName)
        {
            //创建一个队列用于保存子目录
            int i = 0;
            Queue<string> pathQueue = new Queue<string>();
            pathQueue.Enqueue(sPathName);
            //开始循环查找文件，直到队列中无任何子目录
            while (pathQueue.Count > 0)
            {
                DirectoryInfo diParent = new DirectoryInfo(pathQueue.Dequeue());
                foreach (DirectoryInfo diChild in diParent.GetDirectories())
                    pathQueue.Enqueue(diChild.FullName);
                foreach (FileInfo fi in diParent.GetFiles())
                {
                    //if (!fi.Name.StartsWith("~$") )
                    i++;
                }
            }
            return i;
        }
    }
}