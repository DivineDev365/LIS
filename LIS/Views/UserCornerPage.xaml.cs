using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Collections.ObjectModel;
using Windows.Data.Pdf;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LIS.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class UserCornerPage : Page
	{
		public UserCornerPage()
		{
			this.InitializeComponent();
			GetFilesFromDir();
		}

		private ObservableCollection<string> FilesCollection
		{
			get;
			set;
		} = new ObservableCollection<string>();

		public ObservableCollection<BitmapImage> PdfPagesCollection
		{
			get;
			set;
		} = new ObservableCollection<BitmapImage>();

		private async void GetFilesFromDir()
		{
			//FilesCollection.;
			StorageFolder localFolder = ApplicationData.Current.LocalFolder;

			//retrieve folder
			string level1FolderName = "UserDataStore";
			StorageFolder UserDataStore = await localFolder.GetFolderAsync(level1FolderName);

			//retrieve files from selected folder
			IReadOnlyList<StorageFile> items = await UserDataStore.GetFilesAsync();

			foreach (StorageFile file in items)
			{
				FilesCollection.Add(file.Name);
			}
			//FilesList.ItemsSource = FilesCollection;
			FilesList.ItemsSource = FilesCollection;
		}

		private void AddMagClicked(object sender, RoutedEventArgs e)
		{

		}

		private  void AddQuesClicked(object sender, RoutedEventArgs e)
		{
			//var picker = new Windows.Storage.Pickers.FileOpenPicker();
			//picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
			//picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
			//picker.FileTypeFilter.Add(".pdf");

			//Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();

			//StorageFolder localFolder = ApplicationData.Current.LocalFolder;
			//string level1FolderName = "UserDataStore";
			//StorageFolder UserDataStore = await localFolder.GetFolderAsync(level1FolderName);

			//if (file != null)
			//{
			//	var savePicker = new Windows.Storage.Pickers.FileSavePicker();
			//	savePicker.SuggestedStartLocation =
			//		Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
			//	// Dropdown of file types the user can save the file as
			//	savePicker.FileTypeChoices.Add("PDF", new List<string>() { ".pdf" });
			//	// Default file name if the user does not type one in or select a file to replace
			//	savePicker.SuggestedFileName = "New Document";

			//	//save to UserDataStore
			//	// Prevent updates to the remote version of the file until
			//	// we finish making changes and call CompleteUpdatesAsync.
			//	Windows.Storage.CachedFileManager.DeferUpdates(file);
			//	// write to file
			//	await Windows.Storage.FileIO.WriteTextAsync(file, file.Name);
			//	// Let Windows know that we're finished changing the file so
			//	// the other app can update the remote version of the file.
			//	// Completing updates may require Windows to ask for user input.
			//	Windows.Storage.Provider.FileUpdateStatus status =
			//		await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
			//	if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
			//	{
			//		ShowDialog( "File " + file.Name + " was saved.");
			//	}
			//	else
			//	{
			//		ShowDialog( "File " + file.Name + " couldn't be saved.");
			//	}
			//}
			//else
			//{
			//	ShowDialog("Operation cancelled.");
			//}
		}

		private async void FileClicked(object sender, ItemClickEventArgs e)
		{
			try
			{
				FilesList.IsEnabled = false;
				StorageFolder localFolder = ApplicationData.Current.LocalFolder;
				string level1FolderName = "UserDataStore";
				StorageFolder UserDataStore = await localFolder.GetFolderAsync(level1FolderName);

				//string filename = ((StorageFile)e.ClickedItem).Name;
				StorageFile pdf = await UserDataStore.GetFileAsync((e.ClickedItem).ToString());
				PdfDocument pdfDoc = await PdfDocument.LoadFromFileAsync(pdf);
				PdfPagesCollection.Clear();

				for (uint i = 0; i < pdfDoc.PageCount; i++)
				{
					BitmapImage image = new BitmapImage();

					var page = pdfDoc.GetPage(i);

					using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
					{
						await page.RenderToStreamAsync(stream);
						await image.SetSourceAsync(stream);
					}

					PdfPagesCollection.Add(image);
				}


				PdfItemsControl.ItemsSource = PdfPagesCollection;
				FilesList.IsEnabled = true;
			}
			catch (Exception ex)
			{
				ShowDialog(ex.Message);
			}
		}


		private async void ShowDialog(string s)
		{
			ContentDialog message = new ContentDialog()
			{
				Title = "Message",
				Content = s,
				PrimaryButtonText = "Got It!"
			};
			await message.ShowAsync();
		}
	}
}
