using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace D2DLightingEffects
{
    public sealed partial class EffectSelector : UserControl
    {

        private EffectRenderer _EffectRenderer;


        public EffectSelector()
        {
            this.InitializeComponent();
            ResetValues();
        }
        public void Init(EffectRenderer effectRenderer)
        {
            _EffectRenderer = effectRenderer;
            ResetValues();
        }

        void ResetValues()
        {
            cboEffectSelector.SelectedIndex = 0;
            LightPositionZ.Value = 100;
            SpecularConstant.Value = 1;
            SpecularExponent.Value = 2;
            DiffuseConstant.Value = 1;
            SpotFocus.Value = 1;
            LimitingConeAngle.Value = 90;
            Azimuth.Value = 0;
            Elevation.Value = 0;
            SurfaceScale.Value = 3;

            VisualStateManager.GoToState(this.LayoutControl, "SpecularState", true);
            VisualStateManager.GoToState(this.LayoutControl, "PointState", true);
        }


        private void OnLightPositionZValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_EffectRenderer == null) return;
            _EffectRenderer.SetLightingProperty(
                D2DLightingEffects.EffectRenderer.LightingProperty.LightPositionZ,
                (float)e.NewValue
            );
        }

        private void OnSpecularConstantValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_EffectRenderer == null) return;
            _EffectRenderer.SetLightingProperty(
                D2DLightingEffects.EffectRenderer.LightingProperty.SpecularConstant,
                (float)e.NewValue
            );
        }

        private void OnDiffuseConstantValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_EffectRenderer == null) return;
            _EffectRenderer.SetLightingProperty(
                D2DLightingEffects.EffectRenderer.LightingProperty.DiffuseConstant,
                (float)e.NewValue
            );
        }

        private void OnSpecularExponentValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_EffectRenderer == null) return;
            _EffectRenderer.SetLightingProperty(
                D2DLightingEffects.EffectRenderer.LightingProperty.SpecularExponent,
                (float)e.NewValue
            );
        }

        private void OnSpotFocusValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_EffectRenderer == null) return;
            _EffectRenderer.SetLightingProperty(
                D2DLightingEffects.EffectRenderer.LightingProperty.Focus,
                (float)e.NewValue
            );
        }

        private void OnAzimuthValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_EffectRenderer == null) return;
            _EffectRenderer.SetLightingProperty(
                D2DLightingEffects.EffectRenderer.LightingProperty.Azimuth,
                (float)e.NewValue
            );
        }

        private void OnLimitingConeAngleValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_EffectRenderer == null) return;
            _EffectRenderer.SetLightingProperty(
                D2DLightingEffects.EffectRenderer.LightingProperty.LimitingConeAngle,
                (float)e.NewValue
            );
        }

        private void OnElevationValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_EffectRenderer == null) return;
            _EffectRenderer.SetLightingProperty(
                D2DLightingEffects.EffectRenderer.LightingProperty.Elevation,
                (float)e.NewValue
            );
        }

        private void OnSurfaceScaleValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_EffectRenderer == null) return;
            _EffectRenderer.SetLightingProperty(
                D2DLightingEffects.EffectRenderer.LightingProperty.SurfaceScale,
                (float)e.NewValue
            );
        }

        private void OnRestoreDefaultsClick(object sender, RoutedEventArgs e)
        {
            ResetValues();
        }

        private void OnEffectSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_EffectRenderer == null) return;

            switch (cboEffectSelector.SelectedIndex)
                {
                case (int)D2DLightingEffects.EffectRenderer.LightingEffect.PointSpecular:
                    VisualStateManager.GoToState(this.LayoutControl, "SpecularState", true);
                    VisualStateManager.GoToState(this.LayoutControl, "PointState", true);
                    
                    break;
                case (int)D2DLightingEffects.EffectRenderer.LightingEffect.SpotSpecular:
                    VisualStateManager.GoToState(this.LayoutControl, "SpecularState", true);
                    VisualStateManager.GoToState(this.LayoutControl, "SpotState", true);
                    
                    break;
                case (int)D2DLightingEffects.EffectRenderer.LightingEffect.DistantSpecular:
                    VisualStateManager.GoToState(this.LayoutControl, "SpecularState", true);
                    VisualStateManager.GoToState(this.LayoutControl, "DistantState", true);
                    
                    break;
                case (int)D2DLightingEffects.EffectRenderer.LightingEffect.PointDiffuse:
                    VisualStateManager.GoToState(this.LayoutControl, "DiffuseState", true);
                    VisualStateManager.GoToState(this.LayoutControl, "PointState", true);
                    
                    break;
                case (int)D2DLightingEffects.EffectRenderer.LightingEffect.SpotDiffuse:
                    VisualStateManager.GoToState(this.LayoutControl, "DiffuseState", true);
                    VisualStateManager.GoToState(this.LayoutControl, "SpotState", true);
                    
                    break;
                case (int)D2DLightingEffects.EffectRenderer.LightingEffect.DistantDiffuse:
                    VisualStateManager.GoToState(this.LayoutControl, "DiffuseState", true);
                    VisualStateManager.GoToState(this.LayoutControl, "DistantState", true);
                    
                    break;
                default:
                    
                    break;
                }

                //EffectControls->UpdateLayout();



            _EffectRenderer.SetLightingEffect(cboEffectSelector.SelectedIndex);
                //m_app->SetLightingEffect(safe_cast<LightingEffect>(EffectSelector->SelectedIndex));
        }


        private void butHome_Click(object sender, RoutedEventArgs e)
        {
            // Create a Frame to act navigation context and navigate to the first page
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(ChooseDemo));

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }


    }
}
