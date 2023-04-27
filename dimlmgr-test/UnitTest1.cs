namespace RS.SDI.DimlMgr.Client
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task PopulateSamples()
        {
            var testbed = new DimlClient();
            await testbed.Init("dev", "https://raw.githubusercontent.com/Region-Skane-SDI/diml/main/samples/sample_config.dimlcfg");
            await testbed.PopulateSampleDataProductsAsync("https://api.github.com/repos/Region-Skane-SDI/diml/contents/samples/demo", xsdUri: "https://raw.githubusercontent.com/Region-Skane-SDI/diml/main/schemas/v0/dataproduct.xsd", xsdTargetNamespace: "https://github.com/Region-Skane-SDI/diml");
            await testbed.PopulateSampleStatesAsync();
            var states = await testbed.GetAllDataProductBundlesAsync();

            foreach (var state in states)
            {
                Console.WriteLine(state.Dimlid);
            }
        }
    }
}