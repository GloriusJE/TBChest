﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TBChestTracker.Managers;
using com.KonquestUI.Controls;
using System.Windows.Media.Animation;
using System.Globalization;
namespace TBChestTracker
{
    /// <summary>
    /// Interaction logic for ClanStatisticsWindow.xaml
    /// </summary>
    /// 

  
    public partial class ClanStatisticsWindow : Window, INotifyPropertyChanged
    {
        public ClanChestManager ChestManager { get; set; }
        bool isready { get; set; }
        public ObservableCollection<ClanStatisticData> ClanStatisticData { get; set; }

        private DateTime m_StartDate = new DateTime();
        private DateTime m_EndDate = new DateTime();
        public DateTime StartDate
        {
            get => (DateTime)m_StartDate;
            set
            {
                m_StartDate = value;
                OnPropertyChanged(nameof(StartDate));   
            }
        }
        public DateTime EndDate
        {
            get => (DateTime)m_EndDate;
            set
            {
                m_EndDate = (DateTime)value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        

        private bool m_bShowCommonCryptsTotal = true;
        private bool m_bShowRareCryptsTotal = true;
        private bool m_bShowEpicCryptsTotal = true;
        private bool m_bShowCitadelsTotal = true;
        private bool m_bShowArenasTotal = false;
        private bool m_bShowUnionTriumphTotal = false;
        private bool m_bShowVaultAncientsTotal = false;
        private bool m_bShowHeroicsTotal = false;
        private bool m_bShowAncientChestsTotal = false;
        private bool m_bShowJormungadrShopChestsTotal = false;

        private bool m_bShowBanksTotal = false;
        private bool m_bShowStoryChestsTotal = false;
        private bool m_bShowAll = false;
        private bool m_bShowTotal = true;
        private bool m_bShowTotalPoints = true;

        #region PropertyChanged Event
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion

        #region Visibility Variables
        public bool bShowCommonCryptsTotal
        {
            get => m_bShowCommonCryptsTotal;
            set
            {
                m_bShowCommonCryptsTotal = value;
                OnPropertyChanged(nameof(bShowCommonCryptsTotal));
            }
        }
        public bool bShowRareCryptsTotal
        {
            get => m_bShowRareCryptsTotal;
            set
            {
                m_bShowRareCryptsTotal = value;
                OnPropertyChanged(nameof(bShowRareCryptsTotal));
            }
        }
        public bool bShowEpicCryptsTotal
        {
            get => m_bShowEpicCryptsTotal;
            set
            {
                m_bShowEpicCryptsTotal = value;
                OnPropertyChanged(nameof(bShowEpicCryptsTotal));
            }
        }
        public bool bShowCitadelsTotal
        {
            get => m_bShowCitadelsTotal;
            set
            {
                m_bShowCitadelsTotal = value;
                OnPropertyChanged(nameof(bShowCitadelsTotal));
            }
        }
        public bool bShowArenasTotal
        {
            get => m_bShowArenasTotal;
            set
            {
                m_bShowArenasTotal = value;
                OnPropertyChanged(nameof(bShowArenasTotal));
            }
        }
        public bool bShowUnionTriumphsTotal
        {
            get => m_bShowUnionTriumphTotal;
            set
            {
                m_bShowUnionTriumphTotal = value;
                OnPropertyChanged(nameof(bShowUnionTriumphsTotal));
            }
        }
        public bool bShowVaultAncientsTotal
        {
            get => m_bShowVaultAncientsTotal;
            set
            {
                m_bShowCitadelsTotal = value;
                OnPropertyChanged(nameof(bShowVaultAncientsTotal));
            }
        }
        public bool bShowHeroicsTotal
        {
            get => m_bShowHeroicsTotal;
            set
            {
                m_bShowHeroicsTotal = value;
                OnPropertyChanged(nameof(bShowHeroicsTotal));
            }
        }
        public bool bShowAncientChestsTotal
        {
            get => m_bShowAncientChestsTotal;
            set
            {
                m_bShowAncientChestsTotal = value;
                OnPropertyChanged(nameof(bShowAncientChestsTotal));
            }
        }
        public bool bShowJormungandrShopChestsTotal
        {
            get
            {
                return m_bShowJormungadrShopChestsTotal;
            }
            set
            {
                m_bShowJormungadrShopChestsTotal = value;
                OnPropertyChanged(nameof(bShowJormungandrShopChestsTotal));
            }
        }
        public bool bShowStoryChestsTotal
        {
            get => m_bShowStoryChestsTotal;
            set
            {
                m_bShowStoryChestsTotal = value;
                OnPropertyChanged(nameof(bShowStoryChestsTotal));
            }
        }
        public bool bShowBanksTotal
        {
            get => m_bShowBanksTotal;
            set
            {
                m_bShowBanksTotal = value;
                OnPropertyChanged(nameof(bShowBanksTotal));
            }
        }
        public bool bShowTotal
        {
            get => m_bShowTotal;
            set
            {
                m_bShowTotal = value;
                OnPropertyChanged(nameof(bShowTotal));  
            }
        }
        public bool bShowTotalPoints
        {
            get => m_bShowTotalPoints;
            set
            {
                m_bShowTotalPoints = value;
                OnPropertyChanged(nameof(bShowTotalPoints));    
            }
        }
        public bool bShowAll
        {
            get => m_bShowAll;
            set
            {
                m_bShowAll = value;
                OnPropertyChanged(nameof(bShowAll));
            }
        }
        #endregion

        public ClanStatisticsWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ClanStatisticData = new ObservableCollection<ClanStatisticData>();
            isready = false;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ClanStatsListView.ItemsSource = ClanStatisticData;

            CollectionViewSource.GetDefaultView(ClanStatsListView.ItemsSource).Filter = Filter_Clanmate_Results;
            
            try
            {
                DateTime currentDate = DateTime.Now;
                var currentCulture = new System.Globalization.CultureInfo(System.Globalization.CultureInfo.CurrentCulture.Name);
                var firstDateEntry = DateTime.Parse(ChestManager.ClanChestDailyData.First().Key, currentCulture);

                var weekEndDate = firstDateEntry.AddDays(7);
                var firstDate = firstDateEntry.AddDays(-1);

                var lastDateEntry = DateTime.Parse(ChestManager.ClanChestDailyData.Last().Key, currentCulture);
                var lastDate = lastDateEntry.AddDays(1);

                var todayDate = DateTime.Now.Date;

                DateSelection.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), firstDate));
                DateSelection.BlackoutDates.Add(new CalendarDateRange(lastDate, new DateTime(9999, 1, 1)));
                DateSelection.DisplayDate = DateTime.Now;
                StartDate = firstDate.AddDays(1);

