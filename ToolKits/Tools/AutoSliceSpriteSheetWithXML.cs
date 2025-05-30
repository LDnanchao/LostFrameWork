using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Reflection;
using UnityEditor.U2D.Sprites;
using UnityEditor.AssetImporters;

public class AutoSliceSpriteSheetWithXML : AssetPostprocessor
{
    
    private void OnPreprocessTexture()
    {
        //获取各种路径
        string dataPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/") + 1);
        string fullPath = dataPath + assetPath;
        string fileName_without_extension = Path.GetFileNameWithoutExtension(fullPath);
        string extension = Path.GetExtension(fullPath);
        string dirPath = Path.GetDirectoryName(fullPath);
        string xml_fileName = fileName_without_extension + ".xml";
        string xml_fullPath = Path.Combine(dirPath, xml_fileName);

        //检测是否存在同名的.xml文件,不存在则不用左处理
        if (File.Exists(xml_fullPath))
        {
            //根据XML参数进行自动进行图片切割
            XmlDocument doc = new XmlDocument();
            //加载xml文件
            doc.Load(xml_fullPath);

            //从xml中读取第一个节点,该节点imagePath是对应图片的名字,再次确定查看是否和图片的名字匹配
            string target_path = (doc.FirstChild as XmlElement).GetAttribute("imagePath");
            if (target_path != fileName_without_extension + extension)
            {
                throw new System.Exception($"imagePath is {target_path}, but current file is {fileName_without_extension + extension}, please check the xml file");
            }
            // Texture2D texture2D =  context.mainObject as Texture2D;
            
            //将导入对象转换为TextureImporter对象,注意,这里最好在前面加上判断是否是贴图文件
            var importer = assetImporter as TextureImporter;

            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.textureType = TextureImporterType.Sprite;
            importer.GetSourceTextureWidthAndHeight(out int texture_width, out int texture_height);

            //获取所有的目标节点
            var xml_nodes = doc.FirstChild.ChildNodes;

            //新建精灵切片数据集合
            var spriteRectDatas = new SpriteRect[xml_nodes.Count];
            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(assetImporter);

            dataProvider.InitSpriteEditorDataProvider();
            //遍历所有节点信息
            for (int i = 0; i < xml_nodes.Count; i++)
            {
                var node = xml_nodes[i];
                XmlElement element = node as XmlElement;

                //获取节点中所带的信息
                string sprite_name = element.GetAttribute("name");

                float x = float.Parse(element.GetAttribute("x"));
                float y = float.Parse(element.GetAttribute("y"));
                float width = float.Parse(element.GetAttribute("width"));
                float height = float.Parse(element.GetAttribute("height"));

                //re_y是指反向的y,因为unity处理贴图默认以左下角为原点,大部分图集工具中是以左上角为原点,从而导致y值错误,所以这里重新计算y正确的值
                float re_y = texture_height - y - height;

                //新建精灵切片数据对象用于保存单个切片信息
                SpriteRect one_SpriteRectData = new SpriteRect();
                one_SpriteRectData.name = sprite_name;
                one_SpriteRectData.alignment = (int)SpriteAlignment.Center;
                one_SpriteRectData.rect = new Rect(x, re_y, width, height);
                spriteRectDatas[i] = one_SpriteRectData;
            }
            //将所有精灵切片信息数据绑定到TextureImporter上完成设置

            dataProvider.SetSpriteRects(spriteRectDatas);
            dataProvider.Apply();
        }
    }




}
