using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveUnit
{
    private SaveNLoadManager _SaveNLoadManager;

    public SaveUnit()
    {
        _SaveNLoadManager = SaveNLoadManager.Instance;

        _SaveNLoadManager.RegistSaveUnit(this);
    }
    
    
    protected abstract string GetJsonData();
    
    ~SaveUnit()
    {
        _SaveNLoadManager.RemoveSaveUnit(this);
    }

    public abstract object GetData(out string uniqueName);
}
