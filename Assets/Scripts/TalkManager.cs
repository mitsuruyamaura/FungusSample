using UnityEngine;
using Fungus;
using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;

public class TalkManager : AbstractSingleton<TalkManager>
{
    [SerializeField] private Flowchart flowchart;
    [SerializeField] private DialogInput dialogInput;
    [SerializeField] private SayDialog sayDialog;

    [SerializeField] private CanvasGroup sayDialogCanvasGroup;
    [SerializeField] private TMP_Text txtName;

    private CancellationToken token;


    protected override void Awake() {
        base.Awake();
        token = this.GetCancellationTokenOnDestroy();
    }

    public async UniTask SetAndPlayMessage(EventDetailMasterData eventDetailMasterData) {

        // 処理のループする回数を、メッセージのページ回数分に設定
        flowchart.SetIntegerVariable("loopNum", eventDetailMasterData.list.Count);

        // type で分岐し、それらを list の順番に処理する
        for (int i = 0; i < eventDetailMasterData.list.Count; i++) {
            dialogInput.ClickMode = ClickMode.Disabled;
            switch (eventDetailMasterData.list[i].type) {

                case "chat":
                    await OneMessageDialogueAsync(eventDetailMasterData.list[i].text_dialog_1, eventDetailMasterData.list[i].text_name, i);
                    break;

                case "shop":
                    // 会話終了
                    StopTalkDialog();

                    // どのショップかを特定して分岐
                    if (eventDetailMasterData.list[i].shop_index == 1) {
                        // TODO この呼び出しをショップポップアップに変える
                        //PopupManager.instance.Show<GoddessSelectPopup>();
                    }
                    break;

                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 会話終了
    /// </summary>
    private void StopTalkDialog() {
        // 現在のブロックを停止する
        flowchart.StopAllBlocks();
        // 会話終了のブロックを実行
        flowchart.ExecuteBlock("End");

        // 文字を消して処理を止める
        sayDialog.Stop();
        sayDialog.Clear();
    }

    /// <summary>
    /// 1ページ分のメッセージ表示
    /// </summary>
    /// <param name="message"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public async UniTask OneMessageDialogueAsync(string message, string name, int blockNum) {

        // 名前をローカライズ
        //LocalizeMasterData nameData = StringManager.instance.GetLocalizeData(name);
        //string localizeName = nameData.languageDatas[(int)StringManager.instance.CurrentLanguage.Value];

        // 名前設定
        //txtName.text = localizeName;

        // メッセージをローカライズ
        //LocalizeMasterData messageData = StringManager.instance.GetLocalizeData(message);
        //string localizeMessage = messageData.languageDatas[(int)StringManager.instance.CurrentLanguage.Value];

        // メッセージを設定
        flowchart.SetStringVariable("Message", message);

        // メッセージを表示
        string blockName = "Say_" + blockNum;
        flowchart.SendFungusMessage(blockName);

        // 会話用の画面タップを有効化
        dialogInput.ClickMode = ClickMode.ClickAnywhere;

        // マウスボタンクリックか、画面タップされるまで待機
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: token);
#if DEBUG
        Debug.Log("ダイアログ終了");
#endif
    }
}