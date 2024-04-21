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

        // �����̃��[�v����񐔂��A���b�Z�[�W�̃y�[�W�񐔕��ɐݒ�
        flowchart.SetIntegerVariable("loopNum", eventDetailMasterData.list.Count);

        // type �ŕ��򂵁A������ list �̏��Ԃɏ�������
        for (int i = 0; i < eventDetailMasterData.list.Count; i++) {
            dialogInput.ClickMode = ClickMode.Disabled;
            switch (eventDetailMasterData.list[i].type) {

                case "chat":
                    await OneMessageDialogueAsync(eventDetailMasterData.list[i].text_dialog_1, eventDetailMasterData.list[i].text_name, i);
                    break;

                case "shop":
                    // ��b�I��
                    StopTalkDialog();

                    // �ǂ̃V���b�v������肵�ĕ���
                    if (eventDetailMasterData.list[i].shop_index == 1) {
                        // TODO ���̌Ăяo�����V���b�v�|�b�v�A�b�v�ɕς���
                        //PopupManager.instance.Show<GoddessSelectPopup>();
                    }
                    break;

                default:
                    break;
            }
        }
    }

    /// <summary>
    /// ��b�I��
    /// </summary>
    private void StopTalkDialog() {
        // ���݂̃u���b�N���~����
        flowchart.StopAllBlocks();
        // ��b�I���̃u���b�N�����s
        flowchart.ExecuteBlock("End");

        // �����������ď������~�߂�
        sayDialog.Stop();
        sayDialog.Clear();
    }

    /// <summary>
    /// 1�y�[�W���̃��b�Z�[�W�\��
    /// </summary>
    /// <param name="message"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public async UniTask OneMessageDialogueAsync(string message, string name, int blockNum) {

        // ���O�����[�J���C�Y
        //LocalizeMasterData nameData = StringManager.instance.GetLocalizeData(name);
        //string localizeName = nameData.languageDatas[(int)StringManager.instance.CurrentLanguage.Value];

        // ���O�ݒ�
        //txtName.text = localizeName;

        // ���b�Z�[�W�����[�J���C�Y
        //LocalizeMasterData messageData = StringManager.instance.GetLocalizeData(message);
        //string localizeMessage = messageData.languageDatas[(int)StringManager.instance.CurrentLanguage.Value];

        // ���b�Z�[�W��ݒ�
        flowchart.SetStringVariable("Message", message);

        // ���b�Z�[�W��\��
        string blockName = "Say_" + blockNum;
        flowchart.SendFungusMessage(blockName);

        // ��b�p�̉�ʃ^�b�v��L����
        dialogInput.ClickMode = ClickMode.ClickAnywhere;

        // �}�E�X�{�^���N���b�N���A��ʃ^�b�v�����܂őҋ@
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: token);
#if DEBUG
        Debug.Log("�_�C�A���O�I��");
#endif
    }
}