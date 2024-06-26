﻿using PaddleOCRSharp;
using System.Diagnostics;
using System.Text;

namespace Cocr.Util
{
    internal class OCRUtils
    {
        private static volatile OCRUtils INSTANCE = null;
        private static readonly object lockHelper = new object();

        private static volatile PaddleOCREngine? engine;

        private OCRUtils()
        {
            //自带轻量版中英文模型V4模型
            OCRModelConfig? config = null;

            //OCR参数
            OCRParameter oCRParameter = new OCRParameter();
            oCRParameter.cpu_math_library_num_threads = 10;//预测并发线程数
            oCRParameter.enable_mkldnn = true;
            oCRParameter.cls = false; //是否执行文字方向分类；默认false
            oCRParameter.det = true;//是否开启文本框检测，用于检测文本块
            oCRParameter.use_angle_cls = false;//是否开启方向检测，用于检测识别180旋转
            oCRParameter.det_db_score_mode = true;//是否使用多段线，即文字区域是用多段线还是用矩形，
            oCRParameter.max_side_len = 1920;
            oCRParameter.rec_img_h = 48;
            oCRParameter.rec_img_w = 320;
            oCRParameter.det_db_thresh = 0.3f;
            oCRParameter.det_db_box_thresh = 0.618f;

            //初始化OCR引擎
            engine = new PaddleOCREngine(config, oCRParameter);
        }

        public static OCRUtils getInstance()
        {
            if (INSTANCE == null)
            {
                lock (lockHelper)
                {
                    if (INSTANCE == null)
                        INSTANCE = new OCRUtils();
                }
            }
            return INSTANCE;
        }

        private string toString(List<TextBlock> textBlocks)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in textBlocks)
            {
                if (!string.IsNullOrWhiteSpace(item.Text)){
                    sb.Append(item.Text);
                    sb.Append('\n');
                }
            }
            return sb.ToString();
        }

        public string getResult(Image image)
        {
            List<TextBlock> textBlocks;
            try
            {
                OCRResult? ocrResult = engine.DetectText(image);
                textBlocks = ocrResult.TextBlocks;
            } catch (Exception ex)
            {
                textBlocks = new List<TextBlock>();
                MessageBox.Show(ex.Message);
            }
            return toString(textBlocks);

        }
        public string getResult(string imagebase64)
        {
            if (imagebase64 == null || imagebase64.Length <= 1)
            {
                return "";
            }
            int commaIndex = imagebase64.IndexOf(',');

            if (commaIndex != -1)
            {
                imagebase64 = imagebase64.Substring(commaIndex + 1);
            }
            try
            {
                OCRResult? ocrResult = engine.DetectTextBase64(imagebase64);
                return toString(ocrResult.TextBlocks);
            } catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return "";
            }
        }
    }
}
