namespace Alba.Framework.Interop
{
    public enum ASSOCSTR
    {
        /// <summary>A command string associated with a Shell verb.</summary>
        COMMAND = 1,
        /// <summary>An executable from a Shell verb command string. For example, this string is found as the (Default) value for a subkey such as HKEY_CLASSES_ROOT\ApplicationName\shell\Open\command. If the command uses Rundll.exe, set the ASSOCF_REMAPRUNDLL flag in the flags parameter of IQueryAssociations::GetString to retrieve the target executable.</summary>
        EXECUTABLE,
        /// <summary>The friendly name of a document type.</summary>
        FRIENDLYDOCNAME,
        /// <summary>The friendly name of an executable file.</summary>
        FRIENDLYAPPNAME,
        /// <summary>Ignore the information associated with the open subkey.</summary>
        NOOPEN,
        /// <summary>Look under the ShellNew subkey.</summary>
        SHELLNEWVALUE,
        /// <summary>A template for DDE commands.</summary>
        DDECOMMAND,
        /// <summary>The DDE command to use to create a process.</summary>
        DDEIFEXEC,
        /// <summary>The application name in a DDE broadcast.</summary>
        DDEAPPLICATION,
        /// <summary>The topic name in a DDE broadcast.</summary>
        DDETOPIC,
        /// <summary>Corresponds to the InfoTip registry value. Returns an info tip for an item, or list of properties in the form of an IPropertyDescriptionList from which to create an info tip, such as when hovering the cursor over a file name. The list of properties can be parsed with PSGetPropertyDescriptionListFromString.</summary>
        INFOTIP,
        /// <summary>Introduced in Internet Explorer 6. Corresponds to the QuickTip registry value. Same as ASSOCSTR_INFOTIP, except that it always returns a list of property names in the form of an IPropertyDescriptionList. The difference between this value and ASSOCSTR_INFOTIP is that this returns properties that are safe for any scenario that causes slow property retrieval, such as offline or slow networks. Some of the properties returned from ASSOCSTR_INFOTIP might not be appropriate for slow property retrieval scenarios. The list of properties can be parsed with PSGetPropertyDescriptionListFromString.</summary>
        QUICKTIP,
        /// <summary>Introduced in Internet Explorer 6. Corresponds to the TileInfo registry value. Contains a list of properties to be displayed for a particular file type in a Windows Explorer window that is in tile view. This is the same as ASSOCSTR_INFOTIP, but, like ASSOCSTR_QUICKTIP, it also returns a list of property names in the form of an IPropertyDescriptionList. The list of properties can be parsed with PSGetPropertyDescriptionListFromString.</summary>
        TILEINFO,
        /// <summary>Introduced in Internet Explorer 6. Describes a general type of MIME file association, such as image and bmp, so that applications can make general assumptions about a specific file type.</summary>
        CONTENTTYPE,
        /// <summary>Introduced in Internet Explorer 6. Returns the path to the icon resources to use by default for this association. Positive numbers indicate an index into the dll's resource table, while negative numbers indicate a resource ID. An example of the syntax for the resource is "c:\myfolder\myfile.dll,-1".</summary>
        DEFAULTICON,
        /// <summary>Introduced in Internet Explorer 6. For an object that has a Shell extension associated with it, you can use this to retrieve the CLSID of that Shell extension object by passing a string representation of the IID of the interface you want to retrieve as the pwszExtra parameter of IQueryAssociations::GetString. For example, if you want to retrieve a handler that implements the IExtractImage interface, you would specify "{BB2E617C-0920-11d1-9A0B-00C04FC2D6C1}", which is the IID of IExtractImage.</summary>
        SHELLEXTENSION,
        /// <summary>Introduced in Internet Explorer 8.. For a verb invoked through COM and the IDropTarget interface, you can use this flag to retrieve the IDropTarget object's CLSID. This CLSID is registered in the DropTarget subkey. The verb is specified in the pwszExtra parameter in the call to IQueryAssociations::GetString.</summary>
        DROPTARGET,
        /// <summary>Introduced in Internet Explorer 8.. For a verb invoked through COM and the IExecuteCommand interface, you can use this flag to retrieve the IExecuteCommand object's CLSID. This CLSID is registered in the verb's command subkey as the DelegateExecute entry. The verb is specified in the pwszExtra parameter in the call to IQueryAssociations::GetString.</summary>
        DELEGATEEXECUTE,
        /// <summary>Introduced in Windows 8.</summary>
        SUPPORTED_URI_PROTOCOLS,
        /// <summary>The maximum defined ASSOCSTR value, used for validation purposes.</summary>
        MAX,
    }
}