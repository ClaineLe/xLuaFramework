using Framework.Core.Assistant;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewUtility {

    public static Presender CreatePresender(MonoView monoView)
    {
        View view = View.Create(monoView.name).SetupViewGo(monoView);
        return Presender.Create(view.m_Name).SetupView(view);
    }

    public static Presender CreatePresender(GameObject viewGo)
    {
        MonoView monoView = viewGo.GetComponent<MonoView>();
        return CreatePresender(monoView);
    }


}
