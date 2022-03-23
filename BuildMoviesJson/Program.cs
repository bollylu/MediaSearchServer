using System.Text;

using BLTools;
using BLTools.Text;

using MediaSearch.Models;

const string PARAM_TARGET = "target";

ISplitArgs Args = new SplitArgs();
Args.Parse(args);

string TargetFile = Args.GetValue(PARAM_TARGET, @".\movie.json");
TMediaSearchDatabaseJson Database = new TMediaSearchDatabaseJson() {
  StoragePath = Path.GetDirectoryName(TargetFile) ?? ".",
  StorageFilename = Path.GetFileName(TargetFile)
};

Console.WriteLine(Database.ToString().Box("Target file"));

await Database.SaveAsync(CancellationToken.None);