                EndDateSelection.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), firstDate));
                EndDateSelection.BlackoutDates.Add(new CalendarDateRange(lastDate, new DateTime(9999, 1, 1)));
                EndDateSelection.DisplayDate = lastDate;
                EndDate = lastDate.AddDays(-1);
                isready = true;

                LoadDateEntry(StartDate, EndDate);

                Sort();
            }
            catch(Exception ex)
            {
                //--- fault in parsing date from clanchest.db
                //--- if person is from Germany, but Windows time and region is configured as America, it will lead to this issue.
                var message = $"This is a result of different regions. Generally, if you started counting chests using America region format, and now using another region, you will see this error.\n\n\n To fix this error, set your time and date's region format to the correct region.";
                MessageBox.Show(message, "Date Parsing Error");
            }


        }
        void Sort()
        {
            var data = CollectionViewSource.GetDefaultView(ClanStatsListView.ItemsSource);
            using (data.DeferRefresh())
            {
                data.SortDescriptions.Clear();
                if (AscSortRadioButton.IsChecked == true)
                {

                    var sortPath = ClanManager.Instance.ClanChestSettings.ChestPointsSettings.UseChestPoints == true ? "Points" : "Total";

                    data.SortDescriptions.Add(new SortDescription(sortPath, ListSortDirection.Ascending));
                }
                if (DescSortRadioButton.IsChecked == true)
                {
                    var sortPath = ClanManager.Instance.ClanChestSettings.ChestPointsSettings.UseChestPoints == true ? "Points" : "Total";
                    data.SortDescriptions.Add(new SortDescription(sortPath, ListSortDirection.Descending));
                }
            }
        }
        bool Filter_Clanmate_Results(object item)
        {
            var filtered_name = FilterClanmate.Text;
            if (String.IsNullOrEmpty(filtered_name))
                return true;
            else if (String.IsNullOrWhiteSpace(filtered_name)) return true;
            else
            {
                var clanmate = (ClanStatisticData)item;
                return clanmate.Clanmate.StartsWith(filtered_name, StringComparison.CurrentCultureIgnoreCase);

            }
        }
        private void LoadDateEntry(DateTime startdate, DateTime enddate)
        {
            try
            {

                //var dateEntry = ChestManager.ClanChestDailyData.Where(d => d.Key.Equals(date)).ToList()[0];
                var numDays = (enddate - startdate).TotalDays;
                var dateRange = ChestManager.ClanChestDailyData.ToList().GetRange(0, (int)numDays + 1);
                int commoncryptstotal, rarecryptstotal, epiccryptstotal, citadelsstotal,
                        arenastotal, uniontriumphstotal, vaultancienttotal, ancientchests, jormungandrtotal, heroicstotal, bankstotal, storycheststotal, total, otherTotal, totalPoints;

                commoncryptstotal = rarecryptstotal = epiccryptstotal = citadelsstotal = arenastotal =
                heroicstotal = bankstotal = uniontriumphstotal = vaultancienttotal = ancientchests = jormungandrtotal = storycheststotal = total = otherTotal = totalPoints = 0;

                if (ClanStatisticData != null)
                    ClanStatisticData.Clear();

                //-- initialize everything first.
                foreach (var clanmate in ClanManager.Instance.ClanmateManager.Database.Clanmates)
                {
                    ClanStatisticData.Add(new TBChestTracker.ClanStatisticData(clanmate.Name, commoncryptstotal, rarecryptstotal, epiccryptstotal, citadelsstotal,
                      arenastotal, uniontriumphstotal, vaultancienttotal, heroicstotal, ancientchests, jormungandrtotal, storycheststotal, bankstotal, total, totalPoints));
                }

                foreach (var date in dateRange)
                {
                    foreach (var entry in date.Value)
                    {

                        commoncryptstotal = rarecryptstotal = epiccryptstotal = citadelsstotal = arenastotal =
               heroicstotal = bankstotal = uniontriumphstotal = vaultancienttotal = ancientchests = jormungandrtotal = storycheststotal = total = otherTotal = totalPoints = 0;

                        var clanmate = entry.Clanmate;
                        //var chests = dateEntry.Value.Where(name => name.Clanmate.Equals(clanmate)).Select(chest => chest.chests).ToList()[0];
                        var chests = date.Value.Where(name =>
                                                name.Clanmate.Equals(clanmate, StringComparison.CurrentCultureIgnoreCase)).Select(chest => chest.chests).ToList()[0];

                        if (chests != null)
                        {

                            commoncryptstotal = chests.Where(common => common.Type.ToLower().Contains("common")).Count();
                            rarecryptstotal = chests.Where(common => common.Type.ToLower().Contains("rare")).Count();
                            epiccryptstotal = chests.Where(common => common.Type.ToLower().Contains("epic")).Count();
                            citadelsstotal = chests.Where(common => common.Type.ToLower().Contains("citadel")).Count();
                            arenastotal = chests.Where(common => common.Type.ToLower().Contains("arena")).Count();
                            heroicstotal = chests.Where(common => common.Type.ToLower().Contains("heroic")).Count();
                            uniontriumphstotal = chests.Where(common => common.Type.ToLower().Contains("union of triumph")).Count();
                            vaultancienttotal = chests.Where(common => common.Type.ToLower().Contains("vault")).Count();
                            bankstotal = chests.Where(common => common.Type.ToLower().Contains("bank")).Count();
                            ancientchests = chests.Where(common => common.Type.ToLower().Contains("Ancient")).Count();
                            jormungandrtotal = chests.Where(c => c.Type.ToLower().Contains("jormungandr")).Count();
                            storycheststotal = chests.Where(common => common.Type.ToLower().Contains("story")).Count();
                            otherTotal = chests.Where(common => common.Type.ToLower().Contains("other")).Count();

                            total = commoncryptstotal + rarecryptstotal + epiccryptstotal + citadelsstotal + arenastotal + uniontriumphstotal + heroicstotal + vaultancienttotal + bankstotal + storycheststotal + otherTotal;
                            totalPoints = entry.Points;
                        }
                        
                        var updateStats = ClanStatisticData.Where(mate => mate.Clanmate.Equals(clanmate, StringComparison.CurrentCultureIgnoreCase)).ToList()[0];
                        updateStats.CommonCryptsTotal += commoncryptstotal;
                        updateStats.RareCryptsTotal += rarecryptstotal;
                        updateStats.EpicCryptsTotal += epiccryptstotal;
                        updateStats.CitadelsTotal += citadelsstotal;
                        updateStats.ArenasTotal += arenastotal;
                        updateStats.HeroicsTotal += heroicstotal;
                        updateStats.UnionTriumphsTotal += uniontriumphstotal;
                        updateStats.VaultAncientsTotal += vaultancienttotal;
                        updateStats.BanksTotal += bankstotal;
                        updateStats.AncientChestsTotal += ancientchests;
                        updateStats.JormungandrShopChestsTotal += jormungandrtotal;
                        updateStats.StoryChestsTotal += storycheststotal;
                        updateStats.Total += total;
                        updateStats.Points += totalPoints;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void DateSelection_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isready)
                LoadDateEntry(StartDate, EndDate);
        
        }

        private void FilterClanmate_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ClanStatsListView.ItemsSource).Refresh();
        }

        private void ExportClanChest_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ExportWindow exportWindow = new ExportWindow();
            exportWindow.ShowDialog();
        }

        private void Close_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AscSortRadioButton_Click(object sender, RoutedEventArgs e)
        {
            Sort();
        }

        private void DescSortRadioButton_Click(object sender, RoutedEventArgs e)
        {
            Sort();
        }
    }
}
