using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transfer8Pro.Entity
{
    /// <summary>
    /// 文件信息
    /// </summary>
    [Serializable]
    public class FileInfoEntity
    {
        /// <summary>
        /// SQL查询开始时间
        /// </summary>
        public DateTime SqlStartTime { get; set; }

        /// <summary>
        /// Sql查询结束时间
        /// </summary>
        public DateTime SqlEndTime { get; set; }

        /// <summary>
        /// 一般文件全路径
        /// 如 C:\OpenBookCode_New\Transfer8Pro\Code\Transfer8Pro\Transfer8Pro.Test\bin\Debug\Data\NormalDataFile\20181012_093424_d_book.jl
        /// </summary>
        public string NormalFilePath { get; set; }

        /// <summary>
        /// 一般文件名
        /// 如20181012_093424_d_book.jl
        /// </summary>
        public string NormalFileName { get; set; }

        /// <summary>
        /// 压缩文件全路径
        /// 如 C:\OpenBookCode_New\Transfer8Pro\Code\Transfer8Pro\Transfer8Pro.Test\bin\Debug\Data\CompressDataFile\20181012_093424_d_sale_34eidh_ew8sc1.zip
        /// </summary>
        public string CompressFilePath { get; set; }

        /// <summary>
        /// 压缩文件名
        /// 如 20181012_093424_d_sale_34eidh_ew8sc1.zip
        /// </summary>
        public string CompressFileName { get; set; }
    }
}
