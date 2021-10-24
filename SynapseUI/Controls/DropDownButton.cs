using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using SynapseUI.Functions.Utils;

namespace SynapseUI.Controls
{
    [TemplatePart(Name = "borderButton", Type = typeof(Button))]
    [TemplatePart(Name = "contentPresenter", Type = typeof(ContentPresenter))]
    public class DropDownButton : Button
    {
        private const string dropChr = "▼";
        private const string upChr = "▲";

        public bool IsDropped { get; private set; } = false;
        public double TargetHeight { get; set; } = -1;

        private double baseHeight;
        private Window window;
        public Window Window
        {
            get => window;
            set
            {
                baseHeight = value.Height;
                window = value;
            }
        }

        public DropDownButton()
        {
            Loaded += (s, e) => { ContentPresenterElement.Content = dropChr; };
        }

        public Border ButtonElement { get; private set; }
        public ContentPresenter ContentPresenterElement { get; private set; }

        public override void OnApplyTemplate()
        {
            ButtonElement = (Border)GetTemplateChild("borderButton");
            ContentPresenterElement = (ContentPresenter)GetTemplateChild("contentPresenter");
            base.OnApplyTemplate();
        }

        private readonly TimeSpan Duration = TimeSpan.FromMilliseconds(500);

        public Storyboard AnimationStoryboard { get; private set; } = new Storyboard();

        protected override void OnClick()
        {
            if (ButtonElement is null || ContentPresenterElement is null || Window is null || TargetHeight == -1)
                return;

            IsDropped = !IsDropped;
            var anim = new DoubleAnimation
            {
                From = Window.Height,
                To = IsDropped ? TargetHeight : baseHeight,
                Duration = Duration,
                EasingFunction = Animation.QuarticEase
            };

            Storyboard.SetTarget(anim, Window);
            Storyboard.SetTargetProperty(anim, new PropertyPath(HeightProperty));

            AnimationStoryboard.Children.Clear();
            AnimationStoryboard.Children.Add(anim);

            AnimationStoryboard.Begin();

            ContentPresenterElement.Content = IsDropped ? upChr : dropChr;
            base.OnClick();
        }
    }
}
