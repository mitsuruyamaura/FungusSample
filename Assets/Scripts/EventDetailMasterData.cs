using System;
using System.Collections.Generic;

[Serializable]
public class EventDetailMasterData {
    public int event_detail_id;
    public string help;
    public int element;
    public List<EventDetailElement> list;
}

[Serializable]
public class EventDetailElement {
    public int id;
    public string type;
    public string text_name;
    public string text_dialog_1;
    public string text_dialog_2;
    public int give_item_index;
    public int give_item_count;
    public int get_item_index;
    public int get_item_count;
    public int shop_index;
    public int once;
}