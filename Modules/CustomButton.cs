using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NextTheirTown.Modules
{
    public class CustomButton
    {
        public static List<CustomButton> Buttons { get; set; } = new();

        public ActionButton ActionButton { get; set; }
        public float Cooldown { get; set; }
        public Func<bool> CouldUse { get; set; }
        public float EffectTime { get; set; }
        public GameObject GameObject { get; set; }
        public Func<bool> HasButton { get; set; }
        public bool HasEffect { get; set; }
        public KeyCode? Hotkey { get; set; }
        public string HotkeyName { get; set; }

        public HudManager Hud { get; set; }
        public bool IsEffectActive { get; set; }
        public Material Material { get; set; }
        public Action OnClick { get; set; }
        public Action? OnEffect { get; set; }
        public Action OnMeetingEnd { get; set; }
        public PassiveButton PassiveButton { get; set; }
        public Vector3 Position { get; set; }
        public Sprite Sprite { get; set; }
        public SpriteRenderer SpriteRenderer { get; set; }

        public string Text { get; set; }
        public TextMeshPro TextMesh { get; set; }
        public float Timer { get; set; }
        public int UsesLimit { get; set; }
        public int UsesRemaining { get; set; }

        public CustomButton(Action onClick, Action onMeetingEnd, Action onEffect, Func<bool> couldUse, Func<bool> hasButton,
            Sprite sprite, Vector3 position, KeyCode? hotkey, string text, bool hasEffect, float cooldown, float effectTime,
            int usesLimit, string hotkeyName)
        {
            OnClick = onClick;
            OnMeetingEnd = onMeetingEnd;
            OnEffect = onEffect;
            CouldUse = couldUse;
            HasButton = hasButton;
            Sprite = sprite;
            Position = position;
            Hotkey = hotkey;
            Text = text;
            HasEffect = hasEffect;
            Cooldown = cooldown;
            EffectTime = effectTime;
            UsesLimit = UsesRemaining = usesLimit;
            HotkeyName = hotkeyName;
        }

        public CustomButton()
        {
        }

        public static CustomButton Create(Action onClick, Action onMeetingEnd, Action onEffect, Func<bool> couldUse,
            Func<bool> hasButton, Sprite sprite, Vector3 position, KeyCode? hotkey, string text, float cooldown,
            float effectTime, int usesLimit, string hotkeyName = "")
        {
            return new CustomButton(onClick, onMeetingEnd, onEffect, couldUse, hasButton, sprite, position, hotkey, text,
                true, cooldown, effectTime, usesLimit, hotkeyName);
        }

        public static CustomButton Create(Action onClick, Action onMeetingEnd, Func<bool> couldUse, Func<bool> hasButton,
            Sprite sprite, Vector3 position, KeyCode? hotkey, string text, float cooldown, int usesLimit,
            string hotkeyName = "")
        {
            return new CustomButton(onClick, onMeetingEnd, () => { }, couldUse, hasButton, sprite, position, hotkey, text,
                false, cooldown, -1f, usesLimit, hotkeyName);
        }

        public void SetActive(bool active)
        {
            if (active)
                ActionButton.Show();
            else
                ActionButton.Hide();
        }

        public void ResetCooldown() => Timer = Cooldown;

        public void ResetEffectTime() => Timer = EffectTime;

        public void SetCooldown(float cd)
        {
            Cooldown = cd;
            ResetCooldown();
        }

        public void SetEffectTime(float et)
        {
            EffectTime = et;
            ResetEffectTime();
        }

        public void Update()
        {
            var isCoolingDown = Timer > 0f;
            var hotkeyText = "";
            if (HotkeyName == "")
                if (Hotkey.HasValue)
                    hotkeyText = Hotkey.Value.ToString();
                else
                    hotkeyText = HotkeyName;

            var buttonText = $"{Text}<size=75%> ({hotkeyText})</size>";

            if (!PlayerControl.LocalPlayer || MeetingHud.Instance || ExileController.Instance || !HasButton())
            {
                SetActive(false);
                return;
            }

            SetActive(HasButton());
            var lp = PlayerControl.LocalPlayer;
            if (isCoolingDown && !lp.inVent && lp.moveable) Timer -= Time.deltaTime;
            ActionButton.SetCoolDown(Timer, Cooldown);
            ActionButton.OverrideText(buttonText);
            if (UsesLimit > 0)
                ActionButton.SetUsesRemaining(UsesRemaining);
            else
                ActionButton.SetInfiniteUses();

            var desat = Shader.PropertyToID("_Desat");
            if (CouldUse() && !isCoolingDown)
            {
                SpriteRenderer.color = TextMesh.color = Palette.EnabledColor;
                Material.SetFloat(desat, 0f);
            }
            else
            {
                SpriteRenderer.color = TextMesh.color = Palette.DisabledClear;
                Material.SetFloat(desat, 1f);
            }

            if (Hud.UseButton != null)
                GameObject.transform.localPosition = Hud.UseButton.transform.localPosition + Position;

            if (Hotkey.HasValue && Input.GetKeyDown(Hotkey.Value)) CheckClick();
        }

        public void OnMeetingEndSpawn() => OnMeetingEnd();


        public void CheckClick()
        {
            if (this.Timer <= 0f)
            {
                if (this.HasEffect && this.IsEffectActive)
                {
                    this.IsEffectActive = false;
                    this.ActionButton.cooldownTimerText.color = Palette.EnabledColor;
                    this.OnEffect?.Invoke();
                    this.ResetCooldown();
                }
                else
                {
                    if (this.UsesRemaining <= 0 && this.UsesLimit > 0) return;
                    this.OnClick();
                    if (this.HasEffect && !this.IsEffectActive)
                    {
                        this.IsEffectActive = true;
                        this.ResetEffectTime();
                    }

                    if (this.UsesLimit > 0) this.UsesRemaining--;
                }
            }
        }

        // Static methods
        public static void ResetAllCooldown()
        {
            Buttons.ForEach(b => b.ResetCooldown());
        }

        public static void Init(HudManager hud)
        {
            foreach (var button in Buttons)
            {
                button.ActionButton = Object.Instantiate(hud.AbilityButton, hud.AbilityButton.transform.parent);

                button.Hud = hud;
                button.SpriteRenderer = button.ActionButton.graphic;
                button.SpriteRenderer.sprite = button.Sprite;

                if (button.UsesLimit > 0)
                    button.ActionButton.SetUsesRemaining(button.UsesLimit);
                else
                    button.ActionButton.SetInfiniteUses();

                button.ResetCooldown();

                button.Material = button.SpriteRenderer.material;
                button.GameObject = button.ActionButton.gameObject;
                button.PassiveButton = button.ActionButton.GetComponent<PassiveButton>();
                button.TextMesh = button.ActionButton.buttonLabelText;
                button.TextMesh.text = button.Text;
                var tm = button.TextMesh;
                tm.fontSizeMax = tm.fontSizeMin = tm.fontSize;
                button.PassiveButton.OnClick = new();

                button.PassiveButton.OnClick.AddListener((UnityAction)button.CheckClick);
                button.SetActive(false);
            }
        }

        public static void UpdateAll() => Buttons.ForEach(b => b.Update());

        // Position from The Other Roles
        // Link: https://github.com/TheOtherRolesAU/TheOtherRoles/blob/main/TheOtherRoles/Objects/CustomButton.cs#L40
        public static class ButtonPositions
        {
            public static readonly Vector3
                LowerRowRight = new(-2f, -0.06f, 0);

            public static readonly Vector3 LowerRowCenter = new(-3f, -0.06f, 0);
            public static readonly Vector3 LowerRowLeft = new(-4f, -0.06f, 0);

            public static readonly Vector3
                UpperRowRight = new(0f, 1f, 0f);

            public static readonly Vector3
                UpperRowCenter = new(-1f, 1f, 0f);

            public static readonly Vector3 UpperRowLeft = new(-2f, 1f, 0f);
            public static readonly Vector3 UpperRowFarLeft = new(-3f, 1f, 0f);
        }
    }
}


