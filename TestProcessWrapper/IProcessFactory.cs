namespace TestProcessWrapper
{
    internal interface IProcessFactory
    {
        public bool IsCoverletEnabled { get; set; }
        
        BuildConfiguration BuildConfiguration { get; set; }

        ITestProcess Create();
    }
}
