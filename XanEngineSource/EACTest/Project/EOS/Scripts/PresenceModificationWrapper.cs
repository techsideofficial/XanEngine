using Godot;
using Godot.Collections;
using Epic.OnlineServices;
using Epic.OnlineServices.Presence;
using System.Linq;

public partial class PresenceModificationWrapper : RefCounted
{
    public Result delete_data(RefCounted p_options)
    {
        var p_records = ((System.Collections.IEnumerable)p_options.Get("records")).Cast<string>().Select(x =>
        {
            var p_record = new PresenceModificationDataRecordId()
            {
                Key = new Utf8String(x)
            };
            return p_record;
        }).ToArray();

        var options = new PresenceModificationDeleteDataOptions()
        {
            Records = p_records
        };

        return _internalPresenceModification.DeleteData(ref options);
    }

    public Result set_data(RefCounted p_options)
    {
        var p_records = ((System.Collections.IEnumerable)p_options.Get("records")).Cast<Dictionary>().Select(x =>
       {
           var p_record = new DataRecord()
           {
               Key = new Utf8String((string)x["key"]),
               Value = new Utf8String((string)x["value"])
           };
           return p_record;
       }).ToArray();

        var options = new PresenceModificationSetDataOptions()
        {
            Records = p_records
        };
        return _internalPresenceModification.SetData(ref options);
    }

    public Result set_join_info(RefCounted p_options)
    {
        var options = new PresenceModificationSetJoinInfoOptions()
        {
            JoinInfo = new Utf8String((string)p_options.Get("join_info"))
        };
        return _internalPresenceModification.SetJoinInfo(ref options);
    }

    public Result set_raw_rich_text(RefCounted p_options)
    {
        var options = new PresenceModificationSetRawRichTextOptions()
        {
            RichText = new Utf8String((string)p_options.Get("rich_text"))
        };
        return _internalPresenceModification.SetRawRichText(ref options);
    }

    public Result set_status(RefCounted p_options)
    {
        var options = new PresenceModificationSetStatusOptions()
        {
            Status = (Status)p_options.Get("status")
        };
        return _internalPresenceModification.SetStatus(ref options);
    }

    public PresenceModificationWrapper(PresenceModification presenceModification)
    {
        _internalPresenceModification = presenceModification;
    }

    public PresenceModificationWrapper()
    {
    }

    public new string GetClass()
    {
        return "PresenceModificationWrapper";
    }

    public override string ToString()
    {
        return "PresenceModificationWrapper";
    }

    public PresenceModification _internalPresenceModification;
}