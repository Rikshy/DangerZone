﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace DangerZone.Controls
{
    public class GifImage : Image
    {
        private bool isInitialized;
        private GifBitmapDecoder gifDecoder;
        private Int32Animation animation;

        public int FrameIndex
        {
            get => (int)GetValue(FrameIndexProperty);
            set => SetValue(FrameIndexProperty, value);
        }

        private void Initialize()
        {
            gifDecoder = new GifBitmapDecoder(
                new Uri(GifSource, UriKind.RelativeOrAbsolute),
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.Default);
            animation = new Int32Animation(0,
                gifDecoder.Frames.Count - 1,
                new Duration(new TimeSpan(0, 0, 0, gifDecoder.Frames.Count / 25, (int)((gifDecoder.Frames.Count / 25.0 - gifDecoder.Frames.Count / 25) * 1000))));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            Source = gifDecoder.Frames[0];

            isInitialized = true;
        }

        static GifImage()
            => VisibilityProperty.OverrideMetadata(typeof(GifImage), new FrameworkPropertyMetadata(VisibilityPropertyChanged));

        private static void VisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((Visibility)e.NewValue == Visibility.Visible)
                ((GifImage)sender).StartAnimation();
            else
                ((GifImage)sender).StopAnimation();
        }

        public static readonly DependencyProperty FrameIndexProperty =
            DependencyProperty.Register( "FrameIndex", typeof( int ), typeof( GifImage ), new UIPropertyMetadata( 0, new PropertyChangedCallback( ChangingFrameIndex ) ) );

        public static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
        {
            var gifImage = obj as GifImage;
            gifImage.Source = gifImage.gifDecoder.Frames[(int)ev.NewValue];
        }

        /// <summary>
        /// Defines whether the animation starts on it's own
        /// </summary>
        public bool AutoStart
        {
            get => (bool)GetValue(AutoStartProperty);
            set => SetValue(AutoStartProperty, value);
        }

        public static readonly DependencyProperty AutoStartProperty =
            DependencyProperty.Register( "AutoStart", typeof( bool ), typeof( GifImage ), new UIPropertyMetadata( false, AutoStartPropertyChanged ) );

        private static void AutoStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                (sender as GifImage).StartAnimation();
        }

        public string GifSource
        {
            get => (string)GetValue(GifSourceProperty);
            set => SetValue(GifSourceProperty, value);
        }

        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.Register( "GifSource", typeof( string ), typeof( GifImage ), new UIPropertyMetadata( string.Empty, GifSourcePropertyChanged ) );

        private static void GifSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as GifImage).Initialize();
        }

        /// <summary>
        /// Starts the animation
        /// </summary>
        public void StartAnimation()
        {
            if (!isInitialized)
                Initialize();

            BeginAnimation(FrameIndexProperty, animation);
        }

        /// <summary>
        /// Stops the animation
        /// </summary>
        public void StopAnimation() 
            => BeginAnimation(FrameIndexProperty, null);
    }
}
