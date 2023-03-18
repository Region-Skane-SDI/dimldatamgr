namespace RS.SDI.DimlMgr.Client
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task PopulateSamples()
        {
            var testbed = new DataSpace("Test");
            await testbed.PopulateSamplesAsync();
            var states = testbed.GetAllDataProductStates();

            foreach (var state in states)
            {
                Console.WriteLine(state.Dimlid);
            }
        }
    }
}