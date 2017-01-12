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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources.Core;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using System.Collections;
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StreamingTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        PackageCatalog catalog;
        public MainPage()
        {
            this.InitializeComponent();
        }


        #region PackagingEventHandlers
        private void Catalog_PackageContentGroupStaging(PackageCatalog sender, PackageContentGroupStagingEventArgs args)
        {
            var ignored = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                String CGStagingEvent =
                args.Package.Id.FullName + ","
                + args.ContentGroupName + ","
                + args.IsComplete + ","
                + args.Progress + ","
                + args.ErrorCode + ";"
                + Environment.NewLine;

                this.CGStagingEvents_textBox.Text = CGStagingEvent;

                if (Package.Current.Id.FullName.Equals(args.Package.Id.FullName))
                {
                    switch (args.ContentGroupName)
                    {
                        case "Level_1":
                            this.Level1_Progress.Value = args.Progress;
                            if (args.IsComplete)
                            {
                                LoadImageFromMainPackage("ms-appx:///assets/Level1-Car.bmp", this.Level1_Grid);
                            }
                            else
                            {
                                this.Level1_Grid.Background = defaultImageBrush;
                            }
                            break;
                        case "Level_2":
                            this.Level2_Progress.Value = args.Progress;
                            if (args.IsComplete)
                            {
                                LoadImageFromMainPackage("ms-appx:///assets/Level2-Car.bmp", this.Level2_Grid);
                            }
                            else
                            {
                                this.Level2_Grid.Background = defaultImageBrush;
                            }
                            break;
                        case "Level_3":
                            this.Level3_Progress.Value = args.Progress;
                            if (args.IsComplete)
                            {
                                LoadImageFromMainPackage("ms-appx:///assets/Level3-Car.bmp", this.Level3_Grid);
                            }
                            else
                            {
                                this.Level3_Grid.Background = defaultImageBrush;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    String resourcePackageFullName = "";
                    foreach (var package in Package.Current.Dependencies)
                    {
                        if (package.Id.FullName.Contains("language-fr") && package.IsResourcePackage)
                        {
                            resourcePackageFullName = package.Id.FullName;
                            break;
                        }
                    }

                    if (args.Package.Id.FullName.Equals(resourcePackageFullName))
                    {
                        switch (args.ContentGroupName)
                        {
                            case "Level_1_part1":
                                this.Resource_Level1_Progress.Value = args.Progress;
                                if (args.IsComplete)
                                {
                                    LoadImageFromResourcePack("RaceCar-Fr.jpg", this.Resource_Level1);
                                }
                                break;

                            case "Level_1_part2":
                                this.Resource_Level2_Progress.Value = args.Progress;
                                if (args.IsComplete)
                                {
                                    LoadImageFromResourcePack("Level1-Car-Fr.png", this.Resource_Level2);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
            });
        }

        private void Catalog_PackageInstalling(PackageCatalog sender, PackageInstallingEventArgs args)
        {
            var ignored = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                if (Package.Current.Id.FullName.Equals(args.Package.Id.FullName))
                {
                    this.Overall_ProgressBar.Value = args.Progress;
                }

                String eventDetails =
                  args.Package.Id.FullName + ","
                  + args.IsComplete + ","
                  + args.Progress + ","
                  + args.ErrorCode + ";"
                  + Environment.NewLine;
                this.PackageStagingInstallingEvents_textBox.Text = eventDetails;
            });
        }

        private void Catalog_PackageStaging(PackageCatalog sender, PackageStagingEventArgs args)
        {
            var ignored = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                if (Package.Current.Id.FullName.Equals(args.Package.Id.FullName))
                {
                    this.Overall_ProgressBar.Value = args.Progress;
                }

                String eventDetails =
                  args.Package.Id.FullName + ","
                  + args.IsComplete + ","
                  + args.Progress + ","
                  + args.ErrorCode + ";"
                  + Environment.NewLine;
                this.PackageStagingInstallingEvents_textBox.Text = eventDetails;
            });
        }

        #endregion PackagingEventHandlers

        #region UI Event Handlers
        private void Level1_Progress_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ((FrameworkElement)sender).Visibility = (e.NewValue == 100) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Level2_Progress_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ((FrameworkElement)sender).Visibility = (e.NewValue == 100) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Level3_Progress_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ((FrameworkElement)sender).Visibility = (e.NewValue == 100) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Overall_Progress_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ((FrameworkElement)sender).Visibility = (e.NewValue == 100) ? Visibility.Collapsed : Visibility.Visible;
        }

        private async void Resource_Level1_Progress_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ((FrameworkElement)sender).Visibility = (e.NewValue == 100) ? Visibility.Collapsed : Visibility.Visible;
        }

        private async void Resource_Level2_Progress_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ((FrameworkElement)sender).Visibility = (e.NewValue == 100) ? Visibility.Collapsed : Visibility.Visible;
        }

        #endregion UI Event Handlers

        private void LoadImageFromMainPackage(string resourceUri, UIElement uiElement)
        {
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(resourceUri));
            ((Grid)uiElement).Background = brush;
        }



        private async System.Threading.Tasks.Task LoadImageFromResourcePack(string resourceKey, UIElement uiElement)
        {
            var context = ResourceContext.GetForCurrentView().Clone();
            context.QualifierValues["language"] = "fr-fr";


            var resourcesMap = ResourceManager.Current.MainResourceMap.GetSubtree("Files").GetSubtree("Assets");

            NamedResource namedResource;
            if (resourcesMap.TryGetValue(resourceKey, out namedResource))
            {
                var resourceCandidates = namedResource.ResolveAll(context);
                StorageFile file = await resourceCandidates.FirstOrDefault().GetValueAsFileAsync();
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri(file.Path));
                ((Grid)uiElement).Background = brush;
            }
        }

        void HookupCatalog()
        {
            catalog = PackageCatalog.OpenForCurrentPackage();
            //catalog = PackageCatalog.OpenForCurrentUser();

            catalog.PackageContentGroupStaging += Catalog_PackageContentGroupStaging;
            this.PackageEvents_checkBox.IsChecked = true;
            GetContentIntegrityStatus();
        }



        async Task LoadImages()
        {
            var contentGroups = await Package.Current.GetContentGroupsAsync();
            foreach (var contentGroup in contentGroups)
            {
                if(contentGroup.Name == "Level_1" && contentGroup.State == PackageContentGroupState.Staged)
                {
                    LoadImageFromMainPackage("ms-appx:///assets/Level1-Car.bmp", this.Level1_Grid);
                    this.Level1_Progress.Visibility = Visibility.Collapsed;
                }
                else if (contentGroup.Name == "Level_2" && contentGroup.State == PackageContentGroupState.Staged)
                {
                    LoadImageFromMainPackage("ms-appx:///assets/Level2-Car.bmp", this.Level2_Grid);
                    this.Level2_Progress.Visibility = Visibility.Collapsed;
                }
                else if (contentGroup.Name == "Level_3" && contentGroup.State == PackageContentGroupState.Staged)
                {
                    LoadImageFromMainPackage("ms-appx:///assets/Level3-Car.bmp", this.Level3_Grid);
                    this.Level3_Progress.Visibility = Visibility.Collapsed;
                }
            }


            foreach (var package in Package.Current.Dependencies)
            {
                if (package.Id.FullName.Contains("language-fr") && package.IsResourcePackage)
                {
                    var resourceContentGroups = await package.GetContentGroupsAsync();
                    foreach (var contentGroup in resourceContentGroups)
                    {
                        if (contentGroup.Name == "Level_1_part1" && contentGroup.State == PackageContentGroupState.Staged)
                        {
                            LoadImageFromResourcePack("RaceCar-Fr.jpg", this.Resource_Level1);
                            this.Resource_Level1_Progress.Visibility = Visibility.Collapsed;
                        }
                        else if (contentGroup.Name == "Level_1_part2" && contentGroup.State == PackageContentGroupState.Staged)
                        {
                            LoadImageFromResourcePack("Level1-Car-Fr.png", this.Resource_Level2);
                            this.Resource_Level2_Progress.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        async void DisplayNamesOfContentGroups()
        {
            var groups = await Package.Current.GetContentGroupsAsync();
            string groupNames = "";
            foreach(var group in groups)
            {
                if (groupNames != "")
                {
                    groupNames += ",";
                }
                groupNames += group.Name;
            }
            this.StageCG_textBox.Text = groupNames;
        }

        Brush defaultImageBrush = null;
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title_textBox.Text += " " + Package.Current.Id.Version.Major + "." + Package.Current.Id.Version.Minor + "." + Package.Current.Id.Version.Build + "." + Package.Current.Id.Version.Revision;

            // display content group status.
            this.GetCG_button_Click(null, null);
            DisplayNamesOfContentGroups();

            defaultImageBrush = this.Level1_Grid.Background;
            await LoadImages();
            this.HookupCatalog();
        }

        void DisplayContentGroupsStatus(IList<PackageContentGroup> groups, TextBox textbox)
        {
            string cgResults = "";
            foreach (PackageContentGroup oneGroup in groups)
            {
                if (oneGroup != null)
                {
                    cgResults += oneGroup.Name + "," + oneGroup.State.ToString() + ";" + Environment.NewLine;
                }
            }

            var ignored = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                textbox.Text = cgResults;
            });
        }
        private void GetCG_button_Click(object sender, RoutedEventArgs e)
        {
            var operation = Package.Current.GetContentGroupsAsync();
            operation.Completed += (a, b)=> {
                if (a.Status == AsyncStatus.Completed)
                {
                    DisplayContentGroupsStatus(a.GetResults(), this.CGStatus_textBox);
                }
                if (a.Status == AsyncStatus.Error)
                {
                    this.CGStatus_textBox.Text = a.ErrorCode.ToString();
                }
            };
        }

        private void StageCG_button_Click(object sender, RoutedEventArgs e)
        {
            var contentGroupList = this.StageCG_textBox.Text.Split(',').ToList<string>();
            var operation = Package.Current.StageContentGroupsAsync(contentGroupList);
            operation.Completed += (a, b) => {
                if (a.Status == AsyncStatus.Completed)
                {
                    DisplayContentGroupsStatus(a.GetResults(), this.CGStatus_textBox);
                }
                if (a.Status == AsyncStatus.Error)
                {
                    this.CGStatus_textBox.Text = a.ErrorCode.ToString();
                }
            };
        }

        private void PackageEvents_checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            catalog.PackageStaging -= Catalog_PackageStaging;
            catalog.PackageInstalling -= Catalog_PackageInstalling;
        }

        private void PackageEvents_checkBox_Checked(object sender, RoutedEventArgs e)
        {
            catalog.PackageStaging += Catalog_PackageStaging;
            catalog.PackageInstalling += Catalog_PackageInstalling;
        }

        private async void StageResourceCG1_button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var package in Package.Current.Dependencies)
            {
                if (package.Id.FullName.Contains("language-fr") && package.IsResourcePackage)
                {
                    List<string> names = new List<string>();
                    names.Add("Level_1_part1");
                    package.StageContentGroupsAsync(names);
                }
            }
        }

        private void StageResourceCG2_button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var package in Package.Current.Dependencies)
            {
                if (package.Id.FullName.Contains("language-fr") && package.IsResourcePackage)
                {
                    List<string> names = new List<string>();
                    names.Add("Level_1_part2");
                    package.StageContentGroupsAsync(names);
                }
            }
        }

        private async void GetContentIntegrityStatus()
        {
            var isValid = await Package.Current.VerifyContentIntegrityAsync();
            string contentIntegrityStatus;
            string packageStatusString = "";
            PackageStatus packageStatus = Package.Current.Status;
            if (packageStatus.DataOffline)
            {
               packageStatusString += "DataOffline,";
            }
            if (packageStatus.DependencyIssue)
            {
                packageStatusString += "DependencyIssue,";
            }
            if (packageStatus.DeploymentInProgress)
            {
                packageStatusString += "DeploymentInProgress,";
            }
            if (packageStatus.Disabled)
            {
                packageStatusString += "Disabled,";
            }
            if (packageStatus.LicenseIssue)
            {
                packageStatusString += "LicenseIssue,";
            }
            if (packageStatus.Modified)
            {
                packageStatusString += "Modified,";
            }
            if (packageStatus.NeedsRemediation)
            {
                packageStatusString += "NeedsRemediation,";
            }
            if (packageStatus.NotAvailable)
            {
                packageStatusString += "NotAvailable,";
            }
            if (packageStatus.PackageOffline)
            {
                packageStatusString += "PackageOffline,";
            }
            if (packageStatus.Servicing)
            {
                packageStatusString += "Servicing,";
            }
            if (packageStatus.Tampered)
            {
                packageStatusString += "Tampered,";
            }
            if (packageStatus.VerifyIsOK())
            {
                packageStatusString += "OK";
            }

            if (isValid)
            {
                contentIntegrityStatus = "Valid, PackageStatus:" + packageStatusString;
            }
            else
            {
                contentIntegrityStatus = "Not Valid, PackageStatus:" + packageStatusString;
            }
            this.ContentIntegrityStatus_textBlock.Text = contentIntegrityStatus;
        }

        private void VerifyContentIntegrity_button_Click(object sender, RoutedEventArgs e)
        {
            GetContentIntegrityStatus();
        }
    }
}
