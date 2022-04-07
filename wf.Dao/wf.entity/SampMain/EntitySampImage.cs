using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 标本检测图像表
    /// 旧表名:Samp_image 新表名:Sample_image
    /// </summary>
    [Serializable]
    public class EntitySampImage : EntityBase
    {
        /// <summary>
        /// 自增ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_no", MedName = "img_sn", WFName = "Sima_id", DBIdentity = true)]
        public Int32 ImgSn { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_code", MedName = "samp_bar_code", WFName = "Sima_Smain_bar_id")]
        public String SampBarCode { get; set; }


        /// <summary>
        /// 图片名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_image_name", MedName = "img_image_name", WFName = "Sima_name")]
        public String ImgImageName { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_image_path", MedName = "img_image_file", WFName = "Sima_file")]
        public String ImgImageFile { get; set; }

        /// <summary>
        /// 传入时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_date", MedName = "img_date", WFName = "Sima_date")]
        public DateTime ImageDate { get; set; }

        /// <summary>
        /// 设备
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_device", MedName = "img_instrument", WFName = "Sima_instrument")]
        public String ImgInstrument { get; set; }

        /// <summary>
        /// 类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_ctype", MedName = "img_type", WFName = "Sima_type")]
        public Int32 ImgType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_remark", MedName = "img_remark", WFName = "Sima_remark")]
        public String ImagRemark { get; set; }

        /// <summary>
        /// 图像2进制
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_image_chr", MedName = "img_image_value", WFName = "Sima_value")]
        public Byte[] ImgImageValue { get; set; }

    }
}
