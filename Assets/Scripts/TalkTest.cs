using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class TalkTest : MonoBehaviour
{
    [SerializeField] private EventDetailMasterData eventDetailMasterData;


    void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(async _ => await TalkManager.instance.SetAndPlayMessage(eventDetailMasterData));
    }
}