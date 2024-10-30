using UnityEngine;
using TMPro;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using ICTLauncher.Scroll;

namespace ICTLauncher {
    /// <summary>
    /// ランチャーアプリを管理するクラス
    /// </summary>
    public class ApplicationManager : MonoBehaviour
    {
        /// <summary>
        /// コンテンツを表示するスクロールビュー
        /// </summary>
        [SerializeField] private ScrollView scrollView;
        [SerializeField] private UnityEngine.UI.Button playButton; 
        [SerializeField] private DescriptionElements descriptionElements;

        // public static string ContentsPath { get; private set; }

        // void Awake()
        // {
        //     #if UNITY_EDITOR
        //     ContentsPath = $"{Application.persistentDataPath}/Contents";
        //     #endif
            
        //     #if UNITY_STANDALONE_WIN
        //     ContentsPath = $"{Application.dataPath}/Contents";
        //     #endif
        // }

        void Start()
        {
            List<Description> descriptions = Directory.EnumerateDirectories($"{Application.persistentDataPath}/Contents", "*", SearchOption.TopDirectoryOnly)
                                               .Select((directory) => {
                                                   Description description = JsonUtility.FromJson<Description>(File.ReadAllText($"{directory}/description.json", System.Text.Encoding.UTF8));
                                                   description.DirectoryName = directory;
                                                   return description;
                                               }).ToList();
            descriptions.ForEach((description) => {
                description.GetIcon();
            });
            scrollView.UpdateContents(descriptions);
            scrollView.OnSelectionChanged = (index) =>
                      {
                          playButton.onClick.RemoveAllListeners();
                          descriptionElements.title.text = descriptions[index].title;
                          descriptionElements.author.text = descriptions[index].author;
                          descriptionElements.comment.text = descriptions[index].comment.Listformat("");
                          descriptionElements.control.text = descriptions[index].control.Listformat();
                          playButton.onClick.AddListener(() => descriptions[index].OpenApplication());
                      };
            scrollView.SelectCell(0);
        }

        void Update()
        {
            
        }

        [System.Serializable]
        private struct DescriptionElements
        {
            public TextMeshProUGUI title;
            public TextMeshProUGUI author;
            public TextMeshProUGUI comment;
            public TextMeshProUGUI control;
        }
    }
}


