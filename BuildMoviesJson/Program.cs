using System.Text;

using BLTools;
using BLTools.Text;

using MediaSearch.Models;
using MediaSearch.Server.Services;

//const string PARAM_TARGET = "target";

//ISplitArgs Args = new SplitArgs();
//Args.Parse(args);

//string TargetFile = Args.GetValue(PARAM_TARGET, @".\movie.json");
//TMediaSearchDatabaseJson Database = new TMediaSearchDatabaseJson() {
//  StoragePath = Path.GetDirectoryName(TargetFile) ?? ".",
//  StorageFilename = Path.GetFileName(TargetFile)
//};

//Console.WriteLine(Database.ToString().Box("Target file"));

//await Database.SaveAsync(CancellationToken.None);


TMovieCache MovieCache = new TMovieCache() { RootStoragePath = @"\\multimedia.sharenet.priv\multimedia\films" };
using (CancellationTokenSource Timeout = new CancellationTokenSource(100000)) {
  await MovieCache.Parse(Timeout.Token);
}
StringBuilder sb = new StringBuilder();
sb.AppendLine("{\n\"movies\" : [\n");
foreach (IJson MovieItem in MovieCache.GetAllMovies()) {
  sb.Append(MovieItem.ToJson(IJson.DefaultJsonSerializerOptions));
  sb.AppendLine(",");
}
sb.Trim(',', '\n', '\r');
sb.AppendLine("  \n]\n}");
const string TARGET_FOLDER = @"i:\json";
if (!Directory.Exists(TARGET_FOLDER)) {
  Directory.CreateDirectory(TARGET_FOLDER);
}
File.WriteAllText($@"{TARGET_FOLDER}\movies.json", sb.ToString(), new UTF8Encoding(false));
