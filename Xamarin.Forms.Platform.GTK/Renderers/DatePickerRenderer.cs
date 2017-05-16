﻿using System;
using System.ComponentModel;
using Xamarin.Forms.Platform.GTK.Extensions;

namespace Xamarin.Forms.Platform.GTK.Renderers
{
    public class DatePickerRenderer : ViewRenderer<DatePicker, Controls.DatePicker>
    {
        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (Control != null)
                    Control.DateChanged -= OnDateChanged;

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var datePicker = new Controls.DatePicker();
                    datePicker.DateChanged += OnDateChanged;
                    SetNativeControl(datePicker);
                }

                UpdateDate(e.NewElement.Date);
                UpdateMaximumDate();
                UpdateMinimumDate();
                UpdateTextColor();
                UpdateFormat();
            }

            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == DatePicker.DateProperty.PropertyName)
                UpdateDate(Element.Date);
            else if (e.PropertyName == DatePicker.MinimumDateProperty.PropertyName)
                UpdateMinimumDate();
            else if (e.PropertyName == DatePicker.MaximumDateProperty.PropertyName)
                UpdateMaximumDate();
            else if (e.PropertyName == DatePicker.TextColorProperty.PropertyName ||
                   e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
                UpdateTextColor();
            else if (e.PropertyName == DatePicker.FormatProperty.PropertyName)
                UpdateFormat();
        }

        private void UpdateDate(DateTime date)
        {
            Control.CurrentDate = date;
        }

        private void UpdateMaximumDate()
        {
            if (Control == null || Element == null)
                return;

            Control.MaxDate = Element.MaximumDate;
        }

        private void UpdateMinimumDate()
        {
            if (Control == null || Element == null)
                return;

            Control.MinDate = Element.MinimumDate;
        }

        private void UpdateTextColor()
        {
            if (Control == null || Element == null)
                return;

            var textColor = Element.TextColor;

            Control.TextColor = textColor.ToGtkColor();
        }

        private void UpdateFormat()
        {
            Control.DateFormat = Element.Format;
        }

        private void OnDateChanged(object sender, EventArgs e)
        {
            if (Control == null || Element == null)
                return;

            ElementController?.SetValueFromRenderer(DatePicker.DateProperty, Control.CurrentDate.Date);
        }
    }
}