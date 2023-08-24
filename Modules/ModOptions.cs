using System;
using System.Collections.Generic;
using NextTheirTown.Modules;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NextTheirTown.Modules
{
    public class ModOption
    {
        public static List<ModOption> Buttons = new();
        public bool DefaultValue;
        public Func<bool> OnClick;
        public string Text;

        public ToggleButtonBehaviour? ToggleButton;

        public ModOption(string text, Func<bool> onClick, bool defaultValue)
        {
            Text = text;
            OnClick = onClick;
            DefaultValue = defaultValue;
            Buttons.Add(this);
        }
    }
}