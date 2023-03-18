using Newtonsoft.Json;
using RS.SDI.Diml.Core.GitHub;
using System.Xml.Linq;

namespace RS.SDI.DimlMgr.Client
{
    public class DataSpace
    {
        public string EnvironmentId { get; private set; }

        private SortedList<string, DataProductState> DataProductStates { get; set; } = new SortedList<string, DataProductState>();

        public DataSpace(string environmentId)
        {
            EnvironmentId = environmentId;
        }

        public DataProductState[] GetAllDataProductStates() => DataProductStates.Values.ToArray();

        public async Task PopulateSamplesAsync()
        {
            using var _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "ASP.NET");

            var response = await _client.GetAsync("https://api.github.com/repos/Region-Skane-SDI/diml/contents/samples/demo");
            var repoFiles = JsonConvert.DeserializeObject<List<GithubFile>>(await response.Content.ReadAsStringAsync());

            if (repoFiles != null)
            {
                foreach (var file in repoFiles)
                {
                    var state = new DataProductState();
                    var dataProduct = new DataProduct();
                    await dataProduct.LoadAsync(new Dimlid(file.DownloadUrl), null);

                    state.Dimlid = dataProduct.Dimlid;
                    var dimlidHashCode = dataProduct.Dimlid.ToString().GetHashCode();
                    state.ViewCount = dimlidHashCode % 1000;
                    state.LikeCount = dimlidHashCode % 100;
                    state.IsAvailable = true;

                    DataProductStates.Add(state.Dimlid.ToString(), state);
                }
            }
        }
    }
}
