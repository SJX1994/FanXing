using UnityEngine;
using QFramework;
using System.Xml;
using System.IO;
using System.Linq;
using System.Collections.Generic;
namespace FanXing
{
    public interface IDialogueModel : IModel
    {
        BindableProperty<int> CurrentChapter { get; }
        BindableProperty<int> CurrentTakingIndex { get; }
        BindableProperty<string> CurrentContent { get; }
        BindableProperty<string> CurrentName { get; }
        string mLevelFilesFolder { get; }
        List<XmlDocument> allDialogueFiles { get; }
        void ParseChapter(int chapter);
    }
    public class DialogueModel : AbstractModel,IDialogueModel
    {
        public BindableProperty<string> CurrentContent { get; } = new BindableProperty<string>();
        public BindableProperty<int> CurrentTakingIndex { get; } = new BindableProperty<int>();
        public BindableProperty<string> CurrentName { get; } = new BindableProperty<string>();

        public BindableProperty<int> CurrentChapter { get; } = new BindableProperty<int>();

        public string mLevelFilesFolder 
        {
            get;
            private set;
        }
        public List<XmlDocument> allDialogueFiles { get; } = new List<XmlDocument>();
        private XmlNodeList talkings;
        protected override void OnInit()
        {
            // 初始化读取工具
            allDialogueFiles.Clear();
            // CurrentChapter.Value = -1;
            CurrentTakingIndex.Value = 0;
            mLevelFilesFolder = Application.persistentDataPath + "/DialogueFiles";
            if (!Directory.Exists(mLevelFilesFolder))
            {
                Directory.CreateDirectory(mLevelFilesFolder);
            }
            var filePaths = Directory.GetFiles(mLevelFilesFolder);
            if(filePaths.Length == 0)
            {
                Debug.LogError("请将对话文档放置到 "+mLevelFilesFolder+" 文件夹下");
                return;
            }
            Debug.Log("DialogueModel：共读取"+ filePaths.Length + "个章节。");
            foreach (var filePath in filePaths.Where(f=>f.EndsWith("xml")))
            {
                var fileName = Path.GetFileName(filePath);
                var xml = File.ReadAllText(filePath);
                ParseAndSave(xml);
                // ParseAndRun(xml);
            }
            // CurrentChapter.Register(chapter =>
            // {
            //     ParseChapter(chapter);
            // });
            CurrentTakingIndex.Register(index =>
            {
                ParseNextTaking();
            });
        }
        void ParseAndSave(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            allDialogueFiles.Add(document);
        }
        public void ParseNextTaking()
        {
            if(CurrentTakingIndex.Value >= talkings.Count)
            {
                this.SendEvent<OnDialogueChapterLastTalkEvent>();
                return;
            }
            CurrentName.Value = talkings[CurrentTakingIndex.Value].Attributes["name"].Value;
            CurrentContent.Value = talkings[CurrentTakingIndex.Value].Attributes["content"].Value;
        }
        public void ParseChapter(int chapter)
        {
            var document = new XmlDocument();

            foreach (var dialogueFile in allDialogueFiles)
            {
                var chapterNodeTemp = dialogueFile.SelectSingleNode("Chapter");
                var chapterInFile = int.Parse(chapterNodeTemp.Attributes["chapter"].Value);
                if(chapterInFile == chapter)
                {
                    document = dialogueFile;
                    break;
                }
            }
            
            if(document == null)
            {
                Debug.LogError("没有找到章节"+chapter+"的对话文件");
                return;
            }
            CurrentChapter.Value = chapter;

            var chapterNode = document.SelectSingleNode("Chapter");
            CurrentTakingIndex.Value = 0;
            CurrentName.Value = chapterNode.ChildNodes[CurrentTakingIndex.Value].Attributes["name"].Value;
            CurrentContent.Value = chapterNode.ChildNodes[CurrentTakingIndex.Value].Attributes["content"].Value;
            talkings = chapterNode.ChildNodes;
            // foreach (XmlElement talkingNode in chapterNode.ChildNodes)
            // {
            //     var talkingName = talkingNode.Attributes["name"].Value;
            //     var talkingContent = talkingNode.Attributes["content"].Value;
            //     // Debug.Log(" name:" + talkingName+" content:"+talkingContent);
            // }
        }
    }
}
