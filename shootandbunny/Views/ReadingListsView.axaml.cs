using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using shootandbunny.Controls;
using shootandbunny.Entities;

using shootandbunny.Context;

namespace shootandbunny.Views;

public partial class ReadingListsView : UserControl
{
    private List<ReadingList> _entries = new();
    private List<ReadingStatus> _statuses = new();
    private string _activeTab = "all";

    public ReadingListsView()
    {
        InitializeComponent();
        LoadData();
        Render();
    }

    private void LoadData()
    {
        using var db = new MyDbContext();
        _statuses = db.ReadingStatuses.ToList();
        _entries = db.ReadingLists
            .Include(rl => rl.Book).ThenInclude(b => b.Author)
            .Include(rl => rl.Book).ThenInclude(b => b.Reviews)
            .Include(rl => rl.Status)
            .Where(rl => rl.UserId == Core.CurrentUser!.Id)
            .OrderByDescending(rl => rl.AddedAt)
            .ToList();
    }

    private void Render()
    {
        ListPanel.Children.Clear();
        var filtered = _activeTab == "all"
            ? _entries
            : _entries.Where(e => e.Status.Name == _activeTab).ToList();

        EmptyText.IsVisible = filtered.Count == 0;
        if (filtered.Count == 0) return;

        foreach (var entry in filtered)
        {
            var card = new ReadingListCard(entry, _statuses);
            card.BookOpenRequested += (_, book) =>
            {
                var mainWindow = TopLevel.GetTopLevel(this) as MainWindow;
                mainWindow?.Navigate(new BookView(book));
            };
            card.RemoveRequested += (_, e) =>
            {
                using var db = new MyDbContext();
                db.ReadingLists.Remove(db.ReadingLists.First(x => x.Id == e.Id));
                db.SaveChanges();
                _entries.Remove(e);
                Render();
            };
            card.StatusChanged += (_, args) =>
            {
                using var db = new MyDbContext();
                var dbEntry = db.ReadingLists.First(x => x.Id == args.Entry.Id);
                dbEntry.StatusId = args.Status.Id;
                db.SaveChanges();
                args.Entry.StatusId = args.Status.Id;
                args.Entry.Status = args.Status;
            };
            ListPanel.Children.Add(card);
        }
    }

    private void UpdateTabHighlight()
    {
        if (_activeTab == "all")
            TabAll.Opacity = 1.0;
        else
            TabAll.Opacity = 0.4;

        if (_activeTab == "В планах")
            TabPlan.Opacity = 1.0;
        else
            TabPlan.Opacity = 0.4;

        if (_activeTab == "Читаю")
            TabReading.Opacity = 1.0;
        else
            TabReading.Opacity = 0.4;

        if (_activeTab == "Прочитано")
            TabDone.Opacity = 1.0;
        else
            TabDone.Opacity = 0.4;

        if (_activeTab == "Заброшено")
            TabDropped.Opacity = 1.0;
        else
            TabDropped.Opacity = 0.4;
    }

    private void All_Click(object? sender, RoutedEventArgs e)
    {
        _activeTab = "all";
        UpdateTabHighlight();
        Render();
    }

    private void Plan_Click(object? sender, RoutedEventArgs e)
    {
        _activeTab = "В планах";
        UpdateTabHighlight();
        Render();
    }

    private void Reading_Click(object? sender, RoutedEventArgs e)
    {
        _activeTab = "Читаю";
        UpdateTabHighlight();
        Render();
    }

    private void Done_Click(object? sender, RoutedEventArgs e)
    {
        _activeTab = "Прочитано";
        UpdateTabHighlight();
        Render();
    }

    private void Dropped_Click(object? sender, RoutedEventArgs e)
    {
        _activeTab = "Заброшено";
        UpdateTabHighlight();
        Render();
    }
}
