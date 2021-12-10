﻿using System;
using System.IO;
using System.IO.Compression;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Roche
{
    public sealed partial class DownloadPage : Page
    {
        private string _destination = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            @".roche");

        public DownloadPage()
            => InitializeComponent();

        private async void HandleServerFileDrop(object sender, DragEventArgs e)
        {
            if (!e.DataView.Contains(StandardDataFormats.StorageItems))
                return;

            var items = await e.DataView.GetStorageItemsAsync();
            if (items.Count != 1)
                return;

            var file = (StorageFile)items[0];
            _serverFileTextBox.Text = file.Path;
        }

        private void HandleServerFileDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Link;
        }

        private async void HandleBrowseClick(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
                FileTypeFilter = { ".zip" }
            };
            InitializeWithWindow.Initialize(picker, ShellWindow.Handle);

            var file = await picker.PickSingleFileAsync();
            if (file == null)
                return;

            _serverFileTextBox.Text = file.Path;
        }

        private void HandleNextClick(object sender, RoutedEventArgs e)
        {
            // TODO: Progress
            ZipFile.ExtractToDirectory(_serverFileTextBox.Text, _destination);

            Frame.Navigate(typeof(NewWorldPage));
        }
    }
}
